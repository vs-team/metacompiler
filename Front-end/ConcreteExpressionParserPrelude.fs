module ConcreteExpressionParserPrelude

open Utilities
open ParserMonad
open BasicExpression

type Associativity = 
  | Right
  | Left

type KeywordMulteplicity =
    | Single
    | Multiple

type KeywordKind =
    | Data
    | Func
  with static member fromString str =
         match str with
         | "Func" -> Func
         | "Data" -> Data
         | _ -> failwithf "Invalid keyword kind %s, only Data or Func are allowed" str

type State<'a, 's> = 's -> 'a*'s

    
type StateBuilder() =
     member this.Bind (p: State<'a,'s>, k : 'a -> State<'b,'s>) : State<'b, 's> =  
          (fun s -> 
            let a', s0 = p s
            k a' s0)
     member this.Return (x : 'a) = fun s -> x,s
     member this.ReturnFrom s = s 
     member this.Zero () = ()

let st = StateBuilder()

type ParsedKeyword<'k, 'e, 'i, 'di, 'ti> =
  {
    GenericArguments : List<BasicExpression<'k, 'e, 'i, 'di, 'ti>>
    LeftArguments: List<BasicExpression<'k, 'e, 'i, 'di, 'ti>>
    RightArguments: List<BasicExpression<'k, 'e, 'i, 'di, 'ti>>
    Name: string
    Type: List<BasicExpression<'k, 'e, 'i, 'di, 'ti>>
    Associativity: Associativity
    Priority: int
    Kind : KeywordKind
    Multeplicity : KeywordMulteplicity
  }
  with 
    member this.FilledType = 
      match this.Kind with
      | KeywordKind.Data -> this.Type
      | KeywordKind.Func ->
        let res = this.Type |> List.rev |> List.tail |> List.rev
        res
    member this.Arguments = this.LeftArguments @ this.RightArguments
  
type Keyword = Sequence | SmallerThan | SmallerOrEqual | GreaterThan | NotDivisible | Divisible | GreaterOrEqual | Equals | NotEquals | DoubleArrow | FractionLine | Nesting | DefinedAs | Inlined | Custom of string
  with 
  
  static member getName (l:List<BasicExpression<Keyword,Var,Literal,_,_>>) = 
            let rec findExtensionImported k =
              match k with
              | [] -> "", []
              | x::xs ->
                match x with
                | Imported(StringLiteral a, _, _) ->
                  a.ToString(), xs
                | _-> findExtensionImported xs
            findExtensionImported l

  static member getHeader (l:List<BasicExpression<Keyword,Var,Literal,_,_>>) : List<BasicExpression<Keyword,Var,Literal,_,_>>*List<BasicExpression<Keyword,Var,Literal,_,_>> =
      match l with
      | [] -> [], []
      | x::xs ->
        match x with
        | Application(Angle, b, _, _) -> 
             match b with
              | [Application(Angle, k, _, _)] ->
                  match k with 
                  | [Keyword(_, _, _)] ->
                      k, xs
        | Keyword(Custom a, _, _)       ->
          [x], xs
        | _ -> failwith "malformed expression"
  
  static member getGenerics (l:List<BasicExpression<Keyword,Var,Literal,_,_>>) : List<BasicExpression<Keyword,Var,Literal,_,_>> * List<BasicExpression<Keyword,Var,Literal,_,_>> = 
    match l with
      | [] -> [], []
      | x::xs ->
        match x with
        | Application(Square, ext, _, _) ->
          ext, xs
        | _ -> [], l

    static member getArguments k =
              let rec findArguments l =
                  match l with
                    | [] -> [], []
                    | x::xs -> 
                      match x with
                        | Imported(StringLiteral a, _, _) -> [], l
                        | Keyword(Custom ":", _, _) -> [], l
                        | Keyword(Custom "->", _, _) ->
                          let a,b = findArguments xs
                          a, b
                        | a -> 
                          let x,y = findArguments xs
                          a :: x, y
              findArguments k      
   
   static member removeCScharpName k = 
          let rec removeCS l =
            match l with
            | [] -> "", []
            | x::xs ->
              match x with
              | Application(Angle, args, _, _) ->
                match args with
                | [Extension(name, _, _)] ->
                  name.Name, []
                | _-> removeCS xs
          removeCS k

   static member getKeywordType k =
    let rec removeType l =
      match l with
      | [] -> (KeywordMulteplicity.Multiple, []), []
      | x::xs -> 
        match x with
        | Keyword(Custom ":", _, _) -> 
          removeType xs
        | Keyword(Custom "=>", _, _) ->
          let (_, x),y = removeType xs
          (KeywordMulteplicity.Single, x), y
        | Keyword(Custom "==>", _, _) ->
          let (_, x), y = removeType xs
          (KeywordMulteplicity.Multiple, x), y
        | Keyword(Custom "Priority", _,_)
        | Keyword(Custom "Associativity", _, _) -> (KeywordMulteplicity.Multiple, []), x::xs
        | a -> 
          let (m, x), y = removeType xs
          (m, a :: x), y
    removeType k  
                  
    static member getKeywordPriority k  =
              let rec removePriority l =
                    match l with
                    | [] -> 0, []
                    | x::xs -> 
                      match x with
                        | Keyword(Custom "Priority", _, _) ->
                          match xs with
                          | [] -> 0, []
                          | Imported(IntLiteral a, _,_)::ks ->
                            (int)a, ks
                        | _-> 0, xs
              removePriority k

    static member getKeywordAssociativity k  =
              let rec removeAssociativity l =
                    match l with
                    | [] -> Right, []
                    | x::xs -> 
                      match x with
                        | Keyword(Custom "Associativity", _, _) ->
                          match xs with
                          | [] -> Right, []
                          | Extension(a, _, _)::ks ->
                            match a with
                            | { Var.Name = "Right"} -> Right, ks
                            | { Var.Name = "Left"} -> Left, ks
                            | _ -> Right, xs
                          | _ -> failwith "Unexpected keyword shape encountered when finding associativity."
                        | _-> Right, xs
              removeAssociativity k

    static member decode (k:BasicExpression<Keyword,Var,_,_,_>) = 
      match k with
      | Application(Implicit, args, pos, ()) ->
        let x =
          (st{
             let! header = Keyword.getHeader
             match header with 
             | [Keyword(Custom a, _, _)] ->
                let kind = KeywordKind.fromString a
                let! genericArguments = Keyword.getGenerics
                let! leftArguments = Keyword.getArguments
                let! name = Keyword.getName
                let! rightArguments = Keyword.getArguments
                let! keywordMulteplicity, keywordtype = Keyword.getKeywordType
                let! priority = Keyword.getKeywordPriority
                let! associativity = Keyword.getKeywordAssociativity
                return { GenericArguments = genericArguments; LeftArguments = leftArguments; 
                         RightArguments = rightArguments; Name = name; Type = keywordtype; 
                         Associativity = associativity; Priority = priority ; Kind = kind;
                         Multeplicity = keywordMulteplicity }
             | _ ->
                return failwith "malformed expression"
           }) args |> fst
        x 
      | Application(Angle, args, pos, ()) ->
        let x =
          (st{
             let! name = Keyword.removeCScharpName
             return {GenericArguments = []; LeftArguments = []; RightArguments = []; Name = name; Type = []; Associativity = Right; Priority = 0; Kind = Data; Multeplicity = KeywordMulteplicity.Single }
           }) args |> fst
        x 
      | _ -> 
        failwithf "Malformed keyword root syntax %A" k 
     
    
    static member typeToString (keyword:List<BasicExpression<Keyword,Var,Literal,_,_>>) : List<string> =
      let rec dec k =
        match k with
          | x::xs ->
            match x with 
            | Application(Angle, Application(Angle, Extension({ Var.Name = n }, _, _)::[], _, _)::[], _, _)
            | Extension({ Var.Name = n }, _, _)
            | Keyword((Keyword.Custom n), _, _) ->
              n :: (dec xs)
            | Application(Angle, Application(Angle, Extension({ Var.Name = consName }, _, _)::gargs, _, _)::[], _, _) ->
              [consName + (gargs |> dec |> Seq.reduce (+))]
            | _ -> 
              dec xs
          | [] -> []
      dec keyword

    static member nonNativeTypeToString (keyword:List<BasicExpression<Keyword,Var,Literal,_,_>>) : string list =
      let rec dec k =
        match k with
          | x::xs ->
            match x with 
            | Application(Angle, Application(Angle, Extension(n, _, _)::[], _, _)::[], _, _) -> []
            | Extension(n, _, _) ->
              n.Name :: (dec xs)
            | _ -> dec xs
          | [] -> []
      dec keyword

    static member isNative keyword = 
        match keyword with
        | Extension(arg,_,_) -> false
        | Application(Angle, [Extension(arg,_,_)], _, _) -> true
        | Application(Angle, [arg], _, _) -> Keyword.isNative arg
        | Application(Angle, args, _, _) -> true
        | _ -> failwithf "Cannot match argument %A when checking for native keyword" keyword

    static member decodeName (keyword:BasicExpression<Keyword,_,_,_,_>) = 
      match keyword with
      | Extension(a, _, _) ->
        a.Name
      | _ -> (Keyword.decode keyword).Name     
                

    
    static member ParseWithoutComparison = 
      p{
        let! br = !!(word "<<" + word ">>") + p{ return () }
        match br with
        | First _ -> 
          return! fail "Expected keyword"
        | _ ->
          let! ar = word "=>" + ((word "==" + word "!=") + word ":=")
          match ar with
          | First _ -> 
            return DoubleArrow
          | Second ar' ->
            match ar' with
            | First ar'' -> 
              match ar'' with
              | First( _) -> return Equals
              | Second( _) -> return NotEquals
            | Second _ -> 
              return DefinedAs
      }
    static member Parse = 
      p{
        let! br = !!(word "<<" + word ">>") + p{ return () }
        match br with
        | First _ -> 
          return! fail "Expected keyword"
        | _ ->
          let! ar = word "=>" + ((((word "==" + word "!=") + (word "%" + word "!%")) + ((word ">=" + word ">") + (word "<=" + word "<"))) + word ":=")
          match ar with
          | First _ -> 
            return DoubleArrow
          | Second ar' ->
            match ar' with
            | First ar'' -> 
              match ar'' with
              | First(First(First _)) -> return Equals
              | First(First(Second _)) -> return NotEquals
              | First(Second(First _)) -> return Divisible
              | First(Second(Second _)) -> return NotDivisible
              | Second(First(First _)) -> return GreaterOrEqual
              | Second(First(Second _)) -> return GreaterThan
              | Second(Second(First _)) -> return SmallerOrEqual
              | Second(Second(Second _)) -> return SmallerThan
            | Second _ -> 
              return DefinedAs
      }      

    override this.ToString() =
      match this with
      | Inlined -> "<<>>"
      | Sequence -> ""
      | SmallerThan -> "<"
      | SmallerOrEqual -> "<="
      | GreaterThan -> ">"
      | GreaterOrEqual -> ">="
      | Divisible -> "%"
      | NotDivisible -> "!%"
      | Equals -> "=="
      | NotEquals -> "!="
      | DoubleArrow -> "=>"
      | DefinedAs -> ":="
      | FractionLine -> "\n-------------------\n"
      | Nesting -> ""
      | Custom m -> m
           

    static member ArgumentCSharpStyle keyword cleanup =
      match keyword with
      | Application(Angle, Application(Angle, Extension({ Var.Name = genName }, _, _)::genArgs, _, _) :: [], _, _) when genArgs.Length > 0 ->
        let args = 
          [
            for a in genArgs do
              match a with
              | Keyword(Custom(n),_,_) -> yield n
              | Extension({Var.Name = n},_,_) -> yield cleanup n
              | _ -> failwithf "Unexpected generic parameter %A" a
          ]
        let res = sprintf "%s%s" (cleanup genName) (args |> Seq.reduce (+))
        res
      | Application(Generic, arg::args, _ ,_) -> 
          sprintf "%s<%s>" (cleanup (Keyword.decodeName arg)) (args |> Seq.map (fun a -> Keyword.ArgumentCSharpStyle a cleanup) |> Seq.reduce (fun s x -> sprintf "%s, %s" s x))
      | _ ->
        match Keyword.isNative keyword with
        | true -> Keyword.decodeName keyword
        | false -> cleanup (Keyword.decodeName keyword)
      
    static member createExt l = l |> List.map (fun x -> Extension({Name = x}, Position.Zero, ()))                              
                                    
    static member CreateCSharpKeyword (kw : string*List<string>*List<string>*List<string>*int*string)= 
          let (!) l = l |> List.map (fun x -> Extension({Name = x}, Position.Zero, ()))
          let name, generic, left, right, priority, kwtype = kw
          { GenericArguments = !generic; LeftArguments = !left; RightArguments = !right; Name = name; Type = ![kwtype]; Associativity = Right; Priority = priority; Kind = Data; Multeplicity = KeywordMulteplicity.Single }

and ConcreteExpressionContext = 
  {
    PredefinedKeywords : Parser<Keyword, ConcreteExpressionContext>
    CustomKeywords : List<ParsedKeyword<Keyword, Var, Literal, Position, unit>>
    CustomKeywordsByPrefix : List<ParsedKeyword<Keyword, Var, Literal, Position, unit>>
    CustomKeywordsMap : Map<string, ParsedKeyword<Keyword, Var, Literal, Position, unit>>    
    InheritanceRelationships : Map<string, Set<string>>
    ImportedModules : List<string>
  } with
      member this.AllInheritanceRelationships =
        seq{
          for x in this.InheritanceRelationships do
            let i = x.Key
            for o in x.Value do
              yield i,o
        }
      member this.Inherits s1 s2 =
        match this.InheritanceRelationships |> Map.tryFind s1 with
        | Some os -> os.Contains s2
        | _ -> false
      member ctxt.CustomClasses =
        seq{
          for k in ctxt.CustomKeywords do
          yield k.Name
        } |> Set.ofSeq
      static member Empty =
        {
          PredefinedKeywords       = Keyword.Parse
          CustomKeywords           = []
          CustomKeywordsByPrefix   = []
          CustomKeywordsMap        = Map.empty
          InheritanceRelationships = Map.empty
          ImportedModules          = []
        }
      static member (++) (ctxt:ConcreteExpressionContext, ctxt':ConcreteExpressionContext) =
        let concatMap (p:Map<'a,'b>) (q:Map<'a,'b>) = 
            Map(Seq.concat [ (Map.toSeq p) ; (Map.toSeq q) ])
        {
            PredefinedKeywords = ctxt.PredefinedKeywords
            CustomKeywords = ctxt.CustomKeywords |> List.append ctxt'.CustomKeywords
            CustomKeywordsByPrefix = ctxt.CustomKeywordsByPrefix |> List.append ctxt'.CustomKeywordsByPrefix
            CustomKeywordsMap = ctxt.CustomKeywordsMap |> concatMap ctxt'.CustomKeywordsMap
            InheritanceRelationships = ctxt.InheritanceRelationships |> concatMap ctxt'.InheritanceRelationships
            ImportedModules = ctxt.ImportedModules |> List.append ctxt'.ImportedModules
        }
      static member CSharp =
        let ks = 
          [
            ("true", [], [], [], 0, "CSharpExpr")
            ("false", [], [], [], 0, "CSharpExpr")
            ("&&", [], ["CSharpExpr"], ["CSharpExpr"], 1, "CSharpExpr")
            ("||", [], ["CSharpExpr"], ["CSharpExpr"], 1, "CSharpExpr")
            ("!", [], [], ["CSharpExpr"], 1, "CSharpExpr")
            (",", [], ["CSharpExpr"], ["CSharpExpr"], 1, "CSharpExpr")
            ("<", [], ["CSharpExpr"], ["CSharpExpr"], 10, "CSharpExpr")
            (">", [], ["CSharpExpr"], ["CSharpExpr"], 10, "CSharpExpr")
            (">=", [], ["CSharpExpr"], ["CSharpExpr"], 10, "CSharpExpr")
            ("<=", [], ["CSharpExpr"], ["CSharpExpr"], 10, "CSharpExpr")
            ("==", [], ["CSharpExpr"], ["CSharpExpr"], 10, "CSharpExpr")
            ("!=", [], ["CSharpExpr"], ["CSharpExpr"], 10, "CSharpExpr")
            ("/", [], ["CSharpExpr"], ["CSharpExpr"], 100, "CSharpExpr")
            ("*", [], ["CSharpExpr"], ["CSharpExpr"], 10, "CSharpExpr")
            ("%", [], ["CSharpExpr"], ["CSharpExpr"], 100, "CSharpExpr")
            ("+", [], ["CSharpExpr"], ["CSharpExpr"], 100, "CSharpExpr")
            ("-", [], ["CSharpExpr"], ["CSharpExpr"], 100, "CSharpExpr")
            (".", [], ["CSharpExpr"], ["CSharpExpr"], 1000, "CSharpExpr")
          ] |> Seq.map Keyword.CreateCSharpKeyword
            |> Seq.toList
        {
          PredefinedKeywords = Keyword.ParseWithoutComparison
          CustomKeywords = ks
          CustomKeywordsByPrefix = ks |> List.sortBy (fun k -> k.Name) |> List.rev
          CustomKeywordsMap = ks |> Seq.map (fun x -> x.Name, x) |> Map.ofSeq
          InheritanceRelationships = Map.empty
          ImportedModules          = []
        }

and Var = { Name : string }
  with 
    override this.ToString() =
      this.Name

and Literal = StringLiteral of string | IntLiteral of int | BoolLiteral of bool | SingleLiteral of float32 | DoubleLiteral of float
  with 
    override this.ToString() =
      match this with
      | StringLiteral l -> sprintf "\"%s\"" l
      | IntLiteral i -> sprintf "%d" i
      | BoolLiteral b -> sprintf "%b" b
      | SingleLiteral s -> sprintf "%ff" s
      | DoubleLiteral d -> sprintf "%ff" d

    member this.typeString =
        match this with
        | StringLiteral l -> "string"
        | IntLiteral i -> "int"
        | BoolLiteral b -> "bool"
        | SingleLiteral s -> "float32"
        | DoubleLiteral d -> "float"

let getPredefinedKeywords() =
  p{
    let! ctxt = getContext()
    return ctxt.PredefinedKeywords
  }

let getCustomKeywords() =
  p{
    let! ctxt = getContext()
    return ctxt.CustomKeywords
  }

let getCustomKeywordsByPrefix() =
  p{
    let! ctxt = getContext()
    return ctxt.CustomKeywordsByPrefix
  }

let getCustomKeywordsMap() =
  p{
    let! ctxt = getContext()
    return ctxt.CustomKeywordsMap
  }


module ConcreteExpressionParserPrelude

open Utilities
open TypeDefinition
open ParserMonad
open StateMonad
open BasicExpression

type Associativity = 
  | Right
  | Left

type KeywordMulteplicity =
    | Single
    | Multiple

type KeywordKind =
    | Data
    | Func of Type


type ParsedKeyword<'k, 'e, 'i, 'di, 'ti> =
  {
    GenericArguments : List<TypeVariableData>
    LeftArguments: List<Type>
    RightArguments: List<Type>
    Name: string
    BaseType : Type
    Associativity: Associativity
    Priority: int
    Kind : KeywordKind
    Multeplicity : KeywordMulteplicity
    Position : Position
  }
  with 
    member this.Type = 
      let mutable t = 
        match this.Kind with
        | Data -> this.BaseType
        | Func returnType -> returnType
      for a in this.Arguments |> List.rev do
        t <- TypeAbstraction(a,t)
      t
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
                  | _ -> failwith "Malformed header"
              | _ -> failwith "Malformed header"
        | Keyword(Custom a, _, _)       ->
          [x], xs
        | _ -> failwith "malformed expression"
  
  static member getGenerics : State<List<string>, List<BasicExpression<Keyword,Var,Literal,_,_>>> = 
    fun l ->
      match l with
        | [] -> [], []
        | x::xs ->
          match x with
          | Application(Square, Application(Square, args, pos, _) :: [], _, _)
          | Application(Square, args, pos, _) ->
            [
              for a in args do
                match a with
                | Extension({Var.Name = argName},_,_) ->
                  yield argName
                | _ ->
                  failwithf "Error at %A: unexpected generic argument %A" pos a
            ], xs
          | _ -> [], l

  static member getArguments boundTypeVariables k =
    let (!) n = 
      if boundTypeVariables |> Set.contains n then
        TypeVariable(n, GenericParameter)
      else
        TypeConstant(n, TypeConstantDescriptor.FromName n)
    let rec findGenericParameters l =
      match l with
      | [] -> [], []
      | x::xs -> 
        match x with
        | Application(Square, Application(Square, args, _, _) :: [], _, _)
        | Application(Square, args, _, _) ->
          let args',_ = findArguments args
          args', xs
        | _ -> 
          [], l
    and findArguments l =
      match l with
      | [] -> [], []
      | x::xs -> 
        match x with
        | Imported(StringLiteral a, _, _) -> [], l
        | Keyword(Custom ":", _, _) -> [], l
        | Keyword(Custom "->", _, _) ->
          let a,b = findArguments xs
          a, b
        | Extension({Var.Name = name},_,_) -> 
          let genericArguments, xs1 = findGenericParameters xs
          match genericArguments with
          | [] ->
            let x,y = findArguments xs1
            !name :: x, y
          | _ -> 
            let x,y = findArguments xs1
            let res = ConstructedType(!name, genericArguments)
            res :: x, y
        | Application(Angle, Application(Angle, Extension({Var.Name = name},_,_) :: [], _, _) :: [], _, _) -> 
          let x,y = findArguments xs
          !name :: x, y
        | Application(Angle, Application(Angle, Extension({Var.Name = name},_,_) :: genericArgs, _, _) :: [], _, _) -> 
          let args = 
            [
              for arg in genericArgs do
                match arg with
                | Extension({Var.Name = argName},_,_) -> 
                  yield TypeConstant(argName, TypeConstantDescriptor.FromName argName)
                | Keyword(Custom(">"),_,_)
                | Keyword(Custom("<"),_,_)
                | Keyword(Custom(","),_,_) -> ()
                | _ -> 
                  failwithf "Unsupported generic argument %A; this should probably be a recursive parser." arg
            ]
          let x,y = findArguments xs
          ConstructedType(!name, args) :: x, y
        | _ -> 
          failwithf "Unsupported keyword argument %A" x
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
            | _ -> failwith "Malformed header"
        removeCS k

  static member getKeywordType (boundGenericParameters:Set<string>) : State<KeywordMulteplicity * Type * Option<Type>,_> =
    fun k ->
      let (!) n =
        if boundGenericParameters |> Set.contains n then
          TypeVariable(n, GenericParameter)
        else
           TypeConstant(n, TypeConstantDescriptor.FromName n)
      let rec removeType l =
        match l with
        | Keyword(Custom ":", _, _) :: xs -> 
          removeType xs
        | Extension({Var.Name=baseType},_,_) :: Keyword(Custom "=>", _, _) :: returnTypeExpr :: xs ->
          let returnTypes, _ = Keyword.getArguments boundGenericParameters [returnTypeExpr]
          match returnTypes with
          | [returnType] ->
            match xs with
            | Application(Square, Application(Square, args, _, _)::[], _, _)::xs
            | Application(Square, args, _, _)::xs ->
              let genericArguments, _ = Keyword.getArguments boundGenericParameters args
              let res = ConstructedType(returnType, genericArguments)
              (KeywordMulteplicity.Single, !baseType, Some(res)), xs
            | _ ->
              (KeywordMulteplicity.Single, !baseType, Some returnType), xs
          | _ -> failwithf "Cannot extract return type from %A" returnTypeExpr
        | Extension({Var.Name=baseType},_,_) :: Keyword(Custom "==>", _, _) :: returnTypeExpr :: xs ->
          let returnTypes, _ = Keyword.getArguments boundGenericParameters [returnTypeExpr]
          match returnTypes with
          | [returnType] ->
            (KeywordMulteplicity.Multiple, !baseType, Some returnType), xs
          | _ -> failwithf "Cannot extract return type from %A" returnTypeExpr
        | Extension({Var.Name=baseType},_,_) :: (Keyword(Custom "Priority",_,_)::_ as xs)
        | Extension({Var.Name=baseType},_,_) :: (Keyword(Custom "Associativity",_,_)::_ as xs)
        | Extension({Var.Name=baseType},_,_) :: ([] as xs) ->
          (KeywordMulteplicity.Single, !baseType, None), xs
        | Extension({Var.Name=baseType},_,_) :: args ->
          let genericArgs,xs = Keyword.getGenerics args
          let genericArgs = genericArgs |> List.map (!)
          let baseType = ConstructedType(!baseType, genericArgs)
          match xs with
          | Keyword(Custom "==>", _, _) :: returnTypeExpr :: xs
          | Keyword(Custom "=>", _, _) :: returnTypeExpr :: xs ->
            let returnTypes, _ = Keyword.getArguments boundGenericParameters [returnTypeExpr]
            match returnTypes with
            | [returnType] ->
              (KeywordMulteplicity.Multiple, baseType, Some returnType), xs
            | _ -> failwithf "Cannot extract return type from %A" returnTypeExpr
          | (Keyword(Custom "Priority",_,_)::_ as xs)
          | (Keyword(Custom "Associativity",_,_)::_ as xs)
          | ([] as xs) ->
            (KeywordMulteplicity.Single, baseType, None), xs
          | _ -> 
            failwithf "Cannot extract keyword type from %A" k
        | _ -> failwithf "Cannot extract keyword type from %A" k
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
                        | _ -> failwith "Malformed header"
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

  static member decode (k:BasicExpression<Keyword,Var,Literal,Position,Unit>) : ParsedKeyword<Keyword,Var,Literal,Position,Unit> = 
    match k with
    | Application(Implicit, args, pos, ()) ->
      let x =
        (st{
            let! header = Keyword.getHeader
            match header with 
            | [Keyword(Custom a, pos, _)] ->
              let! genericArguments = Keyword.getGenerics
              let genericArgumentsSet = genericArguments |> Set.ofList
              let! leftArguments = Keyword.getArguments genericArgumentsSet
              let! name = Keyword.getName
              let! rightArguments = Keyword.getArguments genericArgumentsSet
              let! keywordMulteplicity, baseType, returnType = Keyword.getKeywordType genericArgumentsSet
              let! priority = Keyword.getKeywordPriority
              let! associativity = Keyword.getKeywordAssociativity
              let kind = 
                match a, returnType with
                | "Data", None -> KeywordKind.Data
                | "Func", Some returnType -> KeywordKind.Func returnType
                | _ -> failwithf "Error at %A: invalid keyword definition with kind %A and return type %A" pos a returnType
              return { GenericArguments = [ for a in genericArguments -> a,GenericParameter ]; 
                        LeftArguments = leftArguments; RightArguments = rightArguments; Name = name; 
                        BaseType = baseType; Kind = kind;
                        Associativity = associativity; Priority = priority ;
                        Multeplicity = keywordMulteplicity; Position = pos }
            | _ ->
              return failwith "malformed expression"
          }) args |> fst
      x 
    | Application(Angle, args, pos, ()) ->
      let x =
        (st{
            let! name = Keyword.removeCScharpName
            return { GenericArguments = []; LeftArguments = []; RightArguments = []; Name = name; 
                     BaseType = TypeConstant(name, Defined); Associativity = Right; Priority = 0; Kind = Data; Multeplicity = KeywordMulteplicity.Single; Position = pos }
          }) args |> fst
      x 
    | _ -> 
      failwithf "Malformed keyword root syntax %A" k 

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
           

    static member ArgumentCSharpStyle (t:Type) cleanup : string =
      match t with
      | TypeConstant(t,_) -> t
      | ConstructedType(t, args) ->
        sprintf "%s<%s>" (Keyword.ArgumentCSharpStyle t cleanup) (args |> Seq.map (fun a -> Keyword.ArgumentCSharpStyle a cleanup) |> Seq.reduce (fun s x -> sprintf "%s, %s" s x))
      | TypeVariable(v,_) -> v |> cleanup
      | _ -> failwithf "C# style printing of %A is not implemented" t
      
    static member createExt l = l |> List.map (fun x -> Extension({Name = x}, Position.Zero, ()))                              

    static member CreateCSharpKeyword (kw : string*List<string>*List<TypeDefinition.Type>*List<TypeDefinition.Type>*int*string) =
      let name, generic, left, right, priority, kwtype = kw
      { GenericArguments = [ for a in generic -> a,GenericParameter ]; LeftArguments = left; RightArguments = right; Name = name; 
        BaseType = TypeConstant(kwtype, TypeDefinition.TypeConstantDescriptor.FromName name); 
        Associativity = Right; Priority = priority; Kind = Data; 
        Multeplicity = KeywordMulteplicity.Single; Position = Position.Zero }

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
        let (!) x = TypeConstant(x, Defined)
        let (!!) = List.map (!)
        let ks = 
          [
            ("true", [], [], [], 0, "CSharpExpr")
            ("false", [], [], [], 0, "CSharpExpr")
            ("&&", [], !!["CSharpExpr"], !!["CSharpExpr"], 1, "CSharpExpr")
            ("||", [], !!["CSharpExpr"], !!["CSharpExpr"], 1, "CSharpExpr")
            ("!", [], [], !!["CSharpExpr"], 1, "CSharpExpr")
            (",", [], !!["CSharpExpr"], !!["CSharpExpr"], 1, "CSharpExpr")
            ("<", [], !!["CSharpExpr"], !!["CSharpExpr"], 10, "CSharpExpr")
            (">", [], !!["CSharpExpr"], !!["CSharpExpr"], 10, "CSharpExpr")
            (">=", [], !!["CSharpExpr"], !!["CSharpExpr"], 10, "CSharpExpr")
            ("<=", [], !!["CSharpExpr"], !!["CSharpExpr"], 10, "CSharpExpr")
            ("==", [], !!["CSharpExpr"], !!["CSharpExpr"], 10, "CSharpExpr")
            ("!=", [], !!["CSharpExpr"], !!["CSharpExpr"], 10, "CSharpExpr")
            ("/", [], !!["CSharpExpr"], !!["CSharpExpr"], 100, "CSharpExpr")
            ("*", [], !!["CSharpExpr"], !!["CSharpExpr"], 10, "CSharpExpr")
            ("%", [], !!["CSharpExpr"], !!["CSharpExpr"], 100, "CSharpExpr")
            ("+", [], !!["CSharpExpr"], !!["CSharpExpr"], 100, "CSharpExpr")
            ("-", [], !!["CSharpExpr"], !!["CSharpExpr"], 100, "CSharpExpr")
            (".", [], !!["CSharpExpr"], !!["CSharpExpr"], 1000, "CSharpExpr")
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
        | SingleLiteral s -> "float"
        | DoubleLiteral d -> "double"

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


module ConcreteExpressionParserPrelude

open Utilities
open ParserMonad
open BasicExpression

type KeywordIndex = Name | LeftArguments | RightArguments | BothArguments | GenericArguments | Class | Priority

type Keyword = Sequence | SmallerThan | SmallerOrEqual | GreaterThan | NotDivisible | Divisible | GreaterOrEqual | Equals | NotEquals | DoubleArrow | FractionLine | Nesting | DefinedAs | Inlined | Custom of string
  with 
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

    static member Fold bracket genericArguments leftArguments name rightArguments priority kwType =
        let rec removeOuterSquares args =
            match args with
            | Application(Square, [arg], _, _) -> arg |> removeOuterSquares
            | Application(Square, args, _, _) -> args
            | arg -> [arg]
        let mergeGenerics arguments generics =
            let genericNames = generics |> List.map Keyword.Name
            let rec merge prev next = 
                if genericNames |> List.exists (fun x -> x = Keyword.Name next) then
                    match prev with
                    | [last] -> 
                        Application(Generic, last :: next :: [], Position.Zero, ()) :: []
                    | last :: list -> 
                        list @ Application(Generic, last :: next :: [], Position.Zero, ()) :: []
                    | [] ->
                        Application(Generic, [next], Position.Zero, ()) :: []
                else 
                    prev @ next :: []
            Application(Square, arguments |> List.fold merge [], Position.Zero, ())
        let genericArgs = removeOuterSquares genericArguments
        let leftArgs = mergeGenerics (removeOuterSquares leftArguments) genericArgs
        let rightArgs = mergeGenerics (removeOuterSquares rightArguments) genericArgs
        Application(bracket, Application(Square, genericArgs, Position.Zero, ()) :: leftArgs :: name :: rightArgs :: priority :: kwType :: [], Position.Zero, ())

    static member ExtractArguments index keyword =
        let rec removeOuterSquares args =
            match args with
            | Application(Square, args, _, _) -> args
            | _ -> failwithf "Cannot extract arguments %A" keyword
        match index with
        | LeftArguments -> removeOuterSquares (Keyword.Extract LeftArguments keyword)
        | RightArguments -> removeOuterSquares (Keyword.Extract RightArguments keyword)
        | GenericArguments -> removeOuterSquares (Keyword.Extract GenericArguments keyword)
        | BothArguments -> (Keyword.ExtractArguments LeftArguments keyword) @ (Keyword.ExtractArguments RightArguments keyword)
        | _ -> failwithf "%A is not a argument index" index

    static member Arguments keyword =
        Keyword.ExtractArguments BothArguments keyword

    static member LeftArguments keyword =
        Keyword.ExtractArguments LeftArguments keyword

    static member RightArguments keyword =
        Keyword.ExtractArguments RightArguments keyword

    static member GenericArguments keyword =
        Keyword.ExtractArguments GenericArguments keyword

    static member ArgumentCSharpStyle keyword cleanup =
        match keyword with
        | Application(Generic, arg::args, _ ,_) -> 
            sprintf "%s<%s>" (cleanup (Keyword.Name arg)) (args |> Seq.map (fun a -> Keyword.ArgumentCSharpStyle a cleanup) |> Seq.reduce (fun s x -> sprintf "%s, %s" s x))
        | _ ->
            match Keyword.IsNative keyword with
            | true -> Keyword.Name keyword
            | false -> cleanup (Keyword.Name keyword)

    static member Class keyword =
        match(Keyword.Extract Class keyword) with
        | Extension(cls, _, _) -> cls.Name
        | _ -> failwithf "Cannot extract class %A" keyword

    static member Priority keyword =
        match(Keyword.Extract Priority keyword) with
        | Imported(IntLiteral(priority),_,_) -> priority
        | _ -> failwith "Cannot find priority %A when looking up keyword priority" keyword

    static member Ariety index keyword =
        match index with
        | LeftArguments -> (Keyword.LeftArguments keyword).Length
        | RightArguments -> (Keyword.RightArguments keyword).Length
        | _ -> failwithf "%A is not a valid ariety... " index
      
    static member IsNative keyword = 
        match keyword with
        | Extension(arg,_,_) -> false
        | Application(Angle, [Extension(arg,_,_)], _, _) -> true
        | Application(Angle, [arg], _, _) -> Keyword.IsNative arg
        | Application(Angle, args, _, _) -> true
        | _ -> failwithf "Cannot match argument %A when checking for native keyword" keyword

    static member Extract index keyword:BasicExpression<Keyword, Var, Literal, Position, unit> =
        match keyword with 
        | Application(Data, exp, _, _) ->
            match index with
            | GenericArguments -> exp.[0]
            | Name -> exp.[2]
            | LeftArguments -> exp.[1]
            | RightArguments -> exp.[3]
            | Priority -> exp.[4]
            | Class -> exp.[5]
            | BothArguments -> failwithf "Cannot extract both arguments from expression"
        | Application(Function, exp, _, _) ->
            match index with
            | GenericArguments -> exp.[0]
            | Name -> exp.[2]
            | LeftArguments -> exp.[1]
            | RightArguments -> exp.[3]
            | Priority -> exp.[4]
            | Class -> exp.[5]
            | BothArguments -> failwithf "Cannot extract both arguments from expression"
        | _ -> failwithf "Invalid keyword %A" keyword

    static member Name keyword =
        match keyword with
        | Application(Generic, arg::args, _, _) -> Keyword.Name arg
        | Extension(name,_,_) -> name.Name
        | Application(Angle, [Extension(arg,_,_)], _, _) -> arg.Name
        | Application(Angle, [arg], _, _) -> arg |> Keyword.Name
        | Application(Angle, args, _, _) -> 
          let rec printSequence =
            function
            | Keyword(Custom k,_,_) :: es -> k + printSequence es
            | Extension(arg,_,_) :: es -> arg.Name + printSequence es
            | [] -> ""
            | es -> failwithf "Cannot match argument %A" es
          args |> printSequence
        | _ ->
            match Keyword.Extract Name keyword with
            | Imported(StringLiteral(name), _, _) -> name
            | _ -> failwithf "Cannot extract keyword name %A" keyword
        
    static member CreateCSharpKeyword (name, genericArguments, leftArguments, rightArguments, priority, className) =
        let transformIntLiteral lit =
            Imported(IntLiteral(lit), Position.Zero, ())
        let transformIdentifier ident =
            Extension({Var.Name = ident}, Position.Zero, ())
        let transformArguments args = 
            Application(Square, args |> List.map transformIdentifier, Position.Zero, ())
        Application(Data, transformArguments genericArguments :: transformArguments leftArguments :: Imported(StringLiteral(name), Position.Zero, ()) :: transformArguments rightArguments :: transformIntLiteral priority :: transformIdentifier className :: [], Position.Zero, ())

and ConcreteExpressionContext = 
  {
    PredefinedKeywords : Parser<Keyword, ConcreteExpressionContext>
    CustomKeywords : List<BasicExpression<Keyword, Var, Literal, Position, unit>>
    CustomKeywordsByPrefix : List<BasicExpression<Keyword, Var, Literal, Position, unit>>
    CustomKeywordsMap : Map<string, BasicExpression<Keyword, Var, Literal, Position, unit>>    
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
          yield Keyword.Name k
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
          CustomKeywordsByPrefix = ks |> List.sortBy (fun k -> Keyword.Extract Name k) |> List.rev
          CustomKeywordsMap = ks |> Seq.map (fun x -> (Keyword.Name x), x) |> Map.ofSeq
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


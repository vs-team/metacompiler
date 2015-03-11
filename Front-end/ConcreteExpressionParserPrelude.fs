module ConcreteExpressionParserPrelude

open Utilities
open ParserMonad
open BasicExpression

type Keyword = Sequence | SmallerThan | SmallerOrEqual | GreaterThan | NotDivisible | Divisible | GreaterOrEqual | Equals | NotEquals | DoubleArrow | FractionLine | Nesting | DefinedAs | Inlined | Custom of name : string
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
      | Custom name -> name

and CustomKeyword = { Name : string; GenericArguments : List<KeywordArgument>; LeftArguments : List<KeywordArgument>; RightArguments : List<KeywordArgument>; Priority : int; Class : KeywordArgument }
  with member this.IsGeneric = this.GenericArguments.IsEmpty |> not
       member this.LeftAriety = this.LeftArguments.Length
       member this.RightAriety = this.RightArguments.Length
       member this.Arguments = this.LeftArguments @ this.RightArguments

and ConcreteExpressionContext = 
  {
    PredefinedKeywords : Parser<Keyword, ConcreteExpressionContext>
    CustomKeywords : List<CustomKeyword>
    CustomKeywordsByPrefix : List<CustomKeyword>
    CustomKeywordsMap : Map<string,CustomKeyword>    
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
          yield k.Class.BaseName
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
      static member CSharp =
        let ks = 
          [
            { Name = "true"
              GenericArguments = []
              LeftArguments = []
              RightArguments = []
              Priority = 0
              Class = Defined "CSharpExpr" }
            { Name = "false"
              GenericArguments = []
              LeftArguments = []
              RightArguments = []
              Priority = 0
              Class = Defined "CSharpExpr" }
            { Name = "&&"
              GenericArguments = []
              LeftArguments = [Defined "CSharpExpr"]
              RightArguments = [Defined "CSharpExpr"]
              Priority = 1
              Class = Defined "CSharpExpr" }
            { Name = "||"
              GenericArguments = []
              LeftArguments = [Defined "CSharpExpr"]
              RightArguments = [Defined "CSharpExpr"]
              Priority = 1
              Class = Defined "CSharpExpr" }
            { Name = "!"
              GenericArguments = []
              LeftArguments = []
              RightArguments = [Defined "CSharpExpr"]
              Priority = 1
              Class = Defined "CSharpExpr" }
            { Name = ","
              GenericArguments = []
              LeftArguments = [Defined "CSharpExpr"]
              RightArguments = [Defined "CSharpExpr"]
              Priority = 1
              Class = Defined "CSharpExpr" }
            { Name = ">"
              GenericArguments = []
              LeftArguments = [Defined "CSharpExpr"]
              RightArguments = [Defined "CSharpExpr"]
              Priority = 10
              Class = Defined "CSharpExpr" }
            { Name = "<"
              GenericArguments = []
              LeftArguments = [Defined "CSharpExpr"]
              RightArguments = [Defined "CSharpExpr"]
              Priority = 10
              Class = Defined "CSharpExpr" }
            { Name = ">="
              GenericArguments = []
              LeftArguments = [Defined "CSharpExpr"]
              RightArguments = [Defined "CSharpExpr"]
              Priority = 10
              Class = Defined "CSharpExpr" }
            { Name = "<="
              GenericArguments = []
              LeftArguments = [Defined "CSharpExpr"]
              RightArguments = [Defined "CSharpExpr"]
              Priority = 10
              Class = Defined "CSharpExpr" }
            { Name = "=="
              GenericArguments = []
              LeftArguments = [Defined "CSharpExpr"]
              RightArguments = [Defined "CSharpExpr"]
              Priority = 10
              Class = Defined "CSharpExpr" }
            { Name = "!="
              GenericArguments = []
              LeftArguments = [Defined "CSharpExpr"]
              RightArguments = [Defined "CSharpExpr"]
              Priority = 10
              Class = Defined "CSharpExpr" }
            { Name = "/"
              GenericArguments = []
              LeftArguments = [Defined "CSharpExpr"]
              RightArguments = [Defined "CSharpExpr"]
              Priority = 100
              Class = Defined "CSharpExpr" }
            { Name = "*"
              GenericArguments = []
              LeftArguments = [Defined "CSharpExpr"]
              RightArguments = [Defined "CSharpExpr"]
              Priority = 100
              Class = Defined "CSharpExpr" }
            { Name = "%"
              GenericArguments = []
              LeftArguments = [Defined "CSharpExpr"]
              RightArguments = [Defined "CSharpExpr"]
              Priority = 100
              Class = Defined "CSharpExpr" }
            { Name = "+"
              GenericArguments = []
              LeftArguments = [Defined "CSharpExpr"]
              RightArguments = [Defined "CSharpExpr"]
              Priority = 100
              Class = Defined "CSharpExpr" }
            { Name = "-"
              GenericArguments = []
              LeftArguments = [Defined "CSharpExpr"]
              RightArguments = [Defined "CSharpExpr"]
              Priority = 100
              Class = Defined "CSharpExpr" }
            { Name = "."
              GenericArguments = []
              LeftArguments = [Defined "CSharpExpr"]
              RightArguments = [Defined "CSharpExpr"]
              Priority = 1000
              Class = Defined "CSharpExpr" }
          ]
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

and KeywordArgument = Native of string | Defined of string | Generic of string * List<KeywordArgument>
  with static member Create (e:BasicExpression<Keyword,Var,Literal,_,_>) =
        match e with
        | Extension(arg,_,_) -> Defined arg.Name
        | Application(Angle, [Extension(arg,_,_)], _, _) -> Native arg.Name
        | Application(Angle, [arg], _, _) -> arg |> KeywordArgument.Create
        | Application(Angle, args, _, _) -> 
          let rec printSequence =
            function
            | Keyword(Custom k,_,_) :: es -> k + printSequence es
            | Extension(arg,_,_) :: es -> arg.Name + printSequence es
            | [] -> ""
            | es -> failwithf "Cannot match argument %A" es
          let res = args |> printSequence
          res |> Native 
        | _ -> failwithf "Cannot match argument %A" e
       member this.Contains tgt = 
         if this = tgt then true
         else
           match this with 
           | Native a -> false
           | Defined a -> false
           | Generic(a,args) -> 
             args |> Seq.exists (fun a -> a.Contains tgt)
       member this.BaseName =
         match this with 
         | Native a -> a 
         | Defined a -> a 
         | Generic(a,args) -> a
       member this.Argument = 
         match this with 
         | Native a -> a 
         | Defined a -> a 
         | Generic(a,args) -> 
           sprintf "%s[%s]" a (args |> Seq.map (fun a -> a.Argument) |> Seq.reduce (fun s x -> sprintf "%s, %s" s x))
       member this.ArgumentCSharpStyle cleanup = 
         match this with 
         | Native a -> a 
         | Defined a -> cleanup a 
         | Generic(a,args) -> 
           sprintf "%s<%s>" (cleanup a) (args |> Seq.map (fun a -> a.ArgumentCSharpStyle cleanup) |> Seq.reduce (fun s x -> sprintf "%s, %s" s x))

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


module ConcreteExpressionParser

#nowarn "40"

open Utilities
open ParserMonad
open BasicExpression

let mutable debug_expr = false
let mutable debug_rules = false

type KeywordArgument = Native of string | Defined of string
  with member this.Argument = match this with | Native a -> a | Defined a -> a

type CustomKeyword = { Name : string; LeftArguments : List<KeywordArgument>; RightArguments : List<KeywordArgument>; Priority : int; Class : string }
  with member this.LeftAriety = this.LeftArguments.Length
       member this.RightAriety = this.RightArguments.Length
       member this.Arguments = this.LeftArguments @ this.RightArguments

type ConcreteExpressionContext = 
  {
    CustomKeywords : List<CustomKeyword>
    CustomKeywordsByPrefix : List<CustomKeyword>
    CustomKeywordsMap : Map<string,CustomKeyword>    
    InheritanceRelationships : List<string * string>
  } with
      static member Empty =
        {
          CustomKeywords           = []
          CustomKeywordsByPrefix   = []
          CustomKeywordsMap        = Map.empty
          InheritanceRelationships = []
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

type Var = { Name : string }
  with 
    override this.ToString() =
      this.Name

type Keyword = Sequence | Equals | NotEquals | DoubleArrow | FractionLine | Nesting | Custom of name : string
  with 
    override this.ToString() =
      match this with
      | Sequence -> ""
      | Equals -> "=="
      | NotEquals -> "!="
      | DoubleArrow -> "=>"
      | FractionLine -> "\n-------------------\n"
      | Nesting -> ""
      | Custom name -> name

type Literal = StringLiteral of string | IntLiteral of int | BoolLiteral of bool | SingleLiteral of float32 | DoubleLiteral of float
  with 
    override this.ToString() =
      match this with
      | StringLiteral l -> sprintf "\"%s\"" l  
      | IntLiteral i -> sprintf "%d" i
      | BoolLiteral b -> sprintf "%b" b
      | SingleLiteral s -> sprintf "%ff" s
      | DoubleLiteral d -> sprintf "%f" d

let rec program() = 
  p{
    let! ks = keywords
    let newContext = 
          {
            CustomKeywords = ks
            CustomKeywordsByPrefix = ks |> List.sortBy (fun k -> k.Name) |> List.rev
            CustomKeywordsMap = ks |> Seq.map (fun x -> x.Name, x) |> Map.ofSeq
            InheritanceRelationships = []
          }
    do! setContext newContext
    let! inheritance = inheritanceRelationships()
    let newContext = { newContext with InheritanceRelationships = inheritance }
    do! setContext newContext
    let! rs = rules 0
    return Application(Regular, Keyword Sequence :: rs)
  }

and inheritanceRelationships() =
  let singleRelation() =
    p{
      let! concreter = customClass()
      let! bs = blank_space()
      let! inherits = word "inherits"
      let! bs = blank_space()
      let! abstracter = customClass()
      let! bs = blank_space()
      let! el = empty_lines()
      return (new System.String(concreter |> Seq.toArray), new System.String(abstracter |> Seq.toArray))
    }
  p{
    let! r1 = singleRelation() + p { return () }
    match r1 with
    | First r ->
      let! rest = inheritanceRelationships()
      return r :: rest
    | _ -> return []
  }

and keywords =
  p{
    let! k = keyword + p{ return () }
    match k with
    | First k -> 
      let! l = empty_lines()
      let! ks = keywords
      return k :: ks
    | Second () -> 
      return []
  }

and keyword : Parser<CustomKeyword,ConcreteExpressionContext> = 
  let equals = 
    p{
      let! bs = blank_space()
      let! eqk = character '='
      let! bs = blank_space()
      return ()
    }
  let label l = 
    p{
      let! bs = blank_space()
      let! l = word l
      let! bs = blank_space()
      return ()
    }
  let rec identifiers() =
    p{
      let! ob = word "<<" + p{ return () }
      let! id = longIdentifier() + p{ return () }
      let! cb = word ">>" + p{ return () }
      match id with
      | First id ->
        let! bs = blank_space()
        let! ids = identifiers()
        match ob,cb with
        | First _, First _ -> 
          return Native id :: ids
        | _ -> 
          return Defined id :: ids        
      | Second _ -> 
        return []
    }
  p{
    do! label "Keyword"
    do! equals
    do! label "\""
    let! bs = blank_space()
    let! name = takeWhile' ((<>) '\"')
    do! label "\""
    do! label "LeftArguments"
    do! equals
    do! label "["
    let! leftArguments = identifiers()
    do! label "]"
    do! label "RightArguments"
    do! equals
    do! label "["
    let! rightArguments = identifiers()
    do! label "]"
    do! label "Priority"
    do! equals
    let! priority = intLiteral()
    do! label "Class"
    do! equals
    do! label "\""
    let! className = takeWhile' ((<>) '\"')
    do! label "\""
    return { Name = new System.String(name |> Seq.toArray); LeftArguments = leftArguments; RightArguments = rightArguments; Priority = priority; Class = new System.String(className |> Seq.toArray) }
  }

and rules depth = 
  p{
    let! r = rule depth
    let! rs = rules depth + end_rules depth
    match rs with
    | First(rs1) ->       
      return r::rs1
    | Second(_) ->
      return [r]
  }

and rule depth =
  p{
    let! cs = clauses depth
    let! nl1 = newline()
    let! m = clause depth
    let! nl2 = newline()
    do! empty_lines()
    let! depth1 = !!indentation
    if depth1 > depth then
      let! nested_rules = rules depth1
      return Application(Bracket.Regular, Keyword Nesting :: Application(Bracket.Regular, Keyword FractionLine :: m :: cs) :: nested_rules)
    else
      return Application(Bracket.Regular, Keyword FractionLine :: m :: cs)
  }

and end_rules depth =
  p{
    let! depth1 = !!indentation
    if depth1 < depth then
      return []
    else
      return! fail()
  } + eof()

and clauses depth = 
  let rec clauses = 
    p{
      let! c = clause depth
      do! empty_line()
      let! cs = clauses + end_clauses depth
      match cs with
      | First cs -> return c::cs
      | _ -> return [c]
    }
  p{
    let! cs = clauses + end_clauses depth
    match cs with
    | First(cs) ->
      return cs
    | _ -> 
      return []
  }

and clause depth =
  p{
    do! require_indentation depth
    let! i = expr()
    let! bs2 = blank_space()
    let! ar = word "=>" + (word "==" + word "!=")
    let! bs3 = blank_space()
    let! o = expr()
    if debug_rules then
      do printfn "%A => %A" i o
    let! bs4 = blank_space()
    match ar with
    | First _ ->
      return Application(Bracket.Regular, Keyword DoubleArrow :: i :: o :: [])
    | Second(First _) ->
      return Application(Bracket.Regular, Keyword Equals :: i :: o :: [])
    | _ ->
      return Application(Bracket.Regular, Keyword NotEquals :: i :: o :: [])
  }

and customClass() =
  let rec customClass = 
    function 
    | [] -> fail()
    | (k : string) :: ks ->
      p{
        let! r = word k + customClass ks
        match r with 
        | First w -> return w
        | Second w' -> return w'
      }
  p{
    let! customKeywordsByPrefix = getCustomKeywordsByPrefix()
    return! customClass [ for k in customKeywordsByPrefix -> k.Class ]
  }

and customKeyword() =
  let rec customKeyword = 
    function 
    | [] -> fail()
    | (k : CustomKeyword) :: ks ->
      p{
        let! r = word k.Name + customKeyword ks
        match r with 
        | First w -> return w
        | Second w' -> return w'
      }
  p{
    let! customKeywordsByPrefix = getCustomKeywordsByPrefix()
    return! customKeyword customKeywordsByPrefix
  }

and literal() =
  let stringLiteral() = 
    p{
      let! oq = word @"""" + p{ return () }
      let! id = longIdentifier()
      let! cq = word @"""" + p{ return () }
      return id
    }
  let boolLiteral() =
    p{
      let! res = word "true" + word "false"
      match res with
      | First _ -> return true
      | Second _ -> return false
    }
  p{
    let! res = (intLiteral() + floatLiteral()) + (stringLiteral() + boolLiteral())
    match res with 
    | First(First i) -> return IntLiteral i
    | First(Second f) -> return DoubleLiteral f
    | Second(First s) -> return StringLiteral s
    | Second(Second b) -> return BoolLiteral b
  }

and expr() = 
  let shrink bracket_type (es:List<BasicExpression<_,_,_>>) customKeywordsMap : BasicExpression<_,_,_> =
    let ariety (b:BasicExpression<_,_,_>) = 
      match b with
      | Keyword(Custom(k)) ->
        if customKeywordsMap |> Map.containsKey k then
          let (kw:CustomKeyword) = customKeywordsMap.[k]
          kw.LeftAriety, kw.RightAriety
        else
          0,0
      | _ -> 0,0
    let priority (b:BasicExpression<_,_,_>,index:int) = 
      match b with
      | Keyword(Custom(k)) ->
        if customKeywordsMap |> Map.containsKey k then
          let kw = customKeywordsMap.[k]
          kw.Priority,-index
        else
          -1,-index
      | _ -> -1,-index

    let merge (n,i) l r =
      Application(Regular, n :: (l |> List.map fst) @ (r |> List.map fst)), i
    let prioritize_es = BottomUpPriorityParser.prioritize es ariety priority merge
    if debug_expr then
      do printfn "%A" es
      do printfn "is prioritized into %A" prioritize_es
      do printfn ""
      let _ = System.Console.ReadLine()
      ()
    match prioritize_es with
    | [x] -> x
    | l -> Application(bracket_type, l)
  let rec base_expr bracket_type = 
    p{
      let! e = inner_expr
      match e with
      | e::es ->
        let! customKeywordsMap = getCustomKeywordsMap()
        return shrink bracket_type (e :: es) customKeywordsMap
      | _ -> return failwith "Unsupported zero-length expression"
    }
  and inner_expr =
    p{
      let! end_line = !!(word "--") + p{ return () }
      match end_line with
      | First _ -> return! fail()
      | Second _ ->
        let open_bracket = (character '(' + character '[') + (character '{' + word "<<")
        let closed_bracket = (character ')' + character ']') + (character '}' + word ">>")
        let! e = (open_bracket + !!closed_bracket) + (customKeyword() + (identifier() + literal()))
        match e with
        | First(First(actual_open_bracket)) -> 
          let extracted_open_bracket = actual_open_bracket.Fold (fun x -> x.Fold id id) (fun x -> x.Fold id (fun _ -> '≪'))
          let! bs = blank_space()
          let! i_e = base_expr (Bracket.FromChar extracted_open_bracket)
          let! bs = blank_space()
          let! actual_closed_bracket = closed_bracket
          let extracted_closed_bracket = actual_closed_bracket.Fold (fun x -> x.Fold id id) (fun x -> x.Fold id (fun _ -> '≫'))
          if matching_brackets extracted_open_bracket extracted_closed_bracket then 
            let! bs = blank_space()
            let! r_es = maybe_inner_expr
            return i_e :: r_es
          else
            return! fail()
        | First(Second(closed_bracket)) -> 
          return []
        | Second(e) ->
          let be = 
            match e with
            | First(k) -> Keyword(Custom(new System.String(k |> Seq.toArray)))
            | Second(First(id)) -> Extension { Var.Name = new System.String(id |> Seq.toArray) }
            | Second(Second(l)) -> Imported(l)
          let! bs = blank_space()
          let! bes = maybe_inner_expr
          return be::bes
    }
  and maybe_inner_expr = 
    p{
      let! es = inner_expr + (!!newline() + !!(word "=>" + word "==" + word "!="))
      match es with
      | First bes -> return bes
      | _ -> return []
    }
  base_expr Bracket.Regular  

and end_clauses depth =
  p{
    do! require_indentation depth
    let! fl1 = character '-'
    let! fl2 = character '-'
    let! fl = takeWhile (character '-')
    do! !!empty_line()
    return ([] : unit list)
  }

and indentation =
  p{
    let! depth = blank_space()
    return depth |> List.length
  }

and require_indentation depth =
  p{
    let! bs = blank_space()
    let depth1 = bs |> List.length
    if depth = depth1 then
      return()
    else
      return! fail()
  }

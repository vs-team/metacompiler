module ConcreteExpressionParser

#nowarn "40"

open Utilities
open ParserMonad
open BasicExpression

type CustomKeyword = { Name : string; LeftAriety : int; RightAriety : int; Priority : int }

type ConcreteExpressionContext = 
  {
    CustomKeywords : List<CustomKeyword>
    CustomKeywordsByPrefix : List<CustomKeyword>
    CustomKeywordsMap : Map<string,CustomKeyword>    
  } with
      static member Empty =
        {
          CustomKeywords          = []
          CustomKeywordsByPrefix  = []
          CustomKeywordsMap       = Map.empty
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

type Keyword = Sequence | DoubleArrow | FractionLine | Nesting | Custom of name : string
  with 
    override this.ToString() =
      match this with
      | Sequence -> ""
      | DoubleArrow -> "=>"
      | FractionLine -> "\n-------------------\n"
      | Nesting -> ""
      | Custom name -> name

let rec program() = 
  p{
    let! ks = keywords
    do! setContext
          {
            CustomKeywords = ks
            CustomKeywordsByPrefix = ks |> List.sortBy (fun k -> k.Name) |> List.rev
            CustomKeywordsMap = ks |> Seq.map (fun x -> x.Name, x) |> Map.ofSeq
          }
    let! rs = rules 0
    return Application(Regular, Keyword Sequence :: rs)
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
  p{
    do! label "Keyword"
    do! equals
    do! label "\""
    let! bs = blank_space()
    let! name = takeWhile' ((<>) '\"')
    do! label "\""
    do! label "LeftAriety"
    do! equals
    let! leftAriety = intLiteral()
    do! label "RightAriety"
    do! equals
    let! rightAriety = intLiteral()
    do! label "Priority"
    do! equals
    let! priority = intLiteral()
    return { Name = new System.String(name |> Seq.toArray); LeftAriety = leftAriety; RightAriety = rightAriety; Priority = priority }
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
    let! ar = word "=>"
    let! bs3 = blank_space()
    let! o = expr()
    let! bs4 = blank_space()
    return Application(Bracket.Regular, Keyword DoubleArrow :: i :: o :: [])
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

and expr() = 
  let shrink bracket_type (es:List<BasicExpression<_,_,_>>) customKeywordsMap : BasicExpression<_,_,_> =
    let ariety (b:BasicExpression<_,_,_>) = 
      match b with
      | Keyword(Custom(k)) ->
        if customKeywordsMap |> Map.containsKey k then
          let kw = customKeywordsMap.[k]
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

    let merge n l r =
      Application(Regular, n :: l @ r)
    let prioritize_es = BottomUpPriorityParser.prioritize es ariety priority merge
//    do printfn "%A" es
//    do printfn "is prioritized into %A" prioritize_es
//    do printfn ""
//    let _ = System.Console.ReadLine()
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
        let! e = (open_bracket + !!closed_bracket) + (customKeyword() + identifier())
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
            | Second(id) -> Extension { Var.Name = new System.String(id |> Seq.toArray) }
          let! bs = blank_space()
          let! bes = maybe_inner_expr
          return be::bes
    }
  and maybe_inner_expr = 
    p{
      let! es = inner_expr + (!!newline() + !!(word "=>"))
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

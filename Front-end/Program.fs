#nowarn "40"

open Utilities
open ParserMonad

type Var = { Name : string }
  with 
    override this.ToString() =
      this.Name

type Bracket = Square | Curly | Angle | Regular
  with 
    static member FromChar = 
      function
      | '(' -> Regular
      | '[' -> Square
      | '{' -> Curly
      | '≪' -> Angle
      | c -> failwithf "Unsupported bracket type %A" c

type Keyword = Sequence | DoubleArrow | FractionLine | Nesting | Custom of name : string
  with 
    override this.ToString() =
      match this with
      | Sequence -> ""
      | DoubleArrow -> "=>"
      | FractionLine -> "\n-------------------\n"
      | Nesting -> ""
      | Custom name -> name

type BasicExpression<'k, 'e, 'i> =
  | Keyword of 'k
  | Application of Bracket * List<BasicExpression<'k, 'e, 'i>>
  | Imported of 'i
  | Extension of 'e
  with 
    override this.ToString() =
      match this with
      | Keyword k -> k.ToString()
      | Application(b,l) -> 
        let ls = l |> Seq.fold (fun s x -> s + " " + x.ToString()) ""
        match b with
        | Regular -> "(" + ls + ")"
        | Square -> "[" + ls + "]"
        | Curly -> "{" + ls + "}"
        | Angle -> "<<" + ls + ">>"
      | Imported i -> i.ToString()
      | Extension e -> e.ToString()


let rec program() = 
  p{
    let! rs = rules 0
    return Application(Regular, Keyword Sequence :: rs)
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
    let! nl1 = newline
    let! m = clause depth
    let! nl2 = newline
    do! empty_lines
    let! depth1 = !!indentation
    if depth1 > depth then
      let! nested_rules = rules depth1
      return Application(Bracket.Regular, Keyword Nesting :: Application(Bracket.Regular, Keyword FractionLine :: m :: cs) :: nested_rules)
    else
      return Application(Bracket.Regular, Keyword FractionLine :: m::cs)
  }

and end_rules depth =
  p{
    let! depth1 = !!indentation
    if depth1 < depth then
      return []
    else
      return! fail()
  } + eof

and clauses depth = 
  let rec clauses = 
    p{
      let! c = clause depth
      do! empty_line
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
    let! bs2 = blank_space
    let! ar = word "=>"
    let! bs3 = blank_space
    let! o = expr()
    let! bs4 = blank_space
    return Application(Bracket.Regular, Keyword DoubleArrow :: i :: o :: [])
  }

and operator =
  p{
    let! op = ((character ',' + character ';') + (word "->" + character '+')) + ((character '-' + character '*') + (character '=' + character '/'))
    let! bs = !!(character ' ' + newline)
    let singleton x = [x]
    let extracted_op = op.Fold (fun x -> x.Fold (fun x -> x.Fold singleton singleton) (fun x -> x.Fold id singleton))
                               (fun x -> x.Fold (fun x -> x.Fold singleton singleton) (fun x -> x.Fold singleton singleton))
    return extracted_op
  }

and split_by_priority (es:BasicExpression<_,_,_>[]) (l:int,u:int) (keywords_by_priority:List<int>) =
  if l > u then
    Application(Regular, es.[l .. u] |> Array.toList)
  elif l = u then
    es.[l]
  else
    match keywords_by_priority with
    | [] -> Application(Regular, es.[l .. u] |> Array.toList)
    | k::ks -> 
      let ks_first,ks_second = ks |> List.partition (fun x -> x < k-1)
      let es_first = split_by_priority es (l,k-1) ks_first
      let es_second = split_by_priority es (k+1,u) ks_second
      Application(Regular, es.[k] :: es_first :: es_second :: [])

and expr() = 
  let shrink bracket_type (es:List<BasicExpression<_,_,_>>) : BasicExpression<_,_,_> =
    Application(bracket_type, es) // "Should use keyword priority"
  let rec base_expr bracket_type = 
    p{
      let! e = inner_expr
      match e with 
      | e::es ->
        return shrink bracket_type (e :: es)
      | _ -> return failwith "Unsupported zero-length expression"
    }
  and inner_expr = 
    p{
      let open_bracket = (character '(' + character '[') + (character '{' + word "<<")
      let closed_bracket = (character ')' + character ']') + (character '}' + word ">>")
      let! e = (open_bracket + !!closed_bracket) + (identifier + operator)
      match e with
      | First(First(actual_open_bracket)) -> 
        let extracted_open_bracket = actual_open_bracket.Fold (fun x -> x.Fold id id) (fun x -> x.Fold id (fun _ -> '≪'))
        let! bs = blank_space
        let! i_e = base_expr (Bracket.FromChar extracted_open_bracket)
        let! bs = blank_space
        let! actual_closed_bracket = closed_bracket
        let extracted_closed_bracket = actual_closed_bracket.Fold (fun x -> x.Fold id id) (fun x -> x.Fold id (fun _ -> '≫'))
        if matching_brackets extracted_open_bracket extracted_closed_bracket then 
          let! bs = blank_space
          let! r_es = maybe_inner_expr
          return i_e :: r_es
        else
          return! fail()
      | First(Second(closed_bracket)) -> 
        return []
      | Second(e) ->
        let be = 
          match e with
          | First(id) -> Extension { Var.Name = new System.String(id |> Seq.toArray) }
          | Second(op) -> Extension { Var.Name = new System.String(op |> Seq.toArray) }
        let! bs = blank_space
        let! bes = maybe_inner_expr
        return be::bes
    }
  and maybe_inner_expr = 
    p{
      let! es = inner_expr + (!!newline + !!(word "=>"))
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
    do! !!empty_line
    return ([] : unit list)
  }

and indentation =
  p{
    let! depth = blank_space
    return depth |> List.length
  }

and require_indentation depth =
  p{
    let! bs = blank_space
    let depth1 = bs |> List.length
    if depth = depth1 then
      return()
    else
      return! fail()
  }


[<EntryPoint>]
let main argv = 
  let ($) p a = p.Parse a

  let input = System.IO.File.ReadAllText @"Content\casanova semantics.mc"

  let output = (program()).Parse (input |> Seq.toList)
  match output with
  | [] -> printfn "Parse error."
  | x::xs -> printfn "%s" (x.ToString())
  0

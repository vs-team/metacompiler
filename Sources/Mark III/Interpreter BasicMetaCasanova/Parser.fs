module Parser

open ParserMonad
open Common
open Lexer

type Keyword = 
  | Func | Data | DoubleArrow | HorizontalBar | SingleArrow | NewLine | Class | Instance

type BasicExpression =
  | Id of Id * Position
  | Keyword of Keyword * Position
  | Literal of Literal * Position
  | Application of Bracket * List<BasicExpression>

let getPosition = 
  fun (tokens,ctxt) ->
    Done(Lexer.tryGetNextPosition tokens, tokens, ctxt)

let read_keyword =
  fun (tokens,ctxt) ->
    match tokens with
    | (Lexer.Keyword(t, _))::ts -> Done(t,ts,ctxt)
    | _ -> Error(sprintf "Error: expected keyword at %A" (Lexer.tryGetNextPosition tokens))

let read_id =
  fun (tokens,ctxt) ->
    match tokens with
    | (Lexer.Id(i, _))::ts -> Done(i,ts,ctxt)
    | _ -> Error(sprintf "Error: expected id at %A" (Lexer.tryGetNextPosition tokens))
  
let read_literal =
  fun (tokens,ctxt) ->
    match tokens with
    | (Lexer.Literal(l, _))::ts -> Done(l,ts,ctxt)
    | _ -> Error(sprintf "Error: expected literal at %A" (Lexer.tryGetNextPosition tokens))

let matching_bracket matches (b:Bracket) : Parser<Token, _, Unit> =
  prs{
    let! t = read_keyword
    if matches t b then
      return ()
    else
      let! pos = getPosition
      return! fail (sprintf "Error: expected closed bracket %A at %A" b pos)
  }

let close_bracket (b:Bracket) : Parser<Token, _, Unit> =
  let matches t b = 
    match t with
    | Close b' when b = b' -> true
    | _ -> false
  matching_bracket matches b

let open_bracket (b:Bracket) : Parser<Token, _, Unit> =
  let matches t b = 
    match t with
    | Open b' when b = b' -> true
    | _ -> false
  matching_bracket matches b

let convert_token : Parser<Token, _, BasicExpression> =
  prs{
    let! pos = getPosition
    let! k = read_keyword
    match k with
    | Lexer.Class -> return Keyword(Class,pos)
    | Lexer.Instance -> return Keyword(Instance,pos)
    | Lexer.Func -> return Keyword(Func,pos)
    | Lexer.Data -> return Keyword(Data,pos)
    | Lexer.DoubleArrow -> return Keyword(DoubleArrow,pos)
    | Lexer.HorizontalBar -> return Keyword(HorizontalBar,pos)
    | Lexer.SingleArrow -> return Keyword(SingleArrow,pos)
    | Lexer.NewLine -> return Keyword(NewLine,pos)
    | _ -> return! fail (sprintf "Error: expected keyword at %A." pos)
  } .||
  prs{
    let! pos = getPosition
    let! i = read_id
    return Id(i,pos)
  } .||
  prs{
    let! pos = getPosition
    let! l = read_literal
    return Literal(l,pos)
  }

let spaces : Parser<_,Unit,_> =
  prs{
    let! t = read_keyword
    match t with
    | Spaces n -> return n
    | _ -> return 0
  } .|| (prs{ return 0 })

#nowarn "40"
let rec skip_spaces : Parser<_,Unit,_> =
  prs{
    let! t = lookahead read_keyword
    match t with
    | Spaces n -> 
      let! _ = read_keyword // commit
      return! skip_spaces
    | _ -> return ()
  } .|| (prs{ return () })

let rec open_close_bracket bracket = 
  prs{
    do! open_bracket bracket
    let! args = traverse()
    do! skip_spaces
    do! close_bracket bracket
    let! rest = traverse()
    return Application(bracket, args) :: rest
  }

and traverse() : Parser<Token, _, List<BasicExpression>> =
  prs{
    do! skip_spaces
    return!
      ((open_close_bracket Curly)
      .|| (open_close_bracket Round)
      .|| (open_close_bracket Square)
      .|| (open_close_bracket Indent))
      .|| (nothing >>
            prs{
                return! 
                  (eof >> prs{ return [] }) .||
                  (lookahead(close_bracket Indent .|| close_bracket Round .|| close_bracket Square .|| close_bracket Curly) >> prs{ return [] }) .||
                  (prs{
                    let! hd = convert_token
                    let! tl = traverse()
                    return hd :: tl })
              })
  }

let parenthesize =
  let regular_load path tokens = 
    match traverse() (tokens,()) with
    | Done(parenthesization,_,_) -> Some parenthesization
    | Error(e) ->
      printfn "%A" e
      None
  Caching.cached_op regular_load 

module Parser

open ParserMonad
open Common
open Lexer

type Keyword = 
  | Import | Inherit | Func | TypeFunc | ArrowFunc 
  | Data | HorizontalBar | SingleArrow | DoubleArrow 
  | PriorityArrow | NewLine | Instance | CommentLine

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
    | _ -> Error(ParserError (Lexer.tryGetNextPosition tokens))

let read_id =
  fun (tokens,ctxt) ->
    match tokens with
    | (Lexer.Id(i, _))::ts -> Done(i,ts,ctxt)
    | _ -> Error(ParserError (Lexer.tryGetNextPosition tokens))
  
let read_literal =
  fun (tokens,ctxt) ->
    match tokens with
    | (Lexer.Literal(l, _))::ts -> Done(l,ts,ctxt)
    | _ -> Error(ParserError (Lexer.tryGetNextPosition tokens))

let matching_bracket matches (b:Bracket) : Parser<Token, _, Unit> =
  prs{
    let! t = read_keyword
    if matches t b then
      return ()
    else
      let! pos = getPosition
      return! fail (ParserError Position.Zero)
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
    | Lexer.Instance -> return Keyword(Instance,pos)
    | Lexer.Import -> return Keyword(Import,pos)
    | Lexer.Inherit -> return Keyword(Inherit,pos)
    | Lexer.Func -> return Keyword(Func,pos)
    | Lexer.TypeFunc -> return Keyword(TypeFunc,pos)
    | Lexer.ArrowFunc -> return Keyword(ArrowFunc,pos)
    | Lexer.Data -> return Keyword(Data,pos)
    | Lexer.HorizontalBar -> return Keyword(HorizontalBar,pos)
    | Lexer.SingleArrow -> return Keyword(SingleArrow,pos)
    | Lexer.DoubleArrow -> return Keyword(DoubleArrow,pos)
    | Lexer.PriorityArrow -> return Keyword(PriorityArrow,pos)
    | Lexer.NewLine -> return Keyword(NewLine,pos)
    | Lexer.CommentLine -> return Keyword(CommentLine,pos)
    | _ -> return! fail (ParserError Position.Zero)
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

let convert_bracket bracket =
  match bracket with
  | Lambda -> Round
  | _ -> bracket

let rec open_close_bracket bracket = 
  prs{
    do! open_bracket bracket
    let! args = traverse()
    do! skip_spaces
    do! close_bracket (convert_bracket bracket)
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
      .|| (open_close_bracket Lambda)
      .|| (open_close_bracket Indent)
      .|| (open_close_bracket Comment))
      .|| (nothing >>
            prs{
                return! 
                  (eof >> prs{ return [] }) .||
                  (lookahead(close_bracket Indent .|| 
                             close_bracket Round  .|| 
                             close_bracket Square .||
                             close_bracket Comment .|| 
                             close_bracket Curly) >> prs{ return [] }) .||
                  (prs{
                    let! hd = convert_token
                    let! tl = traverse()
                    return hd :: tl })
              })
  }

let parse =
  let regular_load path tokens = 
    match traverse() (tokens,()) with
    | Done(parsing,_,_) -> Some parsing
    | Error(p) ->
      printfn "%A" (p)
      None
  Caching.cached_op regular_load 

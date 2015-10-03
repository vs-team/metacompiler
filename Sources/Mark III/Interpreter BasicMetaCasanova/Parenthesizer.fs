module Parenthesizer

open ParserMonad
open Common
open Lexer

type Bracket = Curly | Round | Square | Indent | Implicit

type Keyword = 
  | Func | Data | DoubleArrow | HorizontalBar 
  | SingleArrow | DoubleColon

type BasicExpression =
  | Id of Id * Position
  | Keyword of Keyword * Position
  | Literal of Literal * Position
  | Application of Bracket * List<BasicExpression>

type Context = { IndentationDepth : int }
  with static member Zero = { IndentationDepth = 0 }

let close_bracket (b:Bracket) : Parser<Token, _, Unit> =
  let matches t b = 
    match t, b with
    | CloseIndent, Indent
    | CloseCurly, Curly 
    | CloseRound, Round 
    | CloseSquare, Square -> true
    | _ -> false
  fun (tokens,ctxt,pos) ->
    match tokens with
    | (Lexer.Keyword(t, _))::ts when matches t b -> Done((), ts, ctxt, Lexer.tryGetNextPosition ts pos)
    | _ -> Error(sprintf "Error: expected bracket %A" b, pos)

let open_bracket (b:Bracket) : Parser<Token, _, Unit> =
  let matches t b = 
    match t, b with
    | OpenIndent, Indent
    | OpenCurly, Curly 
    | OpenRound, Round 
    | OpenSquare, Square -> true
    | _ -> false
  fun (tokens,ctxt,pos) ->
    match tokens with
    | (Lexer.Keyword(t, _))::ts when matches t b -> Done((), ts, ctxt, Lexer.tryGetNextPosition ts pos)
    | _ -> Error(sprintf "Error: expected bracket %A" b, pos)

let read_token : Parser<Token, _, BasicExpression> =
  fun (tokens,ctxt,pos) ->
    match tokens with
    | (Lexer.Id(i,p))::ts -> Done(Id(i,p), ts, ctxt, Lexer.tryGetNextPosition ts pos)
    | (Lexer.Literal(l,p))::ts -> Done(Literal(l,p), ts, ctxt, Lexer.tryGetNextPosition ts pos)
    | (Lexer.Keyword(Lexer.Func,p))::ts -> Done(Keyword(Func,p), ts, ctxt, Lexer.tryGetNextPosition ts pos)
    | (Lexer.Keyword(Lexer.Data,p))::ts -> Done(Keyword(Data,p), ts, ctxt, Lexer.tryGetNextPosition ts pos)
    | (Lexer.Keyword(Lexer.DoubleArrow,p))::ts -> Done(Keyword(DoubleArrow,p), ts, ctxt, Lexer.tryGetNextPosition ts pos)
    | (Lexer.Keyword(Lexer.HorizontalBar,p))::ts -> Done(Keyword(HorizontalBar,p), ts, ctxt, Lexer.tryGetNextPosition ts pos)
    | (Lexer.Keyword(Lexer.SingleArrow,p))::ts -> Done(Keyword(SingleArrow,p), ts, ctxt, Lexer.tryGetNextPosition ts pos)
    | (Lexer.Keyword(Lexer.DoubleColon,p))::ts -> Done(Keyword(DoubleColon,p), ts, ctxt, Lexer.tryGetNextPosition ts pos)
    | t::ts -> Error(sprintf "Error: unsupported token %A." t, pos)
    | _ -> Error(sprintf "Error: unexpected end of file.", pos)

let spaces =
  fun (tokens,ctxt,pos) ->
    match tokens with
    | (Lexer.Keyword(Spaces n, _))::ts -> Done(n, ts, ctxt, Lexer.tryGetNextPosition ts pos)
    | _ -> Done(0, tokens, ctxt, pos)

let eof = 
  fun (tokens,ctxt,pos) ->
    match tokens with
    | [] -> Done((), [], ctxt, pos)
    | _ -> Error("Error: expected EOF", pos)  

let rec skip_spaces : Parser<Token, _, Unit> =
  fun (tokens,ctxt,pos) ->
    match tokens with
    | (Lexer.Keyword(Lexer.Spaces _,_))::ts ->
      skip_spaces (ts,ctxt,pos)
    | ts -> Done((),tokens,ctxt,Lexer.tryGetNextPosition ts pos)

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
    let! pos = position
    return!
      ((open_close_bracket Curly)
      .|| (open_close_bracket Round)
      .|| (open_close_bracket Square)
      .|| (open_close_bracket Indent))
      .|| (nothing >>
            prs{
                do! skip_spaces
                let! hd = read_token .|. 
                          (lookahead(close_bracket Indent .|. close_bracket Round .|. close_bracket Square .|. close_bracket Curly) .|. eof)
                match hd with
                | A(hd) ->
                  let! tl = traverse()
                  return hd :: tl
                | _ -> // end of file, or closed bracket lookahead
                  return []
              })
  }

let parenthesize (tokens:List<Token>) =
  traverse() (tokens,Context.Zero,Lexer.tryGetNextPosition tokens Position.Zero)

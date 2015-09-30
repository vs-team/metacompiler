module Parenthesizer

open ParserMonad
open Common
open Lexer

type Bracket = Curly | Round | Square | Indent | Implicit

type Keyword = 
  | Func | Data | DoubleArrow | HorizontalBar 
  | SingleArrow | DoubleColon | NewLine

type BasicExpression =
  | Id of Id * Position
  | Keyword of Keyword * Position
  | Literal of Literal * Position
  | Application of Bracket * List<BasicExpression>

let close_bracket (b:Bracket) : Parser<Token, _, Unit> =
  let matches t b = 
    match t, b with
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
    | (Lexer.Keyword(Lexer.NewLine,p))::ts -> Done(Keyword(NewLine,p), ts, ctxt, Lexer.tryGetNextPosition ts pos)
    | t::ts -> Error(sprintf "Error: unsupported token %A." t, pos)
    | _ -> Error(sprintf "Error: unexpected end of file.", pos)

let rec skip_spaces : Parser<Token, _, Unit> =
  fun (tokens,ctxt,pos) ->
    match tokens with
    | (Lexer.Keyword(Lexer.Spaces _,_))::ts ->
      skip_spaces (ts,ctxt,pos)
    | ts -> Done((),tokens,ctxt,Lexer.tryGetNextPosition ts pos)

let rec traverse() : Parser<Token, _, List<BasicExpression>> =
  prs{
    do! skip_spaces
    let! pos = position
    let! opening_bracket = 
      ((open_bracket Indent .|. open_bracket Curly) .|.
       (open_bracket Round .|. open_bracket Square)) .|. 
       nothing
    match opening_bracket with
    | B() -> // we have found no open bracket, so we proceed on this level
      do! skip_spaces
      let! hd = read_token .|. nothing
      match hd with
      | A hd ->
        let! tl = traverse()
        return hd :: tl
      | _ -> // end of file
        return []
    | A open_bracket -> // we have found an open bracket, so we recurse
      let bracket = 
        match open_bracket with
        | A(A()) -> Indent
        | A(B()) -> Curly
        | B(A()) -> Round
        | B(B()) -> Square
      let! args = traverse()
      do! skip_spaces
      // look for a closing bracket
      let! closing_bracket = 
        ((close_bracket Indent .|. close_bracket Curly) .|.
         (close_bracket Round .|. close_bracket Square)) .|. 
         nothing
      match closing_bracket with
      | B() -> // we have found no closing bracket, so we return an error
        return! fail (sprintf "Error: cannot find matching bracket for %A at %A" bracket pos)
      | A close_bracket -> // we have found a closing bracket; open and close brackets must match
        if open_bracket = close_bracket then
          let! rest = traverse()
          return Application(bracket, args) :: rest
        else
          return! fail (sprintf "Error: brackets %A and %A at %A do not match" open_bracket close_bracket pos)
  }

let parenthesize (tokens:List<Token>) =
  traverse() (tokens,(),Lexer.tryGetNextPosition tokens Position.Zero)

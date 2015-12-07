module Parenthesizer

open ParseMonad
open Common

type Expression = 
  | Block of Bracket * List<Expression>
  | Literal of Literal | Id of Id | Keyword of Keyword

type Context = Unit

let openBracket b =
  prs{
    let! t = getSymbol
    match t with
    | Lexer.Open b' ->
      if b = b' then
        return ()
      else
        return! fail ["Expected open bracket"]
    | _ ->
      return! fail ["Expected open bracket"]
  }

let closeBracket b =
  prs{
    let! t = getSymbol
    match t with
    | Lexer.Closed b' ->
      if b = b' then
        return ()
      else
        return! fail ["Expected closed bracket"]
    | _ ->
      return! fail ["Expected closed bracket"]
  }

let blockTerminator : Parse<_,Context,_> = 
  eof .|| (lookahead (closeBracket Curly .|| closeBracket Round))

let literal = 
  prs{
    let! l = getSymbol
    match l with
    | Lexer.Literal l -> return Literal l
    | _ -> return! fail ["Expected literal"]
  }  

let id = 
  prs{
    let! l = getSymbol
    match l with
    | Lexer.Id l -> return Id l
    | _ -> return! fail ["Expected id"]
  }  

let keyword = 
  prs{
    let! l = getSymbol
    match l with
    | Lexer.Keyword l -> return Keyword l
    | _ -> return! fail ["Expected keyword"]
  }  

let rec block b =
  prs{
    do! openBracket b
    let! inner = parenthesize()
    do! closeBracket b
    let! rest = parenthesize()
    return Block(b,inner) :: rest
  }

and parenthesize() = 
  (block Round)
  .|| (block Curly)
  .|| (blockTerminator >> prs{return []})
  .||
    prs{
      let! first = literal .|| id .|| keyword
      let! rest = parenthesize()
      return first :: rest
    }

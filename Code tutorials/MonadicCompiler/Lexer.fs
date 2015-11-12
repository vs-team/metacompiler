module Lexer

open ParseMonad
open Common

type Token = 
  Literal of Literal | Open of Bracket | Closed of Bracket | Id of Id | Keyword of Keyword

type Context = Unit

let digit : Parse<char,Context,char> =
  prs{
    let! c = getSymbol
    if c >= '0' && c <= '9' then
      return c
    else
      return! fail ["Expected digit"]
  }

let alphabetic : Parse<char,Context,char> =
  prs{
    let! c = getSymbol
    if c >= 'A' && c <= 'z' then
      return c
    else
      return! fail ["Expected alphabetic"]
  }

let integer = 
  prs{
    let! d = digit
    let! ds = repeat digit
    let i = int(System.String(Seq.toArray(d::ds)))
    return Literal(Int i)
  }

let identifier = 
  prs{
    let! x = alphabetic
    let! xs = repeat alphabetic
    return Id (System.String(Seq.toArray(x :: xs)))
  }

let word (s:string) =
  prs{
    for x in s do
      let! c = getSymbol
      if c <> x then
        return! fail ["Expected character"]
  }

let token =
  integer 
  .|| (word "/" >> prs{ return Keyword DividedBy})
  .|| (word "+" >> prs{ return Keyword Plus})
  .|| (word "-" >> prs{ return Keyword Minus})
  .|| (word ">" >> prs{ return Keyword Gt})
  .|| (word "=" >> prs{ return Keyword Equals})
  .|| (word ";" >> prs{ return Keyword Semicolon})
  .|| (word "(" >> prs{ return Open Round})
  .|| (word ")" >> prs{ return Closed Round})
  .|| (word "{" >> prs{ return Open Curly})
  .|| (word "}" >> prs{ return Closed Curly})
  .|| (word "if" >> prs{ return Keyword If})
  .|| (word "while" >> prs{ return Keyword While})
  .|| (word "print" >> prs{ return Keyword Print})
  .|| identifier

let skipWhitespace = 
  repeat(
    prs{
      let! c = getSymbol
      if c = ' ' || c = '\t' || c = '\n' || c = '\r' then
        return ()
      else
        return! fail ["Expected whitespace"]
    }
  ) |> ignore

let rec lexer() = 
  prs{
    do! skipWhitespace
    let! first = token
    do! skipWhitespace
    let! rest = lexer()
    return first :: rest
  } .|| (eof >> prs{ return [] })

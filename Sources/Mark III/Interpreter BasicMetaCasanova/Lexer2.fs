module Lexer2
open Common
open ParserMonad

type Keyword = 
  | Import | Inherit | Func | TypeFunc | ArrowFunc | TypeAlias | Data | HorizontalBar | Instance
  | Open of Bracket| Close of Bracket | NewLine | CommentLine
  | SingleArrow | DoubleArrow | PriorityArrow | Spaces of int

type Token =
  | Id of Id * Position
  | Keyword of Keyword * Position
  | Literal of Literal * Position

let increment_col :Parser<_,Position,unit> =
  fun (char,ctxt) -> Done((),char,ctxt.NextChar)

let increment_line :Parser<_,Position,unit> =
  fun (char,ctxt) -> Done((),char,ctxt.NextLine)

let char (expected:char) :Parser<char,Position,Unit> =
  prs{
    let! next_char = step
    if next_char = expected then 
      do! increment_col
      return () 
    else 
      let! ctxt = getContext
      do!  fail (LexerError ctxt)
  }

let char_between (a:char) (b:char) :Parser<char,Position,char> =
  prs{
    let! next_char = step
    if next_char >= a && next_char <= b then 
      do! increment_col
      return next_char
    else 
      let! ctxt = getContext
      do!  fail (LexerError ctxt)
      return ' '
  }

let digit :Parser<char,Position,char> =
  prs{
    let! d = char_between '0' '9'
    return d
  }

let digits :Parser<char,Position,List<char>> =
  prs{
    let! d  = digit
    let! ds = digit |> repeat
    return d::ds
  }

let unsigned_int_literal :Parser<char,Position,int> =
  prs{
    let! d = digits
    let mutable x = 0
    do for i in d do x <- x * 10 + (int i - int '0')
    return x
  }

let int_literal pos =
  prs{
    let! min = char '-' .|. nothing
    let! x = unsigned_int_literal
    match min with
    | A() -> return Literal(Int(-x),pos)
    | B() -> return Literal(Int(x),pos)
  }

let float_literal pos =
  prs{
    let! min = char '-' .|. nothing
    let! i = unsigned_int_literal
    do! char '.'
    let! d = unsigned_int_literal
    let  f = System.Single.Parse(i.ToString() + "." + d.ToString())
    match min with
    | A() -> return Literal(Float32(-f),pos)
    | B() -> return Literal(Float32( f),pos)
  }

let tokenize :Parser<char,Position,List<Token>> = 
  prs{
    return []
  }
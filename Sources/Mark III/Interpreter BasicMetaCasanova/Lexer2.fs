module Lexer2
open Common
open ParserMonad

type Keyword = 
  | Import | Inherit | Func | TypeFunc | ArrowFunc | TypeAlias | Data | HorizontalBar | Instance
  | Open of Bracket| Close of Bracket | NewLine | CommentLine
  | SingleArrow | DoubleArrow | PriorityArrow | Spaces of int
  | Less | LessEqual | Greater | GreaterEqual | Equal

type Token =
  | Id of Id * Position
  | VarId of Id * Position
  | Keyword of Keyword * Position
  | Literal of Literal * Position

let increment_col :Parser<_,Position,unit> =
  fun (char,ctxt) -> Done((),char,ctxt.NextChar)

let increment_line :Parser<_,Position,unit> =
  fun (char,ctxt) -> Done((),char,ctxt.NextLine)

let get_position :Parser<_,Position,Position> =
  fun (char,ctxt) -> Done(ctxt,char,ctxt)

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

let (!) (str:string) :Parser<char,Position,Unit> =
  let rec traverse_from (s:List<char>) : Parser<char,Position,Unit> =
    prs{
      match s with
      | c::cs -> 
        do! char c
        do! traverse_from cs
        return ()
      | [] -> 
        return ()
    }
  traverse_from (str |> Seq.toList)

let skip_comments :Parser<char,Position,unit> =
  let stop (st:System.String) =
    prs{
      let! halt = (!st >>. ret (fail (LexerError Position.Zero))) .|| 
                  ((step |> ignore) >>. ret nothing)
      return! halt
    }
  let start (st:System.String) (sto:System.String) = prs{ do! !st >>. stop sto |> repeat |> ignore} 
  prs{ do! start "$$" "\n"} .|| prs{ do! start "$*" "*$"}

let char_between (a:char) (b:char) :Parser<char,Position,char> =
  prs{
    let! next_char = step
    if next_char >= a && next_char <= b then 
      do! increment_col
      return next_char
    else 
      let! ctxt = getContext
      return!  fail (LexerError ctxt)
  }

let symbol :Parser<char,Position,char> =
  prs{ return! char_between '!' '/' .|| char_between ':' '@' .||
               char_between '[' ']' .|| char_between '_' '`' .||
               char_between '{' '~' 
  } .|| prs { return! char '^' >>. ret '^'}

let symbol_id :Parser<char,Position,System.String> =
  prs{ 
    let! sym = symbol |> repeat1 
    return sym |> Seq.toArray |> string 
  }

let alpha_char :Parser<char,Position,char> =
  prs{ return! char_between 'A' 'Z' .|| char_between 'a' 'z'}

let digit :Parser<char,Position,char> =
  prs{ return! char_between '0' '9'}

let digits :Parser<char,Position,List<char>> =
  prs{ return! digit |> repeat1}

let alpha_numeric :Parser<char,Position,char> =
  prs{ return! digit .|| alpha_char}

let unsigned_int_literal :Parser<char,Position,int> =
  prs{
    let! d = digits
    let mutable x = 0
    do for i in d do x <- x * 10 + (int i - int '0')
    return x
  }

let int_literal pos :Parser<char,Position,Token> =
  prs{
    let! min = char '-' .|. nothing
    let! x = unsigned_int_literal
    match min with
    | A() -> return Literal(Int(-x),pos)
    | B() -> return Literal(Int(x),pos)
  }

let float_literal pos :Parser<char,Position,Token> =
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

let alpha_numeric_id :Parser<char,Position,System.String>=
  prs{
    let! c  = alpha_char
    let! cs = (alpha_numeric) |> repeat
    return (c::cs) |> System.String.Concat
  }

let all_id pos :Parser<char,Position,Token> =
  prs{
    let! str = alpha_numeric_id .|| symbol_id
    return Id((str|>System.String.Concat),pos)
  }

let string_literal pos :Parser<char,Position,Token> =
  prs{
    do! char '\"'
    let! chars = (alpha_numeric .|| symbol) |> repeat
    do! char '\"'
    return (String (chars|>Array.ofList|>System.String.Concat),pos) |> Literal
  }

let horizontal_bar pos :Parser<char,Position,Token> =
  prs{
    do! !"--"
    do! char '-' |> repeat |> ignore
    return Keyword(HorizontalBar,pos)
  }

let skip_spaces :Parser<char,Position,unit>=
  prs{ do! char ' ' |> repeat |> ignore}

let new_line pos :Parser<char,Position,Token> =
  prs{
    do! !"\r\n" .|| !"\n\r" .|| !"\n"
    return (NewLine,pos) |> Keyword
  }

let end_of_file pos :Parser<char,Position,Token> =
  prs{ return! eof >>. ret ((NewLine,pos)|> Keyword)}

let token :Parser<char,Position,Token> =
  prs{
    let (!>>.) s k = s >>. ret (k|>Keyword)
    do! skip_comments
    do! skip_spaces
    let! pos = get_position
    let! res = 
      float_literal   pos .||
      int_literal     pos .||
      string_literal  pos .||
      !>>. !"import" (Import,pos)     .||
      !>>. !"inherit" (Inherit,pos)     .||
      !>>. !"Func" (Func,pos)           .||
      !>>. !"TypeFunc" (TypeFunc,pos)   .||
      !>>. !"ArrowFunc" (ArrowFunc,pos) .||
      !>>. !"TypeAlias" (TypeAlias,pos) .||
      !>>. !"Data" (Data,pos)           .||
      !>>. !"=>" (DoubleArrow,pos)      .||
      !>>. !"->" (SingleArrow,pos)      .||
      !>>. !"#>" (PriorityArrow,pos)    .||
      !>>. !"{" (Open Curly,pos)        .||
      !>>. !"}" (Close Curly,pos)       .||
      !>>. !"[" (Open Square,pos)       .||
      !>>. !"]" (Close Square,pos)      .||
      !>>. !"(\\" (Open Lambda,pos)     .||
      !>>. !"(" (Open Round,pos)        .||
      !>>. !")" (Close Round,pos)       .||
      horizontal_bar pos  .||
      new_line pos        .||
      all_id pos          .||
      end_of_file pos
    return res
  }

let token_lines :Parser<char,Position,List<Token>> =
  prs{ return! token |> itterate}

let tokenize2 (path:string) :Parser<char,Position,List<Token>> = 
  prs{ 
    if System.IO.File.Exists(path) then
      let source = System.IO.File.ReadAllText(path) |> Seq.toList
      let pos = Position.FromPath path
      do! setBuffer source
      do! setContext pos
      let! res = token_lines 
      return res
    else return! (fail (LexerError Position.Zero))
  }
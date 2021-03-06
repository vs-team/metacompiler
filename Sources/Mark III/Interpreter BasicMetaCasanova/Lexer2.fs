﻿module Lexer2
open Common
open ParserMonad

type Keyword = 
  | Import | Using | Inherit | Func | TypeFunc | ArrowFunc | TypeAlias | Data | HorizontalBar | Instance | Is
  | Open of Bracket| Close of Bracket | NewLine | CommentLine
  | SingleArrow | DoubleArrow | PriorityArrow | Spaces of int
  | Predicate of Predicate

type Token =
  | Id of string * Position
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
      do!  fail (ParserError (sprintf "%A" ctxt))
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
      let! halt = (!st >>. ret (fail (ParserError ""))) .|| 
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
      return!  fail (ParserError (sprintf "Lexer: %A" ctxt))
  }

let symbol :Parser<char,Position,char> =
  prs{ return! char_between '!' '!' .|| char_between '#' '/' .|| 
               char_between ':' '@' .|| char_between '[' ']' .|| 
               char_between '_' '`' .|| char_between '{' '~' 
  } .|| prs { return! char '^' >>. ret '^'}

let symbol_id :Parser<char,Position,System.String> =
  prs{ 
    let! sym = symbol |> repeat1 
    return sym |> Seq.toArray |> System.String 
  }

let alpha_char :Parser<char,Position,char> =
  prs{ return! char_between 'A' 'Z' .|| char_between 'a' 'z'}

let digit :Parser<char,Position,char> =
  prs{ return! char_between '0' '9'}

let digits :Parser<char,Position,List<char>> =
  prs{ return! digit |> repeat1}

let alpha_numeric :Parser<char,Position,char> =
  prs{ return! digit .|| alpha_char}

let unsigned_int_literal :Parser<char,Position,int64> =
  prs{
    let! d = digits
    let mutable x = 0L
    do for i in d do x <- x * 10L + int64(int i - int '0')
    return x
  }

let int_literal pos :Parser<char,Position,Token> =
  prs{
    let! min = char '-' .|. nothing
    let! x = unsigned_int_literal
    match min with
    | A() -> return Literal(I64(-x),pos)
    | B() -> return Literal(I64(x),pos)
  }

let float_literal pos :Parser<char,Position,Token> =
  prs{
    let! min = char '-' .|. nothing
    let! i = unsigned_int_literal
    do! char '.'
    let! d = unsigned_int_literal
    let  f = System.Single.Parse(i.ToString() + "." + d.ToString())
    match min with
    | A() -> return Literal(F32(-f),pos)
    | B() -> return Literal(F32( f),pos)
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
    let! pos = get_position
    if   str = "==" then return Keyword(Predicate(Equal),pos)
    elif str = ">=" then return Keyword(Predicate(GreaterEqual),pos)
    elif str = "<=" then return Keyword(Predicate(LessEqual),pos)
    elif str = ">"  then return Keyword(Predicate(Greater),pos)
    elif str = "<"  then return Keyword(Predicate(Less),pos)
    elif str = "!=" then return Keyword(Predicate(NotEqual),pos)
    else return Id((str|>System.String.Concat),pos)
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
    do! increment_line
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
      !>>. !"import"      (Import,pos)         .||
      !>>. !"using"       (Using,pos)          .||
      !>>. !"inherit"     (Inherit,pos)        .||
      !>>. !"Func"        (Func,pos)           .||
      !>>. !"TypeFunc"    (TypeFunc,pos)       .||
      !>>. !"ArrowFunc"   (ArrowFunc,pos)      .||
      !>>. !"TypeAlias"   (TypeAlias,pos)      .||
      !>>. !"Data"        (Data,pos)           .||
      !>>. !"is"          (Is,pos)             .||
      !>>. !"=>"          (DoubleArrow,pos)    .||
      !>>. !"->"          (SingleArrow,pos)    .||
      !>>. !"#>"          (PriorityArrow,pos)  .||
      !>>. !"{"           (Open Curly,pos)     .||
      !>>. !"}"           (Close Curly,pos)    .||
      !>>. !"["           (Open Square,pos)    .||
      !>>. !"]"           (Close Square,pos)   .||
      !>>. !"(\\"         (Open Lambda,pos)    .||
      !>>. !"("           (Open Round,pos)     .||
      !>>. !")"           (Close Round,pos)    .||
      !>>. !"<"           (Open Angle,pos)     .||
      !>>. !">"           (Close Angle,pos)    .||
      horizontal_bar pos  .||
      new_line pos        .||
      all_id pos          .||
      end_of_file pos
    return res
  } .|| prs{
    let! pos = get_position
    let! er = fail (ParserError (sprintf "%A" pos))
    return! er
  }

let rec trim_newlines (tok:List<Token>) : List<Token> =
  match tok with
  | Keyword(NewLine,_)::xs -> trim_newlines xs
  | x -> x

let token_lines :Parser<char,Position,List<Token>> =
  prs{ 
    let! tok = token |> itterate
    let tok = List.rev (Keyword(NewLine,Position.Zero)::(trim_newlines (List.rev tok)))
    return tok
  } 

let tokenize2 (file_path:string) :Parser<char,Position,List<Token>> = 
  prs{ 
      do! setContext (Position.FromPath file_path)
      return! token_lines 
  }
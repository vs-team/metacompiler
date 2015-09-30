module Lexer

open Common
open ParserMonad

type Keyword = 
  | Func | Data | DoubleArrow | HorizontalBar | OpenCurly | CloseCurly
  | OpenSquare | CloseSquare | OpenRound | CloseRound | SingleArrow
  | DoubleColon | NewLine | Spaces of int

type Token =
  | Id of Id * Position
  | Keyword of Keyword * Position
  | Literal of Literal * Position
  with 
    member t.Position =
      match t with
      | Id(_,pos)
      | Keyword(_,pos)
      | Literal(_,pos) -> pos

let tryGetNextPosition (ts:List<Token>) p = 
  match ts with
  | t::_ -> t.Position
  | _ -> p

let char (expected:char) : Parser<char,_,Unit> =
  fun (chars,ctxt,pos) ->
    match chars with
    | c::cs when expected = c -> 
      Done((), cs, ctxt, if c = '\n' then pos.NextLine else pos.NextChar)
    | _ -> Error(sprintf "Unmatched char when %c expected" expected, pos)

let (!) (s:string) : Parser<char,_,Unit> =
  let rec (!!) (s:List<char>) : Parser<char,_,Unit> =
    prs{
      match s with
      | c::cs -> 
        do! char c
        do! !!cs
        return ()
      | [] -> 
        return ()
    }
  !!(s |> Seq.toList)

let digit : Parser<char,_,char> =
  fun (chars,ctxt,pos) ->
    match chars with
    | c::cs when c >= '0' && c <= '9' -> Done(c, cs, ctxt, pos.NextChar)
    | _ -> Error("Error: expected digit.", pos)

let alpha_numeric : Parser<char,_,char> =
  fun (chars,ctxt,pos) ->
    match chars with
    | c::cs when (c >= 'a' && c <= 'z')
                 || (c >= '0' && c <= '9')
                 || (c >= 'A' && c <= 'Z')
                 || (c = '_') -> Done(c, cs, ctxt, pos.NextChar)
    | _ -> Error("Error: expected digit.", pos)

let digits = 
  prs{
    let! d = digit
    let! ds = digit |> repeat
    return d::ds
  }

let int_literal =
  prs{
    let! pos0 = position
    let! ch0 = chars
    let! min = char '-' .|. nothing
    let! pos1 = position
    let! ch1 = chars
    let! digits = digits
    let! pos2 = position
    let! ch2 = chars
    let mutable x = 0
    do for i in digits do x <- x * 10 + (int i - int '0')
    match min with
    | A() -> return -x
    | B() -> return x
  }

let float_literal =
  prs{
    let! i = int_literal
    do! char '.'
    let _ = failwith "Not implemented"
    return 0.0
  }

let any_id =
  prs{
    let! c = alpha_numeric
    let! chars = alpha_numeric |> repeat
    return new System.String((c::chars) |> Seq.toArray)
  }

let string_literal =
  prs{
    do! char '\"'
    let! chars = any_id
    do! char '\"'
    return new System.String(chars |> Seq.toArray)
  }

let horizontal_bar = 
  prs{
    do! char '-'
    do! char '-'
    do! char '-' |> repeat |> ignore
    return ()
  }

let rec spaces =
  prs{
    let! fst = !" "
    let! rest = !" " |> repeat
    return 1 + (rest |> List.length)
  }

let token : Parser<char,Unit,Token> = 
  prs{
    let! pos = position
    let! t = (((int_literal .|. float_literal) .|. (string_literal .|. any_id)) 
              .|. ((!"Func" .|. (!"Data" .|. !"=>")) .|. (!"::" .|. (!"->" .|. horizontal_bar)))) .|.
             (((!"{" .|. !"}") .|. (!"(" .|. !")")) .|. ((!"[" .|. !"]") .|. (!"\r\n" .|. spaces)))
    let! pos1 = position
    match t with
    | A(A(A(A i))) -> return ((i |> Int),pos) |> Literal
    | A(A(A(B x))) -> return ((x |> Float),pos) |> Literal
    | A(A(B(A s))) -> return ((s |> String),pos) |> Literal
    | A(A(B(B s))) -> return (s,pos) |> Id

    | A(B(A(A ()))) -> return (Func,pos) |> Keyword
    | A(B(A(B(A())))) -> return (Data,pos) |> Keyword
    | A(B(A(B(B())))) -> return (DoubleArrow,pos) |> Keyword

    | A(B(B(A ()))) -> return (DoubleColon,pos) |> Keyword
    | A(B(B(B(A())))) -> return (SingleArrow,pos) |> Keyword
    | A(B(B(B(B())))) -> return (HorizontalBar,pos) |> Keyword

    | B(A(A(A()))) -> return (OpenCurly,pos) |> Keyword
    | B(A(A(B()))) -> return (CloseCurly,pos) |> Keyword
    | B(A(B(A()))) -> return (OpenRound,pos) |> Keyword
    | B(A(B(B()))) -> return (CloseRound,pos) |> Keyword
    | B(B(A(A()))) -> return (OpenSquare,pos) |> Keyword
    | B(B(A(B()))) -> return (CloseSquare,pos) |> Keyword
    | B(B(B(A()))) -> return (NewLine,pos) |> Keyword
    | B(B(B(B n))) -> return (Spaces n,pos) |> Keyword
  }

let tokenize (path:string) = //: Result<char,Unit,List<Token>> =
  let source = System.IO.File.ReadAllText(path) |> Seq.toList
  let pos = Position.FromPath path
  match (token |> repeat) (source,(),pos) with
  | Done(tokens, _, _, _) -> Some tokens
  | Error(_,_) -> None

module Lexer

open Common
open ParserMonad

type Context = { IndentationDepth : int; NumIndents : int }
  with static member Zero = { IndentationDepth = 0; NumIndents = 0 }

type Keyword = 
  | Func | Data | DoubleArrow | HorizontalBar | OpenCurly | CloseCurly
  | OpenSquare | CloseSquare | OpenRound | CloseRound | OpenIndent | CloseIndent
  | SingleArrow | DoubleColon | Spaces of int

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

let eof = 
  fun (tokens,ctxt,pos) ->
    match tokens with
    | [] -> Done((), [], ctxt, pos)
    | _ -> Error("Error: expected EOF", pos)  

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

let rec token : Parser<char,Context,Option<Token>> = 
  prs{
    let! pos = position
    let! t = (((int_literal .|. float_literal) .|. (string_literal .|. any_id)) 
              .|. ((!"Func" .|. (!"Data" .|. !"=>")) .|. (!"::" .|. (!"->" .|. horizontal_bar)))) .|.
             (((!"{" .|. !"}") .|. (!"(" .|. !")")) .|. ((!"[" .|. !"]") .|. (!"\r\n" .|. spaces)))
    let! pos1 = position
    match t with
    | A(A(A(A i))) -> return ((i |> Int),pos) |> Literal |> Some
    | A(A(A(B x))) -> return ((x |> Float),pos) |> Literal |> Some
    | A(A(B(A s))) -> return ((s |> String),pos) |> Literal |> Some
    | A(A(B(B s))) -> return (s,pos) |> Id |> Some

    | A(B(A(A ()))) -> return (Func,pos) |> Keyword |> Some
    | A(B(A(B(A())))) -> return (Data,pos) |> Keyword |> Some
    | A(B(A(B(B())))) -> return (DoubleArrow,pos) |> Keyword |> Some

    | A(B(B(A ()))) -> return (DoubleColon,pos) |> Keyword |> Some
    | A(B(B(B(A())))) -> return (SingleArrow,pos) |> Keyword |> Some
    | A(B(B(B(B())))) -> return (HorizontalBar,pos) |> Keyword |> Some

    | B(A(A(A()))) -> return (OpenCurly,pos) |> Keyword |> Some
    | B(A(A(B()))) -> return (CloseCurly,pos) |> Keyword |> Some
    | B(A(B(A()))) -> return (OpenRound,pos) |> Keyword |> Some
    | B(A(B(B()))) -> return (CloseRound,pos) |> Keyword |> Some
    | B(B(A(A()))) -> return (OpenSquare,pos) |> Keyword |> Some
    | B(B(A(B()))) -> return (CloseSquare,pos) |> Keyword |> Some
    | B(B(B(A()))) -> return None
    | B(B(B(B n))) -> return (Spaces n,pos) |> Keyword |> Some
  }

let rec tokens_line() : Parser<char,Context,List<Token>> = 
  prs{
    let! hd = token
    match hd with
    | None -> return []
    | Some hd ->
      let! tl = tokens_line()
      return hd :: tl
  }

let rec tokens_lines() : Parser<char,Context,List<List<Token>>> = 
  prs{
    let! context = getContext
    let! position = position
    let! indentation_depth = 
      spaces .|| (nothing >> prs{ return 0 })
    if context.IndentationDepth = indentation_depth then
      let! line = tokens_line()
      let! rest = tokens_lines()
      return line :: rest
    else
      do! setContext { context with IndentationDepth = indentation_depth }
      let! line = tokens_line()
      match line with
      | [] ->
        do! setContext context
        return! tokens_lines()
      | _ ->
        let! context' = getContext
        let bracket,num_indents =
          if context.IndentationDepth < indentation_depth then // open indentation bracket
            OpenIndent,context'.NumIndents+1
          else // close indentation bracket
            CloseIndent,context'.NumIndents-1
        do! setContext { context' with NumIndents = num_indents }
        let! rest = tokens_lines()
        return (Keyword(bracket,position) :: line) :: rest
  } .|| 
    (eof >> 
      prs{ 
        let! pos = position
        let! ctxt = getContext
        do! setContext { ctxt with NumIndents = 0 }
        return [for i = 1 to ctxt.NumIndents do yield [Keyword(CloseIndent,pos)]] })

let tokenize (path:string) = //: Result<char,Unit,List<Token>> =
  let source = System.IO.File.ReadAllText(path) |> Seq.toList
  let pos = Position.FromPath path
  match (tokens_lines()) (source,Context.Zero,pos) with
  | Done(tokens, _, _, _) -> Some (tokens |> List.concat)
  | Error(e,pos) -> 
    printfn "%A at %A" e pos
    None

module Lexer

open Common
open ParserMonad

type Context = { IndentationDepth : int; NumIndents : int; Position : Position }
  with static member Zero pos = { IndentationDepth = 0; NumIndents = 0; Position = pos }

type Keyword = 
  | Func | Data | DoubleArrow | HorizontalBar
  | Open of Bracket| Close of Bracket | NewLine
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

let tryGetNextPosition (ts:List<Token>) = 
  match ts with
  | t::_ -> t.Position
  | _ -> Position.Zero

let char (expected:char) : Parser<char,_,Unit> =
  fun (chars,ctxt) ->
    match chars with
    | c::cs when expected = c -> 
      Done((), cs, { ctxt with Position = if c = '\n' then ctxt.Position.NextLine else ctxt.Position.NextChar })
    | _ -> Error(sprintf "Unmatched char when %c expected at %A" expected ctxt.Position)

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
  fun (chars,ctxt) ->
    match chars with
    | c::cs when c >= '0' && c <= '9' -> Done(c, cs, { ctxt with Position = ctxt.Position.NextChar })
    | _ -> Error(sprintf "Error: expected digit at %A." ctxt.Position)

let alpha_numeric : Parser<char,_,char> =
  fun (chars,ctxt) ->
    match chars with
    | c::cs when (c >= 'a' && c <= 'z')
                 || (c >= '0' && c <= '9')
                 || (c >= 'A' && c <= 'Z')
                 || (c = '_') -> Done(c, cs, { ctxt with Position = ctxt.Position.NextChar })
    | _ -> Error(sprintf "Error: expected digit at %A." ctxt.Position)

let digits = 
  prs{
    let! d = digit
    let! ds = digit |> repeat
    return d::ds
  }

let int_literal =
  prs{
    let! min = char '-' .|. nothing
    let! digits = digits
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

let getPosition = 
  fun (chars,ctxt) -> Done(ctxt.Position,chars,ctxt)

let rec token : Parser<char,Context,Token> = 
  prs{
    let! pos = getPosition
    return! 
      (prs{
        let! i = int_literal
        return ((i |> Int),pos) |> Literal 
      }) .||
      (prs{
        let! x = float_literal
        return ((x |> Float),pos) |> Literal 
      }) .||
      (prs{
        let! s = string_literal
        return ((s |> String),pos) |> Literal 
      }) .||
      (prs{
        do! !"Func"
        return (Func,pos) |> Keyword 
      }) .||
      (prs{
        do! !"Data"
        return (Data,pos) |> Keyword 
      }) .||
      (prs{
        do! !"=>"
        return (DoubleArrow,pos) |> Keyword 
      }) .||
      (prs{
        do! !"::"
        return (DoubleColon,pos) |> Keyword 
      }) .||
      (prs{
        do! !"->"
        return (SingleArrow,pos) |> Keyword 
      }) .||
      (prs{
        do! horizontal_bar
        return (HorizontalBar,pos) |> Keyword 
      }) .||
      (prs{
        do! !"{"
        return (Open Curly,pos) |> Keyword 
      }) .||
      (prs{
        do! !"}"
        return (Close Curly,pos) |> Keyword 
      }) .||
      (prs{
        do! !"["
        return (Open Square,pos) |> Keyword 
      }) .||
      (prs{
        do! !"]"
        return (Close Square,pos) |> Keyword 
      }) .||
      (prs{
        do! !"("
        return (Open Round,pos) |> Keyword 
      }) .||
      (prs{
        do! !")"
        return (Close Round,pos) |> Keyword 
      }) .||
      (prs{
        let! n = spaces
        return (Spaces n,pos) |> Keyword 
      }) .||
      (prs{
        do! !"\r\n"
        return (NewLine,pos) |> Keyword
      }) .||
      (prs{
        let! s = any_id
        return (s,pos) |> Id 
      })
  }

let rec tokens_line() : Parser<char,Context,List<Token>> = 
  prs{
    let! hd = token
    match hd with
    | Keyword(NewLine,_) -> return [hd]
    | _ ->
      let! tl = tokens_line()
      return hd :: tl
  }

let rec tokens_lines() : Parser<char,Context,List<List<Token>>> = 
  prs{
    let! context = getContext
    let! position = getPosition
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
            Open Indent,context'.NumIndents+1
          else // close indentation bracket
            Close Indent,context'.NumIndents-1
        do! setContext { context' with NumIndents = num_indents }
        let! rest = tokens_lines()
        return (Keyword(bracket,position) :: line) :: rest
  } .|| 
    (eof >> 
      prs{ 
        let! pos = getPosition
        let! ctxt = getContext
        do! setContext { ctxt with NumIndents = 0 }
        return [for i = 1 to ctxt.NumIndents do yield [Keyword(Close Indent,pos)]] })

let tokenize (path:string) = //: Result<char,Unit,List<Token>> =
  let source = System.IO.File.ReadAllText(path) |> Seq.toList
  let pos = Position.FromPath path
  match (tokens_lines()) (source,Context.Zero pos) with
  | Done(tokens, _, _) -> Some (tokens |> List.concat)
  | Error(e) ->
    printfn "%A" e
    None

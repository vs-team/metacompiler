module Lexer

open Common
open ParserMonad

type Context = { IndentationDepth : int; NumIndents : int; Position : Position }
  with static member Zero pos = { IndentationDepth = 0; NumIndents = 0; Position = pos }

let getPosition = 
  fun (chars,ctxt) -> Done(ctxt.Position,chars,ctxt)

type Keyword = 
  | Func | Data | DoubleArrow | HorizontalBar | Class | Instance
  | Open of Bracket| Close of Bracket | NewLine
  | SingleArrow | Spaces of int

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

let (!) (str:string) : Parser<char,_,Unit> =
  let rec traverse_from  (s_i:int) : Parser<char,_,Unit> =
// faster but less elegant:
    fun (chars,ctxt) ->
      if s_i = str.Length then
        Done((), chars, ctxt)
      else
        match chars with
        | c::cs when c = str.[s_i] -> 
          traverse_from (s_i + 1) (cs,{ ctxt with Position = ctxt.Position.NextChar })
        | _ -> Error(sprintf "Expected %s at %A" str ctxt.Position)
// slower but more elegant:
//    prs{
//      match s with
//      | c::cs -> 
//        do! char c
//        do! !!cs
//        return ()
//      | [] -> 
//        return ()
//    }
  traverse_from 0

// less elegant but fast:
let alpha_numeric_id(chars,ctxt) = 
  let inline is_alpha_num c =
    (c >= 'a' && c <= 'z')
    || (c >= '0' && c <= '9')
    || (c >= 'A' && c <= 'Z')
    || (c = '_')
  let rec alpha_num(chars,ctxt) = 
      match chars with
      | c::cs when is_alpha_num c -> 
        let ds,cs,ctxt = alpha_num(cs,{ ctxt with Position = ctxt.Position.NextChar })
        c::ds,cs,ctxt
      | _ -> [],chars,ctxt
  match chars with
  | c::cs when is_alpha_num c -> 
    let ds,cs,ctxt = alpha_num(cs,ctxt)
    Done(new System.String((c::ds) |> Seq.toArray), cs, { ctxt with Position = ctxt.Position.NextChar })
  | _ -> Error(sprintf "Error: expected digit at %A." ctxt.Position)
  
// more elegant but slow:
//let alpha_numeric : Parser<char,_,char> =
//  fun (chars,ctxt) ->
//    match chars with
//    | c::cs when (c >= 'a' && c <= 'z')
//                 || (c >= '0' && c <= '9')
//                 || (c >= 'A' && c <= 'Z')
//                 || (c = '_') -> Done(c, cs, { ctxt with Position = ctxt.Position.NextChar })
//    | _ -> Error(sprintf "Error: expected digit at %A." ctxt.Position)
//let alpha_numeric_id =
//  prs{
//    let! c = alpha_numeric
//    let! chars = alpha_numeric |> repeat
//    return new System.String((c::chars) |> Seq.toArray)
//  }

// less elegant but fast:
let symbol_id(chars,ctxt) = 
  let inline is_symbol c =
    c = ',' || c = ':' || c = ';' 
    || c = '+' || c = '-' || c = '*' 
    || c = '/' || c = '#' || c = '<' 
    || c = '^' || c = '&' || c = '|' 
    || c = '>' || c = '=' || c = '$'  
  let rec symbol(chars,ctxt) = 
      match chars with
      | c::cs when is_symbol c -> 
        let ds,cs,ctxt = symbol(cs,{ ctxt with Position = ctxt.Position.NextChar })
        c::ds,cs,ctxt
      | _ -> [],chars,ctxt
  match chars with
  | c::cs when is_symbol c -> 
    let ds,cs,ctxt = symbol(cs,ctxt)
    Done(new System.String((c::ds) |> Seq.toArray), cs, { ctxt with Position = ctxt.Position.NextChar })
  | _ -> Error(sprintf "Error: expected digit at %A." ctxt.Position)
  
// more elegant but slow:
//let symbol : Parser<char,_,char> =
//  fun (chars,ctxt) ->
//    match chars with
//    | (',' as c)::cs | (':' as c)::cs | (';' as c)::cs 
//    | ('+' as c)::cs | ('-' as c)::cs | ('*' as c)::cs 
//    | ('/' as c)::cs | ('#' as c)::cs | ('<' as c)::cs 
//    | ('^' as c)::cs | ('&' as c)::cs | ('|' as c)::cs 
//    | ('>' as c)::cs | ('=' as c)::cs | ('$' as c)::cs -> Done(c, cs, { ctxt with Position = ctxt.Position.NextChar })
//    | _ -> Error(sprintf "Error: expected digit at %A." ctxt.Position)
//let symbol_id =
//  prs{
//    let! c = symbol
//    let! chars = symbol |> repeat
//    return new System.String((c::chars) |> Seq.toArray)
//  }

// less elegant but fast:
let digits(chars,ctxt) = 
  let inline is_digit c = c >= '0' && c <= '9'
  let rec digits(chars,ctxt) = 
      match chars with
      | c::cs when is_digit c -> 
        let ds,cs,ctxt = digits(cs,{ ctxt with Position = ctxt.Position.NextChar })
        c::ds,cs,ctxt
      | _ -> [],chars,ctxt
  match chars with
  | c::cs when is_digit c -> 
    let ds,cs,ctxt = digits(cs,ctxt)
    Done(c::ds, cs, { ctxt with Position = ctxt.Position.NextChar })
  | _ -> Error(sprintf "Error: expected digit at %A." ctxt.Position)
  
// more elegant but slow:
//let digit : Parser<char,_,char> =
//  fun (chars,ctxt) ->
//    match chars with
//    | c::cs when c >= '0' && c <= '9' -> Done(c, cs, { ctxt with Position = ctxt.Position.NextChar })
//    | _ -> Error(sprintf "Error: expected digit at %A." ctxt.Position)
//let digits = 
//  prs{
//    let! d = digit
//    let! ds = digit |> repeat
//    return d::ds
//  }

// fast but inelegant
let unsigned_int_literal(chars,ctxt) =
  match digits (chars,ctxt) with
  | Done(digits,chars,ctxt) ->
    let mutable x = 0
    do for i in digits do x <- x * 10 + (int i - int '0')
    Done(x,chars,ctxt)
  | Error e -> Error e

// slow but elegant
//let unsigned_int_literal =
//  prs{
//    let! digits = digits
//    let mutable x = 0
//    do for i in digits do x <- x * 10 + (int i - int '0')
//    return x
//  }

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
    let! i = unsigned_int_literal
    do! char '.'
    let! d = unsigned_int_literal
    return Literal((Float32(System.Single.Parse(i.ToString() + "." + d.ToString()))),pos)
  }

let any_id = alpha_numeric_id .|| symbol_id

let string_literal pos =
  prs{
    do! char '\"'
    let! chars = any_id
    do! char '\"'
    return ((String(new System.String(chars |> Seq.toArray))),pos) |> Literal
  }

let horizontal_bar pos = 
  prs{
    do! char '-'
    do! char '-'
    do! char '-' |> repeat |> ignore
    return Keyword(HorizontalBar,pos)
  }

let rec spaces pos =
  prs{
    let! fst = !" "
    let! rest = !" " |> repeat
    return Keyword(Spaces(1 + (rest |> List.length)),pos)
  }

let rec token : Parser<char,Context,Token> = 
  prs{
    let! pos = getPosition
    return! 
      first_successful
        [
          float_literal pos
          int_literal pos
          string_literal pos
          (prs{
            do! !"Instance"
            return (Func,pos) |> Keyword 
          })
          (prs{
            do! !"Class"
            return (Func,pos) |> Keyword 
          })
          (prs{
            do! !"Func"
            return (Func,pos) |> Keyword 
          })
          (prs{
            do! !"Data"
            return (Data,pos) |> Keyword 
          })
          (prs{
            do! !"=>"
            return (DoubleArrow,pos) |> Keyword 
          })
          (prs{
            do! !"->"
            return (SingleArrow,pos) |> Keyword 
          })
          horizontal_bar pos
          (prs{
            do! !"{"
            return (Open Curly,pos) |> Keyword 
          })
          (prs{
            do! !"}"
            return (Close Curly,pos) |> Keyword 
          })
          (prs{
            do! !"["
            return (Open Square,pos) |> Keyword 
          })
          (prs{
            do! !"]"
            return (Close Square,pos) |> Keyword 
          })
          (prs{
            do! !"("
            return (Open Round,pos) |> Keyword 
          })
          (prs{
            do! !")"
            return (Close Round,pos) |> Keyword 
          })
          spaces pos
          (prs{
            do! !"\r\n"
            return (NewLine,pos) |> Keyword
          })
          (prs{
            let! s = any_id
            return (s,pos) |> Id 
          })
        ]
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

let indentationDepth =
  prs{
    let! fst = !" "
    let! rest = !" " |> repeat
    return 1 + (rest |> List.length)
  }

let rec tokens_lines() : Parser<char,Context,List<List<Token>>> = 
  prs{
    let! context = getContext
    let! position = getPosition
    let! is_eof = eof .|. nothing
    match is_eof with
    | A() ->
      do! setContext { context with NumIndents = 0 }
      return [for i = 1 to context.NumIndents do yield [Keyword(Close Indent,position)]]
    | B() ->
      let! indentation_depth = 
        indentationDepth .|| (nothing >> prs{ return 0 })
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
  }

let tokenize (path:string) = //: Result<char,Unit,List<Token>> =
  let source = System.IO.File.ReadAllText(path) |> Seq.toList
  let pos = Position.FromPath path
  match (tokens_lines()) (source,Context.Zero pos) with
  | Done(tokens, _, _) -> Some (tokens |> List.concat)
  | Error(e) ->
    printfn "%A" e
    None

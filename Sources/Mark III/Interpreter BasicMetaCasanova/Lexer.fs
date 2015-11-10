module Lexer

open Common
open ParserMonad

type Context = { IndentationDepth : int; NumIndents : int; Position : Position }
  with static member Zero pos = { IndentationDepth = 0; NumIndents = 0; Position = pos }

let getPosition = 
  fun (chars,ctxt) -> Done(ctxt.Position,chars,ctxt)

type Keyword = 
  | Import | Inherit | Func | TypeFunc | Data | HorizontalBar | Instance
  | Open of Bracket| Close of Bracket | NewLine
  | SingleArrow | DoubleArrow | PriorityArrow | Spaces of int

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
    | _ -> Error(LexerError [(sprintf "Unmatched char when %c expected at" expected)],ctxt.Position)

let (!) (str:string) : Parser<char,_,Unit> =
  let rec traverse_from (s:List<char>) : Parser<char,_,Unit> =
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

let alpha_numeric : Parser<char,_,char> =
  fun (chars,ctxt) ->
    match chars with
    | c::cs when (c >= 'a' && c <= 'z')
                 || (c >= '0' && c <= '9')
                 || (c >= 'A' && c <= 'Z')
                 || (c = '_') || (c = '\'') -> Done(c, cs, { ctxt with Position = ctxt.Position.NextChar })
    | _ -> Error(LexerError [(sprintf "Error: expected alpha/numeric-char at")],ctxt.Position)

let alpha_numeric_id =
  prs{
    let! c = alpha_numeric
    let! chars = alpha_numeric |> repeat
    return new System.String((c::chars) |> Seq.toArray)
  }
  
let symbol : Parser<char,_,char> =
  fun (chars,ctxt) ->
    match chars with
    | (',' as c)::cs | (':' as c)::cs | (';' as c)::cs 
    | ('+' as c)::cs | ('-' as c)::cs | ('*' as c)::cs 
    | ('/' as c)::cs | ('#' as c)::cs | ('<' as c)::cs 
    | ('&' as c)::cs | ('|' as c)::cs | ('>' as c)::cs 
    | ('=' as c)::cs | ('$' as c)::cs | ('\'' as c)::cs 
    | ('.' as c)::cs | ('@' as c)::cs | ('\\' as c)::cs -> Done(c, cs, { ctxt with Position = ctxt.Position.NextChar })
    | _ -> Error(LexerError ["Error: expected symbol at"],ctxt.Position)

let caret : Parser<char,_,char> =
  fun (chars,ctxt) ->
    match chars with
    | ('^' as c)::cs -> Done(c, cs, { ctxt with Position = ctxt.Position.NextChar })
    | _ -> Error(LexerError ["Error: expected symbol at"],ctxt.Position)

let symbol_id =
  prs{
    let! c = symbol
    let! chars = symbol |> repeat
    return new System.String((c::chars) |> Seq.toArray)
  } .|| prs {
    let! c = caret
    return new System.String((c::[]) |> Seq.toArray)
  }
  
let digit : Parser<char,_,char> =
  fun (chars,ctxt) ->
    match chars with
    | c::cs when c >= '0' && c <= '9' -> Done(c, cs, { ctxt with Position = ctxt.Position.NextChar })
    | _ -> Error(LexerError [(sprintf "Error: expected digit at")],ctxt.Position)

let digits = 
  prs{
    let! d = digit
    let! ds = digit |> repeat
    return d::ds
  }

let unsigned_int_literal =
  prs{
    let! digits = digits
    if digits.Length = 0 then 
      return! fail (LexerError [""])
    else 
      let mutable x = 0
      do for i in digits do x <- x * 10 + (int i - int '0')
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
    let! res = 
        float_literal pos .||
        int_literal pos .||
        string_literal pos .||
        (prs{
          do! !"Instance"
          return (Instance,pos) |> Keyword 
        }) .||
        (prs{
          do! !"import"
          return (Import,pos) |> Keyword 
        }) .||
        (prs{
          do! !"inherit"
          return (Inherit,pos) |> Keyword 
        }) .||
        (prs{
          do! !"Func"
          return (Func,pos) |> Keyword 
        }) .||
        (prs{
          do! !"TypeFunc"
          return (TypeFunc,pos) |> Keyword 
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
          do! !"->"
          return (SingleArrow,pos) |> Keyword 
        }) .||
        (prs{
          do! !"#>"
          return (PriorityArrow,pos) |> Keyword 
        }) .||
        horizontal_bar pos .||
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
        spaces pos .||
        (prs{
          do! !"\r\n" .|| !"\n\r" .|| !"\n"
          return (NewLine,pos) |> Keyword
        }) .||
        (prs{
          do! !"\t"
          return! fail (LexerError ["Don't use tabs"])
        }) .||
        (prs{
          let! s = any_id
          return (s,pos) |> Id 
        }) .|| 
        (prs{
          let! ef = eof
          return (NewLine,pos) |> Keyword
        }) 
    return res
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
    return! 
      (lookahead(!"\n") >> 
        prs{
          let! ctxt = getContext 
          return ctxt.IndentationDepth
        }) .||
      (prs{ return 1 + (rest |> List.length) })
  } .|| (lookahead(!"\n") >> prs{
          let! ctxt = getContext 
          return ctxt.IndentationDepth
        })


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

let tokenize = //: Result<char,Unit,List<Token>> =
  let regular_load path () = 
    if System.IO.File.Exists(path) then 
      let source = System.IO.File.ReadAllText(path) |> Seq.toList
      let pos = Position.FromPath path
      match (tokens_lines()) (source,Context.Zero pos) with
      | Done(tokens, _, _) -> Some (tokens |> List.concat)
      | Error(e,p) ->
        printfn "%A" (e,p)
        None
    else None
  Caching.cached_op regular_load

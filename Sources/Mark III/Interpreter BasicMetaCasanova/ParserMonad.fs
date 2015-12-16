module ParserMonad
open Common

type ErrorType =  ParserMonadError
                | LexerError          of Position
                | ParserError         of Position
                | LineSplitterError   of Position
                | ScopeError          of string*Position
                | TypeError
                | RuleError           of string           
                | PipeLineError
                   
type Result<'char,'ctxt,'result> = 
  | Done of 'result * List<'char> * 'ctxt
  | Error of ErrorType

type Parser<'char,'ctxt,'result> =
  List<'char> * 'ctxt -> Result<'char,'ctxt,'result>

type ParserBuilder() =
  member this.Return(res:'result) : Parser<'char,'ctxt,'result> =
    fun (chars,ctxt) ->
      Done(res, chars, ctxt)
  member this.ReturnFrom p = p
  member this.Bind(p:Parser<'char,'ctxt,'result>, k:'result->Parser<'char,'ctxt,'result'>) =
    fun (chars,ctxt) ->
      match p (chars, ctxt) with
      | Error(p) -> Error(p)
      | Done(res,chars',ctxt') ->
        let out = k res (chars',ctxt')
        out

let prs = ParserBuilder()

type Or<'a,'b> = A of 'a | B of 'b
let (.|.) (p1:Parser<_,_,'a>) (p2:Parser<_,_,'b>) : Parser<_,_,Or<'a,'b>> = 
  fun (chars,ctxt) ->
    match p1(chars,ctxt) with
    | Error(p1) ->
      match p2(chars,ctxt) with
      | Error(p2) ->
        Error(p1)
      | Done(res2,chars',ctxt') ->
        (B(res2),chars',ctxt') |> Done
    | Done(res1,chars',ctxt') ->
       (A(res1),chars',ctxt') |> Done

let inline (>>) p k = 
  prs{
    do! p
    return! k
  }

let rec first_successful (ps:List<Parser<_,_,'a>>) : Parser<_,_,'a> = 
  fun (chars,ctxt) ->
    match ps with
    | [] -> Error ParserMonadError
    | p :: ps ->
      match p(chars,ctxt) with
      | Error(p) ->
        first_successful ps (chars,ctxt)
      | res -> res 

let inline (.||) (p1:Parser<_,_,'a>) (p2:Parser<_,_,'a>) : Parser<_,_,'a> = 
  fun (chars,ctxt) ->
    match p1(chars,ctxt) with
    | Error(p1) ->
      match p2(chars,ctxt) with
      | Error(p2) ->
        Error(p1)
      | Done(res2,chars',ctxt') ->
        (res2,chars',ctxt') |> Done
    | Done(res1,chars',ctxt') ->
       (res1,chars',ctxt') |> Done

let nothing : Parser<_,_,Unit> = 
  fun (chars,ctxt) -> Done((),chars,ctxt)

let rec repeat (p:Parser<_,_,'result>) : Parser<_,_,List<'result>> =
  prs{
    let! curr = p .|. nothing
    match curr with
    | A x -> 
      let! xs = repeat p
      return x :: xs
    | B () -> 
      return []
  }

let step = 
  fun (chars,ctxt) ->
    match chars with
    | c::cs -> Done(c, cs, ctxt)
    | _ -> Error ParserMonadError

let eof = 
  fun (chars,ctxt) ->
    match chars with
    | [] -> Done((), [], ctxt)
    | _ -> Error ParserMonadError

let ignore (p:Parser<_,_,'res>) : Parser<_,_,Unit> = 
  fun (chars,ctxt) -> 
    match p(chars,ctxt) with
    | Error(p) -> Error(p)
    | Done(res,chars',ctxt') ->
      ((),chars',ctxt') |> Done

let lookahead (p:Parser<_,_,_>) : Parser<_,_,_> =
  fun (chars,ctxt) -> 
    match p(chars,ctxt) with
    | Error(p) -> Error(p)
    | Done(res,chars',ctxt') ->
      (res,chars,ctxt) |> Done

let fail (msg:ErrorType) : Parser<_,_,_> =
  fun (_,_) ->
    Error msg

let getBuffer =
  fun (chars,ctxt) -> (chars,chars,ctxt) |> Done

let getContext =
  fun (chars,ctxt) -> (ctxt,chars,ctxt) |> Done

let setBuffer chars' =
  fun (chars,ctxt) -> ((),chars',ctxt) |> Done

let setContext ctxt' =
  fun (chars,ctxt) -> ((),chars,ctxt') |> Done

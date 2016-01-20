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
                | EofError
                | SatisfyError
                   
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

// repeats at least once
let repeat1 (p:Parser<_,_,'result>) : Parser<_,_,List<'result>> =
  prs{
    let! first = p
    let! rest  = repeat p
    return (first::rest)
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

let ret x = prs.Return(x)
let (>>=) p f = prs.Bind(p,f)

let (|>>) (p:Parser<'char,'ctxt,'a>) (f: 'a -> 'b) :Parser<'char,'ctxt,'b> =
  prs{
    let! c = p
    return f c
  }

let (>>.) (l:Parser<'src,'ctxt,_>) (r:Parser<'src,'ctxt,'r>) :Parser<'src,'ctxt,'r> =
  prs{
    do!  l
    let! res = r
    return res
  }

let (.>>) (l:Parser<'src,'ctxt,'r>) (r:Parser<'src,'ctxt,_>) :Parser<'src,'ctxt,'r> =
  prs{
    let! res = l
    do!  r
    return res
  }

let (.>>.) (l:Parser<'src,'ctxt,'lr>) (r:Parser<'src,'ctxt,'rr>) :Parser<'src,'ctxt,'lr*'rr> =
  prs{
    let! res_l = l
    let! res_r = r
    return (res_l,res_r)
  }

let rec itterate (p:Parser<_,_,'result>) : Parser<_,_,List<'result>> =
  prs{return! eof >>. ret []} .||
  prs{
    let! x  = p
    let! xs = itterate p .|| ret []
    return x::xs 
  }

let satisfy (f:'char->bool) :Parser<'char,'ctxt,_> =
  fun(lst,ctxt)->
    match lst with
    | x::xs -> if f x then Done((),xs,ctxt) else Error SatisfyError
    | _ -> Error EofError

let contextSatisfies (f:'ctxt->bool) :Parser<'char,'ctxt,Unit> =
  prs{
    let! context = getContext
    if f context then
      return! nothing
    else
      return! fail SatisfyError
  }

let updateContext (f:'ctxt->'ctxt) :Parser<'char,'ctxt,Unit> =
  prs{
    let! c = getContext
    do! setContext (f c)
    return! nothing
  }

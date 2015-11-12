module ParseMonad
open ErrorMonad

type Parse<'ch,'ct,'a> = List<'ch> -> 'ct -> Result<'a * List<'ch> * 'ct>

let ret x = fun buf ctxt -> ErrorMonad.ret(x, buf, ctxt)
let fail m = fun buf ctxt -> ErrorMonad.fail(m)

let (>>=) (p : Parse<'ch,'ct,'a>) (k : 'a -> Parse<'ch,'ct,'b>) : Parse<'ch,'ct,'b> =
  fun buf ctxt ->
    (p buf ctxt) >>= 
      (fun (x,buf',ctxt') -> k x buf' ctxt')

let (>>) (p : Parse<'ch,'ct,'a>) (k : Parse<'ch,'ct,'b>) : Parse<'ch,'ct,'b> =
  p >>= (fun _ -> k)

type ParseBuilder() =
  member this.Return x = ret x
  member this.ReturnFrom p = p
  member this.Bind(p,k) = p >>= k
  member this.For(s:seq<'a>, k:('a->Parse<'ch,'ct,Unit>)) : Parse<'ch,'ct,Unit> =
    fun buf ctxt ->
      if s |> Seq.isEmpty then
        ret () buf ctxt
      else
        ((k (s |> Seq.head)) >> (this.For(s |> Seq.tail, k))) buf ctxt
  member this.Zero() = ret ()
let prs = ParseBuilder()

let getBuffer  buf ctxt = ret buf  buf ctxt
let getContext buf ctxt = ret ctxt buf ctxt
let setBuffer  buf' buf ctxt = ret () buf' ctxt
let setContext ctxt' buf ctxt = ret () buf ctxt'
let getSymbol buf ctxt = 
  match buf with
  | b :: bs -> 
    ret b bs ctxt
  | [] -> fail ["Unexpected EOF"] buf ctxt
let eof buf ctxt = 
  match buf with
  | b :: bs -> 
    fail ["Expected EOF"] buf ctxt
  | [] -> 
    ret () buf ctxt
let nothing buf ctxt = 
    ret () buf ctxt
let tryCatch (p:Parse<'ch,'ctxt,'a>) (k:'a->Parse<'ch,'ctxt,'b>) (err:List<string>->Parse<'ch,'ctxt,'b>) : Parse<'ch,'ctxt,'b> = 
  fun buf ctxt -> 
    match p buf ctxt with
    | Error e -> err e buf ctxt
    | Result(x,buf',ctxt') ->
      k x buf' ctxt'
let (.||) (p1:Parse<'ch,'ctxt,'a>) (p2:Parse<'ch,'ctxt,'a>) : Parse<'ch,'ctxt,'a> =
  tryCatch p1 ret (fun e1 -> tryCatch p2 ret (fun e2 -> fail(e1 @ e2)))
let rec repeat p =
  prs{
    let! x = p
    let! xs = repeat p
    return x :: xs
  } .|| (nothing >> prs{ return [] })
let ignore p =
  prs{
    let! x = p
    return ()
  }
let lookahead p = 
  fun buf ctxt ->
    match p buf ctxt with
    | Error e -> Error e
    | Result(x,_,_) -> Result(x,buf,ctxt)

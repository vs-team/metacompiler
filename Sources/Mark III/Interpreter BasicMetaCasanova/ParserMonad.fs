module ParserMonad
open Common

type Result<'char,'ctxt,'result> = 
  | Done of 'result * List<'char> * 'ctxt * Position
  | Error of string * Position

type Parser<'char,'ctxt,'result> =
  List<'char> * 'ctxt * Position -> Result<'char,'ctxt,'result>

type ParserBuilder() =
  member this.Return(res:'result) : Parser<'char,'ctxt,'result> =
    fun (chars,ctxt,pos) ->
      Done(res, chars, ctxt, pos)
  member this.ReturnFrom p = p
  member this.Zero() = fun (chars,ctxt,pos) -> Error("Error: unmatched construct.", pos)
  member this.Bind(p:Parser<'char,'ctxt,'result>, k:'result->Parser<'char,'ctxt,'result'>) =
    fun (chars,ctxt,pos) ->
      match p (chars, ctxt, pos) with
      | Error(s,pos') -> Error(s,pos')
      | Done(res,chars',ctxt',pos') ->
        let out = k res (chars',ctxt',pos')
        out

let prs = ParserBuilder()

type Or<'a,'b> = A of 'a | B of 'b
let (.|.) (p1:Parser<_,_,'a>) (p2:Parser<_,_,'b>) : Parser<_,_,Or<'a,'b>> = 
  fun (chars,ctxt,pos) ->
    match p1(chars,ctxt,pos) with
    | Error(e1,pos1) ->
      match p2(chars,ctxt,pos) with
      | Error(e2,pos2) ->
        Error(e1+e2,pos1)
      | Done(res2,chars',ctxt',pos') ->
        (B(res2),chars',ctxt',pos') |> Done
    | Done(res1,chars',ctxt',pos') ->
       (A(res1),chars',ctxt',pos') |> Done

let nothing : Parser<_,_,Unit> = 
  fun (chars,ctxt,pos) -> Done((),chars,ctxt,pos)

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

let ignore (p:Parser<_,_,'res>) : Parser<_,_,Unit> = 
  fun (chars,ctxt,pos) -> 
    match p(chars,ctxt,pos) with
    | Error(e,pos') -> Error(e,pos')
    | Done(res,chars',ctxt',pos') ->
      ((),chars',ctxt',pos') |> Done

let fail (msg:string) : Parser<_,_,_> =
  fun (_,_,pos) ->
    Error(msg,pos)

let position =
  fun (chars,ctxt,pos) -> (pos,chars,ctxt,pos) |> Done

let chars =
  fun (chars,ctxt,pos) -> (chars,chars,ctxt,pos) |> Done

let context =
  fun (chars,ctxt,pos) -> (ctxt,chars,ctxt,pos) |> Done

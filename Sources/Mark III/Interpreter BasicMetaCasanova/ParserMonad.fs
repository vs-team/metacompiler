module ParserMonad
open Common

type ErrorType = ParserError of string
               | ExitStatus  of string
                   
type Result<'char,'ctxt,'result> = 
  | Done of 'result * List<'char> * 'ctxt
  | Error of ErrorType*int 

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
      | Error(e,p) -> Error(e,p)
      | Done(res,chars',ctxt') ->
        let out = k res (chars',ctxt')
        out

let prs = ParserBuilder()

type Or<'a,'b> = A of 'a | B of 'b
let (.|.) (p1:Parser<_,_,'a>) (p2:Parser<_,_,'b>) : Parser<_,_,Or<'a,'b>> = 
  fun (chars,ctxt) ->
    match p1(chars,ctxt) with
    | Error(e1,i1) ->
      match p2(chars,ctxt) with
      | Error(e2,i2) -> if i1 > i2 then Error(e1,i1+1) else Error(e2,i2+1)  
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
    | [] -> Error ((ParserError "ParserMonad: first_succesful. End of list."),0)
    | p :: ps ->
      match p(chars,ctxt) with
      | Error(_) ->
        first_successful ps (chars,ctxt)
      | res -> res 

let inline (.||) (p1:Parser<_,_,'a>) (p2:Parser<_,_,'a>) : Parser<_,_,'a> = 
  fun (chars,ctxt) ->
    match p1(chars,ctxt) with
    | Error(e1,i1) ->
      match p2(chars,ctxt) with
      | Error(e2,i2) -> if i1 > i2 then Error(e1,i1+1) else Error(e2,i2+1)
      | Done(res2,chars',ctxt') ->
        (res2,chars',ctxt') |> Done
    | Done(res1,chars',ctxt') ->
       (res1,chars',ctxt') |> Done

let inline (.|) (p1:Parser<_,_,'a>) (p2:Parser<_,_,'a>) : Parser<_,_,'a> = 
  fun (chars,ctxt) ->
    match p1(chars,ctxt) with
    | Error(e1,i1) ->
      match p2(chars,ctxt) with
      | Error(e2,i2) -> Error(e2,i2)
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
    | _ -> Error (ParserError "ParserMonad: step. No chars left.",0)

let eof = 
  fun (chars,ctxt) ->
    match chars with
    | [] -> Done((), [], ctxt)
    | _ -> Error (ParserError "End of file.",0)

let ignore (p:Parser<_,_,'res>) : Parser<_,_,Unit> = 
  fun (chars,ctxt) -> 
    match p(chars,ctxt) with
    | Error(e,i) -> Error(e,i)
    | Done(res,chars',ctxt') ->
      ((),chars',ctxt') |> Done

let lookahead (p:Parser<_,_,_>) : Parser<_,_,_> =
  fun (chars,ctxt) -> 
    match p(chars,ctxt) with
    | Error(e,i) -> Error(e,i)
    | Done(res,chars',ctxt') ->
      (res,chars,ctxt) |> Done

let fail (msg:ErrorType) : Parser<_,_,_> =
  fun (_,_) ->
    Error (msg,0)

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
  prs{return! eof >>. ret []} .|
  prs{
    let! x  = p
    let! xs = itterate p
    return x::xs 
  }

let satisfy (f:'char->bool) :Parser<'char,'ctxt,_> =
  fun(lst,ctxt)->
    match lst with
    | x::xs -> if f x then Done((),xs,ctxt) else Error (ParserError "Not satisfied.",0)
    | _ -> Error (ParserError "End of file error.",0)

let rec getFirstInstanceOf (f:'char->bool) :Parser<'char,'ctxt,'char> =
  prs{
    let! next = step
    if f next then return next
    else return! getFirstInstanceOf f
  }

let contextSatisfies (f:'ctxt->bool) :Parser<'char,'ctxt,Unit> =
  prs{
    let! context = getContext
    if f context then
      return! nothing
    else
      return! fail (ParserError "Not satisfied.")
  }

let updateContext (f:'ctxt->'ctxt) :Parser<'char,'ctxt,Unit> =
  prs{
    let! c = getContext
    do! setContext (f c)
    return! nothing
  }

let UseDifferentSrc (p:Parser<'char2,'ctxt,'res>) (char':List<'char2>):Parser<'char,'ctxt,'res> =
  fun (char,ctxt) ->
    match p (char',ctxt) with
    | Done(res,char',ctxt) -> Done(res,char,ctxt)
    | Error (e,i) -> Error (e,i)

let UseDifferentCtxt (p:Parser<'char,'ctxt2,'res>) (ctxt':'ctxt2):Parser<'char,'ctxt,'res> =
  fun (char,ctxt) ->
    match p (char,ctxt') with
    | Done(res,char',_) -> Done(res,char',ctxt)
    | Error (e,i) -> Error (e,i)

let UseDifferentSrcAndCtxt (p:Parser<'char2,'ctxt2,'res>)(char':List<'char2>)(ctxt':'ctxt2):Parser<'char,'ctxt,'res> =
  fun (char,ctxt) ->
    match p (char',ctxt') with
    | Done(res,char'',_) -> Done(res,char,ctxt)
    | Error (e,i) -> Error (e,i)

let rec IterateTroughGivenList (ls:List<'item>)(p:'item -> Parser<'char,'ctxt,'res>) 
      :Parser<'char,'ctxt,List<'res>> =
  prs{
    match ls with
    | [] -> return []
    | _  -> return! fail (ParserError "End of list Error.")
  } .|| prs{
    match ls with
    | x::xs -> 
      let! y = p x
      let! ys = IterateTroughGivenList xs p
      return y::ys
    | [] -> return! fail (ParserError "End of list Error.")
  }

let rec FirstSuccesfullInList (ls:List<'item>)(p:'item -> Parser<'char,'ctxt,'res>) 
    :Parser<'char,'ctxt,'res> =
  prs{
    match ls with
    | x::xs -> 
      return! (p x) .|| FirstSuccesfullInList xs p
    | [] -> return! fail (ParserError "End of list Error.")
  }

let CatchError (p1:Parser<'char,'ctxt,'res>) (e:ErrorType) 
  (p2:Parser<'char,'ctxt,'res>) :Parser<'char,'ctxt,'res> =
  fun (char,ctxt) ->
    match p1 (char,ctxt) with
    | Done(res',char',ctxt') -> Done(res',char',ctxt')
    | e -> 
      match p2 (char,ctxt) with
      | x -> x
    | Error (er,i) -> Error (er,i)

let rec RepeatUntil (pb:Parser<_,_,'result>) (pe:Parser<_,_,_>) 
  : Parser<_,_,List<'result>> =
  prs{return! (lookahead pe) >>. ret []} .||
  prs{
    let! x  = pb
    let! xs = RepeatUntil pb pe
    return x::xs 
  }
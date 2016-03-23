module Interpreter
open CodegenInterface
open ParserMonad

(*
type StateMonad<'result,'state> = 'state -> 'result*'state
let (>>=) (m:StateMonad<'a,'s>) (k:'a->StateMonad<'b,'s>) :StateMonad<'b,'s> =
  fun state -> let (a,s) = m state in k a s
let ret (x:'a) :StateMonad<'a,'b> = fun s -> x,s

// look it's a rectangle
type StateBuilder() =
  member this.Return(x)=ret x
  member this.Bind(m,k)=m>>=k
  member this.ReturnFrom(x)=x
let st = new StateBuilder()
*)

(*
type ctxt = {locals:Map<local_id,Type*Option<obj>>;}

type EvalMonad = Parser<premisse,ctxt,obj*Type>

let eval_literal (lit:lit) :obj =
  match lit with
  | I64 x    -> box x
  | U64 x    -> box x
  | F64 x    -> box x
  | String x -> box x
  | Bool x   -> box x

let eval =
  prs{
    let! x = step
    match x with
    | Literal x -> 
      let o = eval_literal x.value
      let! t = getContext
      do!  updateContext (fun c->{c with values=c.values.Add(x.dest,(Some(o)))})
      return o,t
    | 
  }
  *)
//type label = Option<Id>*int*obj

//let eval_step (stack:label list list) (context:fromTypecheckerWithLove) :label list =
  
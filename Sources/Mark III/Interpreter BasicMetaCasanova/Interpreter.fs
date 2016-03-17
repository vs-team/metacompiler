module Interpreter
open CodegenInterface

type StateMonad<'result,'state> = 'state -> 'result*'state
let (>>=) (m:StateMonad<'a,'s>) (k:'a->StateMonad<'b,'s>) :StateMonad<'b,'s> =
  fun state -> let (a,s) = m state in k a s
let ret (x:'a) :StateMonad<'a,'b> = fun s -> x,s

type ParserBuilder() =
  member this.Return(x)=ret x
  member this.Bind(m,k)=m>>=k
  member this.ReturnFrom(x)=x

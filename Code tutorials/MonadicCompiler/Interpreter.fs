module Interpreter

open Common
open StateMonad
open Prioritizer

type Value = Int of int | Bool of bool | Unit
type Memory = Map<Id, Value>

let liftIntOp (f:int->int->int) (x:State<'s,Value>) (y:State<'s,Value>) : State<'s,Value> =
  x >>= (fun a -> 
  y >>= (fun b -> 
    match a, b with
    | Int i, Int j -> 
      ret (Int(f i j))
    | _ -> 
      fail ["Mismatched operands"]))

let liftComparisonOp (f:int->int->bool) (x:State<'s,Value>) (y:State<'s,Value>) : State<'s,Value> =
  x >>= (fun a -> 
  y >>= (fun b -> 
    match a, b with
    | Int i, Int j -> 
      ret (Bool(f i j))
    | _ -> 
      fail ["Mismatched operands"]))

let lookup (i:Id) (m:Memory) =
  ErrorMonad.ret(m.[i], m)

let store (i:Id) (v:Value) (m:Memory) =
  ErrorMonad.ret(Unit, m |> Map.add i v)

let rec interpret (e:Expression) : State<Memory, Value> =
  st{
    match e with
    | Literal(Literal.Int i) ->
      return Int i
    | Id i ->
      return! lookup i
    | Plus(a,b) ->
      let (<+>) = liftIntOp (+)
      return! (interpret a) <+> (interpret b)
    | Minus(a,b) ->
      let (<+>) = liftIntOp (-)
      return! (interpret a) <+> (interpret b)
    | DividedBy(a,b) ->
      let (<+>) = liftIntOp (/)
      return! (interpret a) <+> (interpret b)
    | GreaterThan(a,b) ->
      let (<+>) = liftComparisonOp (>)
      return! (interpret a) <+> (interpret b)
    | Equal(Id(a),b) ->
      let! v = interpret b
      return! store a v
    | Equal(_,b) ->
      return! fail ["Malformed assignment"]
    | If(a,b) ->
      let! cond = interpret a
      match cond with
      | Bool true -> 
        return! interpret b
      | _ -> 
        return Unit
    | While(a,b) ->
      let! cond = interpret a
      match cond with
      | Bool true -> 
        return! interpret (Semicolon(b, While(a,b)))
      | _ -> 
        return Unit
    | Print(a) ->
      let! v = interpret a
      match v with
      | Int i ->
        do printfn "%d" i
        return Unit
      | Bool b ->
        do printfn "%b" b
        return Unit
      | Unit ->
        do printfn "()"
        return Unit
    | Semicolon(a,b) ->
      let! x = interpret a
      return! interpret b
    | Sequence([]) ->
      return Unit
    | Sequence([x]) ->
      return! interpret x
    | Sequence(x::xs) ->
      let! v = interpret x
      return! interpret (Sequence xs)
  }

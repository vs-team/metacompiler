open Utilities
open ParserMonad
open BasicExpression
open ConcreteExpressionParser

[<EntryPoint>]
let main argv = 
  let ($) p a = p.Parse a

  let input = System.IO.File.ReadAllText @"Content\casanova semantics.mc"

  let output = (program()).Parse (input |> Seq.toList) ConcreteExpressionContext.Empty
  match output with
  | [] -> printfn "Parse error."
  | x::xs -> printfn "%s" (x.ToString())
  0

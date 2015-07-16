module GraphBasedCodeGenerator

open TypeInference
open System
open Utilities
open ParserMonad
open BasicExpression
open TypeDefinition
open ConcreteExpressionParserPrelude
open ConcreteExpressionParser

type TypedExpression   = BasicExpression<Keyword, Var, Literal, Position, Type>
type UntypedExpression = BasicExpression<Keyword, Var, Literal, Position, unit>

let generate (originalFilePath:string)
             (program_name:string)
             (rules:UntypedExpression) // <-- ast
             (program:UntypedExpression) // <-- main
             (ctxt:ConcreteExpressionContext) // <-- header
             :unit =
  match rules with
  | Application(Implicit, Keyword(Sequence, _, _) :: rules, pos, _) ->
    let basicgraph = BasicGraph.generate rules ctxt
    Console.ReadKey(true) |> ignore 
  | _ -> failwith "root ast element not a Sequence"
  ()

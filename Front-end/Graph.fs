module Graph

open TypeInference
open System
open Utilities
open ParserMonad
open BasicExpression
open TypeDefinition
open ConcreteExpressionParserPrelude
open ConcreteExpressionParser
open CodeGeneratorDeps

type TypedExpression   = BasicExpression<Keyword, Var, Literal, Position, Type>
type UntypedExpression = BasicExpression<Keyword, Var, Literal, Position, unit>

let hook (classes:Map<string,GeneratedClass>) :unit = ()

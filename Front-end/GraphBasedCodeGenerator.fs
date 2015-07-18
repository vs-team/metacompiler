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

type FunctionDefinition   = ParsedKeyword<Keyword, Var, Literal, Position, unit>

let sanitizeIdentifier (s:string) :string =
  s
    .Replace("_","Under_")
    .Replace(":","Colon_")
    .Replace(";","Semi_")
    + "_"

let forwardDeclareStructs (m:Map<string,FunctionDefinition>) : string =
  let fn s k v = 
    let k = sanitizeIdentifier k
    sprintf "%sstruct %s; typedef struct %s %s;\n" s k k k 
  Map.fold fn "/* forward struct declaration */\n" m

let forwardDeclareUnions (m:Map<string,List<string>>) : string =
  let fn s k (v:List<string>) = 
    if v.Length <= 1 then s else
      let k = sanitizeIdentifier k
      sprintf "%sunion %s; typedef union %s %s;\n" s k k k 
  Map.fold fn "/* forward union declaration */\n" m

let invert_map (m:Map<'a,'b>) : Map<'b,List<'a>> =
  let fn (s:Map<'b,List<'a>>) k v =
    if s |> Map.containsKey v then
      s |> Map.remove v |> Map.add v (k::s.[v])
    else
      s |> Map.add v (k::[])
  m |> Map.fold fn Map.empty

let genPolymorphicTypeMap (m:Map<string,FunctionDefinition>) : Map<string,List<string>> = 
  m |> Map.map (fun k v -> v.BaseType.ToString()) |> invert_map

let generate (originalFilePath:string)
             (program_name:string)
             (rules:UntypedExpression) // <-- ast
             (program:UntypedExpression) // <-- main
             (ctxt:ConcreteExpressionContext) // <-- header
             :unit =
  match rules with
  | Application(Implicit, Keyword(Sequence, _, _) :: rules, pos, _) ->
    let basicgraph = BasicGraph.generate rules ctxt
    let customKeywordsMap = ctxt.CustomKeywordsMap
    let polymorphicTypeMap = genPolymorphicTypeMap customKeywordsMap
    printfn "#include \"mc_runtime.h\"\n"
    printfn "%s" (forwardDeclareStructs customKeywordsMap)
    printfn "%s" (forwardDeclareUnions  polymorphicTypeMap)
    printfn "/* end */\n" 
  | _ -> failwith "root ast element not a Sequence"
  ()

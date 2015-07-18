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
  m |> Map.fold fn ""

let forwardDeclareUnions (m:Map<string,List<FunctionDefinition>>) : string =
  let fn s k (v:List<FunctionDefinition>) = 
    if v.Length <= 1 then s else
      let k = sanitizeIdentifier k
      sprintf "%sunion %s; typedef union %s %s;\n" s k k k 
  m |> Map.fold fn ""

let printTypeConstant (v,t) :string =
  match t with
  | NativeValue ->
    match v with
    | "string" -> "char*"
    | s -> s
  | Defined -> (sanitizeIdentifier v) + "*"
  | NativeRef -> "NativeRef"

let printType (t:Type) :string =
  match t with
  | TypeConstant (k,v) -> (printTypeConstant (k,v))
  | _ -> sprintf "int /*unknown type: %s */" (t.ToString())

let print_struct_body (lst:List<Type>) : string =
  let fn i v = sprintf "%s _%d; " (printType v) i
  match lst with
  | []  -> "char padding;"
  | _   -> lst |> List.mapi fn |> List.fold (fun s t -> s + t) ""

let implementStructs (m:Map<string,FunctionDefinition>) :string = 
  let fn s k (v:FunctionDefinition) =
    let k = sanitizeIdentifier k
    sprintf "%sstruct %s{%s};\n" s k (v.Arguments |> print_struct_body)
  m |> Map.fold fn ""

let implementUnions (m:Map<string,List<FunctionDefinition>>) : string =
  let fn s k (v:List<FunctionDefinition>) =
    let fn (s:string) (x:FunctionDefinition) = 
      let x = sanitizeIdentifier x.Name
      sprintf "%s%s %s_Subtype; " s x x
    let k = sanitizeIdentifier k
    sprintf "%sunion %s{%s};\n" s k (v |> List.fold fn "")
  m |> Map.fold fn ""

let declareDataTables (m:Map<string,FunctionDefinition>) :string = 
  let fn s k (v:FunctionDefinition) =
    let k = sanitizeIdentifier k
    if v.Arguments.Length.Equals 0 then
      sprintf "%s%s %sValue_;\n" s k k
    else
      sprintf "%stable %sTable_;\n" s k
  m |> Map.fold fn ""

let declareUnionTables (m:Map<string,list<FunctionDefinition>>) :string = 
  let fn s k (v:list<FunctionDefinition>) =
    let k = sanitizeIdentifier k
    sprintf "%stable %sTable_;\n" s k
  m |> Map.fold fn ""

let tableInitialization (dataMap:Map<string,FunctionDefinition>) (unionMap:Map<string,List<FunctionDefinition>>) : string =
  let size = 10;
  let data  s k (v:FunctionDefinition) = 
    let k = sanitizeIdentifier k
    if v.Arguments.Length.Equals 0 then s else
      sprintf "%s    %sTable_ = table_alloc(%d,sizeof(%s));\n" s k size k
  let union s k _ =
    let k = sanitizeIdentifier k
    sprintf "%s    %sTable_ = table_alloc(%d,sizeof(%s));\n" s k size k
  sprintf "void initialize_tables(void){\n%s%s}\n" (Map.fold data "" dataMap) (Map.fold union "" unionMap)

let invert_map (m:Map<'a,'b>) : Map<'b,List<'a>> =
  let fn (s:Map<'b,List<'a>>) k v =
    if s |> Map.containsKey v then
      s |> Map.remove v |> Map.add v (k::s.[v])
    else
      s |> Map.add v (k::[])
  m |> Map.fold fn Map.empty

let generate (originalFilePath:string)
             (program_name:string)
             (rules:UntypedExpression) // <-- ast
             (program:UntypedExpression) // <-- main
             (ctxt:ConcreteExpressionContext) // <-- header
             :unit =
  match rules with
  | Application(Implicit, Keyword(Sequence, _, _) :: rules, pos, _) ->
    let basicgraph = BasicGraph.generate rules ctxt
    let funcMap,dataMap = Map.partition (fun _ (c:FunctionDefinition) -> match c.Kind with Func _ -> true | _ -> false) ctxt.CustomKeywordsMap
    let unionMap = dataMap |> Map.map (fun k v -> v.BaseType.ToString()) |> invert_map |> Map.map (fun k v -> List.map (fun l -> dataMap.[l]) v)
    printfn "#include \"mc_runtime.h\"\n"
    printfn "/* forward struct declaration */\n%s" (forwardDeclareStructs dataMap)
    printfn "/* forward union declaration */\n%s"  (forwardDeclareUnions unionMap)
    printfn "/* struct implementation */\n%s" (implementStructs dataMap)
    printfn "/* union implementation */\n%s" (implementUnions unionMap)
    printfn "/* data table declaration */\n%s" (declareDataTables dataMap)
    printfn "/* union table declaration */\n%s" (declareUnionTables unionMap)
    printfn "%s" (tableInitialization dataMap unionMap)
    printfn "int main(void){\n    initialize_tables();\n    return 0;\n}\n" 
    printfn "/* END */"
  | _ -> failwith "root ast element not a Sequence"
  ()

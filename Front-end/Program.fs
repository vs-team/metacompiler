open Utilities
open ParserMonad
open BasicExpression
open ConcreteExpressionParser

let inline (++) (s:#seq<string>) (d:int) = 
  let bs = [ for i in [1..d] -> " " ] |> Seq.fold (+) ""
  s |> Seq.map (fun x -> bs + x + "\n") |> Seq.fold (+) ""

type Method = {
  Rules      : ResizeArray<string>
  Path       : List<int>
} with
    override m.ToString() =
      sprintf "Run_%A = %s\n" m.Path (m.Rules ++ 2)

type GeneratedClass = 
  {
    Name                : string
    Parameters          : ResizeArray<string>
    mutable Methods     : Map<List<int>, Method>
  } with 
      override m.ToString() =
        sprintf "class %s : IRunnable = { \n%s\n%s\n}" m.Name (m.Parameters ++ 2) ((m.Methods |> Seq.map (fun x -> x.Value.ToString())) ++ 2)

let add_rule inputClass (rule:BasicExpression<_,_,_>) path =
  if inputClass.Methods |> Map.containsKey path |> not then
    inputClass.Methods <- inputClass.Methods |> Map.add path { Rules = ResizeArray(); Path = path }
  inputClass.Methods.[path].Rules.Add(rule.ToString())

let rec process_rules (classes:Map<string,GeneratedClass>) (path:List<int>) (rules:List<BasicExpression<_,_,_>>) = 
  for rule,i in rules |> Seq.mapi (fun i r -> r,i) do
    let path' = i :: path
    let self = 
      match rule with
      | Application(_, Keyword Nesting :: self :: children) -> 
        do process_rules classes path' children
        self
      | self -> self
    match self with
    | Application(Regular, Keyword FractionLine :: (Application(Regular, Keyword DoubleArrow :: input :: output)) :: clauses) ->
      let inputKeyword = 
        match input with
        | Keyword(Custom(k)) -> k
        | Application(Regular, Keyword(Custom(k)) :: _) -> k
        | _ -> failwithf "Cannot extract input keyword from %A" input
      let inputClass = classes.[inputKeyword]
      do add_rule inputClass self path'
    | _ -> failwithf "Malformed rule %A" self
    ()


(*
1. A rule that ticks calls its clauses paying attention to scope
2. Scoped rule methods also call parent methods
3. Define IRunnable interface with union of all possible scope methods
*)

let generateCode (e:BasicExpression<Keyword, Var, _>) (ctxt:ConcreteExpressionContext) = 
  match e with
  | Application(Regular, Keyword Sequence :: rules) ->
    let mutable classes = Map.empty
    for keyword in ctxt.CustomKeywords do
      let newClass = { Name = keyword.Name; Parameters = ResizeArray(); Methods = Map.empty }
      for i = 1 to keyword.LeftAriety do
        newClass.Parameters.Add(sprintf "IRunnable P%d" i)
      for i = 1 to keyword.RightAriety do
        newClass.Parameters.Add(sprintf "IRunnable P%d" (i + keyword.LeftAriety))
      classes <- classes.Add(keyword.Name,newClass)

    do process_rules classes [] rules

    for c in classes do
      let c = c.Value
      do printfn "%s\n\n" (c.ToString())
  | _ -> failwith "Cannot extract rules from input program."

[<EntryPoint>]
let main argv = 
  let ($) p a = p.Parse a

  let input = System.IO.File.ReadAllText @"Content\casanova semantics.mc"

  let output = (program()).Parse (input |> Seq.toList) ConcreteExpressionContext.Empty
  match output with
  | [] -> printfn "Parse error."
  | (x,_,ctxt)::xs -> 
    // printfn "%s" (x.ToString())
    generateCode x ctxt
  0

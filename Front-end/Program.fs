open Utilities
open ParserMonad
open BasicExpression
open ConcreteExpressionParser

let inline (++) (s:#seq<string>) (d:int) = 
  let bs = [ for i in [1..d] -> " " ] |> Seq.fold (+) ""
  s |> Seq.map (fun x -> bs + x + ";\n") |> Seq.fold (+) ""

type Instruction = 
    Var of name : string * expr : string
  | VarAs of name : string * expr : string * as_type : string
  | CheckNull of var_name : string
  | Iterate of var_name : string * expr:BasicExpression<Keyword, Var, unit>
  | Yield of expr:BasicExpression<Keyword, Var, unit>

let rec matchCast (tmp_id:int) (e:BasicExpression<Keyword, Var, unit>) (self:string) (prefix:List<Instruction>) =
  match e with
  | Keyword(Custom k) -> 
    if self <> "this" then
      prefix @ 
      [
        VarAs(sprintf "tmp_%d" tmp_id, self, k)
        CheckNull(sprintf "tmp_%d" tmp_id)
      ], tmp_id+1
    else
      prefix @
      [
        VarAs(sprintf "tmp_%d" tmp_id, self, k)
      ], tmp_id+1
  | Extension(v:Var) ->
      prefix @
      [
        Var(v.Name, self)
      ], tmp_id
  | Application(b,(Keyword(Custom k)) :: es) ->
    let output,self,tmp_id = 
      if self <> "this" then
          prefix @
          [
            VarAs(sprintf "tmp_%d" tmp_id, self, k)
            CheckNull(sprintf "tmp_%d" tmp_id)
          ], sprintf "tmp_%d" tmp_id, tmp_id+1
      else
          prefix @
          [
            Var(sprintf "tmp_%d" tmp_id, self)
          ], sprintf "tmp_%d" tmp_id, tmp_id+1
    // es_i -> self . P_i
    let mutable output = output
    let mutable tmp_id = tmp_id
    for e,i in es |> List.mapi (fun i e -> e,(i+1)) do
      let newOutput, newTempId = matchCast tmp_id e (sprintf "%s.P%d" self i) output
      output <- newOutput
      tmp_id <- newTempId
    output, tmp_id
  | Application(b,e::es) ->
    failwithf "Application not starting with %A cannot be matched" e
  | Application(b,[]) ->
    failwith "Application with empty argument list cannot be matched"
  | Imported() ->
    failwith "Imported match not implemented"
  | Keyword(_) -> 
    failwithf "Non-custom keyword %A cannot be matched" e

type Rule = {
  Input      : BasicExpression<Keyword, Var, unit>
  Output     : BasicExpression<Keyword, Var, unit>
  Clauses    : List<BasicExpression<Keyword, Var, unit> * BasicExpression<Keyword, Var, unit>>
} with
    override r.ToString() =
      let i,tmp_id = matchCast 0 r.Input "this" []
      let mutable o = []
      let mutable tmp_id = tmp_id
      for c_i,c_o in r.Clauses do
        o <- Iterate(sprintf "tmp_%d" tmp_id, c_i) :: o
        let o',tmp_id' = matchCast (tmp_id+1) c_o (sprintf "tmp_%d" tmp_id) o
//        // ??????????????????????????????????????????????????????????????????????????????????????????????????????????
//        let o'',tmp_id'' = matchCast tmp_id' r.Input (sprintf "tmp_%d" tmp_id') o' // ???????????????????????????????
//        // ??????????????????????????????????????????????????????????????????????????????????????????????????????????
        o <- o'
        tmp_id <- tmp_id'
      o <- i @ o @ [Yield r.Output]
      sprintf "%A" o

type Method = {
  Rules      : ResizeArray<Rule>
  Path       : List<int>
} with
    override m.ToString() =
      sprintf "Run_%A = %s\n" m.Path ((m.Rules |> Seq.map (fun r -> r.ToString())) ++ 2)

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
  match rule with
  | Application(Regular, Keyword FractionLine :: (Application(Regular, Keyword DoubleArrow :: input :: output :: [])) :: clauses) ->
    inputClass.Methods.[path].Rules.Add(
      { Input = input
        Clauses = 
          [ for c in clauses do
              match c with
              | Application(_, Keyword DoubleArrow :: c_i :: c_o :: []) -> yield c_i, c_o
              | _ -> failwithf "Cannot clause %A" c
          ] 
        Output = output })
  | _ ->
    failwithf "Cannot extract rule shape from %A" rule

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

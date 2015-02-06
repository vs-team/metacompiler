open Utilities
open ParserMonad
open BasicExpression
open ConcreteExpressionParser

let (!) (s:string) =
  s
   .Replace(":=", "_DefinedAs")
   .Replace("->", "_Arrow")
   .Replace("\'", "_Prime")
   .Replace("\\", "_opSlash")
   .Replace("$", "_Dollar")
   .Replace(".", "_opDot")
   .Replace("*", "_opMultiplication")
   .Replace("+", "_opAddition")
   .Replace("-", "_opSubtraction")
   .Replace("/", "_opDivision")
   .Replace("|", "_opVBar")
   .Replace(",", "_Comma")
   .Replace(";", "_Semicolon")
   .Replace(":", "_Colon")
   .Replace("if", "_If")
   .Replace("then", "_Then")
   .Replace("else", "_Else")
   .Replace("for", "_For")
   .Replace("true", "_True")
   .Replace("false", "_False")
   .Replace("with", "_With")

let inline (++) (s:#seq<string>) (d:int) = 
  let bs = [ for i in [1..d] -> " " ] |> Seq.fold (+) ""
  s |> Seq.map (fun x -> bs + x + "\n") |> Seq.fold (+) ""


let escape (s:string) = 
  s.Replace("\\", "\\\\")



type Path = Path of List<int>
  with
    override this.ToString() = 
      match this with Path(p) -> p |> List.rev |> Seq.map (fun i -> string i + "_") |> Seq.fold (+) ""
    member this.Tail =
      match this with 
      | Path(p::ps) -> Path ps
      | _ -> failwith "Cannot reduce empty path"
    member this.ParentCall =
      match this with
      | Path([]) -> ""
      | Path(p::ps) -> sprintf "foreach(var p in Run%s()) yield return p;" (Path(ps).ToString())


type Instruction = 
    Var of name : string * expr : string
  | VarAs of name : string * expr : string * as_type : string
  | CheckNull of var_name : string
  | Iterate of var_name : string * tmp_var_name : string * expr:BasicExpression<Keyword, Var, Literal> * path : Path
  | Compare of comparison : Keyword * expr1:BasicExpression<Keyword, Var, Literal> * expr2:BasicExpression<Keyword, Var, Literal>
  | Yield of expr:BasicExpression<Keyword, Var, Literal>

let create_element (ctxt:ConcreteExpressionContext) = 
  let rec create_element (expectedType:KeywordArgument) = 
    function
    | Keyword(Custom k) | Application(Regular,(Keyword(Custom k)) :: []) -> 
      sprintf "new %s()" !k, []
    | Application(b,(Keyword(Custom k)) :: es) ->
      let actualKeyword = ctxt.CustomKeywordsMap.[k]
      let args,cargs = es |> Seq.mapi (fun i e -> create_element actualKeyword.Arguments.[i] e) |> Seq.reduce (fun (s,cs) (x,cx) -> sprintf "%s, %s" s x, cs @ cx)
      sprintf "new %s(%s)" !k args, cargs
    | Extension(v:Var) ->
      let expectedType =
        match expectedType with
        | Native t -> t
        | Defined d -> !d
      sprintf "%s as %s" !v.Name expectedType, [sprintf "%s is %s" !v.Name expectedType]
    | Application(b,e::es) ->
      failwithf "Application not starting with %A cannot be created" e
    | Application(b,[]) ->
      failwith "Application with empty argument list cannot be created"
    | Imported l ->
      match l with
      | StringLiteral l -> sprintf "\"%s\"" l, []
      | _ -> failwith "Non-string literal not supported for element creation"
    | Keyword(k) -> 
      failwithf "Non-custom keyword %A cannot be matched" k
  function
  | Keyword(Custom k) | Application(Regular,(Keyword(Custom k)) :: []) -> 
    sprintf "new %s()" !k, []
  | Application(b,(Keyword(Custom k)) :: es) ->
    let actualKeyword = ctxt.CustomKeywordsMap.[k]
    let args,cargs = es |> Seq.mapi (fun i e -> create_element actualKeyword.Arguments.[i] e) |> Seq.reduce (fun (s,cs) (x,cx) -> sprintf "%s, %s" s x, cs @ cx)
    sprintf "new %s(%s)" !k args, cargs
  | Extension(v:Var) ->
    sprintf "%s" !v.Name, []
  | Application(b,e::es) ->
    failwithf "Application not starting with %A cannot be created" e
  | Application(b,[]) ->
    failwith "Application with empty argument list cannot be created"
  | Imported l ->
    match l with
    | StringLiteral l -> sprintf "\"%s\"" l, []
    | _ -> failwith "Non-string literal not supported for element creation"
  | Keyword(k) -> 
    failwithf "Non-custom keyword %A cannot be matched" k

let rec generate_instructions (ctxt:ConcreteExpressionContext) = 
  function 
  | [] -> ""
  | x :: xs ->
    match x with
    | Var(name, expr) -> 
      sprintf "var %s = %s; %s" !name expr (generate_instructions ctxt xs)
    | VarAs(name, expr, as_type) ->
      sprintf "var %s = %s as %s; %s" !name expr !as_type (generate_instructions ctxt xs)
    | CheckNull(var_name) ->
      sprintf "\nif (%s != null) { %s }" !var_name (generate_instructions ctxt xs)
    | Compare(comparison, expr1, expr2) ->
      let newElement1, creationConstraints1 = create_element ctxt expr1
      let newElement2, creationConstraints2 = create_element ctxt expr2
      let creationConstraints = creationConstraints1 @ creationConstraints2
      let comparison = match comparison with | Equals -> "==" | NotEquals -> "!=" | _ -> failwith "Unsupported"
      if creationConstraints.IsEmpty |> not then
        let creationConstraints = creationConstraints |> Seq.reduce (fun s x -> sprintf "%s && %s" s x)
        sprintf "\nif(%s) { \nif(%s %s %s) { %s } }" creationConstraints newElement1 comparison newElement2 (generate_instructions ctxt xs)
      else 
        sprintf "\nif(%s %s %s) { %s }" newElement1 comparison newElement2 (generate_instructions ctxt xs)
    | Iterate(var_name, tmp_var_name, expr, path) ->
      let newElement, creationConstraints = create_element ctxt expr
      if creationConstraints.IsEmpty |> not then
        let creationConstraints = creationConstraints |> Seq.reduce (fun s x -> sprintf "%s && %s" s x)
        sprintf "\nif(%s) { \nvar %s = %s;\nforeach (var %s in %s.Run%s()) { %s } }" creationConstraints !tmp_var_name newElement !var_name !tmp_var_name (path.ToString()) (generate_instructions ctxt xs)
      else 
        sprintf "\nvar %s = %s;\nforeach (var %s in %s.Run%s()) { %s }" !tmp_var_name newElement !var_name !tmp_var_name (path.ToString()) (generate_instructions ctxt xs)
    | Yield(expr) ->
      let newElement, creationConstraints = create_element ctxt expr
      if creationConstraints.IsEmpty |> not then
        let creationConstraints = creationConstraints |> Seq.reduce (fun s x -> sprintf "%s && %s" s x)
        sprintf "\nif(%s) { \nvar result = %s;\nyield return result; %s }" creationConstraints newElement (generate_instructions ctxt xs)
      else
        sprintf "\nvar result = %s;\nyield return result; %s" newElement (generate_instructions ctxt xs)

let rec matchCast (tmp_id:int) (e:BasicExpression<Keyword, Var, Literal>) (self:string) (prefix:List<Instruction>) =
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
  | Imported(l) ->
    failwith "Imported match not implemented"
  | Keyword(_) -> 
    failwithf "Non-custom keyword %A cannot be matched" e

type Rule = {
  Input      : BasicExpression<Keyword, Var, Literal>
  Output     : BasicExpression<Keyword, Var, Literal>
  Clauses    : List<Keyword * BasicExpression<Keyword, Var, Literal> * BasicExpression<Keyword, Var, Literal>>
  Path       : Path
  HasScope   : bool
} with
    member r.ToString (ctxt:ConcreteExpressionContext) =
      let path = 
        if r.HasScope then
          r.Path
        else
          r.Path.Tail
      let i,tmp_id = matchCast 0 r.Input "this" []
      let mutable o = []
      let mutable tmp_id = tmp_id
      for k,c_i,c_o in r.Clauses do
        match k with
        | DoubleArrow ->
          o <- o @ [Iterate(sprintf "tmp_%d" tmp_id, sprintf "tmp_%d" (tmp_id+1), c_i, path)]
          let o',tmp_id' = matchCast (tmp_id+2) c_o (sprintf "tmp_%d" tmp_id) []
          o <- o @ o'
          tmp_id <- tmp_id'
        | Equals | NotEquals -> o <- o @ [Compare(k, c_i, c_o)]
        | _ -> failwithf "Unsupported clause keyword %A for code generation" k
      o <- i @ o @ [Yield r.Output]
      sprintf "{\n%s\n}" (generate_instructions ctxt o)

type Method = {
  Rules      : ResizeArray<Rule>
  Path       : Path
} with
    member m.ToString(ctxt:ConcreteExpressionContext) =
      sprintf "public IEnumerable<IRunnable> Run%s() { %s %s }" (m.Path.ToString()) ((m.Rules |> Seq.map (fun r -> r.ToString(ctxt))) ++ 2) m.Path.ParentCall

type Parameter = 
  {
    Name    : string
    IsLeft  : bool
    Type    : KeywordArgument
  }

type GeneratedClass = 
  {
    Name                : string
    Interface           : string
    Parameters          : ResizeArray<Parameter>
    mutable Methods     : Map<Path, Method>
  } with
      member this.MethodPaths = seq{ for x in this.Methods -> x.Key } |> Set.ofSeq
      member c.ToString(all_method_paths:Set<Path>, ctxt:ConcreteExpressionContext) =
        let cons =
          if c.Parameters.Count <> 0 then
            let pars = c.Parameters |> Seq.map (fun x -> sprintf "%s %s" x.Type.Argument x.Name) |> Seq.reduce (fun s x -> sprintf "%s, %s" s x)
            let args = c.Parameters |> Seq.map (fun x -> sprintf "this.%s = %s;" x.Name x.Name) |> Seq.reduce (fun s x -> sprintf "%s %s" s x)
            sprintf "public %s(%s) {%s}\n" !c.Name pars args
          else
            sprintf "public %s() {}\n" !c.Name
        let parameters =
          c.Parameters |> Seq.map (fun p -> sprintf "public %s %s;\n" p.Type.Argument p.Name) |> Seq.fold (+) ""
        let missing_methods =
          let missing_paths = all_method_paths - c.MethodPaths
          [
            for p in missing_paths do
            let parent_call = p.ParentCall
            let parent_or_empty_call =
              if parent_call <> "" then
                parent_call
              else
                "foreach (var p in Enumerable.Range(0,0)) yield return null;"
            yield sprintf "public IEnumerable<IRunnable> Run%s() { %s }\n" (p.ToString()) parent_or_empty_call
          ] |> Seq.fold (+) ""
        let to_string =
          if c.Parameters.Count > 0 then
            let leftParameters = c.Parameters |> Seq.filter (fun x -> x.IsLeft) |> Seq.map (fun x -> sprintf "res += %s.ToString();\n" x.Name) |> Seq.fold (+) ""
            let rightParameters = c.Parameters |> Seq.filter (fun x -> x.IsLeft |> not) |> Seq.map (fun x -> sprintf "res += %s.ToString();\n" x.Name) |> Seq.fold (+) ""
            sprintf "public override string ToString() {\n var res = \"(\"; \n%s\n res += \"%s\"; %s\n res += \")\";\n return res;\n}\n" leftParameters (escape c.Name) rightParameters
          else
            sprintf "public override string ToString() {\nreturn \"%s\";\n}\n" (escape c.Name)
        sprintf "public class %s : %s {\n%s\n%s\n%s\n%s\n%s}\n\n" !c.Name !c.Interface parameters cons ((c.Methods |> Seq.map (fun x -> x.Value.ToString(ctxt))) ++ 2) missing_methods to_string

let add_rule inputClass (rule:BasicExpression<_,_,Literal>) (rule_path:Path) (hasScope:bool) =
  let method_path = rule_path.Tail
  if inputClass.Methods |> Map.containsKey method_path |> not then
    inputClass.Methods <- inputClass.Methods |> Map.add method_path { Rules = ResizeArray(); Path = method_path }
  match rule with
  | Application(Regular, Keyword FractionLine :: (Application(Regular, Keyword DoubleArrow :: input :: output :: [])) :: clauses) ->
    inputClass.Methods.[method_path].Rules.Add(
      { Input = input
        Clauses = 
          [ for c in clauses do
              match c with
              | Application(_, Keyword DoubleArrow :: c_i :: c_o :: []) -> yield DoubleArrow, c_i, c_o
              | Application(_, Keyword Equals :: c_i :: c_o :: []) -> yield Equals, c_i, c_o
              | Application(_, Keyword NotEquals :: c_i :: c_o :: []) -> yield NotEquals, c_i, c_o
              | _ -> failwithf "Cannot process clause %A" c
          ] 
        Output   = output
        Path     = rule_path
        HasScope = hasScope })
  | _ ->
    failwithf "Cannot extract rule shape from %A" rule

let rec process_rules (classes:Map<string,GeneratedClass>) (path:List<int>) (rules:List<BasicExpression<_,_,Literal>>) = 
  for rule,i in rules |> Seq.mapi (fun i r -> r,i) do
    let path' = i :: path
    let self,hasScope = 
      match rule with
      | Application(_, Keyword Nesting :: self :: children) -> 
        do process_rules classes path' children
        self,true
      | self -> self,false
    match self with
    | Application(Regular, Keyword FractionLine :: (Application(Regular, Keyword DoubleArrow :: input :: output)) :: clauses) ->
      let inputKeyword = 
        match input with
        | Keyword(Custom(k)) -> k
        | Application(Regular, Keyword(Custom(k)) :: _) -> k
        | _ -> failwithf "Cannot extract input keyword from %A" input
      let inputClass = classes.[inputKeyword]
      do add_rule inputClass self (Path path') hasScope
    | _ -> failwithf "Malformed rule %A" self
    ()


let generateCode program_name (rules:BasicExpression<Keyword, Var, Literal>) (program:BasicExpression<Keyword, Var, Literal>) (ctxt:ConcreteExpressionContext) = 
  match rules with
  | Application(Regular, Keyword Sequence :: rules) ->
    let mutable classes = Map.empty
    for keyword in ctxt.CustomKeywords do
      let newClass = { Name = keyword.Name; Interface = keyword.Class; Parameters = ResizeArray(); Methods = Map.empty }
      for t,i in keyword.LeftArguments |> Seq.mapi (fun i p -> p,i+1) do
        newClass.Parameters.Add({ Name = sprintf "P%d" i; IsLeft = true; Type = t })
      for t,i in keyword.RightArguments |> Seq.mapi (fun i p -> p,i+1) do
        newClass.Parameters.Add({ Name = sprintf "P%d" (i + keyword.LeftAriety); IsLeft = false; Type = t })
      classes <- classes.Add(keyword.Name,newClass)

    do process_rules classes [] rules

    let classes = classes
    let interfaces = [ for k in ctxt.CustomKeywords -> k.Class ] |> Set.ofSeq
    let interfacesCode = 
      [
        for i in interfaces do
        yield sprintf "public interface %s : IRunnable {}\n" i
      ] |> Seq.fold (+) ""
    let all_method_paths =
      seq{
        for c in classes -> c.Value.MethodPaths
      } |> Seq.reduce (+)
    let run_methods =
      all_method_paths |> Seq.map (fun p -> sprintf "IEnumerable<IRunnable> Run%s();\n" (p.ToString())) |> Seq.reduce (+)
    let prelude = sprintf "using System.Collections.Generic;\nusing System.Linq;\nnamespace %s {\n public interface IRunnable { %s }" program_name run_methods
    let main = sprintf "public class EntryPoint {\n public IEnumerable<IRunnable> Run()\n{\nforeach(var x in %s.Run())\nyield return x;\n}\n}\n" (create_element ctxt program |> fst)
    [
      yield prelude
      yield "\n\n\n"
      yield interfacesCode
      yield "\n\n\n"
      for c in classes do
        let c = c.Value
        yield c.ToString(all_method_paths, ctxt)
      yield "\n\n\n"
      yield main
      yield "\n}\n"
    ] |> Seq.fold (+) "" |> (fun txt -> System.IO.File.WriteAllText(program_name + ".cs", txt))
  | _ -> failwith "Cannot extract rules from input program."

[<EntryPoint>]
let main argv = 
  let ($) p a = p.Parse a

  let casanova = System.IO.File.ReadAllText @"Content\casanova semantics.mc"
  let peano = "PeanoNumbers", System.IO.File.ReadAllText @"Content\peano numbers.mc", "(s(s(z))) * (s(s(z)))\n"
  let lambda_calculus = "LambdaCalculus", System.IO.File.ReadAllText @"Content\lambda calculus.mc", @"(\$""y"".$""y"" | \$""y"".$""y"") | ($""x"" | $""z"")" + "\n"

  let title, rules, input = peano

//  do debug_expr <- true
//  do debug_rules <- true

  match (program()).Parse (rules |> Seq.toList) ConcreteExpressionContext.Empty with
  | [] -> printfn "Parse error in rules."
  | (x,_,ctxt)::xs -> 
    do printfn "Rules parsed correctly."
    match expr().Parse (input |> Seq.toList) ctxt with
    | [] -> printfn "Parse error in expression %s." input
    | (y,_,ctxt')::ys ->
      printfn "%s => " (y.ToString())
      for z in PeanoNumbers.EntryPoint().Run() do
        printfn "%s" (z.ToString()) 
      for z in LambdaCalculus.EntryPoint().Run() do
        printfn "%s" (z.ToString()) 
      generateCode title x y ctxt
  0

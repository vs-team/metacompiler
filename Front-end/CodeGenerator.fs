module CodeGenerator

open System
open Utilities
open ParserMonad
open BasicExpression
open ConcreteExpressionParser

let private generateLineDirectives = false

let (!) (s:string) =
  s
   .Replace(":=", "_DefinedAs")
   .Replace("->", "_Arrow")
   .Replace("\'", "_Prime")
   .Replace("\\", "_opSlash")
   .Replace("&&", "_opAnd")
   .Replace("||", "_opOr")
   .Replace("$", "_opDollar")
   .Replace("!", "_opBang")
   .Replace("?", "_opQuestion")
   .Replace(".", "_opDot")
   .Replace("*", "_opMultiplication")
   .Replace("+", "_opAddition")
   .Replace("-", "_opSubtraction")
   .Replace("/", "_opDivision")
   .Replace("|", "_opVBar")
   .Replace(",", "_Comma")
   .Replace(";", "_Semicolon")
   .Replace(":", "_Colon")
   .Replace(">", "_opGreaterThan")
   .Replace("=", "_opEquals")
   .Replace("if", "_If")
   .Replace("then", "_Then")
   .Replace("else", "_Else")
   .Replace("for", "_For")
   .Replace("true", "_True")
   .Replace("false", "_False")
   .Replace("with", "_With")
   .Replace("[", "<") // keep last or > becomes opGreaterThan
   .Replace("]", ">") // see previous comment

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
  | CustomCheck of condition : string
  | Iterate of var_name : string * tmp_var_name : string * expr:BasicExpression<Keyword, Var, Literal, Position> * path : Path
  | Compare of comparison : Keyword * expr1:BasicExpression<Keyword, Var, Literal, Position> * expr2:BasicExpression<Keyword, Var, Literal, Position>
  | Inline of e:BasicExpression<Keyword, Var, Literal, Position>
  | Yield of expr:BasicExpression<Keyword, Var, Literal, Position>
  | GenericApplicationInstruction of string

let rec create_element (ctxt:ConcreteExpressionContext) = 
  let rec create_element' (expectedType:KeywordArgument) = 
    function
    | Keyword(Custom k, _) 
    | Application(Regular,(Keyword(Custom k, _)) :: [], _)
    | Application(Implicit,(Keyword(Custom k, _)) :: [], _) -> 
      sprintf "%s.Create()" !k, []
    | Application(Regular,(Keyword(Custom k, _)) :: es, pos)
    | Application(Implicit,(Keyword(Custom k, _)) :: es, pos) ->
      let actualKeyword = ctxt.CustomKeywordsMap.[k]
      let args,cargs = es |> Seq.mapi (fun i e -> create_element' actualKeyword.Arguments.[i] e) |> Seq.reduce (fun (s,cs) (x,cx) -> sprintf "%s, %s" s x, cs @ cx)
      sprintf "%s.Create(%s)" !k args, cargs
    | Extension(v:Var, _) ->
      match expectedType with
      | Native t ->
        sprintf "%s" !v.Name, []
      | _ ->
        let expectedTypeString =
          let rec print =
            function
            | Native t -> t
            | Defined d -> !d
            | Generic(t,a::args) -> 
              let pars = a::args |> Seq.map print |> Seq.reduce (fun s x -> sprintf "%s, %s" s x)
              sprintf "%s<%s>" t pars
            | _ -> failwith "Generic types must have at least one argument."
          print expectedType
        sprintf "%s as %s" !v.Name expectedTypeString, [sprintf "%s is %s" !v.Name expectedTypeString]
    | Application(Angle,e::[],pos) ->
      let res = generate_inline e
      sprintf "%s" res, []
    | Application(Angle,e::es,pos) ->
      let res = generate_inline (Application(Implicit,e::es,pos))
      sprintf "%s" res, []
    | Application(Regular,e::[],pos) ->
      let res, conds = create_element' expectedType e
      sprintf "(%s)" res, conds
    | Application(b,e::es,pos) ->
      failwithf "Application %A cannot be created" (Application(b,e::es,pos))
    | Application(b,[],pos) ->
      failwith "Application with empty argument list cannot be created"
    | Imported(l,pos) -> l.ToString(), []
    | Keyword(k, _) -> 
      failwithf "Non-custom keyword %A cannot be matched" k
  function
  | Keyword(Custom k, _) 
  | Application(Regular,(Keyword(Custom k, _)) :: [],_)
  | Application(Implicit,(Keyword(Custom k, _)) :: [],_) -> 
    sprintf "%s.Create()" !k, []
  | Application(Regular,(Keyword(Custom k, _)) :: es,pos)
  | Application(Implicit,(Keyword(Custom k, _)) :: es,pos) ->
    let actualKeyword = ctxt.CustomKeywordsMap.[k]
    let args,cargs = es |> Seq.mapi (fun i e -> create_element' actualKeyword.Arguments.[i] e) |> Seq.reduce (fun (s,cs) (x,cx) -> sprintf "%s, %s" s x, cs @ cx)
    let trimmedArgs = 
      if es |> List.length = 1 && args.[0] = '(' && args.[args.Length - 1] = ')' then
        args.Substring(1, args.Length - 2)
      else
        args
    let res = sprintf "%s.Create(%s)" !k trimmedArgs, cargs
    res
  | Application(Angle,e::es,pos) ->
    let res = generate_inline (Application(Regular,e::es,pos))
    sprintf "%s" res, []
  | Extension(v:Var, _) ->
    sprintf "%s" !v.Name, []
  | Application(b,e::es,pos) ->
    failwithf "Application %A cannot be created" (Application(b,e::es,pos))
  | Application(b,[],pos) ->
    failwith "Application with empty argument list cannot be created"
  | Imported(l,pos) -> l.ToString(), []
  | Keyword(k, _) -> 
    failwithf "Non-custom keyword %A cannot be matched" k

and generate_inline =
  function
  | Keyword(Custom k, _) ->
    sprintf "%s" k
  | Application(_, [], _) ->
    ""
  | Application(Regular,(Keyword(Custom k, _)) :: [],_) -> 
    sprintf "(%s)" k
  | Application(Square,(Keyword(Custom k, _)) :: l :: r :: [],pos) when ConcreteExpressionContext.CSharp.CustomKeywordsMap.[k].LeftAriety = 1 ->
    sprintf "<%s%s%s>" (l |> generate_inline) k (r |> generate_inline)
  | Application(Regular,(Keyword(Custom k, _)) :: l :: r :: [],pos) when ConcreteExpressionContext.CSharp.CustomKeywordsMap.[k].LeftAriety = 1 ->
    sprintf "(%s%s%s)" (l |> generate_inline) k (r |> generate_inline)
  | Extension(v:Var, _) ->
    sprintf "%s" v.Name
  | Application(Angle, (Keyword(Inlined, _)) :: args, di) ->
    let res = Application(Regular, args, di) |> generate_inline
    res
  | Application(Regular, a :: [], _) ->
    sprintf "(%s)" (a |> generate_inline)
  | Application(Regular, arg :: args, _) ->
    let pars = [ for x in args -> x |> generate_inline ] |> Seq.reduce (fun s x -> sprintf "%s, %s" s x)
    sprintf "%s(%s)" (arg |> generate_inline) pars
  | Application(Angle, arg :: args, pos) ->
    let res = Application(Implicit, arg :: args, pos) |> generate_inline
    res
  | Application(Implicit,(Keyword(Custom k, _)) :: [],_) -> 
    sprintf "%s" k
  | Application(Implicit,(Keyword(Custom k, _)) :: l :: r :: [],pos) when ConcreteExpressionContext.CSharp.CustomKeywordsMap.[k].LeftAriety = 1 ->
    sprintf "%s%s%s" (l |> generate_inline) k (r |> generate_inline)
  | Application(Implicit, arg :: [], _) ->
    arg |> generate_inline
  | Application(Implicit, arg :: args, _) ->
    let pars = [ for x in args -> x |> generate_inline ] |> Seq.reduce (fun s x -> sprintf "%s, %s" s x)
    let res = sprintf "%s %s" (arg |> generate_inline) pars
    res
  | Imported(l,di) ->
    l.ToString()
  | i -> 
    failwithf "Unsupported inline pattern %A" i
  

let rec generate_instructions (debugPosition:Position) (originalFilePath:string) (ctxt:ConcreteExpressionContext) = 
  function 
  | [] -> ""
  | x :: xs ->
    let newLine = 
      if generateLineDirectives then sprintf "\n #line %d \"%s\"\n" debugPosition.Line originalFilePath
      else "\n"
    match x with
    | Var(name, expr) -> 
      sprintf "var %s = %s; %s" !name expr (generate_instructions debugPosition originalFilePath ctxt xs)
    | VarAs(name, expr, as_type) ->
      sprintf "var %s = %s as %s; %s" !name expr !as_type (generate_instructions debugPosition originalFilePath  ctxt xs)
    | CheckNull(var_name) ->
      sprintf "%sif (%s != null) { %s }" newLine !var_name (generate_instructions debugPosition originalFilePath  ctxt xs)
    | Compare(comparison, expr1, expr2) ->
      let newElement1, creationConstraints1 = create_element ctxt expr1
      let newElement2, creationConstraints2 = create_element ctxt expr2
      let creationConstraints = creationConstraints1 @ creationConstraints2
      let preComparison, inlineComparison, postComparison = 
        match comparison with 
        | Equals -> "", ".Equals(", ")" 
        | NotEquals -> "!", ".Equals(", ")" 
        | GreaterThan -> "", ">", "" 
        | GreaterOrEqual -> "", ">=", "" 
        | SmallerThan -> "", "<", "" 
        | SmallerOrEqual -> "", "<=", "" 
        | _ -> failwith "Unsupported"
      if creationConstraints.IsEmpty |> not then
        let creationConstraints = creationConstraints |> Seq.reduce (fun s x -> sprintf "%s && %s" s x)
        sprintf "%sif(%s) { %sif(%s%s%s%s%s) { %s } }" newLine creationConstraints newLine preComparison newElement1 inlineComparison newElement2 postComparison (generate_instructions debugPosition originalFilePath  ctxt xs)
      else 
        sprintf "%sif(%s%s%s%s%s) { %s }" newLine preComparison newElement1 inlineComparison newElement2 postComparison (generate_instructions debugPosition originalFilePath  ctxt xs)
    | Iterate(var_name, tmp_var_name, expr, path) ->
      let newElement, creationConstraints = create_element ctxt expr
      if creationConstraints.IsEmpty |> not then
        let creationConstraints = creationConstraints |> Seq.reduce (fun s x -> sprintf "%s && %s" s x)
        sprintf "%sif(%s) { %svar %s = %s;%sforeach (var %s in %s.Run%s()) { %s } }" newLine creationConstraints newLine !tmp_var_name newElement newLine !var_name !tmp_var_name (path.ToString()) (generate_instructions debugPosition originalFilePath  ctxt xs)
      else 
        sprintf "%svar %s = %s;%sforeach (var %s in %s.Run%s()) { %s }" newLine !tmp_var_name newElement newLine !var_name !tmp_var_name (path.ToString()) (generate_instructions debugPosition originalFilePath ctxt xs)
    | CustomCheck(condition) ->
      sprintf "%sif (%s) { %s }" newLine condition (generate_instructions debugPosition originalFilePath  ctxt xs)
    | Inline(c) -> 
      sprintf "%s%s;%s%s" newLine (generate_inline c) newLine (generate_instructions debugPosition originalFilePath  ctxt xs)
    | Yield(expr) ->
      let newElement, creationConstraints = create_element ctxt expr
      if creationConstraints.IsEmpty |> not then
        let creationConstraints = creationConstraints |> Seq.reduce (fun s x -> sprintf "%s && %s" s x)
        sprintf "%sif(%s) { %svar result = %s;%syield return result; %s }" newLine creationConstraints newLine newElement newLine (generate_instructions debugPosition originalFilePath ctxt xs)
      else
        sprintf "%svar result = %s;%syield return result; %s" newLine newElement newLine (generate_instructions debugPosition originalFilePath ctxt xs)
    | GenericApplicationInstruction(args) -> sprintf "<%s>" args

let rec matchCast (tmp_id:int) (e:BasicExpression<Keyword, Var, Literal, Position>) (self:string) (prefix:List<Instruction>) =
  match e with
  | Keyword(Custom k, _) -> 
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
  | Extension(v:Var, _) ->
      prefix @
      [
        Var(v.Name, self)
      ], tmp_id
  | Application(Angle,es,pos) ->
    let output,tmp_id = 
      let expectedSelf = generate_inline(Application(Angle,es,pos))
      prefix @
      [
        CustomCheck(sprintf "%s == %s" self expectedSelf)
      ], tmp_id
    // es_i -> self . P_i
    output, tmp_id
  | Application(b,(Keyword(Custom k, _)) :: es,pos) ->
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
  | Application(Regular,e::[],pos) ->
    matchCast tmp_id e self prefix
  | Application(Square,e::es,pos) ->
    let res = prefix @ [GenericApplicationInstruction(generate_inline(Application(Implicit,e::es,pos)))]
    res, tmp_id
  | Application(b,e::es,pos) ->
    failwith "Application %A cannot be matched" e
  | Application(b,[],pos) ->
    failwith "Application with empty argument list cannot be matched"
  | Imported(l,pos) ->
    let cond = sprintf "%s == %s" self (l.ToString())
    prefix @
    [
      CustomCheck(cond)
    ], tmp_id
  | Keyword(_) -> 
    failwithf "Non-custom keyword %A cannot be matched" e

type Rule = {
  Position   : Position
  Input      : BasicExpression<Keyword, Var, Literal, Position>
  Output     : BasicExpression<Keyword, Var, Literal, Position>
  Clauses    : List<Keyword * BasicExpression<Keyword, Var, Literal, Position> * BasicExpression<Keyword, Var, Literal, Position>>
  Path       : Path
  HasScope   : bool
} with
    member r.ToString (ctxt:ConcreteExpressionContext,originalFilePath) =
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
        | Equals | NotEquals | GreaterThan | GreaterOrEqual | SmallerThan | SmallerOrEqual -> 
          o <- o @ [Compare(k, c_i, c_o)]
        | DefinedAs -> 
          match c_i with
          | Extension(iVar, _) ->
            let oExpr,constraints = create_element ctxt c_o
            if constraints.IsEmpty |> not then
              o <- o @ [CustomCheck(constraints |> Seq.reduce (fun s x -> sprintf "%s && %s" s x))]
            o <- o @ [Var(iVar.Name, oExpr)]
          | _ -> failwithf "Invalid definition. Expected a variable name, found %A" c_i
        | Inlined ->
            o <- o @ [Inline(c_i)]
        | _ -> failwithf "Unsupported clause keyword %A for code generation" k
      o <- i @ o @ [Yield r.Output]
      if generateLineDirectives then
        sprintf "\n { \n #line %d \"%s\"\n%s\n } \n" r.Position.Line originalFilePath (generate_instructions r.Position originalFilePath ctxt o)
      else
        sprintf "\n { \n %s\n } \n" (generate_instructions r.Position originalFilePath ctxt o)

type Method = {
  Rules      : ResizeArray<Rule>
  Path       : Path
} with
    member m.ToString(ctxt:ConcreteExpressionContext,originalFilePath) =
      sprintf "public IEnumerable<IRunnable> Run%s() { %s %s }" (m.Path.ToString()) ((m.Rules |> Seq.map (fun r -> r.ToString(ctxt,originalFilePath))) ++ 2) m.Path.ParentCall

type Parameter = 
  {
    Name    : string
    IsLeft  : bool
    Type    : KeywordArgument
  }

type GeneratedClass = 
  {
    BasicName           : string
    Interface           : string
    GenericArguments    : List<KeywordArgument>
    Parameters          : ResizeArray<Parameter>
    mutable Methods     : Map<Path, Method>
  } with
      member c.Name = 
        if c.GenericArguments.IsEmpty |> not then  
          let args = c.GenericArguments |> Seq.map (fun x -> x.Argument) |> Seq.reduce (fun s x -> sprintf "%s, %s" s x)
          sprintf "%s<%s>" !c.BasicName args
        else
          !c.BasicName
      member this.MethodPaths = seq{ for x in this.Methods -> x.Key } |> Set.ofSeq
      member c.ToString(all_method_paths:Set<Path>, ctxt:ConcreteExpressionContext, originalFilePath) =
        let cons =
          if c.Parameters.Count <> 0 then
            let pars = c.Parameters |> Seq.map (fun x -> sprintf "%s %s" x.Type.Argument x.Name) |> Seq.reduce (fun s x -> sprintf "%s, %s" s x)
            let args = c.Parameters |> Seq.map (fun x -> sprintf "this.%s = %s;" x.Name x.Name) |> Seq.reduce (fun s x -> sprintf "%s %s" s x)
            sprintf "public %s(%s) {%s}\n" !c.BasicName pars args
          else
            sprintf "public %s() {}\n" !c.BasicName
        let staticCons =
          if c.Parameters.Count <> 0 then
            let pars = c.Parameters |> Seq.map (fun x -> sprintf "%s %s" x.Type.Argument x.Name) |> Seq.reduce (fun s x -> sprintf "%s, %s" s x)
            let args = c.Parameters |> Seq.map (fun x -> sprintf "%s" x.Name) |> Seq.reduce (fun s x -> sprintf "%s, %s" s x)
            sprintf "public static %s Create(%s) { return new %s(%s); }\n" c.Name pars c.Name args
          else
            sprintf "public static %s Create() { return new %s(); }\n" c.Name c.Name
        let cons = cons + staticCons
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
            let printParameter (p:Parameter) = 
              match p.Type with
              | Generic _ ->
                sprintf "if (%s is System.Collections.IEnumerable) { res += \"{\"; foreach(var x in %s as System.Collections.IEnumerable) res += x.ToString(); res += \"}\";  } else { res += %s.ToString(); } \n" p.Name p.Name p.Name
              | _ -> 
                sprintf "res += %s.ToString(); \n" p.Name
            let leftParameters = c.Parameters |> Seq.filter (fun x -> x.IsLeft) |> Seq.map (fun x -> printParameter x) |> Seq.fold (+) ""
            let rightParameters = c.Parameters |> Seq.filter (fun x -> x.IsLeft |> not) |> Seq.map (fun x -> printParameter x) |> Seq.fold (+) ""
            sprintf "public override string ToString() {\n var res = \"(\"; \n%s\n res += \"%s\"; %s\n res += \")\";\n return res;\n}\n" leftParameters (escape c.BasicName) rightParameters
          else
            sprintf "public override string ToString() {\nreturn \"%s\";\n}\n" (escape c.BasicName)
        let equals =
          if c.Parameters.Count > 0 then
            let parameters = c.Parameters |> Seq.map (fun x -> sprintf "this.%s.Equals(tmp.%s)" x.Name x.Name) |> Seq.reduce (fun s x -> sprintf "%s && %s" s x)
            sprintf "public override bool Equals(object other) {\n var tmp = other as %s;\n if(tmp != null) return %s; \n else return false; }\n" c.Name parameters
          else
            sprintf "public override bool Equals(object other) {\n return other is %s; \n}\n" c.Name
        sprintf "public class %s : %s {\n%s\n%s\n%s\n%s\n%s\n%s}\n\n" c.Name !c.Interface parameters cons ((c.Methods |> Seq.map (fun x -> x.Value.ToString(ctxt,originalFilePath))) ++ 2) missing_methods to_string equals

let add_rule inputClass (rule:BasicExpression<_,_,Literal, Position>) (rule_path:Path) (hasScope:bool) =
  let method_path = rule_path.Tail
  if inputClass.Methods |> Map.containsKey method_path |> not then
    inputClass.Methods <- inputClass.Methods |> Map.add method_path { Rules = ResizeArray(); Path = method_path }
  match rule with
  | Application(Implicit, Keyword(FractionLine, _) :: (Application(Implicit, Keyword(DoubleArrow, _) :: input :: output :: [], innerPos)) :: clauses, pos) ->
    inputClass.Methods.[method_path].Rules.Add(
      { Position = pos
        Input = input
        Clauses = 
          [ for c in clauses do
              match c with
              | Application(_, Keyword(Inlined, _) :: _, clausePos) -> yield Inlined, c, c
              | Application(_, Keyword(DoubleArrow, _) :: c_i :: c_o :: [], clausePos) -> yield DoubleArrow, c_i, c_o
              | Application(_, Keyword(Equals, _) :: c_i :: c_o :: [], clausePos) -> yield Equals, c_i, c_o
              | Application(_, Keyword(NotEquals, _) :: c_i :: c_o :: [], clausePos) -> yield NotEquals, c_i, c_o
              | Application(_, Keyword(DefinedAs, _) :: c_i :: c_o :: [], clausePos) -> yield DefinedAs, c_i, c_o
              | Application(_, Keyword(GreaterThan, _) :: c_i :: c_o :: [], clausePos) -> yield GreaterThan, c_i, c_o
              | Application(_, Keyword(GreaterOrEqual, _) :: c_i :: c_o :: [], clausePos) -> yield GreaterOrEqual, c_i, c_o
              | Application(_, Keyword(SmallerThan, _) :: c_i :: c_o :: [], clausePos) -> yield SmallerThan, c_i, c_o
              | Application(_, Keyword(SmallerOrEqual, _) :: c_i :: c_o :: [], clausePos) -> yield SmallerOrEqual, c_i, c_o
              | _ -> failwithf "Cannot process clause %A" c
          ] 
        Output   = output
        Path     = rule_path
        HasScope = hasScope })
  | _ ->
    failwithf "Cannot extract rule shape from %A" rule

let rec process_rules (classes:Map<string,GeneratedClass>) (path:List<int>) (rules:List<BasicExpression<_,_,Literal,Position>>) = 
  for rule,i in rules |> Seq.mapi (fun i r -> r,i) do
    let path' = i :: path
    let self,hasScope = 
      match rule with
      | Application(_, Keyword(Nesting, _) :: self :: children, pos) -> 
        do process_rules classes path' children
        self,true
      | self -> self,false
    match self with
    | Application(Implicit, Keyword(FractionLine, _) :: (Application(Implicit, Keyword(DoubleArrow, _) :: input :: output, clausesPos)) :: clauses, pos) ->
      let inputKeyword = 
        match input with
        | Keyword(Custom k, _) -> k
        | Application(Implicit, (Keyword(Custom k, _)) :: _, pos) -> k
        | _ -> failwithf "Cannot extract input keyword from %A" input
      let inputClass = classes.[inputKeyword]
      do add_rule inputClass self (Path path') hasScope
    | _ -> failwithf "Malformed rule %A" self
    ()


type Interface = { Name : string; BaseInterfaces : ResizeArray<string> }

let generateCode (originalFilePath:string) (program_name:string) (rules:BasicExpression<Keyword, Var, Literal, Position>) (program:BasicExpression<Keyword, Var, Literal, Position>) (ctxt:ConcreteExpressionContext) = 
  match rules with
  | Application(Implicit, Keyword(Sequence, _) :: rules, pos) ->
    let mutable classes = Map.empty
    let mutable inheritanceRelationships = Map.empty
    for c,a in ctxt.InheritanceRelationships do
      match inheritanceRelationships |> Map.tryFind c with
      | Some i -> i.BaseInterfaces.Add a
      | None -> inheritanceRelationships <- inheritanceRelationships |> Map.add c { Name = c; BaseInterfaces = ResizeArray([a]) }
    for keyword in ctxt.CustomKeywords do
      let newClass = { GeneratedClass.BasicName = keyword.Name
                       GeneratedClass.GenericArguments = keyword.GenericArguments
                       GeneratedClass.Interface = keyword.Class
                       GeneratedClass.Parameters = ResizeArray()
                       GeneratedClass.Methods = Map.empty }
      for t,i in keyword.LeftArguments |> Seq.mapi (fun i p -> p,i+1) do
        newClass.Parameters.Add({ Name = sprintf "P%d" i; IsLeft = true; Type = t })
      for t,i in keyword.RightArguments |> Seq.mapi (fun i p -> p,i+1) do
        newClass.Parameters.Add({ Name = sprintf "P%d" (i + keyword.LeftAriety); IsLeft = false; Type = t })
      classes <- classes.Add(keyword.Name,newClass)

    do process_rules classes [] rules

    let classes = classes
    let extensions = @"public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }"
    let interfaces = [ for k in ctxt.CustomKeywords -> k.Class ] |> Set.ofSeq
    let inheritanceRelationships = inheritanceRelationships
    let interfacesCode = 
      [
        for i in interfaces do
          match inheritanceRelationships |> Map.tryFind i with
          | Some ir ->
            let explicitInterfaces = ir.BaseInterfaces
            yield sprintf "public interface %s : %s {}\n" !i (explicitInterfaces |> Seq.reduce (fun s x -> s + ", " + x))
          | _ ->
            yield sprintf "public interface %s : IRunnable {}\n" !i
      ] |> Seq.fold (+) ""
    let all_method_paths =
      seq{
        for c in classes -> c.Value.MethodPaths
      } |> Seq.reduce (+)
    let run_methods =
      all_method_paths |> Seq.map (fun p -> sprintf "IEnumerable<IRunnable> Run%s();\n" (p.ToString())) |> Seq.reduce (+)
    let prelude = sprintf "using System.Collections.Generic;\nusing System.Linq;\nnamespace %s {\n %s\n public interface IRunnable { %s }" (program_name.Replace(" ", "_")) extensions run_methods
    let main = sprintf "public class EntryPoint {\n static public IEnumerable<IRunnable> Run(bool printInput)\n{\n #line 1 \"input\"\n var p = %s;\nif(printInput) System.Console.WriteLine(p.ToString());\nforeach(var x in p.Run())\nyield return x;\n}\n}\n" (create_element ctxt program |> fst)
    [
      yield prelude
      yield "\n\n\n"
      yield interfacesCode
      yield "\n\n\n"
      for c in classes do
        let c = c.Value
        yield c.ToString(all_method_paths, ctxt, originalFilePath)
      yield "\n\n\n"
      yield main
      yield "\n}\n"
    ] |> Seq.fold (+) "" |> (fun txt -> System.IO.File.WriteAllText(program_name + ".cs", txt); program_name + ".cs")
  | _ -> failwith "Cannot extract rules from input program."

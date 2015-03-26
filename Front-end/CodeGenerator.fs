module CodeGenerator

open System
open Utilities
open ParserMonad
open BasicExpression
open ConcreteExpressionParserPrelude
open ConcreteExpressionParser

let private generateLineDirectives = true

let cleanupWithoutDot (s:string) =
  match s with
  | "is" -> "_Is"
  | "as" -> "_As"
  | "if" -> "_If"
  | "while" -> "_While"
  | "then" -> "_Then"
  | "else" -> "_Else"
  | "for" -> "_For"
  | "true" -> "_True"
  | "false" -> "_False"
  | "with" -> "_With"
  | _ ->
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
     .Replace("[", "<") // keep last or > becomes opGreaterThan
     .Replace("]", ">") // see previous comment

let (!) (s:string) = (cleanupWithoutDot s).Replace(".", "_opDot")

let (|Native|Defined|Generic|) kw =
    match kw with
    | Application(Generic, _, _, _) -> Generic(Keyword.Name kw)
    | _ ->
        match Keyword.IsNative kw with
        | true -> Native(Keyword.Name kw)
        | false -> Defined(Keyword.Name kw)



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
  | Iterate of var_name : string * tmp_var_name : string * expr:BasicExpression<Keyword, Var, Literal, Position, TypeInference.TypeAnnotation> * path : Path
  | Compare of comparison : Keyword * expr1:BasicExpression<Keyword, Var, Literal, Position, TypeInference.TypeAnnotation> * expr2:BasicExpression<Keyword, Var, Literal, Position, TypeInference.TypeAnnotation>
  | Inline of e:BasicExpression<Keyword, Var, Literal, Position, TypeInference.TypeAnnotation>
  | Yield of expr:BasicExpression<Keyword, Var, Literal, Position, TypeInference.TypeAnnotation>

let rec create_element (ctxt:ConcreteExpressionContext) (e:BasicExpression<Keyword, Var, Literal, Position, TypeInference.TypeAnnotation>) = 
  let rec create_element' (expectedType:BasicExpression<Keyword, Var, Literal, Position, unit>) (e:BasicExpression<Keyword, Var, Literal, Position, TypeInference.TypeAnnotation>) = 
    match e with
    | Keyword(Custom k, _, ti) 
    | Application(Regular,(Keyword(Custom k, _, ti)) :: [], _, _)
    | Application(Implicit,(Keyword(Custom k, _, ti)) :: [], _, _) -> 
      sprintf "%s.Create()" !k, []
    | Application(Regular,(Keyword(Custom k, _, ti)) :: es, pos, _)
    | Application(Implicit,(Keyword(Custom k, _, ti)) :: es, pos, _) ->
      let actualKeyword = ctxt.CustomKeywordsMap.[k]
      if es.Length = (Keyword.Arguments actualKeyword).Length then
        let args,cargs = es |> Seq.mapi (fun i e -> create_element' (Keyword.Arguments actualKeyword).[i] e) |> Seq.reduce (fun (s,cs) (x,cx) -> sprintf "%s, %s" s x, cs @ cx)
        //do printfn "Inner creation of %A with expectedType %A" (k, args) expectedType
        sprintf "%s.Create(%s)" !k args, cargs
      else
        failwithf "Invalid number of keyword arguments @ %A" pos
    | Extension(v:Var, _, ti) ->
      match expectedType with
      | Native(t) ->
        sprintf "%s" !v.Name, []
      | _ ->
        let t = Keyword.Name expectedType
        sprintf "%s as %s" !v.Name t, [sprintf "%s is %s" !v.Name t]
    | Application(Angle,e::[],pos, _) ->
      let res = generate_inline e
      sprintf "%s" res, []
    | Application(Angle,e::es,pos,typeInfo) ->
      let res = generate_inline (Application(Implicit,e::es,pos,typeInfo))
      sprintf "%s" res, []
    | Application(Regular,e::[],pos,typeInfo) ->
      let res, conds = create_element' expectedType e
      sprintf "(%s)" res, conds
    | Application(b,e::es,pos,typeInfo) ->
      failwithf "Code generation error @ %A" pos
    | Application(b,[],pos,typeInfo) ->
      failwithf "Code generation error @ %A" pos
    | Imported(l,pos,typeInfo) -> l.ToString(), []
    | Keyword(k, pos, typeInfo) -> 
      failwithf "Unexpected keyword %A @ %A cannot be matched" k pos
  match e with
  | Keyword(Custom k, _,ti) 
  | Application(Regular,(Keyword(Custom k, _,ti)) :: [],_,_)
  | Application(Implicit,(Keyword(Custom k, _,ti)) :: [],_,_) -> 
    sprintf "%s.Create()" !k, []
  | Application(Regular,(Keyword(Custom k, _,ti)) :: es,pos,_)
  | Application(Implicit,(Keyword(Custom k, _,ti)) :: es,pos,_) ->
    let actualKeyword = ctxt.CustomKeywordsMap.[k]
    let args,cargs = es |> Seq.mapi (fun i e -> create_element' (Keyword.Arguments actualKeyword).[i] e) |> Seq.reduce (fun (s,cs) (x,cx) -> sprintf "%s, %s" s x, cs @ cx)
    let trimmedArgs = 
      if es |> List.length = 1 && args.[0] = '(' && args.[args.Length - 1] = ')' then
        args.Substring(1, args.Length - 2)
      else
        args
    //do printfn "Outer creation of %A" (k, args)
    let res = sprintf "%s.Create(%s)" !k trimmedArgs, cargs
    res
  | Application(Angle,e::es,pos,ti) ->
    let res = generate_inline (Application(Regular,e::es,pos,ti))
    sprintf "%s" res, []
  | Extension(v:Var,_,_) ->
    sprintf "%s" !v.Name, []
  | Application(b,e::es,pos,ti) ->
    failwithf "Code generation error @ %A" pos
  | Application(b,[],pos,_) ->
    failwithf "Code generation error @ %A" pos
  | Imported(l,pos,_) -> l.ToString(), []
  | Keyword(k,pos,_) -> 
    failwithf "Non-custom keyword %A @ %A cannot be matched" k pos

and generate_inline =
  function
  | Keyword(Custom k,_,_) ->
    sprintf "%s" k
  | Application(_,[],_,_) ->
    ""
  | Application(Regular,(Keyword(Custom k,_,_)) :: [],_,_) -> 
    sprintf "(%s)" k
  | Application(Square,(Keyword(Custom k, _,_)) :: l :: r :: [],pos,_) when (Keyword.Ariety LeftArguments ConcreteExpressionContext.CSharp.CustomKeywordsMap.[k]) = 1 ->
    sprintf "<%s%s%s>" (l |> generate_inline) k (r |> generate_inline)
  | Application(Regular,(Keyword(Custom k,_,_)) :: l :: r :: [],pos,_) when (Keyword.Ariety LeftArguments ConcreteExpressionContext.CSharp.CustomKeywordsMap.[k]) = 1 ->
    sprintf "(%s%s%s)" (l |> generate_inline) k (r |> generate_inline)
  | Extension(v:Var,_,_) ->
    sprintf "%s" !v.Name
  | Application(Angle, (Keyword(Inlined, _, _)) :: args, di, ti) ->
    let res = Application(Implicit, args, di, ti) |> generate_inline
    res
  | Application(Regular, a :: [], _, _) ->
    sprintf "(%s)" (a |> generate_inline)
  | Application(Regular, arg :: args, _, _) ->
    let pars = [ for x in args -> x |> generate_inline ] |> Seq.reduce (fun s x -> sprintf "%s%s" s x)
    sprintf "%s(%s)" (arg |> generate_inline) pars
  | Application(Angle, arg :: args, pos, ti) ->
    let res = Application(Implicit, arg :: args, pos, ti) |> generate_inline
    res
  | Application(Implicit,(Keyword(Custom k, _, _)) :: [],_,_) -> 
    sprintf "%s" k
  | Application(Implicit,(Keyword(Custom k, _, _)) :: l :: r :: [], pos, _) when (Keyword.Ariety LeftArguments ConcreteExpressionContext.CSharp.CustomKeywordsMap.[k]) = 1 ->
    sprintf "%s%s%s" (l |> generate_inline) k (r |> generate_inline)
  | Application(Implicit, arg :: [], _, _) ->
    arg |> generate_inline
  | Application(Implicit, arg :: args, _, _) ->
    let res = [ for x in arg :: args -> x |> generate_inline ] |> Seq.reduce (fun s x -> sprintf "%s%s" s x)
    res
  | Imported(l,di,_) ->
    l.ToString()
  | i -> 
    failwithf "Code generation error @ %A" i.DebugInformation
  

let rec generate_instructions (debugPosition:Position) (originalFilePath:string) (ctxt:ConcreteExpressionContext) = 
  function 
  | [] -> ""
  | x :: xs ->
    let newLine = 
      if generateLineDirectives then sprintf "\n #line %d \"%s\"\n" debugPosition.Line debugPosition.File
      else "\n"
    match x with
    | Var(name, expr) -> 
      let res = sprintf "var %s = %s; %s" !name expr (generate_instructions debugPosition originalFilePath ctxt xs)
      res
    | VarAs(name, expr, as_type) ->
      sprintf "var %s = %s as %s; %s" !name expr as_type (generate_instructions debugPosition originalFilePath  ctxt xs)
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
        | Divisible -> "", "%", "== 0" 
        | NotDivisible -> "", "%", "!= 0" 
        | _ -> failwith "Unsupported comparison operator %s @ %A" comparison expr1.DebugInformation
      let res =
        if creationConstraints.IsEmpty |> not then
          let creationConstraints = creationConstraints |> Seq.reduce (fun s x -> sprintf "%s && %s" s x)
          sprintf "%sif(%s) { %sif(%s%s%s%s%s) { %s } }" newLine creationConstraints newLine preComparison newElement1 inlineComparison newElement2 postComparison (generate_instructions debugPosition originalFilePath  ctxt xs)
        else 
          sprintf "%sif(%s%s%s%s%s) { %s }" newLine preComparison newElement1 inlineComparison newElement2 postComparison (generate_instructions debugPosition originalFilePath  ctxt xs)
      res
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

let rec matchCast (tmp_id:int) (e:BasicExpression<Keyword, Var, Literal, Position, TypeInference.TypeAnnotation>) (self:string) (prefix:List<Instruction>) 
                  (ctxt:ConcreteExpressionContext) (typeConstraint) =
  match e with
  | Keyword(Custom k, _, ti) -> 
    if self <> "this" then
      prefix @ 
      [
        VarAs(sprintf "tmp_%d" tmp_id, self, !k)
        CheckNull(sprintf "tmp_%d" tmp_id)
      ], tmp_id+1
    else
      prefix @
      [
        VarAs(sprintf "tmp_%d" tmp_id, self, !k)
      ], tmp_id+1
  | Extension(v:Var, _, _) ->
      prefix @
      [
        Var(v.Name, self)
      ], tmp_id
  | Application(Angle,es,pos,ti) ->
    let output,tmp_id = 
      let expectedSelf = generate_inline(Application(Angle,es,pos,ti))
      prefix @
      [
        CustomCheck(sprintf "%s == %s" self expectedSelf)
      ], tmp_id
    // es_i -> self . P_i
    output, tmp_id
  | Application(b,(Keyword(Custom k, _, k_ti)) :: es,pos,e_ti) ->
    let output,self,tmp_id = 
      if self <> "this" then
          prefix @
          [
            VarAs(sprintf "tmp_%d" tmp_id, self, !k)
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
    let actualKeyword = ctxt.CustomKeywordsMap.[k]
    let keywordArgumentConstraints = Keyword.Arguments actualKeyword
    for e_constraint,(e,i) in es |> List.mapi (fun i e -> e,(i+1)) |> Seq.zip keywordArgumentConstraints do
      let newOutput, newTempId = matchCast tmp_id e (sprintf "%s.P%d" self i) output ctxt (Some e_constraint)
      output <- newOutput
      tmp_id <- newTempId
    output, tmp_id
  | Application(Regular,e::[],pos,_) ->
    matchCast tmp_id e self prefix ctxt None
  | Application(Square,e::es,pos,_) ->
    prefix, tmp_id
  | Application(b,e::es,pos,_) ->
    failwithf "Code generator error @ %A" pos
  | Application(b,[],pos,_) ->
    failwithf "Code generator error @ %A" pos
  | Imported(l,pos,_) ->
    let cond = sprintf "%s == %s" self (l.ToString())
    prefix @
    [
      CustomCheck(cond)
    ], tmp_id
  | Keyword(k,pos,ti) -> 
    failwithf "Unexpected keyword %A @ %A" k pos

type Rule = {
  Position   : Position
  Input      : BasicExpression<Keyword, Var, Literal, Position, TypeInference.TypeAnnotation>
  Output     : BasicExpression<Keyword, Var, Literal, Position, TypeInference.TypeAnnotation>
  Clauses    : List<Keyword * BasicExpression<Keyword, Var, Literal, Position, TypeInference.TypeAnnotation> * BasicExpression<Keyword, Var, Literal, Position, TypeInference.TypeAnnotation>>
  Path       : Path
  HasScope   : bool
} with
    member r.ToString (ctxt:ConcreteExpressionContext,originalFilePath) =
      let path = 
        if r.HasScope then
          r.Path
        else
          r.Path.Tail
      let i,tmp_id = matchCast 0 r.Input "this" [] ctxt None
      let mutable o = []
      let mutable tmp_id = tmp_id
      for k,c_i,c_o in r.Clauses do
        match k with
        | DoubleArrow ->
          o <- o @ [Iterate(sprintf "tmp_%d" tmp_id, sprintf "tmp_%d" (tmp_id+1), c_i, path)]
          let o',tmp_id' = matchCast (tmp_id+2) c_o (sprintf "tmp_%d" tmp_id) [] ctxt None
          o <- o @ o'
          tmp_id <- tmp_id'
        | Equals | NotEquals | Divisible | NotDivisible | GreaterThan | GreaterOrEqual | SmallerThan | SmallerOrEqual -> 
          o <- o @ [Compare(k, c_i, c_o)]
        | DefinedAs -> 
          match c_i with
          | Extension(iVar, _, _) ->
            let oExpr,constraints = create_element ctxt c_o
            if constraints.IsEmpty |> not then
              o <- o @ [CustomCheck(constraints |> Seq.reduce (fun s x -> sprintf "%s && %s" s x))]
            o <- o @ [Var(iVar.Name, oExpr)]
          | _ -> 
            let iVar = c_i.ToStringCompact
            let oExpr,constraints = create_element ctxt c_o
            if constraints.IsEmpty |> not then
              o <- o @ [CustomCheck(constraints |> Seq.reduce (fun s x -> sprintf "%s && %s" s x))]
            o <- o @ [Var(iVar, oExpr)]
        | Inlined ->
            o <- o @ [Inline(c_i)]
        | _ -> failwithf "Unexpected operator %A @ %A" k c_i.DebugInformation
      o <- i @ o @ [Yield r.Output]
      if generateLineDirectives then
        sprintf "\n { \n #line %d \"%s\"\n%s\n } \n" r.Position.Line r.Position.File (generate_instructions r.Position originalFilePath ctxt o)
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
    Type    : BasicExpression<Keyword, Var, Literal, Position, unit>
  }

type GeneratedClass = 
  {
    BasicName           : string
    Interface           : string
    GenericArguments    : List<BasicExpression<Keyword, Var, Literal, Position, unit>>
    Parameters          : ResizeArray<Parameter>
    mutable Methods     : Map<Path, Method>
  } with
      member c.Name = 
        if c.GenericArguments.IsEmpty |> not then  
          let args = c.GenericArguments |> Seq.map (fun x -> Keyword.ArgumentCSharpStyle x (!)) |> Seq.reduce (fun s x -> sprintf "%s, %s" s x)
          sprintf "%s<%s>" !c.BasicName args
        else
          !c.BasicName
      member this.MethodPaths = seq{ for x in this.Methods -> x.Key } |> Set.ofSeq
      member c.ToString(all_method_paths:Set<Path>, ctxt:ConcreteExpressionContext, originalFilePath) =
        let genericConstraints = 
          if c.GenericArguments.IsEmpty |> not then  
            let args = c.GenericArguments |> Seq.map (fun x -> sprintf "where %s : class" (Keyword.ArgumentCSharpStyle x (!))) |> Seq.reduce (fun s x -> sprintf "%s %s" s x)
            args
          else
            ""
        let cons =
          if c.Parameters.Count <> 0 then
            let pars = c.Parameters |> Seq.map (fun x -> sprintf "%s %s" (Keyword.ArgumentCSharpStyle x.Type cleanupWithoutDot) x.Name) |> Seq.reduce (fun s x -> sprintf "%s, %s" s x)
            let args = c.Parameters |> Seq.map (fun x -> sprintf "this.%s = %s;" x.Name x.Name) |> Seq.reduce (fun s x -> sprintf "%s %s" s x)
            sprintf "public %s(%s) {%s}\n" !c.BasicName pars args
          else
            sprintf "public %s() {}\n" !c.BasicName
        let staticCons =
          if c.Parameters.Count <> 0 then
            let pars = c.Parameters |> Seq.map (fun x -> sprintf "%s %s" (Keyword.ArgumentCSharpStyle x.Type cleanupWithoutDot) x.Name) |> Seq.reduce (fun s x -> sprintf "%s, %s" s x)
            let args = c.Parameters |> Seq.map (fun x -> sprintf "%s" x.Name) |> Seq.reduce (fun s x -> sprintf "%s, %s" s x)
            sprintf "public static %s Create(%s) { return new %s(%s); }\n" c.Name pars c.Name args
          else
            sprintf "public static %s Create() { return new %s(); }\n" c.Name c.Name
        let cons = cons + staticCons
        let parameters =
          let res = c.Parameters |> Seq.map (fun p -> sprintf "public %s %s;\n" (Keyword.ArgumentCSharpStyle p.Type cleanupWithoutDot) p.Name) |> Seq.fold (+) ""
          res
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
              | Generic(t)
              | Native(t) when t <> "int" && t <> "float" && t <> "double" && t <> "bool" ->
                sprintf "if (%s is System.Collections.IEnumerable) { res += \"{\"; foreach(var x in %s as System.Collections.IEnumerable) res += x.ToString(); res += \"}\";  } else { res += %s.ToString(); } \n" p.Name p.Name p.Name
              | _ -> 
                sprintf "res += %s.ToString(); \n" p.Name
            let leftParameters = c.Parameters |> Seq.filter (fun x -> x.IsLeft) |> Seq.map (fun x -> printParameter x) |> Seq.fold (+) ""
            let rightParameters = c.Parameters |> Seq.filter (fun x -> x.IsLeft |> not) |> Seq.map (fun x -> printParameter x) |> Seq.fold (+) ""
            sprintf "public override string ToString() {\n var res = \"(\"; \n%s\n res += \" %s \"; %s\n res += \")\";\n return res;\n}\n" leftParameters (escape c.BasicName) rightParameters
          else
            sprintf "public override string ToString() {\nreturn \"%s\";\n}\n" (escape c.BasicName)
        let equals =
          if c.Parameters.Count > 0 then
            let parameters = c.Parameters |> Seq.map (fun x -> sprintf "this.%s.Equals(tmp.%s)" x.Name x.Name) |> Seq.reduce (fun s x -> sprintf "%s && %s" s x)
            sprintf "public override bool Equals(object other) {\n var tmp = other as %s;\n if(tmp != null) return %s; \n else return false; }\n" c.Name parameters
          else
            sprintf "public override bool Equals(object other) {\n return other is %s; \n}\n" c.Name
        let hash =
          sprintf "public override int GetHashCode() {\n return 0; \n}\n"
        sprintf "public class %s : %s %s {\n%s\n%s\n%s\n%s\n%s\n%s\n%s\n}\n\n" c.Name c.Interface genericConstraints parameters cons ((c.Methods |> Seq.map (fun x -> x.Value.ToString(ctxt,originalFilePath))) ++ 2) missing_methods to_string equals hash

let add_rule inputClass (rule:BasicExpression<_,_,Literal, Position, Unit>) (rule_path:Path) (hasScope:bool) ctxt =
  let method_path = rule_path.Tail
  if inputClass.Methods |> Map.containsKey method_path |> not then
    inputClass.Methods <- inputClass.Methods |> Map.add method_path { Rules = ResizeArray(); Path = method_path }
  match rule with
  | Application(Implicit, Keyword(FractionLine, _, _) :: (Application(Implicit, Keyword(DoubleArrow, _, _) :: input :: output :: [], innerPos, _)) :: clauses, pos, _) ->
    let input, output, clauses = TypeInference.inferTypeAnnotations input output clauses ctxt
    inputClass.Methods.[method_path].Rules.Add(
      { Position = pos
        Input = input
        Clauses = 
          [ for c in clauses do
              match c with
              | Application(_, Keyword(Inlined, _, _) :: _, clausePos, _) -> yield Inlined, c, c
              | Application(_, Keyword(DoubleArrow, _, _) :: (Application(Angle, _, _, _) as c_i) :: (Extension(_,_,_) as c_o) :: [], clausePos, _) -> yield DefinedAs, c_o, c_i
              | Application(_, Keyword(DoubleArrow, _, _) :: c_i :: c_o :: [], clausePos, _) -> yield DoubleArrow, c_i, c_o
              | Application(_, Keyword(Equals, _, _) :: c_i :: c_o :: [], clausePos, _) -> yield Equals, c_i, c_o
              | Application(_, Keyword(NotEquals, _, _) :: c_i :: c_o :: [], clausePos, _) -> yield NotEquals, c_i, c_o
              | Application(_, Keyword(DefinedAs, _, _) :: c_i :: c_o :: [], clausePos, _) -> yield DefinedAs, c_i, c_o
              | Application(_, Keyword(GreaterThan, _, _) :: c_i :: c_o :: [], clausePos, _) -> yield GreaterThan, c_i, c_o
              | Application(_, Keyword(GreaterOrEqual, _, _) :: c_i :: c_o :: [], clausePos, _) -> yield GreaterOrEqual, c_i, c_o
              | Application(_, Keyword(SmallerThan, _, _) :: c_i :: c_o :: [], clausePos, _) -> yield SmallerThan, c_i, c_o
              | Application(_, Keyword(SmallerOrEqual, _, _) :: c_i :: c_o :: [], clausePos, _) -> yield SmallerOrEqual, c_i, c_o
              | Application(_, Keyword(Divisible, _, _) :: c_i :: c_o :: [], clausePos, _) -> yield Divisible, c_i, c_o
              | Application(_, Keyword(NotDivisible, _, _) :: c_i :: c_o :: [], clausePos, _) -> yield NotDivisible, c_i, c_o
              | _ -> failwithf "Unexpected clause @ %A" c.DebugInformation
          ] 
        Output   = output
        Path     = rule_path
        HasScope = hasScope })
  | _ ->
    failwithf "Cannot extract rule shape @ %A" rule.DebugInformation

let rec process_rules (classes:Map<string,GeneratedClass>) (path:List<int>) (rules:List<BasicExpression<_,_,Literal,Position, Unit>>) ctxt = 
  for rule,i in rules |> Seq.mapi (fun i r -> r,i) do
    let path' = i :: path
    let self,hasScope = 
      match rule with
      | Application(_, Keyword(Nesting, _, _) :: self :: children, pos, _) -> 
        do process_rules classes path' children ctxt
        self,true
      | self -> self,false
    match self with
    | Application(Implicit, Keyword(FractionLine, _, _) :: (Application(Implicit, Keyword(DoubleArrow, _, _) :: input :: output, clausesPos, _)) :: clauses, pos, _) ->
      let inputKeyword = 
        match input with
        | Keyword(Custom k, _, _) -> k
        | Application(Implicit, (Keyword(Custom k, _, _)) :: _, pos, _) -> k
        | _ -> failwithf "Cannot extract input keyword @ %A" input.DebugInformation
      let inputClass = classes.[inputKeyword]
      do add_rule inputClass self (Path path') hasScope ctxt
    | _ -> failwithf "Malformed rule @ %A" self.DebugInformation
    ()


type Interface = { Name : string; BaseInterfaces : ResizeArray<string> }

let generateCode (originalFilePath:string) (program_name:string)
                 (rules:BasicExpression<Keyword, Var, Literal, Position, Unit>)
                 (program:BasicExpression<Keyword, Var, Literal, Position, Unit>)
                 (ctxt:ConcreteExpressionContext) =
  
  match rules with
  | Application(Implicit, Keyword(Sequence, _, _) :: rules, pos, _) ->
    let mutable classes = Map.empty
    let mutable inheritanceRelationships = Map.empty
    for c,a in ctxt.AllInheritanceRelationships do
      match inheritanceRelationships |> Map.tryFind c with
      | Some i -> i.BaseInterfaces.Add a
      | None -> inheritanceRelationships <- inheritanceRelationships |> Map.add c { Name = c; BaseInterfaces = ResizeArray([a]) }
    for keyword in ctxt.CustomKeywords do
      let newClass = { GeneratedClass.BasicName = Keyword.Name keyword
                       GeneratedClass.GenericArguments = Keyword.GenericArguments keyword
                       GeneratedClass.Interface = Keyword.Class keyword
                       GeneratedClass.Parameters = ResizeArray()
                       GeneratedClass.Methods = Map.empty }
      for t,i in Keyword.LeftArguments keyword |> Seq.mapi (fun i p -> p,i+1) do
        newClass.Parameters.Add({ Name = sprintf "P%d" i; IsLeft = true; Type = t })
      for t,i in Keyword.RightArguments keyword |> Seq.mapi (fun i p -> p,i+1) do
        newClass.Parameters.Add({ Name = sprintf "P%d" (i + Keyword.Ariety LeftArguments keyword); IsLeft = false; Type = t })
      classes <- classes.Add(Keyword.Name keyword,newClass)

    do process_rules classes [] rules ctxt

    let programTyped,_,_ = TypeInference.inferTypeAnnotations program (Extension({ Name = "___tmp" }, Position.Zero, ())) [] ctxt

    let classes = classes
    let extensions = @"public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }"
    let interfaces = [ for k in ctxt.CustomKeywords -> Keyword.Class k ] |> Set.ofSeq
    let inheritanceRelationships = inheritanceRelationships
    let interfacesCode = 
      [
        for i in interfaces do
          match inheritanceRelationships |> Map.tryFind (i) with
          | Some ir ->
            let explicitInterfaces = ir.BaseInterfaces
            yield sprintf "public interface %s : %s {}\n" ((!) i) (explicitInterfaces |> Seq.reduce (fun s x -> s + ", " + x))
          | _ ->
            yield sprintf "public interface %s : IRunnable {}\n" ((!) i)
      ] |> Seq.fold (+) ""
    let all_method_paths =
      seq{
        for c in classes -> c.Value.MethodPaths
      } |> Seq.reduce (+)
    let run_methods =
      all_method_paths |> Seq.map (fun p -> sprintf "IEnumerable<IRunnable> Run%s();\n" (p.ToString())) |> Seq.reduce (+)
    let imports = 
        if ctxt.ImportedModules.Length > 0 then
            (ctxt.ImportedModules
                    |> List.map (fun x -> sprintf "using %s;\n" x)
                    |> List.reduce (fun x y -> x + y))
        else
            ""
    let prelude = sprintf "using System.Collections.Generic;\nusing System.Linq;\nnamespace %s {\n %s\n public interface IRunnable { %s }" (program_name.Replace(" ", "_")) extensions run_methods
    let main = sprintf "public class EntryPoint {\n static public int Sleep(float s) { int t = (int)(s * 1000.0f); ; return 0; } \nstatic public IEnumerable<IRunnable> Run(bool printInput)\n{\n #line 1 \"input\"\n var p = %s;\nif(printInput) System.Console.WriteLine(p.ToString());\nforeach(var x in p.Run())\nyield return x;\n}\n}\n" (create_element ctxt programTyped |> fst)
    [
      yield imports
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
  | _ -> failwithf "Cannot extract rules from input program @ %A" rules.DebugInformation

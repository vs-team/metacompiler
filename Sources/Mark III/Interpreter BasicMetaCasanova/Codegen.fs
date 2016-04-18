module Codegen
open Common
open CodegenInterface
open Mangle

let mutable flags:CompilerFlags={debug=false};

let ice () = 
    do System.Console.BackgroundColor <- System.ConsoleColor.Red
    do System.Console.Write "INTERNAL COMPILER ERROR"
    do System.Console.ResetColor()

type NamespacedItem = Ns       of string*List<NamespacedItem>
                    | Data     of string*data
                    | Function of string*List<rule>
                    | Lambda   of int*rule

let construct_tree (input:fromTypecheckerWithLove) :List<NamespacedItem> =
  let rec datatree (s:List<NamespacedItem>) (idx:List<string>,v:string*data) :List<NamespacedItem> =
    match idx with
    | []    -> (Data(v))::s
    | n::ns -> match s |> List.partition (fun x->match x with Ns(n,_)->true | _->false) with
               | [Ns(n,body)],rest -> Ns(n,datatree body (ns,v))::rest
               | [],list           -> Ns(n,datatree []   (ns,v))::list
  let rec functree (s:List<NamespacedItem>) (idx:List<string>,v:string*List<rule>) :List<NamespacedItem> =
    match idx with
    | []    -> (Function(v))::s
    | n::ns -> match s |> List.partition (fun x->match x with Ns(n,_)->true | _->false) with
               | [Ns(n,body)],rest -> Ns(n,functree body (ns,v))::rest
               | [],list           -> Ns(n,functree []   (ns,v))::list
  let rec lambdatree (s:List<NamespacedItem>) (idx:List<string>,v:int*rule) :List<NamespacedItem> =
    match idx with
    | []    -> (Lambda(v))::s
    | n::ns -> match s |> List.partition (fun x->match x with Ns(n,_)->true | _->false) with
               | [Ns(n,body)],rest -> Ns(n,lambdatree body (ns,v))::rest
               | [],list           -> Ns(n,lambdatree []   (ns,v))::list
  let go input output state = input |> Seq.map (fun (n,d)->(List.rev n.Namespace),(n.Name,d)) |> Seq.fold output state
  [] |> go (Map.toSeq input.lambdas) lambdatree 
     |> go (Map.toSeq input.funcs) functree 
     |> go input.datas datatree

let print_literal (lit:Literal) =
  match lit with
  | I64 i    -> sprintf "%dL"  i
  | U64 i    -> sprintf "%uUL" i
  | I32 i    -> sprintf "%u" i
  | U32 i    -> sprintf "%uU" i
  | F64 i    -> sprintf "%f" i
  | F32 i    -> sprintf "%ff" i
  | String s -> sprintf "\"%s\"" s
  | Bool b   -> if b then "true" else "false"
  | Void     -> "void"

let print_predicate (p:Predicate) :string= 
  match p with Less -> "<" | LessEqual -> "<=" | Equal -> "=" | GreaterEqual -> ">=" | Greater -> ">" | NotEqual -> "!="

let field (n:int) (t:Type) :string =
  sprintf "public %s _arg%d;\n" (mangle_type t) n

let highest_tmp (typemap:Map<local_id,Type>): int =
  typemap |> Map.fold (fun s k _ -> match k with Tmp(x) when x>s -> x | _ -> s) 0

let get_map (a:local_id) (m:Map<local_id,int>) :int*Map<local_id,int> =
  match Map.tryFind a m with None -> 0,(Map.add a 1 m) | Some x -> x,(Map.add a (x+1) m)

let overloadableOps:Map<string,string> =
  [ 
    "op_Equality","=="
    "op_Inequality","!="
    "op_GreaterThan",">"
    "op_LessThan","<"
    "op_GreaterThanOrEqual",">="
    "op_LessThanOrEqual","<="
    "op_BitwiseAnd","&"
    "op_BitwiseOr","|"
    "op_Addition","+"
    "op_Subtraction","-"
    "op_Division","/"
    "op_Modulus","%"
    "op_Multiply","*"
    "op_LeftShift","<<"
    "op_RightShift",">>"
    "op_ExclusiveOr","^"
    "op_UnaryNegation","-"
    "op_UnaryPlus","+"
    "op_LogicalNot","!"
    "op_OnesComplement","~"
    "op_False","false"
    "op_True","true"
    "op_Increment","++"
    "op_Decrement","--"
  ] |> Map.ofList

let print_label (i:int) = sprintf "_skip%d" i

let premisse (p:premisse) (m:Map<local_id,Type>) (app:Map<local_id,int>) (rule_nr:int) =
  match p with
  | Literal x -> app,sprintf "/*LITR*/var %s = %s;\n"
                   (mangle_local_id x.dest)
                   (print_literal x.value)
  | Conditional x -> app,sprintf "/*COND*/if(!(%s %s %s)){goto %s;}\n"
                       (mangle_local_id x.left)
                       (print_predicate x.predicate)
                       (mangle_local_id x.right)
                       (print_label rule_nr)
  | Destructor x ->
    let new_id  = (Tmp(1+(highest_tmp m)))
    app,sprintf "/*DTOR*/var %s = %s as %s;\nif(%s==null){goto %s;}\n%s"
      (mangle_local_id new_id)
      (mangle_local_id x.source)
      (mangle_id       x.destructor)
      (mangle_local_id new_id)
      (print_label rule_nr)
      (x.args|>List.mapi(fun nr arg->sprintf "var %s=%s._arg%d;\n" (mangle_local_id arg) (mangle_local_id new_id) nr)|>String.concat "")
  | ConstructorClosure x
  | FuncClosure x -> (app|>Map.add x.dest 0),sprintf "/*FUNC*/var %s = new %s();\n" 
                       (mangle_local_id x.dest)
                       (mangle_id x.func)
  | LambdaClosure x -> (app|>Map.add x.dest 0),sprintf "/*LAMB*/var %s = new %s();\n" 
                         (mangle_local_id x.dest)
                         (mangle_lambda x.func)
  | DotNetCall x -> 
    let isVoid = m.[x.dest]=Type.DotNetType({Namespace=[];Name="void"})
    app,sprintf  "/*NDCA*/%s%s.%s(%s);\n"
      (if isVoid then "" else sprintf "var %s = " (mangle_local_id x.dest))
      (mangle_local_id x.instance)
      x.func
      (x.args |> List.map mangle_local_id|>String.concat ",")
  | DotNetStaticCall x -> 
        let isVoid = m.[x.dest]=Type.DotNetType({Namespace=[];Name="void"})
        if overloadableOps.ContainsKey(x.func.Name) then 
          let args = x.args |> List.rev
          app,sprintf "/*NSCA*/%s%s %s %s;\n"
            (if isVoid then "" else sprintf "var %s = " (mangle_local_id x.dest))
            (match x.args.Length with
             | 1 -> ""
             | 2 -> mangle_local_id args.[1])
            overloadableOps.[x.func.Name]
            (mangle_local_id args.[0])
        else
          app,sprintf "/*NSCA*/%s%s(%s);\n" 
            (if isVoid then "" else sprintf "var %s = " (mangle_local_id x.dest))
            (x.func.Namespace@[x.func.Name]|>String.concat ".")
            (x.args |> List.map mangle_local_id|>String.concat ",")
  | DotNetConstructor x -> app,sprintf "/*NCON*/var %s = new %s(%s);\n" 
                             (mangle_local_id x.dest)
                             (x.func.Namespace@[x.func.Name]|>String.concat ".")
                             (x.args |> List.map mangle_local_id|>String.concat ",")
  | DotNetGet x -> app,sprintf "/*NGET*/var %s = %s.%s;\n" 
                     (mangle_local_id x.dest)
                     (mangle_local_id x.instance)
                     x.field
  | DotNetSet x -> app,sprintf "/*NSET*/%s.%s = %s;\n" 
                     (mangle_local_id x.instance)
                     x.field
                     (mangle_local_id x.src)
  | Application x -> 
    let i = match app|>Map.tryFind x.closure with Some(x)->x | None-> failwith (sprintf "Application failed: %s is not a closure." (mangle_local_id x.closure))
    (app|>Map.add x.dest (i+1)),sprintf "/*APPL*/var %s = %s; %s.%s=%s;\n"
                       (mangle_local_id  x.dest)
                       (mangle_local_id  x.closure)
                       (mangle_local_id  x.dest)
                       (sprintf "_arg%d" i)
                       (mangle_local_id  x.argument)
  | ApplicationCall x -> 
    let i = match app|>Map.tryFind x.closure with Some(x)->x | None-> failwith (sprintf "ApplicationCall failed: %s is not a closure." (mangle_local_id x.closure))
    (app|>Map.add x.dest (i+1)),sprintf "/*CALL*/%s.%s=%s; var %s = %s._run();\n"
      (mangle_local_id  x.closure)
      (sprintf "_arg%d" i)
      (mangle_local_id  x.argument)
      (mangle_local_id  x.dest)
      (mangle_local_id  x.closure)

let print_rule (rule_nr:int) (rule:rule) =
  let linegroups:seq<int*seq<premisse>> = 
    rule.premis |> Seq.groupBy (fun(_,x)->x) |> Seq.map (fun(l,p)->l,(p|>Seq.map(fun(x,_)->x)))
  let fn (app:Map<local_id,int>,str:string) (p:premisse) =
    let (a:Map<local_id,int>,s:string) = premisse p rule.typemap app rule_nr
    a,(str+s)
  let lines =
    linegroups |> Seq.map 
      (fun(linenumber,premisses)->
        let _,s = ((Map.empty,""),premisses) ||> Seq.fold fn in s)
  let breakpointed = 
    if flags.debug then
      lines |> Seq.mapi (fun i s->sprintf "if(_DBUG_breakpoints[%d]){/*HANDLE BREAKPOINT*/}\n%s" i s) |> String.concat ""
    else
      lines |> String.concat ""
  sprintf "{\n%s%sreturn %s;}\n%s:\n"
    (rule.input|>List.mapi (fun i x->sprintf "var %s=_arg%d;\n" (mangle_local_id x) i) |> String.concat "")
    breakpointed
    (mangle_local_id rule.output)
    (print_label rule_nr)

let print_rule_bodies (rules:rule list) =
  rules |> List.mapi print_rule |> String.concat ""

let generate_breakpoint_array (rules:rule seq) =
  rules |> Seq.map (fun x->x.premis) |> Seq.concat |> Seq.map (fun(a,b)->b) |> Seq.distinct |> Seq.map (fun _->"false") |> String.concat "," |> sprintf "static bool[] _DBUG_breakpoints = {%s};\n"

let print_main (rule:rule) =
  let return_type = mangle_type rule.typemap.[rule.output]
  let body = sprintf "static %s body(){\n%sthrow new System.MissingMethodException();\n}" return_type (print_rule_bodies [rule])
  let main = "static void Main() {\nSystem.Console.WriteLine(System.String.Format(\"{0}\", body()));\n}"
  sprintf "class _main{\n%s%s%s}\n" (if flags.debug then generate_breakpoint_array [rule] else "") body main 

let rec print_tree (lookup:fromTypecheckerWithLove) (ns:List<NamespacedItem>) :string =
  let build_func (name:string) (rules:rule list) = 
    let breakpoints = 
      if flags.debug then 
        generate_breakpoint_array rules 
      else ""
    let rule = List.head rules
    let args = rule.input |> Seq.mapi (fun nr id-> sprintf "public %s _arg%d;\n" (mangle_type rule.typemap.[id]) nr) |> String.concat ""
    let ret_type = mangle_type rule.typemap.[rule.output]
    let rules = print_rule_bodies rules
    sprintf "class %s{\n%s%spublic %s _run(){\n%sthrow new System.MissingMethodException();\n}\n}\n" (CSharpMangle name) args breakpoints ret_type rules
  let print_base_types (ns:List<NamespacedItem>) = 
    let types = ns |> List.fold (fun types item -> match item with Data (_,v) -> v.outputType::types | _ -> types) [] |> List.distinct
    let print t = sprintf "public class %s{}\n" (t|>remove_namespace_of_type|>mangle_type)
    List.map print types
  let go ns =
    match ns with
    | Ns(n,ns)      -> sprintf "namespace %s{\n%s}\n" (CSharpMangle n) (print_tree lookup ns)
    | Data(n,d)     -> sprintf "public class %s:%s{\n%s}\n" (CSharpMangle n) (d.outputType|>remove_namespace_of_type|>mangle_type) (d.args|>List.mapi field|>String.concat "")
    | Function(name,rules) -> build_func name rules
    | Lambda(number,rule)  -> build_func (sprintf "_lambda%d" number) [rule]
  (print_base_types ns)@(ns|>List.map go)|>String.concat "\n"
    
let get_locals (ps:(premisse*int) list) :local_id list =
  ps |> List.collect (fun (p,_) ->
    match p with
    | Literal             x -> [x.dest]
    | Conditional         x -> [x.left;x.right]
    | Destructor          x -> x.source::x.args
    | LambdaClosure       x -> [x.dest]
    | FuncClosure         x -> [x.dest]
    | DotNetCall          x -> [x.dest]
    | DotNetStaticCall    x -> [x.dest]
    | DotNetConstructor   x -> [x.dest]
    | DotNetGet           x -> [x.dest]
    | DotNetSet           x -> [x.src]
    | ConstructorClosure  x -> [x.dest]
    | Application         x -> [x.dest]
    | ApplicationCall     x -> [x.closure;x.dest;x.argument] )

let foldi (f:int->'state->'element->'state) (s:'state) (lst:seq<'element>) :'state =
  let fn ((counter:int),(state:'state)) (element:'element) :int*'state = 
    counter+1,(f counter state element)
  let _,ret = lst|>Seq.fold fn (0,s)
  ret 

let validate (input:fromTypecheckerWithLove) :bool =
  let print_local_id (id:local_id) = match id with Named(x)->x | Tmp(x)->sprintf "temporary(%d)" x
  let print_id (id:Id) = String.concat "^" (id.Name::id.Namespace)
  let check_typemap (id:Id) (rule:rule) :bool =
    let expected = (get_locals rule.premis) @ (rule.output::rule.input) |> List.distinct |> List.sort
    let received  = rule.typemap |> Map.toList |> List.map (fun (x,_)->x) |> List.sort
    if expected = received then true
    else 
      let missing = expected |> List.filter (fun x -> received |> List.exists (fun y->x=y) |> not)
      let extra   = received |> List.filter (fun x -> expected |> List.exists (fun y->x=y) |> not)
      do ice()
      do printf " incorrect typemap in rule %s:\n  missing %A\n  extra: %A\n" (print_id id) missing extra
      false
  let check_dest_constness (id:Id) (rule:rule) (success:bool):bool =
      let per_premisse (statementnr:int) (set:Set<local_id>,success:bool) (premisse:premisse,i:int) :Set<local_id>*bool =
        let check (set:Set<local_id>,success:bool) (local:local_id) =
          if set.Contains(local) then
            do ice()
            do printf " %s assigned twice in rule %s, statement %d on line %d\n" (print_local_id local) (print_id id) statementnr i
            set,false
          else set.Add(local),success
        match premisse with
        | Literal x               -> check (set,success) x.dest
        | Conditional _           -> set,success
        | Destructor x            -> x.args |> Seq.fold check (set,success)
        | ConstructorClosure x    -> check (set,success) x.dest
        | FuncClosure  x          -> check (set,success) x.dest
        | LambdaClosure x         -> check (set,success) x.dest
        | DotNetCall x            -> check (set,success) x.dest
        | DotNetStaticCall x      -> check (set,success) x.dest
        | DotNetConstructor x     -> check (set,success) x.dest
        | DotNetGet x             -> check (set,success) x.dest
        | DotNetSet x             -> set,success
        | Application x           -> check (set,success) x.dest
        | ApplicationCall x       -> check (set,success) x.dest
      let _,ret = rule.premis |> foldi per_premisse (Set.empty,success)
      ret
  (true,input.funcs) ||> Map.fold (fun (success:bool) (id:Id) (rules:rule list)-> 
    if rules.IsEmpty then
      do ice()
      do printf " empty rule: %s\n" (print_id id)
      false
    else
      (true,input.main::rules) ||> List.fold (fun (success:bool) (rule:rule) -> if check_typemap id rule then (check_dest_constness id rule success) else false))

let failsafe_codegen(input:fromTypecheckerWithLove) :Option<string>=
  do flags <- input.flags
  if validate input then
    let foo = input |> construct_tree |> print_tree input
    foo+(print_main input.main) |> Some

  else None

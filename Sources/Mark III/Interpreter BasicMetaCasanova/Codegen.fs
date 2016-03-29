module Codegen
open Common
open CodegenInterface
open Mangle

let ice () = 
    do System.Console.BackgroundColor <- System.ConsoleColor.Red
    do System.Console.Write "INTERNAL COMPILER ERROR"
    do System.Console.ResetColor()

// TODO: generate dotnet closures

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
     |> go (Map.toSeq input.rules) functree 
     |> go input.datas datatree

let print_literal lit =
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

let print_predicate (p:predicate) :string= 
  match p with Less -> "<" | LessEqual -> "<=" | Equal -> "=" | GreaterEqual -> ">=" | Greater -> ">" | NotEqual -> "!="

let field (n:int) (t:Type) :string =
  sprintf "public %s _arg%d;\n" (mangle_type t) n

let highest_tmp (typemap:Map<local_id,Type>): int =
  typemap |> Map.fold (fun s k _ -> match k with Tmp(x) when x>s -> x | _ -> s) 0

let get_map (a:local_id) (m:Map<local_id,int>) :int*Map<local_id,int> =
  match Map.tryFind a m with None -> 0,(Map.add a 1 m) | Some x -> x,(Map.add a (x+1) m)

let overloadableOps =
  [  "+"
     "-"
     "!"
     "~"
     "++"
     "--"
     "true"
     "false"
     "*"
     "/"
     "%"
     "&"
     "|"
     "^"
     "<<"
     ">>"
     "=="
     "!="
     "<"
     ">"
     "<="
     ">="
     "&&"
     "||"
  ] |> Set.ofList

let rec premisse (m:Map<local_id,Type>) (app:Map<local_id,int>) (ps:premisse list) (ret:local_id) =
  match ps with 
  | [] -> sprintf "_ret.Add(%s);\n" (mangle_local_id ret)
  | p::ps -> 
    match p with
    | Literal x -> sprintf "/*LITR*/var %s = %s;\n%s"
                     (mangle_local_id x.dest)
                     (print_literal x.value)
                     (premisse m app ps ret)
    | Conditional x -> sprintf "/*COND*/if(%s %s %s){%s}"
                         (mangle_local_id x.left)
                         (print_predicate x.predicate)
                         (mangle_local_id x.right)
                         (premisse m app ps ret)
    | Destructor x ->
      let new_id  = (Tmp(1+(highest_tmp m)))
      sprintf "/*DTOR*/var %s = %s as %s;\nif(%s!=null){\n%s%s}\n"
        (mangle_local_id new_id)
        (mangle_local_id x.source)
        (mangle_id       x.destructor)
        (mangle_local_id new_id)
        (x.args|>List.mapi(fun nr arg->sprintf "var %s=%s._arg%d;\n" (mangle_local_id arg) (mangle_local_id new_id) nr)|>String.concat "")
        (premisse (m|>Map.add new_id (McType(x.destructor))) app ps ret)
    | ConstructorClosure x
    | FuncClosure x -> sprintf "/*FUNC*/var %s = new %s();\n%s" 
                         (mangle_local_id x.dest)
                         (mangle_id x.func)
                         (premisse m (app|>Map.add x.dest 0) ps ret)
    | LambdaClosure x -> sprintf "/*LAMB*/var %s = new %s();\n%s" 
                           (mangle_local_id x.dest)
                           (mangle_lambda x.func)
                           (premisse m (app|>Map.add x.dest 0) ps ret)
    | DotNetStaticCall x -> 
          let s = x.func.Name.Split([|'.'|])
          let last = s.[s.Length-1]
          if overloadableOps.Contains(last) then 
            let args = x.args |> List.rev
            sprintf "/*NCAL*/var %s = %s %s %s;\n %s"
              (mangle_local_id x.dest)
              (match x.args.Length with
               | 1 -> ""
               | 2 -> mangle_local_id args.[1])
              last
              (mangle_local_id args.[0])
              (premisse m (app|>Map.add x.dest 0) ps ret)
          else
            sprintf "/*NCAL*/var %s = %s(%s);\n%s" 
              (mangle_local_id x.dest)
              (mangle_id x.func)
              (x.args |> List.map mangle_local_id|>String.concat ",")
              (premisse m (app|>Map.add x.dest 0) ps ret)
    | DotNetConstructor x -> sprintf "/*NCON*/var %s = new %s(%s);\n%s" 
                               (mangle_local_id x.dest)
                               (mangle_id {x.func with Namespace = x.func.Namespace |> List.rev})
                               (x.args |> List.map mangle_local_id|>String.concat ",")
                               (premisse m (app|>Map.add x.dest 0) ps ret)
    | DotNetProperty x -> sprintf "/*NPRO*/var %s = %s.%s;\n%s" 
                               (mangle_local_id x.dest)
                               (mangle_local_id x.instance)
                               x.property
                               (premisse m (app|>Map.add x.dest 0) ps ret)
    | Application x -> 
      let i = match app|>Map.tryFind x.closure with Some(x)->x | None-> failwith (sprintf "Application failed: %s is not a closure." (mangle_local_id x.closure))
      sprintf "/*APPL*/var %s = %s; %s.%s=%s;\n%s"
                         (mangle_local_id  x.dest)
                         (mangle_local_id  x.closure)
                         (mangle_local_id  x.dest)
                         (sprintf "_arg%d" i)
                         (mangle_local_id  x.argument)
                         (premisse m (app|>Map.add x.dest (i+1)) ps ret)
    | ImpureApplicationCall x
    | ApplicationCall x -> 
      let i = match app|>Map.tryFind x.closure with Some(x)->x | None-> failwith (sprintf "ApplicationCall failed: %s is not a closure." (mangle_local_id x.closure))
      sprintf "/*CALL*/%s.%s=%s;\nforeach(var %s in %s._run()){\n%s}\n"
        (mangle_local_id  x.closure)
        (sprintf "_arg%d" i)
        (mangle_local_id  x.argument)
        (mangle_local_id  x.dest)
        (mangle_local_id  x.closure)
        (premisse m (app|>Map.add x.dest (i+1)) ps ret)

let print_rule (rule:rule) = 
  sprintf "{\n%s%s}\n"
    (rule.input|>List.mapi (fun i x->sprintf "var %s=_arg%d;\n" (mangle_local_id x) i) |> String.concat "")
    (premisse rule.typemap Map.empty rule.premis rule.output)

let print_rule_bodies (rules:rule list) =
  rules |> List.map print_rule |> String.concat ""

let print_main (rule:rule) =
  let return_type = mangle_type rule.typemap.[rule.output]
  let body = sprintf "static System.Collections.Generic.List<%s> body(){var _ret = new System.Collections.Generic.List<%s>();\n%sreturn _ret;\n}" return_type return_type (print_rule_bodies [rule])
  let main = "static void Main() {\nforeach(var res in body()){System.Console.WriteLine(System.String.Format(\"{0}\", res));\n}\n}"
  sprintf "class _main{\n%s%s}\n" body main 

let rec print_tree (lookup:fromTypecheckerWithLove) (ns:List<NamespacedItem>) :string =
  let build_func (name:string) (rules:rule list) = 
    let rule = List.head rules
    let args = rule.input |> Seq.mapi (fun nr id-> sprintf "public %s _arg%d;\n" (mangle_type rule.typemap.[id]) nr) |> String.concat ""
    let ret_type = mangle_type rule.typemap.[rule.output]
    let rules = print_rule_bodies rules
    sprintf "class %s{\n%spublic System.Collections.Generic.List<%s> _run(){\nvar _ret = new System.Collections.Generic.List<%s>();\n%sreturn _ret;\n}\n}\n" (CSharpMangle name) args ret_type ret_type rules
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
    
let get_locals (ps:premisse list) :local_id list =
  ps |> List.collect (fun p ->
    match p with
    | Literal             x -> [x.dest]
    | Conditional         x -> [x.left;x.right]
    | Destructor          x -> x.source::x.args
    | LambdaClosure       x -> [x.dest]
    | FuncClosure         x -> [x.dest]
    | DotNetCall          x -> [x.dest]
    | DotNetStaticCall    x -> [x.dest]
    | DotNetConstructor   x -> [x.dest]
    | DotNetProperty      x -> [x.dest]
    | ConstructorClosure  x -> [x.dest]
    | Application         x
    | ImpureApplicationCall x 
    | ApplicationCall     x -> [x.closure;x.dest;x.argument] )

let foldi (f:int->'state->'element->'state) (s:'state) (lst:seq<'element>) :'state =
  let fn ((counter:int),(state:'state)) (element:'element) :'counter*'state = 
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
      let per_premisse (statementnr:int) (set:Set<local_id>,success:bool) (premisse:premisse) :Set<local_id>*bool =
        let check (set:Set<local_id>,success:bool) (local:local_id) =
          if set.Contains(local) then
            do ice()
            do printf " %s assigned twice in rule %s, statement %d\n" (print_local_id local) (print_id id) statementnr
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
        | DotNetProperty x        -> check (set,success) x.dest
        | Application x           -> check (set,success) x.dest
        | ApplicationCall x       -> check (set,success) x.dest
        | ImpureApplicationCall x -> check (set,success) x.dest
      let _,ret = rule.premis |> foldi per_premisse (Set.empty,success)
      ret
  (true,input.rules) ||> Map.fold (fun (success:bool) (id:Id) (rules:rule list)-> 
    if rules.IsEmpty then
      do ice()
      do printf " empty rule: %s\n" (print_id id)
      false
    else
      (true,input.main::rules) ||> List.fold (fun (success:bool) (rule:rule) -> if check_typemap id rule then (check_dest_constness id rule success) else false))

let failsafe_codegen(input:fromTypecheckerWithLove) :Option<string>=
  if validate input then
    let foo = input |> construct_tree |> print_tree input
    foo+(print_main input.main) |> Some

  else None

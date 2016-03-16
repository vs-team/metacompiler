module Codegen
open Common
open CodegenInterface

// mangling ////////////////////////////////////////////////////////////////////
    
let genericMangle (name:string) :string =
  let readables = 
    Map.ofArray <| Array.zip "!#$%&'*+,-./\\:;<>=?@^_`|~"B [|
      "bang";"hash";"cash";"perc";"amp";"prime";"star";"plus";"comma";
      "dash"; "dot";"slash";"back";"colon";"semi";"less";"great";
      "equal";"quest";"at";"caret";"under";"tick";"pipe";"tilde"|]
  let mangleChar c =
    if (c>='A'&&c<='Z') || (c>='a'&&c<='z') || (c>='0'&&c<='9') then
      sprintf "%c" c
    else 
      let lookup = readables |> Map.tryFind (System.Convert.ToByte c)
      match lookup with
      | None   ->  failwith <| sprintf "ERROR(codegen): expecting printable ASCII character, got (0x%04X)" (System.Convert.ToUInt16(c))
      | Some x -> sprintf "_%s" x
  name |> String.collect mangleChar

let CSharpMangle (name:string) :string =
  let keywords = Set.ofArray [| "abstract" ; "as" ; "base" ; "bool" ; 
    "break" ; "byte" ; "case" ; "catch" ; "char" ; "checked" ; "class" ; "const" ;
    "continue" ; "decimal" ; "default" ; "delegate" ; "do" ; "double" ; "else" ;
    "enum" ; "event" ; "explicit" ; "extern" ; "false" ; "finally" ; "fixed" ; 
    "float" ; "for" ; "foreach" ; "goto" ; "if" ; "implicit" ; "in" ; "int" ; 
    "interface" ; "internal" ; "is" ; "lock" ; "long" ; "namespace" ; "new" ;
    "null" ; "object" ; "operator" ; "out" ; "override" ; "params" ; "private" ;
    "protected" ; "public" ; "readonly" ; "ref" ; "return" ; "sbyte" ; "sealed" ;
    "short" ; "sizeof" ; "stackalloc" ; "static" ; "string" ; "struct" ; 
    "switch" ; "this" ; "throw" ; "true" ; "try" ; "typeof" ; "uint" ; "ulong" ; 
    "unchecked" ; "unsafe" ; "ushort" ; "using" ; "virtual" ; "void" ; 
    "volatile" ; "while" |]
  let name = genericMangle name
  if keywords.Contains(name) then sprintf "@%s" name else name 

let rec mangle_type_suffix(t:Type):string=
  match t with
  | DotNetType (id) -> id.Namespace@[id.Name] |> Seq.map genericMangle |> String.concat "_ns"
  | McType     (id) -> id.Namespace@[id.Name] |> Seq.map genericMangle |> String.concat "_ns"
  | TypeApplication (fn,lst) -> (mangle_type_suffix fn)+"_of"+(lst |> List.map mangle_type_suffix |> String.concat "_t")

let rec remove_namespace_of_type(t:Type):Type=
  match t with
  | DotNetType (id) -> DotNetType({id with Namespace=[]})
  | McType     (id) -> McType({id with Namespace=[]})
  | TypeApplication (fn,lst) -> TypeApplication((remove_namespace_of_type fn),lst)
  
let rec mangle_type(t:Type):string=
  match t with
  | DotNetType (id) -> id.Namespace@[id.Name] |> Seq.map CSharpMangle |> String.concat "."
  | McType     (id) -> id.Namespace@[id.Name] |> Seq.map (fun x->if x="System" then "_System" else CSharpMangle x) |> String.concat "."
  | TypeApplication (fn,lst) -> (mangle_type fn)+"_of"+(lst|>List.map mangle_type_suffix|>String.concat "_t")

let mangle_local_id n = match n with Named x -> CSharpMangle x | Tmp x -> sprintf "_tmp%d" x 
let mangle_id (id:Id) = (id.Name::id.Namespace) |> List.rev |> List.map (fun x->if x="System" then "_System" else CSharpMangle x) |> String.concat "."
let mangle_lambda (id:LambdaId) = sprintf "%s._lambda%d" (id.Namespace|>List.rev|>String.concat ".") id.Name
let mangle_rule_id(id:rule_id) =
  match id with 
  | Lambda x -> mangle_lambda x
  | Func x -> mangle_id x

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
  | I64 i    -> sprintf "%d" i
  | U64 i    -> sprintf "%u" i
  | F64 i    -> sprintf "%f" i
  | String s -> sprintf "\"%s\"" s
  | Bool b   -> if b then "true" else "false"

let print_predicate (p:predicate) :string= 
  match p with Less -> "<" | LessEqual -> "<=" | Equal -> "=" | GreaterEqual -> ">=" | Greater -> ">" | NotEqual -> "!="

let field (n:int) (t:Type) :string =
  sprintf "public %s _arg%d;\n" (mangle_type t) n

let highest_tmp (typemap:Map<local_id,Type>): int =
  typemap |> Map.fold (fun s k _ -> match k with Tmp(x) when x>s -> x | _ -> s) 0

let rec premisse (m:Map<local_id,Type>) (ps:premisse list) (ret:local_id) =
  match ps with 
  | [] -> sprintf "_ret.Add(%s);\n" (mangle_local_id ret)
  | p::ps -> 
    match p with
    | Literal x -> sprintf "var %s = %s;\n%s"
                     (mangle_local_id x.dest)
                     (print_literal x.value)
                     (premisse m ps ret)
    | Conditional x -> sprintf "if(%s %s %s){%s}"
                         (mangle_local_id x.left)
                         (print_predicate x.predicate)
                         (mangle_local_id x.right)
                         (premisse m ps ret)
    | Destructor x ->
      let new_id  = (Tmp(1+(highest_tmp m)))
      sprintf "var %s = %s as %s;\nif(%s!=null){\n%s%s}\n"
        (mangle_local_id new_id)
        (mangle_local_id x.source)
        (mangle_id       x.destructor)
        (mangle_local_id new_id)
        (x.args|>List.mapi(fun nr arg->sprintf "var %s=%s._arg%d;\n" (mangle_local_id arg) (mangle_local_id new_id) nr)|>String.concat "")
        (premisse (m|>Map.add new_id (McType(x.destructor))) ps ret)
    | McClosure x -> sprintf "var %s = new %s();\n%s" 
                       (mangle_local_id x.dest)
                       (mangle_rule_id x.func)
                       (premisse m ps ret)
    | DotNetClosure x -> sprintf "var %s = new _dotnet.%s();\n%s" 
                           (mangle_local_id x.dest)
                           (mangle_id x.func)
                           (premisse m ps ret)
    | ConstructorClosure x -> sprintf "var %s = new %s();\n%s" 
                                (mangle_local_id x.dest)
                                (mangle_id x.func)
                                (premisse m ps ret)
    | Application x -> sprintf "var %s = %s; %s.%s=%s;\n%s"
                         (mangle_local_id  x.dest)
                         (mangle_local_id  x.closure)
                         (mangle_local_id  x.dest)
                         (sprintf "_arg%d" x.argnr)
                         (mangle_local_id  x.argument)
                         (premisse m ps ret)
    | ImpureApplicationCall x
    | ApplicationCall x -> sprintf "%s.%s=%s;\nforeach(var %s in %s._run()){\n%s}\n"
                             (mangle_local_id  x.closure)
                             (sprintf "_arg%d" x.argnr)
                             (mangle_local_id  x.argument)
                             (mangle_local_id  x.dest)
                             (mangle_local_id  x.closure)
                             (premisse m ps ret)

let print_rule (rule:rule) = 
  sprintf "{\n%s%s}\n"
    (rule.input|>List.mapi (fun i x->sprintf "var %s=_arg%d;\n" (mangle_local_id x) i) |> String.concat "")
    (premisse rule.typemap rule.premis rule.output)

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
    
// TEST DATA ///////////////////////////////////////////////////////////////////

let test_data:fromTypecheckerWithLove=
  let int_t:Type   = DotNetType({Namespace=["System"];Name="Int32"})
  let float_t:Type = DotNetType({Namespace=["System"];Name="Single"})
  let star_t:Type  = TypeApplication((McType({Namespace=["test";"mc"];Name="star"})),[int_t;float_t])
  let pipe_t:Type  = TypeApplication((McType({Namespace=["test";"mc"];Name="pipe"})),[int_t;float_t])
  let comma_id:Id  = {Namespace=["test";"mc"];Name="comma"}
  let left_id:Id   = {Namespace=["test";"mc"];Name="left"}
  let right_id:Id  = {Namespace=["test";"mc"];Name="right"}
  let comma_data:data = 
    { 
      args=[int_t; float_t; ];
      outputType=star_t;
    }
  let left_data:data =
    {
      args=[int_t;]; 
      outputType=pipe_t;
    }
  let right_data:data =
    {
      args=[float_t;]; 
      outputType=pipe_t;
    }
  let datas = [comma_id,comma_data; left_id,left_data; right_id,right_data]
  let main = {input=[];output=Tmp(0);premis=[];typemap=Map.empty;side_effect=true}
  {rules=Map.empty;lambdas=Map.empty;datas=datas;main=main}

let list_test:fromTypecheckerWithLove =
  let int_t:Type   = DotNetType({Namespace=["System"];Name="Int32"})
  let add_t:Type = Arrow(int_t,Arrow(int_t,int_t))
  let add_id:Id  = {Namespace=["builtin"];Name="add"}

  // data "nil" -> List
  let list_t:Type  = TypeApplication((McType({Namespace=["mc";"test"];Name="List"})),[int_t])
  let nil_id:Id    = {Namespace=["test";"mc"];Name="nil"}
  let nil_data:data =
    {
      args=[];
      outputType=list_t;
    }

  // data Int -> "::" -> List -> List
  let append_id:Id = {Namespace=["test";"mc"];Name="::"}
  let append_data:data =
    {
      args=[int_t; list_t];
      outputType=list_t;
    }
  
  // length nil -> 0
  let length_t:Type= Arrow (list_t,int_t)
  let length_id:Id = {Namespace=["test";"mc"];Name="length"}
  let length_nil:rule =
    {
      side_effect=false
      input=[Tmp(0)]
      premis=[Destructor({source=Tmp(0); destructor=nil_id; args=[]})
              Literal   ({value=I64(0L);dest=Tmp(1)}) ]
      output=Tmp(1)
      typemap=Map.ofSeq [Tmp(0),list_t
                         Tmp(1),int_t ]
    }

  // length xs -> r
  // -------------------------------
  // length x::xs -> add^builtin r 1
  let length_append:rule =
    {
      side_effect=false
      input=[Tmp(0)]
      premis=[Destructor({source=Tmp(0); destructor=append_id; args=[Named("x");Named("xs")]})
              McClosure({func=Func(length_id); dest=Tmp(1)})
              ApplicationCall({argnr=0;closure=Tmp(1); argument=Named("xs"); dest=Named("r")})
              Literal({value=I64(1L); dest=Tmp(2)})
              DotNetClosure({func={Name="add";Namespace=["Int32";"System"]};dest=Tmp(3)})
              Application({argnr=0;closure=Tmp(3); argument=Named("r"); dest=Tmp(4)})
              ApplicationCall({argnr=1;closure=Tmp(4); argument=Tmp(2); dest=Tmp(5)}) ]
      output=Tmp(5)
      typemap=Map.ofSeq [Tmp(0),list_t
                         Named("x"),int_t
                         Named("xs"),list_t
                         Tmp(1),Arrow(list_t,int_t)
                         Named("r"),int_t
                         Tmp(2),int_t
                         Tmp(3),Arrow(int_t,(Arrow(int_t,int_t)))
                         Tmp(4),Arrow(int_t,int_t)
                         Tmp(5),int_t ]
    }

  let datas = [nil_id,nil_data; append_id,append_data]
  let Funcs = Map.ofSeq <| [length_id,[length_nil;length_append]]
  let main = 
    {
      input=[]
      output=Tmp(7)
      premis=[ConstructorClosure({func=nil_id;dest=Named("end")})
              Literal({value=I64(2L);dest=Named("second")})
              ConstructorClosure({func=append_id;dest=Tmp(0)})
              Application({closure=Tmp(0); argnr=0; argument=Named("second"); dest=Tmp(1)})
              Application({closure=Tmp(1); argnr=1; argument=Named("end"); dest=Tmp(2)})
              Literal({value=I64(1L);dest=Named("first")})
              ConstructorClosure({func=append_id;dest=Tmp(3)})
              Application({closure=Tmp(3); argnr=0; argument=Named("first"); dest=Tmp(4)})
              Application({closure=Tmp(4); argnr=1; argument=Tmp(2); dest=Tmp(5)})
              McClosure({func=Func(length_id); dest=Tmp(6)})
              ApplicationCall({argnr=0; closure=Tmp(6); argument=Tmp(5); dest=Tmp(7)})]
      typemap=Map.ofSeq [Named("end"),list_t
                         Named("second"),int_t
                         Named("first"),int_t
                         Tmp(0),Arrow(int_t,(Arrow(list_t,list_t)))
                         Tmp(1),Arrow(list_t,list_t)
                         Tmp(2),list_t
                         Tmp(3),Arrow(int_t,(Arrow(list_t,list_t)))
                         Tmp(4),Arrow(list_t,list_t)
                         Tmp(5),list_t
                         Tmp(6),Arrow(list_t,int_t)
                         Tmp(7),int_t]
      side_effect=true
    }
  {rules=Funcs;datas=datas;lambdas=Map.empty;main=main}

let get_locals (ps:premisse list) :local_id list =
  ps |> List.collect (fun p ->
    match p with
    | Literal             x -> [x.dest]
    | Conditional         x -> [x.left;x.right]
    | Destructor          x -> x.source::x.args
    | McClosure           x -> [x.dest]
    | DotNetClosure       x -> [x.dest]
    | ConstructorClosure  x -> [x.dest]
    | Application         x
    | ImpureApplicationCall x 
    | ApplicationCall     x -> [x.closure;x.dest;x.argument] )

let foldi (f:'int->'state->'element->'state) (s:'state) (lst:seq<'element>) :'state =
  let fn ((counter:'int),(state:'state)) (element:'element) :'counter*'state = 
    counter+1,(f counter state element)
  let _,ret = lst|>Seq.fold fn (0,s)
  ret 

let validate (input:fromTypecheckerWithLove) :bool =
  let ice () = 
      do System.Console.BackgroundColor <- System.ConsoleColor.Red
      do System.Console.Write "INTERNAL COMPILER ERROR"
      do System.Console.ResetColor()
  let print_local_id (id:local_id) = match id with Named(x)->x | Tmp(x)->sprintf "temporary(%d)" x
  let print_id (id:Id) = String.concat "^" (id.Name::id.Namespace)
  let check_typemap (id:Id) (rule:rule) :bool =
    let expected = (get_locals rule.premis) @ rule.input |> List.distinct |> List.sort
    let received  = rule.typemap |> Map.toList |> List.map (fun (x,_)->x) |> List.sort
    if expected = received then true
    else 
      do ice()
      do printf " incorrect typemap in rule %s:\n  expected: %A\n  received: %A\n" (print_id id) expected received
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
        | McClosure  x            -> check (set,success) x.dest
        | DotNetClosure x         -> check (set,success) x.dest
        | ConstructorClosure x    -> check (set,success) x.dest
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

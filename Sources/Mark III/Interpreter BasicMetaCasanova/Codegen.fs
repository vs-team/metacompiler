module Codegen
open Common
open ParserMonad
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
  | TypeApplication (fn,lst) -> fn::lst |> List.map mangle_type_suffix |> String.concat "_of"
  
let rec mangle_type(t:Type):string=
  match t with
  | DotNetType (id) -> id.Namespace@[id.Name] |> Seq.map CSharpMangle |> String.concat "."
  | McType     (id) -> id.Namespace@[id.Name] |> Seq.map (fun x->if x="System" then "_System" else CSharpMangle x) |> String.concat "."
  | TypeApplication (fn,lst) -> mangle_type fn

 (*
let rec strip_namespace_type(t:Type) :Type = 
  match t with
  | DotNetType (id) -> DotNetType ([],id.Name)
  | McType     (_,n) -> McType     ([],n)
  | TypeApplication (fn,lst) -> TypeApplication ((strip_namespace_type fn),lst)
  *)

let print_local_id n = match n with Named x -> CSharpMangle x | Tmp x -> sprintf "_tmp%d" x 

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
  let go input output state = input |> Seq.map (fun (n,d)->n.Namespace,(n.Name,d)) |> Seq.fold output state
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

  (*
let print_global_id id = "ID"
  //match id with
  //| global_id.Lambda l -> sprintf "%s.%s" (String.concat l.Namespace ".") l.Name

let rec func_arity (t:Type) :int =
  match t with
  | Arrow (_,x) -> 1 + (func_arity x)
  | _ -> 0


let rec print_rule(r:rule)(t:Map<string,Type>):string =
  match p with
  | [] -> ""
  | p::ps ->
    match p with
    | Literal(l,id) -> sprintf "val %s = %s;\n%s" (print_local_id id) (print_literal l) (print_premises p highest_tmp)
    | Conditional(x,c,y) -> 
      let cond = 
        match c with
        | Less -> "<"
        | LessEqual -> "<="
        | Equal -> "="
        | GreaterEqual -> ">="
        | Greater -> ">"
        | NotEqual -> "!="
      sprintf "if(%s%s%s){%s}" (print_local_id x) cond (print_local_id y) (print_premises ps highest_tmp)
    | Destructor(from,id,ret) ->
      // var $tmp = $from as $specific_type
      // if($tmp!=null){
      //   var %arg[i] = %tmp.%arg[i]
      let new_highest_tmp = highest_tmp+1
      let new_tmp = print_local_id (Tmp(new_tmp))
      let cast = sprintf "var %s = %s as %s;" new_tmp (print_local_id from) (print_global id)
      let args = ret |> List.map (fun x-> sprintf "var %s=%s.%s;")
      let cond = sprintf "if(%s!=null){%s%s}" new_tmp 
      
      

let print_func (s:string) (t:Type) (rules:List<rule>) =
  //let print_premisse (p:premisse) =
  //  match p with
  //  | Literal (loc,lit)    -> sprintf "var %s=%s;\n" (print_local_id loc) (print_literal lit)
  //  | Call (loc,func,args) -> sprintf "var %s=%s" (print_local_id loc) (print_global_id func)
  let print_rule = "{\nrule\n}\n" 
  let body = sprintf "List<%s> _ret;\n%s\nreturn _ret;\n" "RET_TYPE" "BODY"
  let print_func = sprintf "public static List<%s> _call(%s){%s}" "RET_TYPE" "RULES" body
  let closure_call = sprintf "public %s _closure_call(){return call(%s);}" "RET_TYPE" "ARGS"
  sprintf "public class %s{\n%s}" (CSharpMangle s) body
*)

let field (n:local_id,t:Type) = sprintf "public %s %s;\n" (mangle_type t) (print_local_id n)

let get_locals (ps:premisse list) :local_id list =
  ps |> List.collect (fun p ->
    match p with
    | Literal             x -> [x.dest]
    | Conditional         x -> [x.left;x.right]
    | Destructor          x -> x.source::x.args
    | McClosure           x -> [x.dest]
    | DotNetClosure       x -> [x.dest]
    | ConstructorClosure  x -> [x.dest]
    | Application         x -> [x.closure;x.dest;x.argument] ) |> List.distinct

let highest_tmp (ps:premisse list) :int =
  ps |> get_locals |> List.fold 
    (fun s x-> match x with Tmp(x)->x::s | _ -> s) []
    |> List.max
    (*
let print_rule (r:rule) (lookup:fromTypecheckerWithLove) =
  let max_tmp = highest_tmp r.premis
  let prem (p:premisse) (ps:premisse list) (max_tmp:int) =
    match p with
    | Destructor (from,type_id,args) ->
      let max_tmp = max_tmp+1
      let data = lookup.data.[type_id].output |> strip_namespace_type |>
      let cast_line = sprintf "var %s = %s as %s;\n" (Tmp(max_tmp))
      *)
let rec print_tree (lookup:fromTypecheckerWithLove) (ns:List<NamespacedItem>) :string =
  let print_base_types (ns:List<NamespacedItem>) = 
    let types = ns |> List.fold (fun types item -> match item with Data (_,v) -> v.outputType::types | _ -> types) [] |> List.distinct
    let print t = sprintf "public class %s{}\n" (t|>mangle_type_suffix)
    List.map print types
  let go ns = match ns with
              | Ns(n,ns)     -> sprintf "namespace %s{\n%s}\n" (CSharpMangle n) (print_tree lookup ns)
              | Data(n,d)    -> sprintf "public class %s:%s{\n%s}\n" (CSharpMangle n) (d.outputType|>mangle_type_suffix) (d.args|>List.map field|>String.concat "")
              | Function(s,_)-> sprintf "// todo: Func %s\n" (CSharpMangle s)
              | Lambda(s,_)  -> sprintf "// todo: Lambda nr %d\n" s
  (print_base_types ns)@(ns|>List.map go)|>String.concat "\n"
    
let failsafe_codegen(input:fromTypecheckerWithLove) =
  input |> construct_tree |> print_tree input |> printf "%s"

// TEST DATA ///////////////////////////////////////////////////////////////////

let test_data:fromTypecheckerWithLove=
  let int_t:Type   = DotNetType({Namespace=["System"];Name="Int32"})
  let float_t:Type = DotNetType({Namespace=["System"];Name="Single"})
  let star_t:Type  = TypeApplication((McType({Namespace=["mc";"test"];Name="star"})),[int_t;float_t])
  let pipe_t:Type  = TypeApplication((McType({Namespace=["mc";"test"];Name="pipe"})),[int_t;float_t])
  let comma_id:Id  = {Namespace=["mc";"test"];Name="comma"}
  let left_id:Id   = {Namespace=["mc";"test"];Name="left"}
  let right_id:Id  = {Namespace=["mc";"test"];Name="right"}
  let comma_data:data = 
    { 
      args=[Named("a"),int_t; Named("b"),float_t; ];
      outputType=star_t;
    }
  let left_data:data =
    {
      args=[Named("a"),int_t;]; 
      outputType=pipe_t;
    }
  let right_data:data =
    {
      args=[Named("a"),float_t;]; 
      outputType=pipe_t;
    }
  let datas = [comma_id,comma_data; left_id,left_data; right_id,right_data]
  let main = {input=[];output=Tmp(0);premis=[];typemap=Map.empty;side_effect=true}
  {rules=Map.empty;lambdas=Map.empty;datas=datas;main=main}

let list_test:fromTypecheckerWithLove =
  let int_t:Type   = DotNetType({Namespace=["System"];Name="Int32"})
  let add_t:Type = Arrow(int_t,Arrow(int_t,int_t))
  let add_id:Id  = {Namespace=["builtin"];Name="add"}

  // data "nil" -> List int
  let list_t:Type  = TypeApplication((McType({Namespace=["mc";"test"];Name="List"})),[int_t])
  let nil_id:Id    = {Namespace=["mc";"test"];Name="nil"}
  let nil_data:data =
    {
      args=[];
      outputType=list_t;
    }

  // data x -> "::" -> xs -> List int
  let append_id:Id = {Namespace=["mc";"test"];Name="::"}
  let append_data:data =
    {
      args=[Named("x"),int_t; Named("xs"),int_t];
      outputType=list_t;
    }
  
  // length nil -> 0
  let length_t:Type= Arrow (list_t,int_t)
  let length_id:Id = {Namespace=["mc";"test"];Name="length"}
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
  // length x::xs -> add^builtin r 0
  let length_append:rule =
    {
      side_effect=false
      input=[Tmp(0)]
      premis=[Destructor({source=Tmp(0); destructor=append_id; args=[Named("x");Named("xs")]})
              McClosure({func=Func(length_id); dest=Tmp(1)})
              Application({closure=Tmp(1); argument=Named("xs"); dest=Named("r")})
              Literal({value=I64(0L); dest=Tmp(2)})
              DotNetClosure({func={Name="add";Namespace=["System";"Int32"]};dest=Tmp(3)})
              Application({closure=Tmp(3); argument=Named("r"); dest=Tmp(4)})
              Application({closure=Tmp(4); argument=Tmp(2);     dest=Tmp(5)}) ]
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
  let main = {input=[];output=Tmp(0);premis=[];typemap=Map.empty;side_effect=true}
  {rules=Funcs;datas=datas;lambdas=Map.empty;main=main}
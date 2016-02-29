module IR
open Common
open ParserMonad
open TypeChecker

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
  | DotNetType (ns,n) -> ns@[n] |> Seq.map genericMangle |> String.concat "_ns"
  | McType     (ns,n) -> ns@[n] |> Seq.map genericMangle |> String.concat "_ns"
  | TypeApplication (fn,lst) -> fn::lst |> List.map mangle_type_suffix |> String.concat "_of"
  
let rec mangle_type(t:Type):string=
  match t with
  | DotNetType (ns,n) -> ns@[n] |> Seq.map CSharpMangle |> String.concat "."
  | McType     (ns,n) -> ns@[n] |> Seq.map (fun x->if x="System" then "_System" else CSharpMangle x) |> String.concat "."
  | TypeApplication (fn,lst) -> mangle_type fn

let rec strip_namespace_type(t:Type) :Type = 
  match t with
  | DotNetType (_,n) -> DotNetType ([],n)
  | McType     (_,n) -> McType     ([],n)
  | TypeApplication (fn,lst) -> TypeApplication ((strip_namespace_type fn),lst)

let print_local_id n = match n with Named x -> CSharpMangle x | Tmp x -> sprintf "_tmp%d" x 


type NamespacedItem = Ns     of string*List<NamespacedItem>
                    | Data   of string*data
                    | Func   of string*List<rule>
                    | Lambda of int*rule

let construct_tree (input:fromTypecheckerWithLove) :List<NamespacedItem> =
  let rec datatree (s:List<NamespacedItem>) (idx:List<string>,v:string*data) :List<NamespacedItem> =
    match idx with
    | []    -> (Data(v))::s
    | n::ns -> match s |> List.partition (fun x->match x with Ns(n,_)->true | _->false) with
               | [Ns(n,body)],rest -> Ns(n,datatree body (ns,v))::rest
               | [],list           -> Ns(n,datatree []   (ns,v))::list
  let rec functree (s:List<NamespacedItem>) (idx:List<string>,v:string*List<rule>) :List<NamespacedItem> =
    match idx with
    | []    -> (Func(v))::s
    | n::ns -> match s |> List.partition (fun x->match x with Ns(n,_)->true | _->false) with
               | [Ns(n,body)],rest -> Ns(n,functree body (ns,v))::rest
               | [],list           -> Ns(n,functree []   (ns,v))::list
  let rec lambdatree (s:List<NamespacedItem>) (idx:List<string>,v:int*rule) :List<NamespacedItem> =
    match idx with
    | []    -> (Lambda(v))::s
    | n::ns -> match s |> List.partition (fun x->match x with Ns(n,_)->true | _->false) with
               | [Ns(n,body)],rest -> Ns(n,lambdatree body (ns,v))::rest
               | [],list           -> Ns(n,lambdatree []   (ns,v))::list
  let go input output state = input |> Map.toSeq |> Seq.map (fun (n,d)->n.Namespace,(n.Name,d)) |> Seq.fold output state
  [] |> go input.lambdas lambdatree |> go input.rules   functree |> go input.datas datatree
          
let rec print_tree (ns:List<NamespacedItem>) :string =
  let print_base_types (ns:List<NamespacedItem>) = 
    let types = ns |> List.fold (fun types item -> match item with Data (_,v) -> v.output::types | _ -> types) [] |> List.distinct
    let print t = sprintf "public class %s{}\n" (t|>strip_namespace_type|>mangle_type_suffix)
    List.map print types
  let field (n:local_id,t:Type) = sprintf "public %s %s;\n" (mangle_type t) (print_local_id n)
  let go ns = match ns with
              | Ns (n,ns) -> sprintf "namespace %s{\n%s}\n" (CSharpMangle n) (print_tree ns)
              | Data (n,d) -> sprintf "public class %s:%s{\n%s}\n" (CSharpMangle n) (d.output|>strip_namespace_type|>mangle_type_suffix) (d.args|>List.map field|>String.concat "")
              | Func (s,_) -> sprintf "// todo: Func %s\n" (CSharpMangle s)
              | Lambda (s,_) -> sprintf "// todo: Lambda nr %d\n" s
  (print_base_types ns)@(ns|>List.map go)|>String.concat "\n"
    
let failsafe_codegen(input:fromTypecheckerWithLove) =
  input |> construct_tree |> print_tree |> printf "%s"

let test_data:fromTypecheckerWithLove=
  let int_t:Type   = DotNetType(["System"],"Int32");
  let float_t:Type = DotNetType(["System"],"float");
  let star_t:Type  = TypeApplication((McType(["mc";"test"],"star")),[int_t;float_t])
  let pipe_t:Type  = TypeApplication((McType(["mc";"test"],"pipe")),[int_t;float_t])
  let comma_id:Id  = {Namespace=["mc";"test"];Name="comma";Type=star_t}
  let left_id:Id   = {Namespace=["mc";"test"];Name="left"; Type=pipe_t}
  let right_id:Id  = {Namespace=["mc";"test"];Name="right";Type=pipe_t}
  let comma_data:data = 
    { 
      args=[Named("a"),int_t; Named("b"),float_t; ];
      output=star_t;
    }
  let left_data:data =
    {
      args=[Named("a"),int_t;]; 
      output=pipe_t;
    }
  let right_data:data =
    {
      args=[Named("a"),float_t;]; 
      output=pipe_t;
    }
  let datas = Map.ofSeq <| [comma_id,comma_data; left_id,left_data; right_id,right_data]
  {rules=Map.empty;lambdas=Map.empty;datas=datas}

let list_test:fromTypecheckerWithLove =
  let int_t:Type   = DotNetType(["System"],"Int32");
  let list_t:Type  = TypeApplication((McType(["mc";"test"],"List")),[int_t])
  let append_id:Id = {Namespace=["mc";"test"];Name="::";Type=list_t}
  let nil_id:Id    = {Namespace=["mc";"test"];Name="nil";Type=list_t}
  let append_data:data =
    {
      args=[Named("x"),int_t; Named("xs"),int_t];
      output=list_t;
    }
  let nil_data:data =
    {
      args=[];
      output=list_t;
    }

  let add_t:Type = Arrow(int_t,Arrow(int_t,int_t))
  let add_id:Id  = {Namespace=["builtin"];Name="add";Type=add_t}

  let length_t:Type= Arrow (list_t,int_t)
  let length_id:Id = {Namespace=["mc";"test"];Name="length";Type=length_t}
  let length_nil:rule =
    {
      input=[DestructorCall(nil_id,[])]
      premis=[]
      output=Lit(I32(0))
      typemap=Map.empty
    }
  let length_append:rule =
    {
      input=[DestructorCall(append_id,[Named("x");Named("xs")])]
      premis=[Assignment(RuleCall(global_id.Named(length_id),[Named("xs")]),Id(Named("r")))
              Assignment(Lit(I32(1)),Id(Tmp(0)))]
      output=DotNetCall(global_id.Named(add_id),[Named("r");Tmp(0)])
      typemap=Map.ofSeq [Named("x"),int_t;
                         Named("xs"),list_t;
                         Named("r"),int_t;
                         Tmp(0),int_t]
    }

  let datas = Map.ofSeq <| [nil_id,nil_data; append_id,append_data]
  let Funcs = Map.ofSeq <| [length_id,[length_nil;length_append]]
  {rules=Funcs;datas=datas;lambdas=Map.empty}
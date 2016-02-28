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

// data generation /////////////////////////////////////////////////////////////

let group_by_type (constructors:Map<Id,data>) :Map<Type,List<Id*data>> =
  (Map.empty,constructors) ||> Map.fold (fun r k v ->
    let t = v.output
    match r |> Map.tryFind t with
    | Some ds -> Map.add t ((k,v)::ds) r
    | None    -> Map.add t [k,v]     r )

let generate_type ((t:Type),(constructors:List<Id*data>)) :string =
  let field (n:local_id,t:Type) = sprintf "public %s %s;" (mangle_type t) (print_local_id n)
  let elements = constructors |> List.map (fun (id:Id,d:data) -> 
    sprintf "public class %s:%s{%s};\n" (CSharpMangle id.Name) (t|>strip_namespace_type|>mangle_type_suffix) ((List.map field d.args)|>String.concat " ") )
  sprintf "class %s{\n%s};\n" (t|>strip_namespace_type|>mangle_type_suffix) (elements|>String.concat "")

// rule generation /////////////////////////////////////////////////////////////

// namespace generation ////////////////////////////////////////////////////////


// program and tests ///////////////////////////////////////////////////////////

type namespace_body = {
  Types   : Map<Type,List<Id*data>>
  Lambdas : Map<LambdaId,rule>
  Funcs   : Map<Id,List<rule>>
}

type tree<'i,'d when 'i : comparison> = {children:Map<'i,tree<'i,'d>>; data:seq<'d>}

let tree_empty = {children=Map.empty;data=Seq.empty}

let rec tree_add (index:List<'i>) (f:('d->'d)) (tree:tree<'i,'d>) :tree<'i,'d> =
  match index with
  | []    -> {tree with data = Seq.map f tree.data}
  | x::xs -> match Map.tryFind x tree.children with
             | Some node -> tree_add xs f node
             | None      -> {tree with children = tree.children |> Map.add x (tree_add xs f tree_empty)}

let rec tree_flatten (paropen:('i->'a)) (element:(seq<'d>->'a)) (parclose:('i->'a)) (i:'i) (tree:tree<'i,'d>):seq<'a> =
  let tmp x = x 
  [ Seq.ofList [paropen i; element tree.data]; 
    tree.children |> Map.map (fun k v ->tree_flatten paropen element parclose k v) |> Map.toSeq |> Seq.map (fun (_,x)->x) |> Seq.concat;
    Seq.ofList [parclose i]] |> Seq.concat

let rec get_namespace (t:Type) :List<string> =
  match t with
  | DotNetType (ns,_) -> ns
  | McType     (ns,_) -> ns
  | TypeApplication (t,_) -> get_namespace t

let real_codegen input =
  let body = {Types=group_by_type input.datas; Lambdas=input.lambdas; Funcs=input.rules}
  body.Types |> Map.toSeq |> Seq.map generate_type |> String.concat "\n"

type NamespacedItem = Ns   of string*List<NamespacedItem>
                    | Data of string*data
                    | Func of string*rule
let failsafe_codegen(input:fromTypecheckerWithLove) =
  let namespaced:tree<string,fromTypecheckerWithLove> = (tree_empty,input.datas)   ||> Map.fold (fun tree id data-> 
    tree |> tree_add id.Namespace (fun x->{x with datas=x.datas|>Map.add id data}))
  //let namespaced:tree<string,fromTypecheckerWithLove> = (namespaced,input.rules)   ||> Map.fold (fun tree id rule-> tree |> tree_add id.Namespace (fun x->{x with rules=Map.add id rule x.rules}))
  //let namespaced:tree<string,fromTypecheckerWithLove> = (namespaced,input.lambdas) ||> Map.fold (fun tree id lambda-> tree |> tree_add id.Namespace (fun x->{x with lambdas=Map.add id lambda x.lambdas}))
  tree_flatten 
    (fun i->sprintf "namespace %s {" i)
    (fun xs-> Seq.map real_codegen xs|> String.concat "\n")
    (fun _->"}")
    "mc"
    namespaced
  |> String.concat "\n" |> printf "%s"
  //let mix = (Map.empty,Map.toList grouped_data) ||> List.fold (fun s (k,v) -> Map.add (get_namespace k) (Data(k,v)) s)
  //let mix = (mix,Map.toList input.lambdas)      ||> List.fold (fun s (l,r) -> Map.add l.Namespace (Lambda(l,r))     s)
  //let mix = (mix,Map.toList input.rules)        ||> List.fold (fun s (f,r) -> Map.add f.Namespace (Func(f,r))       s)

let test_data:fromTypecheckerWithLove=
  let int_t:Type   = DotNetType(["System"],"Int32");
  let float_t:Type = DotNetType(["System"],"float");
  let star_t:Type  = TypeApplication((McType(["test"],"star")),[int_t;float_t])
  let pipe_t:Type  = TypeApplication((McType(["test"],"pipe")),[int_t;float_t])
  let comma_id:Id  = {Namespace=["test"];Name="comma";Type=star_t}
  let left_id:Id   = {Namespace=["test"];Name="left"; Type=pipe_t}
  let right_id:Id  = {Namespace=["test"];Name="right";Type=pipe_t}
  let comma_data:data = 
    { 
      args=[Named("a"),(DotNetType(["System"],"Int32"));
            Named("b"),(DotNetType(["System"],"Single")); ];
      output=star_t;
    }
  let left_data:data =
    {
      args=[Named("a"),(DotNetType(["System"],"Int32"));]; 
      output=pipe_t;
    }
  let right_data:data =
    {
      args=[Named("a"),(DotNetType(["System"],"Single"));]; 
      output=pipe_t;
    }
        
  let datas = Map.ofSeq <| [comma_id,comma_data; left_id,left_data; right_id,right_data]
  {rules=Map.empty;lambdas=Map.empty;datas=datas}

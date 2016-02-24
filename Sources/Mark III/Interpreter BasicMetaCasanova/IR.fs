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

// data generation /////////////////////////////////////////////////////////////

let group_data (constructors:Map<Id,data>) :Map<Type,List<data>> =
  (Map.empty,constructors) ||> Map.fold (fun r k v ->
    let t = v.output
    match r |> Map.tryFind t with
    | Some ds -> Map.add t (v::ds) r
    | None    -> Map.add t [v]     r )

let gen_data (constructors:Map<Id,data>) =
  let foreach_type ((t:Type),(constructors:List<data>)) :string =
    let field (n:string,t:Type) = sprintf "public %s %s;" (mangle_type t) (CSharpMangle n)
    let elements = constructors |> List.map (fun (d:data) -> 
      sprintf "public class %s:%s{%s};\n" (CSharpMangle d.id.Name) (t|>strip_namespace_type|>mangle_type_suffix) ((List.map field d.args)|>String.concat " ") )
    sprintf "class %s{\n%s};\n" (t|>strip_namespace_type|>mangle_type_suffix) (elements|>String.concat "")
  group_data constructors |> Map.toSeq |> Seq.map foreach_type |> String.concat "\n"

// program and tests ///////////////////////////////////////////////////////////

let failsafe_codegen(input:fromTypecheckerWithLove) =
  printf "%s" (gen_data input.datas)

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
      id=comma_id;
      args=["a",(DotNetType(["System"],"Int32"));
            "b",(DotNetType(["System"],"Single")); ];
      output=star_t;
    }
  let left_data:data =
    {
      id=left_id;
      args=["a",(DotNetType(["System"],"Int32"));]; 
      output=pipe_t;
    }
  let right_data:data =
    {
      id=right_id;
      args=["a",(DotNetType(["System"],"Single"));]; 
      output=pipe_t;
    }
        
  let datas = Map.ofSeq <| [comma_id,comma_data; left_id,left_data; right_id,right_data]
  {rules=Map.empty;lambdas=Map.empty;datas=datas}

module Mangle
open CodegenInterface

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


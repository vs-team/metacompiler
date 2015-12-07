module CodeGen

type VarId  = string
type TypeId = string

type OutputAstType = Int8 | Int16 | Int32 | Int64 | Float | Double | TypeId of TypeId

type OutputAstVar  = VarId      of VarId
                   | Temporary  of int
                   | Argument   of int

type OutputAstExpr = Call of string*seq<OutputAstExpr>
                   | Var       of OutputAstVar
                   | IntLit    of int
                   | StringLit of string
                   | FloatLit  of double

type OutputAstStatement = Assignment  of OutputAstVar*OutputAstExpr
                        | UnionSwitch of OutputAstVar*seq<seq<OutputAstStatement>>
                        | Return      of OutputAstExpr

type OutputAstBody = 
  | Struct       of VarId*seq<OutputAstBody>
  | TaggedUnion  of VarId*seq<OutputAstBody>
  | Data         of OutputAstType*VarId
  | Func         of OutputAstFunc
and OutputAstFunc = {
  Static     : bool
  Name       : string
  ReturnType : OutputAstType
  Args       : seq<OutputAstType*VarId>
  Body       : seq<OutputAstStatement>
}

type OutputAst = Namespace    of VarId*seq<OutputAst>
               | NonNamespace of OutputAstBody

let genericMangle (name:string) :string =
  let normalize (name:string) :string =
    let bytes = System.Text.Encoding.Default.GetBytes(name)
    let str   = System.Text.Encoding.UTF8.GetString(bytes)
    System.String.Intern(str).Normalize(System.Text.NormalizationForm.FormKC)
  let readables = 
    Map.ofArray <| Array.zip "!#$%&'*+,-./\\:;<>=?@^_`|~"B [|
      "bang";"hash";"cash";"perc";"amp";"prime";"star";"plus";"comma";
      "dash"; "dot";"slash";"back";"colon";"semi";"less";"great";
      "equal";"quest";"at";"caret";"under";"tick";"pipe";"tilde"|]
  let mangleChar c =
    if (c>='a'&&c<='z') || (c>='A'&&c<='Z') || (c>='0'&&c<='9')
    then sprintf "%c" c
    else let lookup = readables |> Map.tryFind (System.Convert.ToByte c)
         match lookup with
         | None   -> sprintf "_%02X" (System.Convert.ToByte(c))
         | Some x -> sprintf "_%s" x
  name |> normalize |> String.collect mangleChar

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
  let name = if keywords.Contains(name) then sprintf "@%s" name else name 
  if  name = "System" then "_System" else name

let rec print_ast (ast:OutputAst) :string =
  let print_type (x:OutputAstType) =
    match x with
    | Int8   -> "System.Int8"
    | Int16  -> "System.Int16"
    | Int32  -> "System.Int32"
    | Int64  -> "System.Int64"
    | Float  -> "System.Single"
    | Double -> "System.Float"
    | TypeId s -> CSharpMangle s

  let print_var (x:OutputAstVar) =
    match x with
    | VarId      s -> CSharpMangle s
    | Temporary  d -> sprintf "_tmp%d" d
    | Argument   d -> sprintf "_arg%d" d

  let rec print_expr (x:OutputAstExpr) =
    match x with
    | Var        v -> print_var v
    | Call (name,args) -> sprintf "%s(%s)" (CSharpMangle name) (args |> Seq.map print_expr |> String.concat ",")
    | IntLit     d -> sprintf "%d" d
    | FloatLit   f -> sprintf "%f" f
    | StringLit  s -> sprintf "\"%s\"" s

  let rec print_statement (x:OutputAstStatement) =
    match x with
    | Assignment (l,r) -> sprintf "%s=%s;" (print_var l) (print_expr r)
    | UnionSwitch (var,lst)  -> 
      let body d s = sprintf "case%d:%sbreak;" d (s|> Seq.map print_statement |> String.concat "")
      sprintf "switch(%s._id){%s};" (print_var var) (lst |> Seq.mapi body |> String.concat "")
    | Return x -> sprintf "return %s;" (print_expr x)

  let print_namespace (name:VarId) (body:seq<OutputAst>) =
      let print_body (ast:OutputAst) = sprintf "public %s" (print_ast ast)
      sprintf "namespace %s{%s}" name (body |> Seq.map print_body |> String.concat "")

  let rec print_body (x:OutputAstBody) =
    match x with
    | TaggedUnion (name,body) ->
      let preamble = "[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]"
      let field_offset d = sprintf "[System.Runtime.InteropServices.FieldOffset(%d)]" d 
      let print_union_body (body:OutputAstBody) = sprintf "%s public %s" (field_offset 8) (print_body body)
      sprintf "%sstruct %s{%spublic System.UInt64 _id;%s}" preamble name (field_offset 0) (body |> Seq.map print_union_body |> String.concat "")
    | Struct      (name,body) ->
      let print_struct_body (ast:OutputAstBody) = sprintf "public %s" (print_body ast)
      sprintf "struct %s{%s}" name (body |> Seq.map print_body |> String.concat "")
    | Data (t,name) -> sprintf "public %s %s;" (print_type t) (CSharpMangle name)
    | Func f ->
      let return_type = print_type f.ReturnType
      let name        = CSharpMangle f.Name
      let args = f.Args |> Seq.map (fun (t,n)-> sprintf "%s %s" (print_type t) (CSharpMangle n)) |> String.concat ","
      let body = f.Body |> Seq.map print_statement |> String.concat ""
      let result = sprintf "%s %s(%s){%s}" return_type name args body
      let result = if f.Static then sprintf "static %s" result else result
      sprintf "public %s" result

  match ast with
  | Namespace (name,body) -> print_namespace name body
  | NonNamespace body     -> print_body body

let genCSharp (asts:seq<OutputAst>) :string = 
  let preamble:string = System.IO.File.ReadAllText "primitives.cs"
  asts |> Seq.map print_ast |> String.concat "\n" |> sprintf "%s\n%s" preamble
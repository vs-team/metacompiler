module CodeGen

let csharp_keywords = Set.ofArray [| "abstract" ; "as" ; "base" ; "bool" ; 
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

let mangle (name:string) :string =
  let normalize (name:string) :string =
    let bytes = System.Text.Encoding.Default.GetBytes(name)
    let str   = System.Text.Encoding.UTF8.GetString(bytes)
    System.String.Intern(str).Normalize(System.Text.NormalizationForm.FormKC)
  let mangleChar c =
    if (c>='a'&&c<='z') || (c>='A'&&c<='Z') || (c>='0'&&c<='9')
    then sprintf "%c" c
    else sprintf "_%02X" (System.Convert.ToByte(c))
  let mangledName = name |> normalize |> String.collect mangleChar
  if csharp_keywords.Contains(mangledName) then sprintf "@%s" mangledName else mangledName

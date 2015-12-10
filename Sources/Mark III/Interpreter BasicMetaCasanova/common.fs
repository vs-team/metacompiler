module Common

type Position = { File : string; Line : int; Col : int }
  with 
    member pos.NextLine = { pos with Line = pos.Line + 1; Col = 1 }
    member pos.NextChar = { pos with Col = pos.Col + 1 }
    static member FromPath(path:string) = { File = path; Line = 1; Col = 1 }
    static member Zero = { File = ""; Line = 1; Col = 1 }

type Id = string
type Namespace = string
type Literal = Int of int | String of string | Float32 of float32
type Bracket = Curly | Round | Square | Indent | Lambda | Implicit | Comment

let pos_to_namespace (pos:Position) : Namespace =
  let filepath = pos.File
  let dot = 
    if filepath.Contains ".mc" then 
      filepath.LastIndexOf ".mc"
    else failwith "no .mc found in the position"
  let name = 
    if filepath.Contains "/" then 
      filepath.LastIndexOf "/"
    elif filepath.Contains "\\" then 
      filepath.LastIndexOf "\\"
    else failwith "no path found in the position"
  filepath.Substring ((name+1),(dot-name-1))


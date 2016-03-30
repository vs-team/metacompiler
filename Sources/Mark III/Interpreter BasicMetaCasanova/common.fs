module Common

type Position = { File : string; Line : int; Col : int }
  with 
    member pos.NextLine = { pos with Line = pos.Line + 1; Col = 1 }
    member pos.NextChar = { pos with Col = pos.Col + 1 }
    static member FromPath(path:string) = { File = path; Line = 1; Col = 1 }
    static member Zero = { File = ""; Line = 1; Col = 1 }

type Id = string
type Namespace = List<string>
type Literal = Int of int | String of string | Float32 of float32
type Bracket = Curly | Round | Square | Indent | Lambda | Implicit | Comment

type lit = I64 of System.Int64
         | U64 of System.UInt64
         | I32 of System.Int32
         | U32 of System.Int32
         | F64 of System.Double
         | F32 of System.Single
         | String of System.String
         | Bool of System.Boolean
         | Void
module Common

type Position = { File : string; Line : int; Col : int }
  with 
    member pos.NextLine = { pos with Line = pos.Line + 1; Col = 1 }
    member pos.NextChar = { pos with Col = pos.Col + 1 }
    static member FromPath(path:string) = { File = path; Line = 1; Col = 1 }
    static member Zero = { File = ""; Line = 1; Col = 1 }

type Id = string
type Literal = Int of int | String of string | Float of float

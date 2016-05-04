module Common

type Position = { File : string; Line : int; Col : int }
  with 
    member pos.NextLine = { pos with Line = pos.Line + 1; Col = 1 }
    member pos.NextChar = { pos with Col = pos.Col + 1 }
    static member FromPath(path:string) = { File = path; Line = 1; Col = 1 }
    static member Zero = { File = ""; Line = 1; Col = 1 }
    override this.ToString() =
      sprintf "File : %s, Line : %d, Column: %d" this.File this.Line this.Col

type Bracket = Curly | Round | Square | Lambda | Implicit | Comment

type Predicate = Less | LessEqual | Equal | GreaterEqual | Greater | NotEqual

type genericId<'a>= {Namespace:List<string>;Name:'a;}
type Id       = genericId<string>

type Literal = I64 of System.Int64
             | U64 of System.UInt64
             | I32 of System.Int32
             | U32 of System.Int32
             | F64 of System.Double
             | F32 of System.Single
             | String of System.String
             | Bool of System.Boolean
             | Void

module DataNormalizer2

open Common
open ParserMonad
open DeclParser2

type DataType =  DotNetType      of Id*Namespace
               | McGeneric       of Id*Position
               | McType          of Id*Position
               | TypeApplication of DataType*List<DataType>
               | Arrow           of DataType*DataType

type NormalizedData =
  {
    Name : Id
    CurrentNamespace : Namespace
    Args : List<DataType>
    Return : DataType
    Pos : Position
  }

let rec normalize_decl_type (decl:DeclType):DataType =
  match decl with
  | Id(id,pos)        -> McType (id,pos)
  | IdVar(id,pos)     -> McGeneric (id,pos)
  | DeclParser2.Arrow(l,r) -> 
    Arrow((normalize_decl_type l),(normalize_decl_type r))
  | Application(id,l,r) -> 
    TypeApplication(McType(id,Position.Zero),
      ((normalize_decl_type l)::(normalize_decl_type r)::[]))

let normalize_arg_structure (arg:ArgStructure):List<DataType> =
  match arg with
  | LeftArg(l,r) -> (normalize_decl_type l)::(normalize_decl_type r)::[]
  | RightArgs(ls) -> List.collect (fun x -> [normalize_decl_type x]) ls
  
let normalize_data :Parser<SymbolDeclaration,Id,NormalizedData> =
  prs{
    let! next = step
    let args = normalize_arg_structure next.Args
    let ret = normalize_decl_type next.Return
    return {Name = next.Name ; CurrentNamespace = next.CurrentNamespace ;
            Args = args ; Return = ret ; Pos = next.Pos}
  } 

let normalize_datas :Parser<Id*DeclParseScope,Id,Id*List<NormalizedData>> =
  prs{
    let! id,scp = step
    let! res = UseDifferentSrcAndCtxt (normalize_data |> itterate) scp.DataDecl ""
    return "",res
  } 
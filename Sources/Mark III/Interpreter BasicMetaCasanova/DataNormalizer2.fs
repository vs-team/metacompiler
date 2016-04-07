module DataNormalizer2

open Common
open ParserMonad
open DeclParser2
open CodegenInterface

let rec normalize_decl_type (decl:DeclType):Type =
  match decl with
  | Id(id,pos)        -> McType id
  | IdVar(id,pos)     -> failwith "not implemented yet"
  | IdKind(id,pos)    -> failwith "not implemented yet"
  | TypeArrow(id,pos) -> failwith "not implemented yet"
  | DeclParser2.Arrow(l,r) -> 
    Arrow((normalize_decl_type l),(normalize_decl_type r))
  | DeclParser2.Application(id,l,r) -> 
    TypeApplication(McType(id),
      ((normalize_decl_type l)::(normalize_decl_type r)::[]))

let normalize_arg_structure (arg:ArgStructure):List<Type> =
  match arg with
  | LeftArg(l,r) -> (normalize_decl_type l)::(normalize_decl_type r)::[]
  | RightArgs(ls) -> List.collect (fun x -> [normalize_decl_type x]) ls
  
let normalize_data :Parser<SymbolDeclaration,string,Id*data> =
  prs{
    let! next = step
    let args = normalize_arg_structure next.Args
    let ret = normalize_decl_type next.Return
    return next.Name,{args = args ; outputType = ret}
  } 

let normalize_datas :Parser<string*DeclParseScope,string,string*List<Id*data>> =
  prs{
    let! id,scp = step
    let! res = UseDifferentSrcAndCtxt (normalize_data |> itterate) scp.DataDecl ""
    return id,res
  } 
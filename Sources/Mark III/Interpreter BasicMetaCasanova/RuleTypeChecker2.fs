module RuleTypeChecker2

open Common
open ParserMonad
open RuleNormalizer2
open DataNormalizer2
open DeclParser2
open CodegenInterface

type RuleCheckerCtxt =
  {
    RuleDecl    : List<SymbolDeclaration>
    NormalData  : List<NormalizedData>
    TypeMap     : Map<local_id,Type>
  }

let build_local_id (id:NormalId):local_id =
  match id with
  | VarId(st,pos) -> Named(st)
  | TempId(i,pos) -> Tmp(i)

let convert_literal (literal:Common.Literal):(lit*Type) =
  match literal with
  | Int(i) -> I64(int64(i)),DotNetType{Namespace = ["System"] ; Name = "Int64"}
  | Common.String(st) -> String(st),DotNetType{Namespace = ["System"] ; Name = "String"}
  | Float32(f) -> F32(f),DotNetType{Namespace = ["System"] ; Name = "Single"}

let convert_predicate (con:RuleParser2.Condition):predicate =
  match con with
  | RuleParser2.Less -> Less
  | RuleParser2.LessEqual -> LessEqual
  | RuleParser2.Equal -> Equal
  | RuleParser2.GreaterEqual -> GreaterEqual
  | RuleParser2.Greater-> Greater

let rec convert_type (datatype:DataType) : Type =
  match datatype with
  | DataNormalizer2.DotNetType(id,ns) -> DotNetType{Namespace = ns; Name = id}
  | DataNormalizer2.McType(id,ns) -> DotNetType{Namespace = []; Name = id}
  | DataNormalizer2.TypeApplication(t,ls) ->
    TypeApplication((convert_type t),(List.collect (fun x -> [convert_type x]) ls))
  | DataNormalizer2.Arrow(l,r) -> Arrow((convert_type l),(convert_type r))

let rec bind_id_to_type (args:List<local_id>)(types:List<Type>) =
  match args with
  | x::xs -> 
    match types with
    | y::ys -> (x,y)::bind_id_to_type xs ys
    | [] -> []
  | [] -> 
    match types with
    | y::ys -> failwith "there are more types than arguments"
    | [] -> []

let build_func_closure ((id,ns):GlobalId) (normalid:NormalId) (map:Map<local_id,Type>) 
  (ctxt:List<SymbolDeclaration>):(premisse*Map<local_id,Type>) =
  let rule = List.find (fun (x:SymbolDeclaration) -> x.Name = id && x.CurrentNamespace = ns) ctxt
  (FuncClosure{func = {Namespace = ns; Name = id} ; dest = build_local_id normalid}),map

let add_destructor_map (source:local_id) (destructor:Id) 
  (args:List<local_id>) (map:Map<local_id,Type>) (ctxt:List<NormalizedData>) =
  let destr = List.find (fun (x:NormalizedData) -> x.Name = destructor.Name && x.CurrentNamespace = destructor.Namespace) ctxt
  let map = map.Add(source,(convert_type destr.Return))
  let type_list = List.collect (fun x -> [convert_type x]) destr.Args
  let arg_list = bind_id_to_type args type_list
  List.fold (fun (state:Map<local_id,Type>) (arg,typ) -> state.Add(arg,typ)) map arg_list
  

let match_premis (prem:RuleNormalizer2.Premisse) (map:Map<local_id,Type>) (ctxt:RuleCheckerCtxt)
  :(premisse*Map<local_id,Type>) =
  match prem with 
    | RuleNormalizer2.Alias(_) -> failwith "No aliases alowed during typechecking"
    | RuleNormalizer2.Literal(l,r) -> 
      let lit,typ = convert_literal l
      let map = map.Add((build_local_id r),typ)
      (Literal{value = lit; dest = build_local_id r}),map
    | RuleNormalizer2.Conditional(l,c,r) ->
      (Conditional{left = build_local_id l; predicate = convert_predicate c; right = build_local_id r}),map
    | RuleNormalizer2.Destructor(l,(name,ns),r) -> 
      let dest_id = {Namespace = ns ; Name = name}
      let args = List.collect (fun x -> [build_local_id x]) r
      let map = add_destructor_map (build_local_id l) dest_id args map ctxt.NormalData
      (Destructor{source = build_local_id l; destructor = dest_id; args = args}),map
    //| RuleNormalizer2.McClosure(g,r) -> 
    //| RuleNormalizer2.DotNetClosure(s,r) -> ()
    //| RuleNormalizer2.ConstructorClosure(s,r) -> ()
    //| RuleNormalizer2.Apply(l,a,r) -> ()
    //| RuleNormalizer2.ApplyCall (l,a,r) -> ()

let type_check_premis :Parser<RuleNormalizer2.Premisse,RuleCheckerCtxt,_> =
  prs{
    let! next = step 
    return ()
  }

let type_check_rules :Parser<NormalizedRule,RuleCheckerCtxt,List<Id*rule>> =
  prs{
    let! next = step
    let name = {Namespace = next.CurentNamespace ; Name = next.Name}
    let input = List.collect (fun x -> [build_local_id x]) next.Input
    let output = build_local_id next.Output
    let res = {side_effect = false ; input = input ; output = output ;
               premis = [] ; typemap = Map.empty}
    return name,res
  } |> itterate
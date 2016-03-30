module RuleTypeChecker2

open Common
open ExceptionMonad
open RuleNormalizer2
open DataNormalizer2
open DeclParser2
open CodegenInterface

type RuleCheckerCtxt =
  {
    RuleDecl    : List<SymbolDeclaration>
    DataDecl    : List<SymbolDeclaration>
    TypeMap     : Map<local_id,Type>
    Premise     : List<premisse>

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
  | _ -> failwith "not implemented in datatype"

let rec convert_decl_to_type (decltype:DeclType)(ns:Namespace) : Type =
  match decltype with
  | Id(id,_) -> McType{Namespace = ns ; Name = id}

let convert_arg_structure (argstruct:ArgStructure)(ret:DeclType)(id:local_id)(ns:Namespace) :local_id*Type =
  match argstruct with
  | LeftArg(l,r) -> 
    let left = convert_decl_to_type l ns
    let right = convert_decl_to_type r ns
    id,Arrow(left,right) 
  | RightArgs(ls) -> 
    let ret = convert_decl_to_type ret ns
    id,(List.fold (fun state x -> Arrow((convert_decl_to_type x ns),state)) ret (List.rev ls))

let arg_structure_to_list (argstruct:ArgStructure)(ns:Namespace):List<DeclType> =
  match argstruct with
  | LeftArg(l,r) -> [l;r]
  | RightArgs(ls) -> ls

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

let build_closure ((id,ns):GlobalId) (normalid:NormalId) (symbol_table:Map<local_id,Type>) 
  (ctxt:List<SymbolDeclaration>) (lift_closure:closure<Id>->premisse) :(premisse*Map<local_id,Type>) =
  let rule_option = 
    List.tryFind (fun (x:SymbolDeclaration) -> x.Name = id && x.CurrentNamespace = ns) ctxt
  match rule_option with
  | Some(rule) -> 
    let dest = build_local_id normalid
    let symbol = convert_arg_structure rule.Args rule.Return dest rule.CurrentNamespace
    let symbol_table = symbol_table.Add(symbol)
    (lift_closure {func = {Namespace = ns; Name = id} ; dest = dest}),symbol_table
  | None -> failwithf "can not find symbol in %s" id

let add_destructor_map (source:local_id) (destructor:Id) 
  (args:List<local_id>) (symbol_table:Map<local_id,Type>) (ctxt:List<SymbolDeclaration>) =
  let destr = List.find (fun (x:SymbolDeclaration) -> x.Name = destructor.Name && x.CurrentNamespace = destructor.Namespace) ctxt
  let symbol_table = symbol_table.Add(source,(convert_decl_to_type destr.Return destr.CurrentNamespace))
  let destr_arg_list = arg_structure_to_list destr.Args destr.CurrentNamespace
  let type_list = List.collect (fun x -> [convert_decl_to_type x destr.CurrentNamespace]) destr_arg_list
  let arg_list = bind_id_to_type args type_list
  List.fold (fun (state:Map<local_id,Type>) (arg,typ) -> state.Add(arg,typ)) symbol_table arg_list

let Lift_apply s a d ctxt lift :Exception<RuleCheckerCtxt> =
  exc{
    let source = build_local_id s
    let! type_signature_source = UseOptionMonad (Map.tryFind source ctxt.TypeMap) "Source not found in symboltalbe: apply." 
    match type_signature_source with
    | Arrow(l,r) ->
      let argument = build_local_id a
      let! type_argument = UseOptionMonad (Map.tryFind argument ctxt.TypeMap) "argument not found in symboltalbe: apply." 
      if type_argument = l then
        let destination = build_local_id d
        let res = Application{closure = source; argument = argument; dest = destination}
        let symbol_table = ctxt.TypeMap.Add(destination,r)
        return {ctxt with Premise = res:: ctxt.Premise; TypeMap = symbol_table}
      else return! Exception (sprintf "Type %A of argument %A do not match type %A of %A" argument type_argument l source)
    | _ -> return! Exception "Arrow expected during typechecking apply."
  }

let type_check_premise (prem:RuleNormalizer2.Premisse) 
  (ctxt:RuleCheckerCtxt):Exception<RuleCheckerCtxt> =
  exc{
    match prem with 
    | RuleNormalizer2.Alias(_) -> return! Exception "alias is not alowed during typechecking."
    | RuleNormalizer2.Literal(l,r) -> 
      let lit,typ = convert_literal l
      let symbol_table = ctxt.TypeMap.Add((build_local_id r),typ)
      let res = (Literal{value = lit; dest = build_local_id r})
      return {ctxt with Premise = res::ctxt.Premise; TypeMap = symbol_table}
    | RuleNormalizer2.Conditional(l,c,r) ->
      let left = build_local_id l
      let right = build_local_id r
      let! type_left = UseOptionMonad (Map.tryFind left ctxt.TypeMap) "left conditional error undifined argument." 
      let! type_right = UseOptionMonad (Map.tryFind right ctxt.TypeMap) "right conditional error undifined argument." 
      if type_left = type_right then
        let res = (Conditional{left = left; predicate = convert_predicate c; right = right})
        return {ctxt with Premise = res::ctxt.Premise}
      else return! Exception (sprintf "conditional type mismatch between %A and %A ." type_left type_right)
    | RuleNormalizer2.Destructor(l,(name,ns),r) -> 
      let dest_id = {Namespace = ns ; Name = name}
      let args = List.collect (fun x -> [build_local_id x]) r
      let symbol_table = add_destructor_map (build_local_id l) dest_id args ctxt.TypeMap ctxt.DataDecl
      let res = (Destructor{source = build_local_id l; destructor = dest_id; args = args})
      return {ctxt with Premise = res::ctxt.Premise; TypeMap = symbol_table}
    | RuleNormalizer2.McClosure(g,r) -> 
      let res,symbol_table = build_closure g r ctxt.TypeMap ctxt.RuleDecl (fun x -> FuncClosure x)
      return {ctxt with Premise = res::ctxt.Premise; TypeMap = symbol_table}
    | RuleNormalizer2.ConstructorClosure(s,r) ->
      let res,symbol_table = build_closure s r ctxt.TypeMap ctxt.DataDecl (fun x -> ConstructorClosure x)
      return {ctxt with Premise = res::ctxt.Premise; TypeMap = symbol_table}
    | RuleNormalizer2.Apply(s,a,d) -> 
      return! Lift_apply s a d ctxt (fun x -> Application x)
    | RuleNormalizer2.ApplyCall (s,a,d) -> 
      return! Lift_apply s a d ctxt (fun x -> ApplicationCall x)
    | _ -> return! Exception "not implemented yet."
    //| RuleNormalizer2.DotNetClosure(s,r) -> 
  
  }

let type_check_rules (rules:List<NormalizedRule>) (ctxt:RuleCheckerCtxt)
  :Exception<List<Id*rule>> =
  exc{
    let res = rules |> List.map (fun next ->
      let name = {Namespace = next.CurentNamespace ; Name = next.Name}
      let input = List.map (fun x -> build_local_id x) next.Input
      let output = build_local_id next.Output
      let premise = []

      let res = {side_effect = false ; input = input ; output = output ;
                 premis = [] ; typemap = Map.empty}
      name,res
    ) 
    return res
  }
module RuleTypeChecker2

open Common
open ExceptionMonad
open RuleNormalizer2
open DataNormalizer2
open ParserTypes
open DeclParser2
open CodegenInterface

type RuleCheckerCtxt =
  {
    RuleDecl    : List<SymbolDeclaration>
    DataDecl    : List<SymbolDeclaration>
    TypeMap     : Map<local_id,Type>
    Premise     : List<premisse>

  } with
    static member Zero =
      {
        RuleDecl  = []
        DataDecl  = []
        TypeMap   = Map.empty
        Premise   = []
      }

let build_local_id (id:NormalId):local_id =
  match id with
  | VarId(st,pos) -> Named(st.Name)
  | TempId(i,pos) -> Tmp(i)

let convert_literal (literal:Common.Literal):(Literal*Type) =
  match literal with
  | I64(i) -> I64(int64(i)),DotNetType{Namespace = ["System"] ; Name = "Int64"}
  | Common.String(st) -> String(st),DotNetType{Namespace = ["System"] ; Name = "String"}
  | F32(f) -> F32(f),DotNetType{Namespace = ["System"] ; Name = "Single"}
  | Void -> Void,DotNetType{Namespace = ["System"] ; Name = "void"}
  | _ -> failwith "Literal not implemented yet."

let convert_predicate (con:ParserTypes.Condition):predicate =
  match con with
  | ParserTypes.Less -> Less
  | ParserTypes.LessEqual -> LessEqual
  | ParserTypes.Equal -> Equal
  | ParserTypes.GreaterEqual -> GreaterEqual
  | ParserTypes.Greater-> Greater

let rec convert_decl_to_type (decltype:DeclType) : Type =
  match decltype with
  | ParserTypes.Id(id,_) -> McType id
  | ParserTypes.IdVar(id,_) -> McType id
  | ParserTypes.Application(id,ls) ->
    let to_type = List.map (fun x -> convert_decl_to_type x) ls
    TypeApplication((McType id), to_type)
  | ParserTypes.Arrow(l,r) -> 
    Arrow((convert_decl_to_type l),(convert_decl_to_type r))
  | _ -> failwith "kind id should not be present during typechecking"

let convert_arg_structure (argstruct:ArgStructure)(ret:DeclType)(id:local_id) :local_id*Type =
  match argstruct with
  | LeftArg(l,r) -> 
    let left = convert_decl_to_type l
    let right = convert_decl_to_type r
    let ret = convert_decl_to_type ret
    id,Arrow(left,Arrow(right,ret)) 
  | RightArgs(ls) -> 
    let ret = convert_decl_to_type ret
    id,(List.fold (fun state x -> Arrow((convert_decl_to_type x),state)) ret (List.rev ls))

let arg_structure_to_list (argstruct:ArgStructure):List<DeclType> =
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

let build_closure (id:Id) (normalid:NormalId) (symbol_table:Map<local_id,Type>) 
  (ctxt:List<SymbolDeclaration>) (lift_closure:closure<Id>->premisse) :(premisse*Map<local_id,Type>) =
  let rule_option = 
    List.tryFind (fun (x:SymbolDeclaration) -> x.Name = id) ctxt
  match rule_option with
  | Some(rule) -> 
    let dest = build_local_id normalid
    let symbol = convert_arg_structure rule.Args rule.Return dest 
    let symbol_table = symbol_table.Add(symbol)
    (lift_closure {func = id ; dest = dest}),symbol_table
  | None -> failwithf "can not find symbol in %A" id

let add_destructor_map (source:local_id) (destructor:Id) 
  (args:List<local_id>) (symbol_table:Map<local_id,Type>) (ctxt:List<SymbolDeclaration>) =
  let destr = List.find (fun (x:SymbolDeclaration) -> x.Name = destructor) ctxt
  let symbol_table = symbol_table.Add(source,(convert_decl_to_type destr.Return))
  let destr_arg_list = arg_structure_to_list destr.Args 
  let type_list = List.collect (fun x -> [convert_decl_to_type x]) destr_arg_list
  let arg_list = bind_id_to_type args type_list
  List.fold (fun (state:Map<local_id,Type>) (arg,typ) -> state.Add(arg,typ)) symbol_table arg_list

let Lift_apply s a d ctxt lift :Exception<RuleCheckerCtxt> =
  exc{
    let source = build_local_id s
    let! type_signature_source = 
      UseOptionMonad (Map.tryFind source ctxt.TypeMap) "Source not found in symbol table: apply." 
    match type_signature_source with
    | Arrow(l,r) ->
      let argument = build_local_id a
      let err = sprintf "argument not found in symboltable: %A." argument 
      let! type_argument = UseOptionMonad (Map.tryFind argument ctxt.TypeMap) err
      if type_argument = l then
        let destination = build_local_id d
        let res = lift source argument destination
        let symbol_table = ctxt.TypeMap.Add(destination,r)
        return {ctxt with Premise = res:: ctxt.Premise; TypeMap = symbol_table}
      else return! Exception (sprintf "Type %A of argument %A do not match type %A of %A" argument type_argument l source)
    | _ -> return! Exception "Arrow expected during typechecking apply."
  }

let type_check_premise (ctxt:Exception<RuleCheckerCtxt>)
  (prem:RuleNormalizer2.Premisse) :Exception<RuleCheckerCtxt> =
  exc{
    let! ctxt = ctxt
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
    | RuleNormalizer2.Destructor(l,name,r) -> 
      let dest_id = name
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
      return! Lift_apply s a d ctxt (fun c a d ->
         Application {closure = c; argument = a; dest = d})
    | RuleNormalizer2.ApplyCall (s,a,d) -> 
      return! Lift_apply s a d ctxt (fun c a d ->
         ApplicationCall {closure = c; argument = a; dest = d; side_effect = false})
    | _ -> return! Exception "not implemented yet."
    //| RuleNormalizer2.DotNetClosure(s,r) -> 
  
  }

let try_get_argument_type (id:Id) (input_args:List<local_id>) 
  (ctxt:RuleCheckerCtxt) :Exception<RuleCheckerCtxt> =
  exc{
    let ruleopt = ctxt.RuleDecl |> List.tryFind (fun (x:SymbolDeclaration) -> 
      x.Name = id)
    match ruleopt with
    | Some(rule) ->
      let arg_types = arg_structure_to_list rule.Args
      let arg_types = List.map (fun x -> convert_decl_to_type x) arg_types 
      let res = List.zip input_args arg_types
      let symbol_table = res |> List.fold (fun (state:Map<local_id,Type>) (id,typ) -> 
        state.Add (id,typ)) ctxt.TypeMap
      return {ctxt with TypeMap = symbol_table}
    | None -> return! Exception (sprintf "could not find rule: %A." id)
  }

let type_check_rule (ctxt:RuleCheckerCtxt) (next:NormalizedRule)
  :Exception<Id*rule> =
  exc{
    let name = next.Name
    let input = List.map (fun x -> build_local_id x) next.Input
    let output = build_local_id next.Output
    let! ctxt = try_get_argument_type name input ctxt
    let! premise = fold type_check_premise (Result ctxt) next.Premis
    
    let res = {side_effect = false ; input = input ; output = output ;
               premis = List.rev premise.Premise ; typemap = premise.TypeMap}
    return (name,res)
    
  }

let type_check_rules (rules:List<NormalizedRule>) (ctxt:RuleCheckerCtxt)
  :Exception<List<Id*rule>> =
  exc{
    let! res = map (type_check_rule ctxt) rules
    return res
  }
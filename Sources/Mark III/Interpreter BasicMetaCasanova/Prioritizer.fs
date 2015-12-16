module Prioritizer
open Common
open ParserMonad
open ScopeBuilder

type Priority = int*Associativity

type Type = Star 
          | Module 
          | ModuleDef  of TypedScope
          | TypeId     of Id
          | TypeIdVar  of Id
          | TypeIdList of List<Type>
          | BigArrow   of Type*Type
          | SmallArrow of Type*Type
          | Bar        of Type*Type
          | Tuple      of Type*Type
and TypeSignature = Nop
                  | Name of Priority*TypeSignature*TypeSignature*TypeSignature
                  | Sig  of Type*TypeSignature
and TableSymbols = DataSym  of Id*Namespace*TypeSignature
                 | FuncSym  of Id*Namespace*TypeSignature
                 | TypeSym  of Id*Namespace*TypeSignature
                 | ArrowSym of Id*Namespace*TypeSignature
                 | AliasSym of Id*Namespace*TypeSignature
and TypedScope = 
  {
    ImportDecls           : List<Id>
    InheritDecls          : List<Id>
    SymbolTable           : List<TableSymbols>
    FuncDecls             : Map<Id,TypeSignature>
    TypeFuncDecls         : Map<Id,TypeSignature>
    ArrowDecls            : Map<Id,TypeSignature>
    AliasDecls            : Map<Id,TypeSignature>
    DataDecls             : Map<Id,TypeSignature>
  }
  with 
    static member Zero =
      {
        ImportDecls     = []
        InheritDecls    = []
        SymbolTable     = []
        FuncDecls       = Map.empty
        TypeFuncDecls   = Map.empty
        ArrowDecls      = Map.empty
        AliasDecls      = Map.empty
        DataDecls       = Map.empty
      }
let filter_typescopes typscp st = List.filter (fun (s,t) -> if st = s then false else true) typscp
let find_typescope typscp st = 
  if List.exists (fun (s,t) -> if st = s then true else false) typscp then
    let _,res = List.find (fun (s,t) -> if st = s then true else false) typscp
    Done(res,[],())
  else Error TypeError 
let rec filter_lists (l1:List<'a>) (l2:List<'a>) =
  match l1 with
  | x::xs -> filter_lists xs (List.filter (fun st -> if x = st then false else true) l2)
  | [] -> l2

let end_of_list =
  fun (li,ctxt) ->
    match li with 
    | x::xs -> Error TypeError
    | [] -> Done((),li,ctxt)

let rec itterate (p:Parser<_,_,'res>) :Parser<_,_,List<'res>> =
  prs{
    let! x = p
    let! xs = itterate p
    return x::xs
  } .|| prs{
    do! end_of_list
    return []
  }

let move_ctxt_to_scp p  =
  fun (scp,ctxt) ->
    match p (ctxt,ctxt) with
    | Done(res,_,ctxt') -> Done(res,scp,ctxt')
    | Error (p) -> Error (p)

let build_empty_typescope_list : Parser<string*ScopeBuilder.Scope,List<string*TypedScope>,_> =
  fun (scp,ctxt) ->
    match scp with
    | (s,x)::xs -> Done((),xs,(s,TypedScope.Zero)::ctxt)
    | [] -> Error TypeError 

let use_new_scope p =
  fun (scp,ctxt) ->
    match p (scp,ctxt) with
    | Done(res,_,ctxt') -> Done(res,scp,ctxt')
    | Error (p) -> Error (p)

let rec check_size_carry (carry:List<Type>) =
  match (List.rev carry) with
  | [TypeId("*")] -> Star
  | [x] -> x
  | x::TypeId("*")::xs -> Tuple (x,(check_size_carry xs))
  | x::xs -> TypeIdList(x::xs)
  | [] -> TypeIdList([])

let rec multiple_scopetype_to_type (typ:ScopeBuilder.Type) (carry:List<Type>) : Type =
  match typ with
  | x :: xs ->
    match x with
    | Id (s,p) -> 
      if (s.StartsWith "'") then multiple_scopetype_to_type xs ((TypeIdVar (s))::carry)
      elif (s.StartsWith "|") then Bar ((check_size_carry carry),(multiple_scopetype_to_type xs []))
      else multiple_scopetype_to_type xs ((TypeId (s))::carry)
    | Arrow (SingleArrow,p) -> SmallArrow ((check_size_carry carry),(multiple_scopetype_to_type xs []))
    | Arrow (DoubleArrow,p) -> BigArrow ((check_size_carry carry),(multiple_scopetype_to_type xs []))
    | Application (b,l) ->  multiple_scopetype_to_type xs ((scopetype_to_type l)::carry)
    | _ -> (check_size_carry carry)
  | [] -> (check_size_carry carry)

and scopetype_to_type (typ:ScopeBuilder.Type) : Type =
  match typ with
  | [x] ->
    match x with 
    | Id (s,p) -> 
      if (s.Contains "'") then TypeIdVar (s)
      elif (s.Equals "*") then Star
      else TypeId (s)
    | Application (b,l) -> scopetype_to_type l
    | _ -> Star
  | x::xs -> multiple_scopetype_to_type typ []
  | [] -> Star

let rec type_to_typesig (typ:List<ScopeBuilder.Type>) : TypeSignature =
  match typ with
  | x::xs -> Sig(scopetype_to_type x,(type_to_typesig xs))
  | [] -> Nop

let args_to_typesig (sym:SymbolDeclaration) : TypeSignature = 
  let l = type_to_typesig sym.LeftArgs
  let r = type_to_typesig sym.RightArgs
  let ret = type_to_typesig [sym.Return]
  Name ((sym.Priority,sym.Associativity),l,r,ret)

let symdec_to_typedscope : Parser<SymbolDeclaration,TypedScope,Id*TypeSignature> =
  fun (sym,ctxt) ->
    match sym with 
    | x::xs -> Done((x.Name,args_to_typesig x),xs,ctxt)
    | [] -> Error TypeError

let import_type : Parser<ScopeBuilder.Scope,TypedScope,_> =
  fun (scp,ctxt) ->
    let ctxt' = {ctxt with ImportDecls = scp.Head.ImportDeclaration}
    Done((),scp,ctxt') 

let lift_type p i o : Parser<ScopeBuilder.Scope,_,_> =
  fun (scp,ctxt) ->
    match (p |> itterate) (i scp ctxt) with
    | Done(x,y,z) -> Done((),scp,(o ctxt x)) 
    | Error (p) -> Error (p)   

let data_type : Parser<ScopeBuilder.Scope,TypedScope,_> =
  lift_type symdec_to_typedscope (fun scp ctxt -> (scp.Head.DataDeclarations,ctxt))
    (fun ctxt x -> {ctxt with DataDecls = (Map.ofList x)})
let func_type : Parser<ScopeBuilder.Scope,TypedScope,_> =
  lift_type symdec_to_typedscope (fun scp ctxt -> (scp.Head.FunctionDeclarations,ctxt))
    (fun ctxt x -> {ctxt with FuncDecls = (Map.ofList x)})
let typefunc_type : Parser<ScopeBuilder.Scope,TypedScope,_> =
  lift_type symdec_to_typedscope (fun scp ctxt -> (scp.Head.TypeFunctionDeclarations,ctxt))
    (fun ctxt x -> {ctxt with TypeFuncDecls = (Map.ofList x)})
let arrowfunc_type : Parser<ScopeBuilder.Scope,TypedScope,_> =
  lift_type symdec_to_typedscope (fun scp ctxt -> (scp.Head.ArrowFunctionDeclarations,ctxt))
    (fun ctxt x -> {ctxt with ArrowDecls = (Map.ofList x)})
let typealias_type : Parser<ScopeBuilder.Scope,TypedScope,_> =
  lift_type symdec_to_typedscope (fun scp ctxt -> (scp.Head.TypeAliasDeclarations,ctxt))
    (fun ctxt x -> {ctxt with AliasDecls = (Map.ofList x)})


let procces_scopes (p) 
    : Parser<string*ScopeBuilder.Scope,List<string*_>,string*_> =
  fun (scp,ctxt) ->
    match scp with
    |(st,sc)::xs -> 
      match (find_typescope ctxt st) with
      | Done(found,_,_) ->
        match p ([sc],found) with
        | Done(typscp,_,_) ->
          let res = ((st,typscp)::(filter_typescopes ctxt st)) 
          Done((st,typscp),xs,res)
        | Error (p) -> Error (p)
      | Error (p) -> Error (p)
    | [] -> Error TypeError 

let procces_table (p:Parser<string*TypedScope,List<string*TypedScope>,_>) 
    : Parser<string*TypedScope,List<string*TypedScope>,_> =
  fun (typscp,ctxt) ->
    match typscp with
    | x::xs ->
      match p([x],ctxt) with
      | Done (res,_,ctxt') -> Done(res,xs,ctxt')
      | Error (p) -> Error (p)
    | [] -> Error TypeError 

let get_import_list :Parser<string*TypedScope,List<string*TypedScope>,List<Id>>=
  let rec get_imports find (ctxt:List<string*TypedScope>) :List<Id> =
    match find with
    |x::xs ->
      match find_typescope ctxt x with
      | Done(res,_,_) -> 
        let l1 = (get_imports res.ImportDecls ctxt) 
        x :: l1 @ (filter_lists l1 (get_imports xs ctxt))
      | Error _ -> []
    | [] -> []
  fun (typscp,ctxt) ->
    match typscp with
    | [(st,_)] -> Done ((get_imports [st] ctxt),typscp,ctxt)
    | _ -> Error TypeError
    
let lift_build_table (idlist:List<Id>) sym decls =
  let rec add_namespace_to_list symlist ns =
    match symlist with
    | (st,sign)::xs -> (st,ns,sign) :: add_namespace_to_list xs ns
    | [] -> []
  let rec decls_to_table (typlist:List<Id*Namespace*TypeSignature>) =
    match typlist with
    | x::xs -> (sym x) :: decls_to_table xs
    | [] -> []
  let rec build_table idlist ctxt =
    match (idlist:List<Id>) with
    | x::xs -> 
      match find_typescope ctxt x with
      | Done(res,_,_) -> (add_namespace_to_list (Map.toList (decls res)) x) @ build_table xs ctxt
      | Error _ -> []
    | [] -> []
  fun (typscp,(ctxt:List<string*TypedScope>)) -> 
    Done((decls_to_table (build_table idlist ctxt)),typscp,ctxt)

let build_data_table  (id:List<Id>) = lift_build_table id (fun x -> DataSym(x))  (fun x -> x.DataDecls)
let build_func_table  (id:List<Id>) = lift_build_table id (fun x -> FuncSym(x))  (fun x -> x.FuncDecls)
let build_type_table  (id:List<Id>) = lift_build_table id (fun x -> TypeSym(x))  (fun x -> x.TypeFuncDecls)
let build_arrow_table (id:List<Id>) = lift_build_table id (fun x -> ArrowSym(x)) (fun x -> x.ArrowDecls)
let build_alias_table (id:List<Id>) = lift_build_table id (fun x -> AliasSym(x)) (fun x -> x.AliasDecls)

let set_table_in_ctxt (table:List<TableSymbols>) : Parser<string*TypedScope,List<string*TypedScope>,_>=
  fun (typscp,ctxt) ->
    match typscp with
    | [(st,_)] -> 
      match find_typescope ctxt st with
      | Done (res,_,_) -> 
        let ctxt' = (st,{res with SymbolTable = table}) :: (filter_typescopes ctxt st)
        Done((),[],ctxt')
      | Error e -> Error e
    | _ -> Error TypeError
    
let build_symbol_table : Parser<string*TypedScope,List<string*TypedScope>,_>=
  prs{
    let! importlist = get_import_list
    let! data     = build_data_table  importlist
    let! func     = build_func_table  importlist
    let! typefunc = build_type_table  importlist
    let! arrow    = build_arrow_table importlist
    let! alias    = build_alias_table importlist
    do! set_table_in_ctxt (data@typefunc@arrow@func@alias)
    printfn "%A" importlist
    return ()
  }

let declcheck : Parser<ScopeBuilder.Scope,TypedScope,TypedScope> =
  prs {
    do! import_type
    do! data_type
    do! func_type
    do! typefunc_type
    do! typealias_type
    do! arrowfunc_type
    return! getContext
  }

let ctxtret  = 
  fun (scp,ctxt) -> Done(ctxt,scp,ctxt)

let decls_check() : Parser<string*ScopeBuilder.Scope,List<string*TypedScope>,List<string*TypedScope>> =
  prs{
    let! dump = (build_empty_typescope_list |> itterate) |> use_new_scope
    let! res = (procces_scopes declcheck) |> itterate |> use_new_scope
    let! res = (procces_table build_symbol_table) |> itterate |> move_ctxt_to_scp |> use_new_scope
    let! res = ctxtret
    return res
  }


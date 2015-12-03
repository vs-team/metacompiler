module Prioritizer
open Common
open ParserMonad
open ScopeBuilder

type TreeExpr = Abs of TreeExpr*Type
              | App of TreeExpr*TreeExpr*Type
              | Var of Id*Type
              | Lit of Literal*Type
and Rule = {
  Input    :TypeSignature
  Output   :TypeSignature
  Premises :List<Premise>
}and Premise = Assignment  of TreeExpr*TreeExpr
             | Conditional of TreeExpr
and Type = Star       // type
         | Signature  
         | ModuleDec  of TypedScope
         | ModuleDef  of TypedScope
         | TypeId     of Id
         | TypeIdVar  of Id
         | TypeIdList of List<Type>
         | BigArrow   of Type*Type
         | SmallArrow of Type*Type
         | Bar        of Type*Type
         | Union      of Type*TypeConstructors
and TypeConstructors = Map<Id,Type>
and TypeSignature = Nop
                  | Name of TypeSignature*TypeSignature*TypeSignature
                  | Sig  of Type*TypeSignature
and TypedScope = 
  {
    ImportDecls           : List<Id>
    InheritDecls          : List<Id>
    FuncDecls             : Map<Id,TypeSignature>
    TypeFuncDecls         : Map<Id,TypeSignature>
    ArrowDecls            : Map<Id,TypeSignature>
    DataDecls             : Map<Id,TypeSignature>
    TypeFuncRules         : Map<Id,List<Rule>>
    FuncRules             : Map<Id,List<Rule>>
  }
  with 
    static member Zero =
      {
        ImportDecls     = []
        InheritDecls    = []
        FuncDecls       = Map.empty
        TypeFuncDecls   = Map.empty
        ArrowDecls      = Map.empty
        DataDecls       = Map.empty
        TypeFuncRules   = Map.empty
        FuncRules       = Map.empty
      }
let filter_typescopes typscp st = List.filter (fun (s,t) -> if st = s then false else true) typscp
let find_typescope typscp st = 
  let _,res = List.find (fun (s,t) -> if st = s then true else false) typscp
  res

let rec multiple_scopetype_to_type (typ:ScopeBuilder.Type) (carry:List<Type>) : Type =
  match typ with
  | x :: xs ->
    match x with
    | Id (s,p) -> 
      if (s.Contains "'") then multiple_scopetype_to_type xs ((TypeIdVar (s))::carry)
      else multiple_scopetype_to_type xs ((TypeId (s))::carry)
    | Arrow (SingleArrow,p) -> SmallArrow (TypeIdList(List.rev carry),(multiple_scopetype_to_type xs []))
    | Arrow (DoubleArrow,p) -> BigArrow (TypeIdList(List.rev carry),(multiple_scopetype_to_type xs []))
    | _ -> TypeIdList (List.rev carry)
  | [] -> TypeIdList (List.rev carry)

let rec scopetype_to_type (typ:ScopeBuilder.Type) : Type =
  match typ with
  | [x] ->
    match x with 
    | Id (s,p) -> 
      if (s.Contains "'") then TypeIdVar (s)
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
  Name (l,r,ret)

let symdec_to_typedscope : Parser<SymbolDeclaration,TypedScope,Id*TypeSignature> =
  fun (sym,ctxt) ->
    match sym with 
    | x::xs -> Done((x.Name,args_to_typesig x),xs,ctxt)
    | [] -> Error ((TypeError[",symdec"]),Position.Zero)

let rule_to_typerule : Parser<ScopeBuilder.Rule,TypedScope,Id*List<Rule>> =
  fun (rule,ctxt) ->
    match rule with
    | x::xs -> Done(("",[{Input    = type_to_typesig [x.Input]; 
                          Output   = type_to_typesig [x.Output]; 
                          Premises = []}]),xs,ctxt)
    | [] -> Error ((TypeError[",rule"]),Position.Zero)

let import_type : Parser<ScopeBuilder.Scope,TypedScope,_> =
  fun (scp,ctxt) ->
    let ctxt' = {ctxt with ImportDecls = scp.Head.ImportDeclaration}
    Done((),scp,ctxt') 
   
let data_type : Parser<ScopeBuilder.Scope,TypedScope,_> =
  fun (scp,ctxt) ->
    match (symdec_to_typedscope |> repeat) (scp.Head.DataDeclarations,ctxt) with
    | Done(x,y,z) ->
      let ctxt' = {ctxt with DataDecls = (Map.ofList x)}
      Done((),scp,ctxt') 
    | Error (s,p) -> Error (s,p)   

let func_type : Parser<ScopeBuilder.Scope,TypedScope,_> =
  fun (scp,ctxt) ->
    match (symdec_to_typedscope |> repeat) (scp.Head.FunctionDeclarations,ctxt) with
    | Done(x,y,z) ->
      let ctxt' = {ctxt with FuncDecls = (Map.ofList x)}
      Done((),scp,ctxt') 
    | Error (s,p) -> Error (s,p)   

let typefunc_type : Parser<ScopeBuilder.Scope,TypedScope,_> =
  fun (scp,ctxt) ->
    match (symdec_to_typedscope |> repeat) (scp.Head.TypeFunctionDeclarations,ctxt) with
    | Done(x,y,z) ->
      let ctxt' = {ctxt with TypeFuncDecls = (Map.ofList x)}
      Done((),scp,ctxt') 
    | Error (s,p) -> Error (s,p)   

let arrowfunc_type : Parser<ScopeBuilder.Scope,TypedScope,_> =
  fun (scp,ctxt) ->
    match (symdec_to_typedscope |> repeat) (scp.Head.ArrowFunctionDeclarations,ctxt) with
    | Done(x,y,z) ->
      let ctxt' = {ctxt with ArrowDecls = (Map.ofList x)}
      Done((),scp,ctxt') 
    | Error (s,p) -> Error (s,p)   

let rule_type : Parser<ScopeBuilder.Scope,TypedScope,_> =
  fun (scp,ctxt) ->
    match (rule_to_typerule |> repeat) (scp.Head.Rules,ctxt) with
    | Done(x,y,z) ->
      let ctxt' = {ctxt with FuncRules = (Map.ofList x)}
      Done((),scp,ctxt') 
    | Error (s,p) -> Error (s,p)   

let typerule_type : Parser<ScopeBuilder.Scope,TypedScope,_> =
  fun (scp,ctxt) ->
    match (rule_to_typerule |> repeat) (scp.Head.TypeFunctionRules,ctxt) with
    | Done(x,y,z) ->
      let ctxt' = {ctxt with TypeFuncRules = (Map.ofList x)}
      Done((),scp,ctxt') 
    | Error (s,p) -> Error (s,p)   

let procces_scopes (p:Parser<ScopeBuilder.Scope,TypedScope,TypedScope>) 
    : Parser<string*ScopeBuilder.Scope,List<string*TypedScope>,string*TypedScope> =
  fun (scp,ctxt) ->
    match scp with
    |(st,sc)::xs -> 
      match p ([sc],(find_typescope ctxt st)) with
      | Done(typscp,_,_) ->
        let res = ((st,typscp)::(filter_typescopes ctxt st)) 
        Done((st,typscp),xs,res)
      | Error (s,p) -> Error (s,p)
    | [] -> Error (TypeError ["nothing to procces"],Position.Zero)

let build_empty_typescope_list : Parser<string*ScopeBuilder.Scope,List<string*TypedScope>,_> =
  fun (scp,ctxt) ->
    match scp with
    | (s,x)::xs -> Done((),xs,(s,TypedScope.Zero)::ctxt)
    | [] -> Error (TypeError ["no scopes left"],Position.Zero)

let use_new_scope p =
  fun (scp,ctxt) ->
    match p (scp,ctxt) with
    | Done(res,_,ctxt') -> Done(res,scp,ctxt')
    | Error (s,p) -> Error (s,p)

let move_ctxt_to_scp p : Parser<string*ScopeBuilder.Scope,List<string*TypedScope>,_> =
  fun (scp,ctxt) ->
    match p (ctxt,ctxt) with
    | Done(res,_,ctxt') -> Done(res,scp,ctxt')
    | Error (s,p) -> Error (s,p)

let declcheck : Parser<ScopeBuilder.Scope,TypedScope,TypedScope> =
  prs {
    do! import_type
    do! data_type
    do! func_type
    do! typefunc_type
    do! arrowfunc_type
    return! getContext
  }

let typerulecheck : Parser<ScopeBuilder.Scope,TypedScope,TypedScope> =
  prs {
    do! rule_type
    do! typerule_type
    return! getContext
  }

let decls_check() : Parser<string*ScopeBuilder.Scope,List<string*TypedScope>,List<string*TypedScope>> =
  prs{
    let! dump = use_new_scope (build_empty_typescope_list |> repeat) 
    let! res = (procces_scopes declcheck) |> repeat |> use_new_scope
    let! res = (procces_scopes typerulecheck) |> repeat |> use_new_scope
    return res
  }


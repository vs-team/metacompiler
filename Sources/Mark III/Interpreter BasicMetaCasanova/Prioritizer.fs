module Prioritizer
open Common
open ParserMonad
open ScopeBuilder

type Priority = int*Associativity

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
and TableSymbols = DataSym  of Id*TypeSignature
                 | FuncSym  of Id*TypeSignature
                 | TypeSym  of Id*TypeSignature
                 | ArrowSym of Id*TypeSignature
and TypedScope = 
  {
    ImportDecls           : List<Id>
    InheritDecls          : List<Id>
    SymbolTable           : List<TableSymbols>
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
        SymbolTable     = []
        FuncDecls       = Map.empty
        TypeFuncDecls   = Map.empty
        ArrowDecls      = Map.empty
        DataDecls       = Map.empty
        TypeFuncRules   = Map.empty
        FuncRules       = Map.empty
      }
let filter_typescopes typscp st = List.filter (fun (s,t) -> if st = s then false else true) typscp
let find_typescope typscp st = 
  if List.exists (fun (s,t) -> if st = s then true else false) typscp then
    let _,res = List.find (fun (s,t) -> if st = s then true else false) typscp
    Done(res,[],())
  else Error (TypeError["typescope does not exist"],Position.Zero)

let move_ctxt_to_scp p  =
  fun (scp,ctxt) ->
    match p (ctxt,ctxt) with
    | Done(res,_,ctxt') -> Done(res,scp,ctxt')
    | Error (s,p) -> Error (s,p)

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


let make_rule =
  fun (rule:List<ScopeBuilder.Rule>,ctxt) ->
    match rule with
    | r::xs ->
      Done (("",[{Input    = type_to_typesig [r.Input]
                  Output   = type_to_typesig [r.Output]
                  Premises = []}]),xs,ctxt)
    |[] -> Error (TypeError[",rule"],Position.Zero)
  
let rule_to_typedrule : Parser<ScopeBuilder.Rule,TypedScope,Id*List<Rule>> =
  prs{
    let! r = make_rule
    return r
  }

let import_type : Parser<ScopeBuilder.Scope,TypedScope,_> =
  fun (scp,ctxt) ->
    let ctxt' = {ctxt with ImportDecls = scp.Head.ImportDeclaration}
    Done((),scp,ctxt') 

let lift_type p i o : Parser<ScopeBuilder.Scope,TypedScope,_> =
  fun (scp,ctxt) ->
    match (p |> repeat) (i scp ctxt) with
    | Done(x,y,z) -> Done((),scp,(o ctxt x)) 
    | Error (s,p) -> Error (s,p)   

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

let rule_type : Parser<ScopeBuilder.Scope,TypedScope,_> =
  lift_type rule_to_typedrule (fun scp ctxt -> (scp.Head.Rules,ctxt))
    (fun ctxt x -> {ctxt with FuncRules = (Map.ofList x)})
let typerule_type : Parser<ScopeBuilder.Scope,TypedScope,_> =
  lift_type rule_to_typedrule (fun scp ctxt -> (scp.Head.TypeFunctionRules,ctxt))
    (fun ctxt x -> {ctxt with TypeFuncRules = (Map.ofList x)})

let procces_scopes (p:Parser<ScopeBuilder.Scope,TypedScope,TypedScope>) 
    : Parser<string*ScopeBuilder.Scope,List<string*TypedScope>,string*TypedScope> =
  fun (scp,ctxt) ->
    match scp with
    |(st,sc)::xs -> 
      match (find_typescope ctxt st) with
      | Done(found,_,_) ->
        match p ([sc],found) with
        | Done(typscp,_,_) ->
          let res = ((st,typscp)::(filter_typescopes ctxt st)) 
          Done((st,typscp),xs,res)
        | Error (s,p) -> Error (s,p)
      | Error (s,p) -> Error (s,p)
    | [] -> Error (TypeError ["nothing to procces"],Position.Zero)



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
    let! dump = (build_empty_typescope_list |> repeat) |> use_new_scope
    let! res = (procces_scopes declcheck) |> repeat |> use_new_scope
    let! res = (procces_scopes typerulecheck) |> repeat |> use_new_scope
    return res
  }


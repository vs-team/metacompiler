module Prioritizer
open Common
open ParserMonad
open ScopeBuilder


type TreeExpr = Abs of TreeExpr*MaybeType*Position
              | App of TreeExpr*TreeExpr*MaybeType*Position
              | Var of Id*MaybeType*Position
              | Lit of Literal*Type*Position
and Rule = {
  Input    :TreeExpr
  Output   :TreeExpr
  Premises :List<Premise>
}and Premise = Assignment  of TreeExpr*TreeExpr
             | Conditional of TreeExpr
and Type = Star       // type
         | Signature  // module
         | TypeId     of Id
         | BigArrow   of Type*Type
         | SmallArrow of Type*Type
         | Union      of Type*TypeConstructors
and TypeConstructors = Map<Id,Type>
and TypeSignature = Nop
                  | Name of TypeSignature*TypeSignature
                  | Sig  of Type*TypeSignature
and MaybeType = Conflict of List<TreeExpr*TreeExpr>
              | Known    of Type
              | Unknown
and TypedScope = 
  {
    Parents       : List<Id*TypedScope>
    FuncDecls     : Map<Id,TypeSignature>
    TypeFuncDecls : Map<Id,TypeSignature>
    DataDecls     : Map<Id,TypeSignature>
    TypeFuncRules : Map<Id,List<Rule>>
    FuncRules     : Map<Id,List<Rule>>
  }
  with 
    static member Zero =
      {
        Parents       = []
        FuncDecls     = Map.empty
        TypeFuncDecls = Map.empty
        DataDecls     = Map.empty
        TypeFuncRules = Map.empty
        FuncRules     = Map.empty
      }

type Expr = Basic of BasicExpression | Tree of TreeExpr

let scopetype_to_type (typ:ScopeBuilder.Type) : Type =
  match typ with
  | [x] ->
    match x with 
    | Id (s,p) -> TypeId s
    | _ -> Star
  | x::xs -> Star
  | [] -> Star

let rec type_to_typesig (typ:List<ScopeBuilder.Type>) : TypeSignature =
  match typ with
  | x::xs -> Sig(scopetype_to_type x,(type_to_typesig xs))
  | [] -> Nop

let args_to_typesig (sym:SymbolDeclaration) : TypeSignature = 
  let l = type_to_typesig sym.LeftArgs
  let r = type_to_typesig sym.RightArgs
  Name (l,r)

let symdec_to_typedscope : Parser<SymbolDeclaration,TypedScope,Id*TypeSignature> =
  fun (sym,ctxt) ->
    match sym with 
    | x::xs -> Done((x.Name,args_to_typesig x),xs,ctxt)
    | [] -> Error ((TypeError[",symdec"]),Position.Zero)
   
let data_type : Parser<ScopeBuilder.Scope,TypedScope,_> =
  fun (scp,ctxt) ->
    match (symdec_to_typedscope |> repeat) (scp.Head.DataDeclarations,ctxt) with
    | Done(x,y,z) ->
      let ctxt' = {ctxt with DataDecls = (Map.ofList x)}
      Done((),scp,ctxt') 
    | Error (s,p) -> Error (s,p)   

let typecheck() : Parser<ScopeBuilder.Scope,TypedScope,TypedScope> =
  prs {
    do! data_type
    return! getContext
  }

let rec decls_check() : Parser<string*ScopeBuilder.Scope,List<string*TypedScope>,List<string*TypedScope>> =
  fun (scp,ctxt) ->
    match scp with
    |(st,sc)::xs -> 
      match (typecheck()) ([sc],TypedScope.Zero) with
      | Done(resa,y,z) ->  
        match decls_check() (xs,ctxt) with
        | Done(resb,y,z) -> Done((st,resa)::resb,xs,ctxt)
        | Error (s,p) -> Error (s,p)
      | Error (s,p) -> Error (s,p)
    | [] -> Done([],[],[])
(* -- see TypeChecker.fs -- *)
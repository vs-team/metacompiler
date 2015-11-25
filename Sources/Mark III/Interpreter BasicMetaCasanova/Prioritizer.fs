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
                  | Name of Id*TypeSignature*TypeSignature
                  | Signature of Type*TypeSignature
and MaybeType = Conflict of List<TreeExpr*TreeExpr>
              | Known    of Type
              | Unknown
and TypedScope = 
  {
    Parents       : List<Id*TypedScope>
    FuncDecls     : Map<Id,SymbolDeclaration*TypeSignature>
    TypeFuncDecls : Map<Id,SymbolDeclaration*TypeSignature>
    DataDecls     : Map<Id,SymbolDeclaration*TypeSignature>
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

let symdec_to_typedscope : Parser<SymbolDeclaration,TypedScope,Id*(SymbolDeclaration*TypeSignature)> =
  fun (sym,ctxt) ->
    match sym with 
    | x::xs -> Done((x.Name,(x,Nop)),xs,ctxt)
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
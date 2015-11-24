module Prioritizer
open Common
open ScopeBuilder

type Result<'scope,'typescope,'result> = 
  | Done of 'result * 'scope * 'typescope
  | Error of string * Position

type TypeParser<'scope,'typescope,'result> = 
  'scope * 'typescope -> Result<'scope,'typescope,'result>

type TypeParserBuilder() =
  member this.Return(res:'result) : TypeParser<'scope,'typescope,'result> =
    fun (scope,typescope) ->
      Done(res,scope,typescope)
  member this.ReturnFrom p = p
  member this.Bind(p:TypeParser<'scope,'typescope,'result>, k:'result->TypeParser<'scope,'typescope,'result'>) =
    fun (scope,typescope) ->
      match p (scope,typescope) with
      | Error(e,p) -> Error(e,p)
      | Done(res,scope',typescope') ->
        let out = k res (scope',typescope')
        out

let typ = TypeParserBuilder()

type Or<'a,'b> = A of 'a | B of 'b
let (.|.) (p1:TypeParser<_,_,'a>) (p2:TypeParser<_,_,'b>) : TypeParser<_,_,Or<'a,'b>> = 
  fun (chars,ctxt) ->
    match p1(chars,ctxt) with
    | Error(e1,p1) ->
      match p2(chars,ctxt) with
      | Error(e2,p2) ->
        Error(e1,p1)
      | Done(res2,chars',ctxt') ->
        (B(res2),chars',ctxt') |> Done
    | Done(res1,chars',ctxt') ->
       (A(res1),chars',ctxt') |> Done
let inline (.||) (p1:TypeParser<_,_,'a>) (p2:TypeParser<_,_,'a>) : TypeParser<_,_,'a> = 
  fun (chars,ctxt) ->
    match p1(chars,ctxt) with
    | Error(e1,p1) ->
      match p2(chars,ctxt) with
      | Error(e2,p2) ->
        Error((e1+e2),p1)
      | Done(res2,chars',ctxt') ->
        (res2,chars',ctxt') |> Done
    | Done(res1,chars',ctxt') ->
       (res1,chars',ctxt') |> Done
let nothing : TypeParser<_,_,Unit> = 
  fun (chars,ctxt) -> Done((),chars,ctxt)
let rec repeat (p:TypeParser<_,_,'result>) : TypeParser<_,_,List<'result>> =
  typ{
    let! curr = p .|. nothing
    match curr with
    | A x -> 
      let! xs = repeat p
      return x :: xs
    | B () -> 
      return []
  }
let getContext =
  fun (chars,ctxt) -> (ctxt,chars,ctxt) |> Done


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

let rec symdec_to_typedscope : TypeParser<List<SymbolDeclaration>,TypedScope,Id*(SymbolDeclaration*TypeSignature)> =
  fun (sym,ctxt) ->
    match sym with 
    | x::xs -> Done((x.Name,(x,Nop)),xs,ctxt)
    | [] -> Error (",symdec",Position.Zero)
   
let data_type : TypeParser<ScopeBuilder.Scope,TypedScope,_> =
  fun (scp,ctxt) ->
    match (symdec_to_typedscope |> repeat) (scp.DataDeclarations,ctxt) with
    | Done(x,y,z) ->
      let ctxt' = {ctxt with DataDecls = (Map.ofList x)}
      Done((),scp,ctxt') 
    | Error (s,p) -> Error (s,p)   

let typecheck() : TypeParser<ScopeBuilder.Scope,TypedScope,TypedScope> =
  typ {
    do! data_type
    return! getContext
  }

let rec decls_check : TypeParser<List<string*ScopeBuilder.Scope>,List<string*TypedScope>,List<string*TypedScope>> =
  fun (scp,ctxt) ->
    match scp with
    |(st,sc)::xs -> 
      match (typecheck()) (sc,TypedScope.Zero) with
      | Done(res,y,z) -> 
        
      Done([],xs,ctxt)

(* -- see TypeChecker.fs -- *)
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

type TypedScope = {
  Parents       : List<Id*TypedScope>
  FuncDecls     : Map<Id,SymbolDeclaration*Type>
  TypeFuncDecls : Map<Id,SymbolDeclaration*Type>
  DataDecls     : Map<Id,SymbolDeclaration*Type>
  TypeFuncRules : Map<Id,List<Rule>>
  FuncRules     : Map<Id,List<Rule>>
}
and TreeExpr = Abs of TreeExpr*MaybeType*Position
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
and MaybeType = Conflict of List<TreeExpr*TreeExpr>
              | Known    of Type
              | Unknown
type Expr = Basic of BasicExpression | Tree of TreeExpr

let rec typecheck() : TypeParser<ScopeBuilder.Scope,TypedScope,_> =
  typ {
    return 0
  }



(* -- see TypeChecker.fs -- *)
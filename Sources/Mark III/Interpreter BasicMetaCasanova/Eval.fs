module Eval
open Common;
open TypeChecker

type TypedScope = {
  Parents       : List<Id*TypedScope>
  FuncDecls     : Map<Id,Type>
  DataDecls     : Map<Id,Type>
  TypeFuncDecls : Map<Id,Kind>
  FuncRules     : Map<Id,List<Rule>>
  TypeFuncRules : Map<Id,List<Id>>
}

type CodeGenScope = {
  Parents       : List<Id*CodeGenScope>
  FuncDecls     : Map<Id,Type>
  DataDecls     : Map<Id,Type>
  FuncRules     : Map<Id,List<Rule>>
}

(* TODO
let evalScope(scope:TypedScope) : CodeGenScope =
  let funcs = scope.TypeFuncRules |> Map.map evalTypeFuncRules
*)
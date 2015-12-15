module RuleChecker
open Common
open ParserMonad
open ScopeBuilder
open Prioritizer

type RuleTypedScope = 
  {
    InheritDecls          : List<Id>
    SymbolTable           : List<TableSymbols>
    TypeFuncRules         : Map<Id,List<Rule>>
    FuncRules             : Map<Id,List<Rule>>
  }
  with 
    static member Zero =
      {
        InheritDecls    = []
        SymbolTable     = []
        TypeFuncRules   = Map.empty
        FuncRules       = Map.empty
      }


let Rules_check() : Parser<string*ScopeBuilder.Scope,List<string*TypedScope>,List<string*TypedScope>> =
  prs{
    let! res = (procces_scopes typerulecheck) |> repeat |> use_new_scope
    return res
  }

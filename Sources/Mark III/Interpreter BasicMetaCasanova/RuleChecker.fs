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

let make_rule =
  fun (rule:List<ScopeBuilder.Rule>,ctxt) ->
    match rule with
    | r::xs ->
      Done (("",[{Input    = type_to_typesig [r.Input]
                  Output   = type_to_typesig [r.Output]
                  Premises = []}]),xs,ctxt)
    |[] -> Error TypeError
  
let rule_to_typedrule : Parser<ScopeBuilder.Rule,RuleTypedScope,Id*List<Rule>> =
  prs{
    let! r = make_rule
    return r
  }


let rule_type : Parser<ScopeBuilder.Scope,RuleTypedScope,_> =
  lift_type rule_to_typedrule (fun scp ctxt -> (scp.Head.Rules,ctxt))
    (fun ctxt x -> {ctxt with FuncRules = (Map.ofList x)})
let typerule_type : Parser<ScopeBuilder.Scope,RuleTypedScope,_> =
  lift_type rule_to_typedrule (fun scp ctxt -> (scp.Head.TypeFunctionRules,ctxt))
    (fun ctxt x -> {ctxt with TypeFuncRules = (Map.ofList x)})


let typerulecheck : Parser<ScopeBuilder.Scope,RuleTypedScope,RuleTypedScope> =
  prs {
    do! rule_type
    do! typerule_type
    return! getContext
  }

let Rules_check() : Parser<string*ScopeBuilder.Scope,List<string*RuleTypedScope>,List<string*RuleTypedScope>> =
  prs{
    let! res = (procces_scopes typerulecheck) |> repeat |> use_new_scope
    return res
  }

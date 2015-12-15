module RuleChecker
open Common
open ParserMonad
open ScopeBuilder
open Prioritizer

type Rule = {
  Input    :TypeSignature
  Output   :TypeSignature
  Premises :List<Premise>}

and Premise = Assignment  
             | Conditional 

and RuleTypedScope = 
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

let rule_to_typedrule : Parser<ScopeBuilder.Rule,RuleTypedScope,Id*List<Rule>> =
  fun (rule:List<ScopeBuilder.Rule>,ctxt) ->
    match rule with
    | r::xs ->
      Done (("",[{Input    = type_to_typesig [r.Input]
                  Output   = type_to_typesig [r.Output]
                  Premises = []}]),xs,ctxt)
    |[] -> Error TypeError

let rec sort_rules sort sorted =
  let rule_exists st list = List.exists (fun (s,ru) -> if s = st then true else false) list
  let rule_find st list = List.find (fun (s,ru) -> if s = st then true else false) list
  let rule_filter st list = List.filter (fun (s,ru) -> if s = st then false else true) list 
  match sort with
  | (st,ru)::xs -> 
    if rule_exists st sorted then
      let st',found = rule_find st sorted
      sort_rules xs ((st,(ru@found))::(rule_filter st sorted))
    else sort_rules xs ((st,ru)::sorted)
  | [] -> sorted

let rule_type : Parser<ScopeBuilder.Scope,RuleTypedScope,_> =
  lift_type rule_to_typedrule (fun scp ctxt -> (scp.Head.Rules,ctxt))
    (fun ctxt x -> {ctxt with FuncRules = (Map.ofList (sort_rules x []))})
let typerule_type : Parser<ScopeBuilder.Scope,RuleTypedScope,_> =
  lift_type rule_to_typedrule (fun scp ctxt -> (scp.Head.TypeFunctionRules,ctxt))
    (fun ctxt x -> {ctxt with TypeFuncRules = (Map.ofList (sort_rules x []))})

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

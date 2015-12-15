module RuleChecker
open Common
open ParserMonad
open ScopeBuilder
open Prioritizer

type TreeExpr = Name of Id*TreeExpr*TreeExpr
              | App  of TreeExpr*TreeExpr
              | Var  of Id*Type
              | Lit  of Literal*Type

type Rule = {
  Input    :TreeExpr
  Output   :TreeExpr
  Premises :List<Premise>}

and Premise = Assignment    of TreeExpr*TreeExpr
             | Conditional  of TreeExpr

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
let rec collect_correct (p) =
  fun (ts,expr) ->
    match ts with
    | x::xs -> 
      match p (ts,expr) with
      | Done(res,ts',ctxt) -> 
        match collect_correct p (ts',expr) with
        | Done(tail,ts'',ctxt') -> Done((res::tail),ts'',expr)
        | Error e -> Error e
      | Error _ -> 
        match collect_correct p (xs,expr) with
        | Done(tail,ts'',ctxt') -> Done((tail),ts'',expr)
        | Error e -> Error e
    | [] -> Done([],[],expr)

let rec find_id_in_basicexpresion (id:Id) (expr:List<BasicExpression>) =
  match expr with
  | x::xs -> 
    match x with
    | Id(i,_) -> if i = id then true else find_id_in_basicexpresion id xs
    | Application(_,li) -> find_id_in_basicexpresion id li || find_id_in_basicexpresion id xs
    | _ -> find_id_in_basicexpresion id xs
  | [] -> false

let get_tablesymbol_name (ts:TableSymbols) :Id =
  match ts with
  | DataSym(id,_,_)  -> id
  | FuncSym(id,_,_)  -> id
  | TypeSym(id,_,_)  -> id
  | ArrowSym(id,_,_) -> id
  | AliasSym(id,_,_) -> id

let rec match_rule_to_decl : Parser<TableSymbols,List<BasicExpression>,TableSymbols> =
  fun (ts,expr) ->
    match ts with
    | x::xs -> 
      if find_id_in_basicexpresion (get_tablesymbol_name x) expr 
      then Done(x,xs,expr) else Error TypeError
    | [] -> Error TypeError 

let rule_to_ruleinput : Parser<ScopeBuilder.Rule,RuleTypedScope,Id*TreeExpr> =
  fun (rule,ctxt) ->
    match rule with
    | x::xs ->
      match ((match_rule_to_decl |> collect_correct) (ctxt.SymbolTable,x.Input)) with
      | Done(res,a,b) ->
        match res with 
        | x::xs ->
          Done(((get_tablesymbol_name x),(Lit(String(""),Star))),rule,ctxt)
        | [] -> Error TypeError
      | Error e -> Error e
    | [] -> Error TypeError

let next_rule : Parser<ScopeBuilder.Rule,RuleTypedScope,_> =
  fun (rule,ctxt) -> 
    match rule with
    | x::xs -> Done((),xs,ctxt)
    | [] -> Error TypeError

let rule_to_typedrule : Parser<ScopeBuilder.Rule,RuleTypedScope,Id*Rule> =
  prs{
    let! name,input = rule_to_ruleinput
    let output = Lit(String(""),Star)
    do! next_rule
    return (name,{Input = input; Output = output; Premises = []})
  }
let rule_to_typedrule2 : Parser<ScopeBuilder.Rule,RuleTypedScope,Id*List<Rule>> =
  fun (rule:List<ScopeBuilder.Rule>,ctxt) ->
    match rule with
    | r::xs ->
      Done (("",[{Input    = Lit(String(""),Star)
                  Output   = Lit(String(""),Star)
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
      sort_rules xs ((st,(ru::found))::(rule_filter st sorted))
    else sort_rules xs ((st,[ru])::sorted)
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

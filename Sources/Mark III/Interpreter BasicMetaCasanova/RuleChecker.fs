module RuleChecker
open Common
open ParserMonad
open ScopeBuilder
open Prioritizer

type TreeExpr = Name of Id*TreeExpr*TreeExpr*Type
              | LeR  of TreeExpr*TreeExpr
              | Lef  of TreeExpr*TreeExpr
              | Rig  of TreeExpr*TreeExpr
              | Var  of Id*Type
              | Lit  of Literal*Type

type ExprSig = Element     of Id*TypeSignature*ScopeBuilder.BasicExpression
             | ElementList of List<ExprSig>

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

let lift_tablesymbol_content (ts:TableSymbols) ret =
  match ts with
  | DataSym (x,y,z)  -> ret (x,y,z)
  | FuncSym (x,y,z)  -> ret (x,y,z)
  | TypeSym (x,y,z)  -> ret (x,y,z)
  | ArrowSym(x,y,z)  -> ret (x,y,z)
  | AliasSym(x,y,z)  -> ret (x,y,z)

let get_tablesymbol_name (ts:TableSymbols) :Id =
  lift_tablesymbol_content ts (fun (x,_,_) -> x)

let get_tablesymbol_typesignature (ts:TableSymbols) =
  lift_tablesymbol_content ts (fun (_,_,x) -> x)

let get_priority_of_symbol (ts:TableSymbols) =
  let pri sign =
    match sign with
    | Prioritizer.Name((i,a),_,_,_) -> i
    | _ -> failwith "no name found"  
  lift_tablesymbol_content ts (fun (_,_,x) -> pri x)

let rec match_rule_to_decl : Parser<TableSymbols,List<BasicExpression>,TableSymbols> =
  fun (ts,expr) ->
    match ts with
    | x::xs -> 
      if find_id_in_basicexpresion (get_tablesymbol_name x) expr 
      then Done(x,xs,expr) else Error TypeError
    | [] -> Error (RuleError "match_rule_to_decl")

let order_tablesymbols (ts:List<TableSymbols>) = 
  List.sortWith (fun x y -> (get_priority_of_symbol y) - (get_priority_of_symbol x)) ts 

let table_symbol_exist (ts:List<TableSymbols>) (id:Id) =
  List.exists (fun table -> (get_tablesymbol_name table) = id) ts

let get_typesignature_from_tablesym (ts:List<TableSymbols>) (id:Id) =
  let table = List.find (fun table -> (get_tablesymbol_name table) = id) ts
  get_tablesymbol_typesignature table

let rec slot_check_symbol_tree : Parser<TableSymbols,List<ExprSig>,TreeExpr> =
  let get_index id = 
    List.tryFindIndex (fun x -> 
      match x with 
      | Element(i,typ,be) -> id = i
      | ElementList(_) -> false 
    )
  fun (ts,expr) -> 
    let rec split_list ts expr = 
      match ts with
      | x::xs -> 
        match get_index (get_tablesymbol_name x) expr  with
        | Some (i) -> List.splitAt i expr
        | None -> split_list xs expr
      | [] -> [],expr
    let a,b = (split_list ts expr)
    printfn "%A\n+++++++\n%A\n========" a b
    Done((Lit(String(""),Star)),ts,expr)
      
let build_symbol_tree : Parser<TableSymbols,List<ExprSig>,TreeExpr> =
  let rec fill_exprsig (ts:List<TableSymbols>) (be:List<ExprSig>) : List<ExprSig> =
    match be with
    | Element(_,typsig,expr)::xs ->
      match expr with
      | Id(i,p) -> 
        if table_symbol_exist ts i then 
          Element(i,(get_typesignature_from_tablesym ts i),expr)::(fill_exprsig ts xs)
        else Element(i,Nop,expr)::(fill_exprsig ts xs)
      | _ -> failwith "not implemented yet"
    | ElementList(x)::xs -> 
      ElementList(fill_exprsig ts x)::fill_exprsig ts xs
    | [] -> []
  fun (ts,be) -> 
    let exprstructure = fill_exprsig ts be
    let ruletree = 
      match (slot_check_symbol_tree) (ts,exprstructure) with
      | Done(res,_,_) -> res
      | Error e -> failwith "not implemented yet"
    //printfn "%A\n____________" (bla)
    Done((Lit(String(""),Star)),ts,be)

let rec build_exprsig_list (be:List<ScopeBuilder.BasicExpression>) : List<ExprSig> =
  match be with
  | Id(x,p)::xs -> Element("",Nop,Id(x,p))::(build_exprsig_list xs)
  | Application(_,x)::xs ->ElementList(build_exprsig_list x)::(build_exprsig_list xs)
  | [] -> []
  | _ -> failwith "not implemented yet"
    
let rule_to_ruleinput : Parser<ScopeBuilder.Rule,RuleTypedScope,Id*TreeExpr> =
  fun (rule,ctxt) ->
    match rule with
    | x::xs ->
      match ((match_rule_to_decl |> collect_correct) (ctxt.SymbolTable,x.Input)) with
      | Done(res,a,b) ->
        match res with 
        | ts::tsxs ->
          match (build_symbol_tree) (order_tablesymbols res,(build_exprsig_list x.Input)) with
          | Done (tree,_,_) -> 
            //printfn "%A\n____________" (order_tablesymbols res)
            Done(((get_tablesymbol_name ts),(Lit(String(""),Star))),rule,ctxt)
          | Error e -> Error e
        | [] -> Error (RuleError (sprintf "No symbol found in:%A" x.Input))
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

let rec sort_rules sort sorted =
  let rule_exists st list = List.exists (fun (s,ru) -> s = st) list
  let rule_find st list = List.find (fun (s,ru) -> s = st) list
  let rule_filter st list = List.filter (fun (s,ru) -> s = st |> not) list 
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
    //do! typerule_type
    return! getContext
  }

let Rules_check() : Parser<string*ScopeBuilder.Scope,List<string*RuleTypedScope>,List<string*RuleTypedScope>> =
  prs{
    let! res = (procces_scopes typerulecheck) |> itterate |> use_new_scope
    return res
  }

module DeclarationMatcher
open Common
open ScopeBuilder // Scope

type MatchedScope = {
  Funcs: List<Rule*SymbolDeclaration>
  Scope: Scope
}

let findSymbol (id:Id) (scope:Scope) :SymbolDeclaration =
  scope.FunctionDeclarations |> List.find (fun x -> x.Name=id)

let RankFuncs (scope:Scope) (expr:List<BasicExpression>) :List<SymbolDeclaration> =
  let rec findFuncs (expr:BasicExpression) :List<SymbolDeclaration> = 
    match expr with
    | Id (str,_) -> [findSymbol str scope]
    | Application (Round,exprs) -> exprs |> List.collect findFuncs
    | _ -> []
  expr |> List.map findFuncs |> List.concat |> List.sortWith (fun x y -> x.Priority - y.Priority)

let MatchFuncs (scope:Scope) : List<Rule*SymbolDeclaration> =
  let matchrules (rules:List<Rule>) (declaration : SymbolDeclaration) : Rule*SymbolDeclaration = 
    // TODO: implement
    // 1) get toplevel expression from rule input
    // 2) get rule with most priority
    // 3) if any with same priority check associativity
    // 4) if associativity conflicts (1 (L) 2 (R) 3) error
    let perRule (rule:Rule) =
      let symbol_table:List<SymbolDeclaration> = rule.Input |> RankFuncs scope
      0
    let test = perRule(rules.Head)
    (rules.Head,declaration)
  scope.FunctionDeclarations |> List.map (matchrules scope.Rules)

let MatchDeclarations (scope:Scope) : Option<MatchedScope> =
  Some({
    Funcs = MatchFuncs scope
    Scope = scope
  })
module DeclarationMatcher
open Common
open ScopeBuilder // Scope
open LineSplitter // BasicExpression

let RankFuncs (expr:BasicExpression) (scope:Scope) :List<SymbolDeclaration> =
  let findSymbol (id:Id) :SymbolDeclaration =
    scope.FunctionDeclarations |> List.find (fun x -> x.Name=id)
  let rec findFuncs (expr:BasicExpression) :List<SymbolDeclaration> = 
    match expr with
    | Id (str,_) -> [findSymbol str]
    | Application (Round,exprs) -> exprs |> List.collect findFuncs
    | _ -> []
  findFuncs expr |> List.sortWith (fun x y -> x.Priority - y.Priority)

let MatchFuncs (scope:ScopeBuilder.Scope) : List<Rule*SymbolDeclaration> =
  let matchrules (rules:List<Rule>) (declaration : SymbolDeclaration) : Rule*SymbolDeclaration = 
    // TODO: implement
    (rules.Head,declaration)
  scope.FunctionDeclarations |> List.map (matchrules scope.Rules)

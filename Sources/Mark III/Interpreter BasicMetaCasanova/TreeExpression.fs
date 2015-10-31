module TreeExpression
open Common
open ScopeBuilder

let findSymbol (id:Id) (scope:Scope) :SymbolDeclaration =
  scope.FunctionDeclarations |> List.find (fun x -> x.Name=id)

type TreeExpression = Literal of Literal*Position
                    | Symbol of List<TreeExpression>*SymbolDeclaration*List<TreeExpression>*Position

let rec treeify (scope:Scope) (exprs:List<BasicExpression>) :Option<TreeExpression> =
  exprs 
  // first filter out non operators, and lookup the declaration while were at it
  |> List.choose (fun expr ->
    match expr with
    | Id(str,pos) -> let symbol = findSymbol str scope
                     if symbol.LeftArgs.IsEmpty && symbol.RightArgs.IsEmpty 
                     then None else Some(symbol,pos) 
    | _           -> None )
  // then fold the list, keep the operator of highest priority
  |> List.reduce (fun (acc,acc_pos) (right,right_pos) ->
      if   acc.Priority > right.Priority then acc,acc_pos
      elif acc.Priority < right.Priority then right,right_pos
      elif acc.Associativity = right.Associativity then
        do printfn "Warning in %s on line %d col %d: Ambiguous order of operations." acc_pos.File acc_pos.Line acc_pos.Col
        do printfn "  Operators %s and %s have the same priority, but different associativity." acc.Name right.Name
        do printfn "  Add parentheses to disambiguate. (will continue with left associativity)"
        acc,acc_pos
      elif acc.Associativity = Left then acc,acc_pos
      else right,right_pos )
  // when we have the highest priority operator, we have to find its operands
  |> (fun (operatorSymbol,operatorPosition) -> 
      // since we don't have List.chooseIndex, we have to search for the original index
      let operatorIndex = exprs |> List.findIndex (fun expr -> 
        match expr with 
        | Id(sym,found_pos) -> operatorPosition=found_pos 
        | _ -> false)
      if operatorSymbol.LeftArgs.Length < operatorIndex then
        do printfn "Error in %s on line %d col %d: Left argument(s) expected." operatorPosition.File operatorPosition.Line operatorPosition.Col
        do printfn "  Operator %s expects %d arguments to its left, but received %d." operatorSymbol.Name operatorSymbol.LeftArgs.Length operatorIndex
        do printfn "  Add missing operand(s) to its left."
        None
      elif operatorSymbol.RightArgs.Length > operatorIndex + exprs.Length then
        do printfn "Error in %s on line %d col %d: Right argument(s) expected." operatorPosition.File operatorPosition.Line operatorPosition.Col
        do printfn "  Operator %s expects %d arguments to its right, but received %d." operatorSymbol.Name operatorSymbol.RightArgs.Length (exprs.Length - operatorIndex)
        do printfn "  Add missing operand(s) to its right."
        None
      else
        // TODO finish
        None)
      
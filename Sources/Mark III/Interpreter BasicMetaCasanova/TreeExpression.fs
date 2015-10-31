module TreeExpression
open Common
open ScopeBuilder

let findSymbol (id:Id) (scope:Scope) :SymbolDeclaration =
  scope.FunctionDeclarations |> List.find (fun x -> x.Name=id)

let pivot (f:'a -> bool) (lst:List<'a>) :List<'a>*'a*List<'a> =
  let left,pivot::right = lst |> List.splitAt (lst |> List.findIndex f)
  left,pivot,right

// I am unhappy about this
let rec tryExtractPosition(expr:BasicExpression) :Option<Position> =
  match expr with
  | Id          (_,pos) -> Some pos
  | Literal     (_,pos) -> Some pos
  | Application (_,lst) -> lst |> List.tryHead |> Option.bind tryExtractPosition
  | Scope       (scope) -> None

let tryAny (f:'a->option<'b>) (lst:List<'a>) :Option<'b> =
  lst |> List.tryFind (f>>Option.isSome) |> Option.bind f

type TreeExpression = Literal of Literal*Position
                    | Symbol of List<TreeExpression>*SymbolDeclaration*List<TreeExpression>*Position

let extractOperators scope exprs= 
  exprs |> List.choose (fun expr ->
    match expr with
    | Id(str,pos) -> let symbol = findSymbol str scope
                     if symbol.LeftArgs.IsEmpty && symbol.RightArgs.IsEmpty 
                     then None else Some(symbol,pos) 
    | _           -> None )

let getHighestPriorityOperator operators = 
  operators |> List.reduce (fun (acc,acc_pos) (right,right_pos) ->
    if   acc.Priority > right.Priority then acc,acc_pos
    elif acc.Priority < right.Priority then right,right_pos
    elif acc.Associativity = right.Associativity then
      do printfn "Warning in %s on line %d col %d: Ambiguous order of operations." acc_pos.File acc_pos.Line acc_pos.Col
      do printfn "  Operators %s and %s have the same priority, but different associativity." acc.Name right.Name
      do printfn "  Add parentheses to disambiguate. (will continue with left associativity)"
      acc,acc_pos
    elif acc.Associativity = Left then acc,acc_pos
    else right,right_pos )

let pivotPosition pos exprs =
  exprs |> pivot (fun expr -> 
    match expr with 
    | Id(sym,found_pos) -> pos=found_pos 
    | _ -> false)

let rec treeify (scope:Scope) (exprs:List<BasicExpression>) :Option<TreeExpression> =
  match exprs with
  | [BasicExpression.Literal (l,pos)] -> Some (Literal (l,pos))
  | exprs  -> 
    let ops = exprs |> extractOperators scope
    if ops.IsEmpty then
      // very unhappy indeed
      match tryAny tryExtractPosition exprs with
      | Some pos -> do printfn "Error in %s on line %d col %d: No operator in subexpression." pos.File pos.Line pos.Col
                    None
      | None     -> do printfn "Error somewhere idk, maybe in a file or something?: No operator in subexpression."
                    None
    else
      let operatorSymbol,operatorPosition = ops |> getHighestPriorityOperator
      let leftArgs,_,rightArgs = exprs |> pivotPosition operatorPosition
      if operatorSymbol.LeftArgs.Length < leftArgs.Length then
        do printfn "Error in %s on line %d col %d: Left argument(s) expected." operatorPosition.File operatorPosition.Line operatorPosition.Col
        do printfn "  Operator %s expects %d arguments to its left, but received %d." operatorSymbol.Name operatorSymbol.LeftArgs.Length leftArgs.Length
        do printfn "  Add missing operand(s) to its left."
        None
      elif operatorSymbol.RightArgs.Length > rightArgs.Length then
        do printfn "Error in %s on line %d col %d: Right argument(s) expected." operatorPosition.File operatorPosition.Line operatorPosition.Col
        do printfn "  Operator %s expects %d arguments to its right, but received %d." operatorSymbol.Name operatorSymbol.RightArgs.Length rightArgs.Length
        do printfn "  Add missing operand(s) to its right."
        None
      else 
        // TODO: finish
        None
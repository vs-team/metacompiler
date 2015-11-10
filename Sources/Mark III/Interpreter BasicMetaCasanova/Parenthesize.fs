module Parenthesizer
open Common
open ScopeBuilder

let rec prettyPrintExprs exprs =
  exprs |> List.map (fun expr ->
    match expr:BasicExpression with
    | Id      (str,_) -> sprintf "%s" str
    | Literal (l,_) ->
      match l:Literal with
      | Int     i -> sprintf "%d" i
      | Float32 f -> sprintf "%f" f
      | String  s -> sprintf "\"%s\"" s
    | Application(b,e) ->
      match b:Bracket with
      | Curly  -> sprintf "{%s}" (prettyPrintExprs e)
      | Round  -> sprintf "(%s)" (prettyPrintExprs e)
      | Square -> sprintf "[%s]" (prettyPrintExprs e)
      | Indent -> sprintf "<indent: %s>" (prettyPrintExprs e)
      | Implicit -> sprintf "<implicit: %s>" (prettyPrintExprs e)
    | Scope s -> "<scope>") |> List.toSeq |> String.concat " "


let tryFindSymbol (id:Id) (fdecls:List<SymbolDeclaration>) :Option<SymbolDeclaration> =
  fdecls |> List.tryFind (fun x -> x.Name=id)

let pivot (f:'a -> bool) (lst:List<'a>) :List<'a>*'a*List<'a> =
  let left,pivot::right = lst |> List.splitAt (lst |> List.findIndex f)
  left,pivot,right

let rec takeBack (n:int) (lst:List<'a>) : List<'a> =
  if List.length lst <= n then lst else takeBack n lst.Tail

// I am unhappy about this
let rec tryExtractPosition(expr:BasicExpression) :Option<Position> =
  match expr with
  | Id          (_,pos) -> Some pos
  | Literal     (_,pos) -> Some pos
  | Application (_,lst) -> lst |> List.tryHead |> Option.bind tryExtractPosition
  | Scope       (scope) -> None

let tryAny (f:'a->option<'b>) (lst:List<'a>) :Option<'b> =
  lst |> List.tryFind (f>>Option.isSome) |> Option.bind f

let getOperator fdecls expr =
  match expr with
  | Id(str,pos) -> 
    let symbol = tryFindSymbol str fdecls
    match symbol with
    | None -> None
    | Some symbol -> if symbol.LeftArgs.IsEmpty && symbol.RightArgs.IsEmpty 
                     then None else Some (symbol,pos) 
  | _ -> None

let extractOperators fdecls exprs = 
  exprs |> List.choose (getOperator fdecls)

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

let getPriorityOperator (acc:SymbolDeclaration,acc_pos:Position) (right:SymbolDeclaration,right_pos:Position) :SymbolDeclaration*Position =
  if   acc.Priority > right.Priority then acc,acc_pos
  elif acc.Priority < right.Priority then right,right_pos
  elif acc.Associativity <> right.Associativity then
    do printfn "Warning in %s on line %d col %d: Ambiguous order of operations." acc_pos.File acc_pos.Line acc_pos.Col
    do printfn "  Operators %s and %s have the same priority, but different associativity." acc.Name right.Name
    do printfn "  Add parentheses to disambiguate. (will continue with left associativity)"
    acc,acc_pos
  elif acc.Associativity = Left then acc,acc_pos
  else right,right_pos 

let compare fdecls (acc:SymbolDeclaration*Position) (expr:BasicExpression) =
  match getOperator fdecls expr with
  | None   -> acc
  | Some x -> getPriorityOperator acc x

let rec Parenthesize (fdecls:List<SymbolDeclaration>) (exprs:List<BasicExpression>) :List<BasicExpression> =
  let exprs = exprs |> List.map (fun x -> 
    match x with
    | Application(b,x) -> Application(b,Parenthesize fdecls x)
    | foo -> foo )
  let fromFirstId = exprs |> List.skipWhile ((getOperator fdecls)>>Option.isNone)
  if fromFirstId.IsEmpty then exprs
  else 
    let operatorSymbol,operatorPosition = 
      fromFirstId |> List.fold (compare fdecls) (Option.get (getOperator fdecls fromFirstId.Head))
    let left,_,right = pivotPosition operatorPosition exprs 
    if operatorSymbol.LeftArgs.Length > left.Length then
      do printfn "Error in %s on line %d col %d: Left argument(s) expected." operatorPosition.File operatorPosition.Line operatorPosition.Col
      do printfn "  in expression: %s" (prettyPrintExprs exprs)
      do printfn "  Operator %s expects %d arguments to its left, but received %d." operatorSymbol.Name operatorSymbol.LeftArgs.Length left.Length
      []
    elif operatorSymbol.RightArgs.Length > right.Length then
      do printfn "Error in %s on line %d col %d: Right argument(s) expected." operatorPosition.File operatorPosition.Line operatorPosition.Col
      do printfn "  in expression: %s" (prettyPrintExprs exprs)
      do printfn "  Operator %s expects %d arguments to its right, but received %d." operatorSymbol.Name operatorSymbol.RightArgs.Length right.Length
      []
    else 
      let newExpr = Id(operatorSymbol.Name,operatorPosition)
      let leftArgs  = left  |> takeBack  operatorSymbol.LeftArgs.Length
      let rightArgs = right |> List.take operatorSymbol.RightArgs.Length
      if leftArgs.Length=left.Length && rightArgs.Length = right.Length then
        leftArgs@(newExpr::rightArgs)
      else
        let farLeft   = left  |> List.take (left.Length-leftArgs.Length)
        let farRight  = right |> takeBack  (right.Length-rightArgs.Length)
        let newParen= Application(Round,leftArgs@(newExpr::rightArgs))
        let newArgs = farLeft@(newParen::farRight)
        Parenthesize fdecls newArgs
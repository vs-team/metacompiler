module Prioritizer

open ParseMonad
open Common

type Expression = 
  | Literal of Literal 
  | Id of Id
  // in order of (growing) priority
  | Plus of Expression * Expression
  | Minus of Expression * Expression
  | DividedBy of Expression * Expression
  | GreaterThan of Expression * Expression
  | Equal of Expression * Expression
  | If of Expression * Expression
  | While of Expression * Expression
  | Print of Expression
  | Semicolon of Expression * Expression
  | Sequence of List<Expression>

type IntermediateExpression =
  Prioritized of Expression | Unprioritized of Parenthesizer.Expression

type Context = Unit

let rec toIntermediate l =
  match l with
  | [] -> []
  | [Parenthesizer.Keyword Keyword.Semicolon] -> []
  | x :: xs -> Unprioritized x :: toIntermediate xs

let rec (!) ie =
  match ie with
  | Prioritized(e) -> e
  | Unprioritized(e) -> prioritize([e])

and groupBinaryOperator f op (l:List<IntermediateExpression>) = 
  match l with
  | x :: Unprioritized(Parenthesizer.Keyword op') :: y :: xs when op = op' ->
    groupBinaryOperator f op (Prioritized(f(!x,!y)) :: xs)
  | x :: y :: xs ->
    x :: groupBinaryOperator f op (y :: xs)
  | _ -> l

and groupPlus = groupBinaryOperator Expression.Plus Keyword.Plus
and groupMinus = groupBinaryOperator Expression.Minus Keyword.Minus
and groupDividedBy = groupBinaryOperator Expression.DividedBy Keyword.DividedBy
and groupGreaterThan = groupBinaryOperator Expression.GreaterThan Keyword.Gt
and groupEqual = groupBinaryOperator Expression.Equal Keyword.Equals
and groupSemicolon = groupBinaryOperator Expression.Semicolon Keyword.Semicolon

and groupDoubleApplication f op (l:List<IntermediateExpression>) = 
  match l with
  | Unprioritized(Parenthesizer.Keyword op') :: x :: y :: xs when op = op' ->
    Prioritized(f(!x,!y)) :: (groupDoubleApplication f op xs)
  | x :: xs ->
    x :: groupDoubleApplication f op xs
  | _ -> l

and groupIf = groupDoubleApplication Expression.If Keyword.If
and groupWhile = groupDoubleApplication Expression.While Keyword.While

and groupSingleApplication f op (l:List<IntermediateExpression>) = 
  match l with
  | Unprioritized(Parenthesizer.Keyword op') :: x :: xs when op = op' ->
    Prioritized(f(!x)) :: (groupSingleApplication f op xs)
  | x :: xs ->
    x :: groupSingleApplication f op xs
  | _ -> l

and groupPrint = groupSingleApplication Expression.Print Keyword.Print

and prioritize (l:List<Parenthesizer.Expression>) =
    let l' = l |> toIntermediate
               |> groupPlus |> groupMinus |> groupDividedBy |> groupGreaterThan |> groupEqual
               |> groupIf |> groupWhile |> groupPrint |> groupSemicolon
    let res = 
      [
        for x in l' do
          match x with
          | Prioritized e -> yield e
          | Unprioritized(Parenthesizer.Id x) -> yield Id x
          | Unprioritized(Parenthesizer.Literal x) -> yield Literal x
          | Unprioritized(Parenthesizer.Block(_,l)) -> yield prioritize l
          | _ -> failwith "Unexpected unprioritized expression!"
      ]
    match res with
    | [x] -> x
    | _ -> Sequence res

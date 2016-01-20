module TypeChecker
open Common
open ScopeBuilder // Scope

type TreeExpr = Fun of Id*Namespace*TreeExpr*TreeExpr //points to library function
              | Rul of Id*Namespace*TreeExpr*TreeExpr //points to rule
              | DataLR of Id*Namespace*TreeExpr*TreeExpr //points to data
              | DataRL of Id*Namespace*TreeExpr*TreeExpr //points to data
              | Lambda of Id*Namespace*TreeExpr*TreeExpr 
              | App of TreeExpr*TreeExpr     
              | Var of Id*Type
              | Lit of Literal

and Rule = {
  Input    :TreeExpr
  Output   :TreeExpr
  Premises :List<Premise>
}

and Conditional = Less | LessEqual | Greater | GreaterEqual | Equal | NotEqual

and Premise = Assignment      of TreeExpr*TreeExpr
            | Conditional     of TreeExpr*Conditional*TreeExpr
            | ArrowAssignment of TreeExpr*TreeExpr

and Data = {
  Input    :TreeExpr
  Output   :TreeExpr
}

and Type = Id           of Id
         | Arrow        of Type*Type
         | Union        of Type*Type
         | Tuple        of Type*Type

type Namespace = {
    Lambdas             :List<int*Rule>
    LibraryFunctions    :List<Id*TreeExpr*TreeExpr>
    Rules               :List<Id*List<Rule>>
    Datas               :List<Id*List<Data>>
}
type NamespaceId = List<string>
type Scope =  Map<NamespaceId,Namespace>

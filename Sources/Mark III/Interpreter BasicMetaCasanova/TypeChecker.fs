module TypeChecker
open Common
open ScopeBuilder // Scope

type Id       = List<string>
type LambdaId = List<string>*int

type LTreeExpr = Fun    of Id*LTreeExpr*LTreeExpr //points to library function
               | Rul    of Id*LTreeExpr*LTreeExpr //points to rule
               | Data   of Id*LTreeExpr*LTreeExpr
               | Lambda of LambdaId*LTreeExpr*LTreeExpr 
               | App    of LTreeExpr*LTreeExpr     
               | Var    of Id*Type
               | Lit    of Literal

type RTreeExpr = Var  of Id*Type
               | Data of Id*RTreeExpr*RTreeExpr

and Rule = {
  Input    :RTreeExpr
  Output   :RTreeExpr
  Premises :List<Premise>
}

and Conditional = Less | LessEqual | Greater | GreaterEqual | Equal | NotEqual

and Premise = Assignment      of LTreeExpr*RTreeExpr
            | Conditional     of LTreeExpr*Conditional*LTreeExpr

and Data = {
  Input    :List<Type>
  Output   :Type
}

and EasyData = {
  Constructor : Id
  Args   : List<Type>
  Result : Type
}

and Type = Id           of Id 
         | App          of Type*Type
         | Arrow        of Type*Type

type Namespace = {
    Lambdas             :List<LambdaId*Rule>
    LibraryFunctions    :List<Id*LTreeExpr*LTreeExpr>
    Rules               :List<Id*List<Rule>>
    Datas               :List<Id*List<Data>>
}

type Scope =  Map<Id,Namespace>

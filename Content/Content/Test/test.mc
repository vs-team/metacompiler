import System
import System.Collections.Immutable


Data Expr -> "<" -> Expr : Expr          Priority 1000
Data Expr -> ">" -> Expr : Expr          Priority 1000
Data Expr -> "<=" -> Expr : Expr          Priority 1000
Data Expr -> ">=" -> Expr : Expr          Priority 1000
Func "run" : Runner => Unit
Data "unit" : Unit

Func "eval" -> <<int>> : Evaluator => Expr Priority 10


Unit is Expr

---------------------
eval (unit > unit) => unit

------------------
run => unit

Func "run" : Expr => Value                        Priority 0 

Func "eval" -> Expr : Expr => Value               Priority 1
Data Expr -> "+" -> Expr : Expr                   Priority 10 
Data Expr -> "*" -> Expr : Expr                   Priority 20 
Data "$" -> <<int>> : Value                       Priority 10000

Value is Expr


eval ($10 + $20 * $2) => res
-----------------------------
run => res


----------------
eval ($i) => $i

eval a => $x
eval b => $y
<<x+y>> => res
-------------------
eval (a+b) => $res

eval a => $x
eval b => $y
<<x*y>> => res
--------------------
eval (a*b) => $res

Keyword [] "run" [] Priority 0 Class Expr

Keyword [] "eval" [Expr] Priority 1 Class Expr
Keyword [Expr] "+" [Expr] Priority 10 Class Expr
Keyword [Expr] "*" [Expr] Priority 20 Class Expr
Keyword [] "$" [<<int>>] Priority 10000 Class Value

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

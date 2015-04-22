Func [] "run" [] Priority 0 Type Expr => Value

Func [] "eval" [Expr] Priority 1 Type Expr => Value
Data [] [Expr] "+" [Expr] Priority 10 Type Expr
Data [] [Expr] "*" [Expr] Priority 20 Type Expr
Data [] [] "$" [<<int>>] Priority 10000 Type Value

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

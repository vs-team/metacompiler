import System
import System.Collections.Immutable


Data "$i" -> <<int>> : Value             Priority 10000
Data "$s" -> <<string>> : Value          Priority 10000
Data "$b" -> <<bool>> : Value            Priority 10000
Data "$f" -> <<float>> : Value           Priority 10000

Data "$" -> <<string>> : ID              Priority 10000

Data Expr -> "+" -> Expr : Expr          Priority 500
Data Expr -> "-" -> Expr : Expr          Priority 500
Data Expr -> "*" -> Expr : Expr          Priority 1000
Data Expr -> "/" -> Expr : Expr          Priority 1000
Data Expr -> "||" -> Expr : Expr         Priority 1000
Data Expr -> "&&" -> Expr : Expr         Priority 1000

Func "eval" -> Expr -> <<ImmutableDictionary<string, Value> >> : Evaluator => Value      Priority 1000

Value is Expr
ID is Expr

<<m.GetKey(a)>> => res
-----------------
eval $a m => res

---------------------
eval ($b a) m => $b a

-----------------------
eval ($i a) m => $i a

-----------------------
eval ($s s) m => $s s

-----------------------
eval ($f f) m => $f f

eval a m => $i c
eval b m => $i d
<<c + d>> => res
-----------------------------
eval (a + b) m => $i res

eval a m => $i c
eval b m => $i d
<<c - d>> => res
-----------------------------
eval (a - b) m => $i res


eval a  m => $i c
eval b m => $i d
<<c * d>> => res
-----------------------------
eval (a * b) m => $i res

eval a m => $i c
eval b m => $i d
<<c / d>> => res
-----------------------------
eval (a / b) m => $i res

eval a m => $f c
eval b m => $f d
<<c + d>> => res
-----------------------------
eval (a + b) m => $f res

eval a m => $f c
eval b m => $f d
<<c * d>> => res
-----------------------------
eval (a * b) m => $f res

eval a m => $f c
eval b m => $f d
<<c / d>> => res
-----------------------------
eval (a / b) m => $f res

eval a m => $f c
eval b m => $f d
<<c - d>> => res
-----------------------------
eval (a - b) m => $f res

eval a m => $b c
c == true
---------------------
eval (a || b) m => $b c

eval a m => $b c
c == false
eval b m => $b d
---------------------
eval (a || b) m => $b d

eval a m => $b c
c == true
eval b m => $b d
---------------------
eval (a && b) m => $b d

eval a m => $b c
c == false
---------------------
eval (a && b) m => $b c


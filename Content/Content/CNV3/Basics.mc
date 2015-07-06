include Content.GenericLists.transform.mc

import System
import System.Collections.Immutable




Data "$i" -> <<int>> : Value             Priority 10000
Data "$s" -> <<string>> : Value          Priority 10000
Data "$b" -> <<bool>> : Value            Priority 10000
Data "$f" -> <<float>> : Value           Priority 10000
Data "$l" -> List[Value] : Value         Priority 10000

Data "$" -> <<string>> : ID              Priority 10000

Data Expr -> "+" -> Expr : Expr          Priority 500
Data Expr -> "-" -> Expr : Expr          Priority 500
Data Expr -> "*" -> Expr : Expr          Priority 1000
Data Expr -> "/" -> Expr : Expr          Priority 1000
Data Expr -> "||" -> Expr : Expr         Priority 1000
Data Expr -> "&&" -> Expr : Expr         Priority 1000
Data Expr -> "++" -> Expr : Expr         Priority 1000
Data Expr -> "@" -> Expr : Expr          Priority 1000
Data Expr -> "ls" -> Expr : Expr         Priority 1000
Data Expr -> "leq" -> Expr : Expr        Priority 1000
Data Expr -> "gt" -> Expr : Expr         Priority 1000
Data Expr -> "geq" -> Expr : Expr        Priority 1000



Data "Context" -> <<ImmutableDictionary<string, Value> >> -> <<ImmutableDictionary<string, Value> >> -> <<ImmutableDictionary<string, Value> >> : ctxt

Func "eval" -> Expr -> ctxt : Evaluator => Value      Priority 10
Func "test" : Test => Value                           Priority 10

Value is Expr
ID is Expr

<<locals.ContainsKey(a)>> == true
<<locals.GetKey(a)>> => res
--------------------------------------------
eval $a (Context locals entity world) => res

<<entity.GetKey(a)>> => res
--------------------------------------------
eval $a (Context locals entity world) => res

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


eval a m => $i c
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

eval a m => $i x
eval b m => $i y
-------------------------
eval (a ls b) m => $b (<<x < y>>)

eval a m => $i x
eval b m => $i y
-------------------------
eval (a leq b) m => $b (<<x <= y>>)

eval a m => $i x
eval b m => $i y
-------------------------
eval (a gt b) m => $b (<<x > y>>)

eval a m => $i x
eval b m => $i y
-------------------------
eval (a geq b) m => $b (<<x >= y>>)

eval a m => $f x
eval b m => $f y
-------------------------
eval (a ls b) m => $b (<<x < y>>)

eval a m => $f x
eval b m => $f y
-------------------------
eval (a leq b) m => $b (<<x <= y>>)

eval a m => $f x
eval b m => $f y
-------------------------
eval (a gt b) m => $b (<<x > y>>)

eval a m => $f x
eval b m => $f y
-------------------------
eval (a geq b) m => $b (<<x >= y>>)


--------------------
eval ($l li) m => $l li

eval e m => v
eval el m => ($l li)
-------------------------
eval (e ++ el) m => ($l (v :: li))

eval ex m => ($l xs)
eval ey m => ($l ys)
xs append ys => zs
-----------------------
eval ex @ ey m => ($l zs)


x := $l (($i 1) :: ($i 2) :: ($i 3) :: nil)
y := $l ($i 4 :: nil)
m := Context <<ImmutableDictionary<string, Value>.Empty>> <<ImmutableDictionary<string, Value>.Empty>> <<ImmutableDictionary<string, Value>.Empty>>
eval (x @ y) m => res
----------------------------------
test => res


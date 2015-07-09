include Content.GenericLists.transform.mc
include Content.CNV3.Tuples.mc

import System
import System.Collections.Immutable




Data "$i" -> <<int>> : Value             Priority 10000
Data "$s" -> <<string>> : Value          Priority 10000
Data "$b" -> <<bool>> : Value            Priority 10000
Data "$f" -> <<float>> : Value           Priority 10000
Data "$l" -> List[Value] : Value         Priority 5000
Data "$t" -> Tuple[Value Value] : Value      Priority 5000
Data "$first" -> Expr : Expr               Priority 10
Data "$second" -> Expr : Expr               Priority 10

Data "$" -> <<string>> : ID              Priority 10000

Data Expr -> "+" -> Expr : Expr          Priority 500
Data Expr -> "-" -> Expr : Expr          Priority 500
Data Expr -> "*" -> Expr : Expr          Priority 1000
Data Expr -> "/" -> Expr : Expr          Priority 1000
Data Expr -> "||" -> Expr : Expr         Priority 1000
Data Expr -> "&&" -> Expr : Expr         Priority 1000
Data Expr -> "++" -> Expr : Expr         Priority 1000
Data Expr -> "@" -> Expr : Expr          Priority 1000
Data Expr -> "lt" -> Expr : Expr         Priority 1000
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

---------------------
eval ($t t) m => $t t

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
eval (a lt b) m => $b (<<x < y>>)

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
eval (a lt b) m => $b (<<x < y>>)

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

eval expr m => ($t t)
<<Console.WriteLine(t)>>
fst t => res
------------------------
eval $first expr m => res

eval expr m => ($t t)
snd t => res
------------------------
eval $second expr m => res


t := $t ($f 1.0,($t ($f 2.0,$f 3.0)))
m := Context <<ImmutableDictionary<string, Value>.Empty>> <<ImmutableDictionary<string, Value>.Empty>> <<ImmutableDictionary<string, Value>.Empty>>
eval $first t m => res
----------------------------------
test => res


﻿include Content.GenericLists.transform.mc
include Content.CNV3.Tuples.mc

import System
import UnityEngine
import System.Collections.Immutable



Data "$wrapperSet" -> << WrapperTest >> -> Expr : Value  Priority 10000
Data "$wrapper" -> << WrapperTest >> : Value Priority 10000
Data "$i" -> <<int>> : Value             Priority 10000
Data "$s" -> <<string>> : Value          Priority 10000
Data "$b" -> <<bool>> : Value            Priority 10000
Data "$f" -> <<float>> : Value           Priority 10000
Data "$Vector3" -> << Vector3 >> : Value  Priority 10000 
Data "$l" -> List[Value] : Value         Priority 5000
Data "$t" -> Tuple[Value Value] : Value      Priority 5000
Data "$first" -> Expr : Expr               Priority 10
Data "$second" -> Expr : Expr               Priority 10

Data "$" -> <<string>> : ID              Priority 10000
Data Expr -> "+" -> Expr : Expr                       Priority 500
Data Expr -> "-" -> Expr : Expr                       Priority 500
Data Expr -> "*" -> Expr : Expr                       Priority 1000
Data Expr -> "/" -> Expr : Expr                       Priority 1000
Data Expr -> "||" -> Expr : Expr                      Priority 1000
Data Expr -> "&&" -> Expr : Expr                      Priority 1000
Data Expr -> "++" -> Expr : Expr                      Priority 1000
Data Expr -> "@" -> Expr : Expr                       Priority 1000
Data Expr -> "lt" -> Expr : Expr                      Priority 1000
Data Expr -> "leq" -> Expr : Expr                     Priority 1000
Data Expr -> "gt" -> Expr : Expr                      Priority 1000
Data Expr -> "geq" -> Expr : Expr                     Priority 1000
Data "vectorx" -> Expr : Expr                         Priority 1000
Data "vectory" -> Expr : Expr                         Priority 1000
Data "vectorz" -> Expr : Expr                         Priority 1000




Data "Context" -> <<ImmutableDictionary<string, Value> >> -> <<ImmutableDictionary<string, Value> >> -> <<ImmutableDictionary<string, Value> >> : ctxt

Func "eval" -> Expr -> ctxt : Evaluator => Value            Priority 10
Func "createArgString" -> List[Expr] : Utility => Value     Priority 10
Func "test" : Test => Value                                 Priority 10

Value is Expr
Imported is Value
ID is Expr


<< locals.ContainsKey(a) >> == true
<< locals.GetKey(a) >> => res
--------------------------------------------
eval $a (Context locals entity world) => res

<< entity.GetKey(a) >> => r
eval r (Context locals entity world) => res
--------------------------------------------
eval $a (Context locals entity world) => res

<< entity.GetKey(a) >> => res
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

------------------------
eval ($Vector3 v) m => $Vector3 v


eval a m => $Vector3 v1
eval b m => $Vector3 v2
<<v1 + v2>> => res
-----------------------------
eval (a + b) m => $Vector3 res

eval a m => $Vector3 v1
eval b m => $Vector3 v2
<<v1 - v2>> => res
-----------------------------
eval (a - b) m => $Vector3 res

eval v m => $Vector3 v1
eval s m => $f s1
<< v1 * s1 >> => res
--------------------------------
eval (v * s) m => $Vector3 res

eval v m => $Vector3 v1
eval s m => $f s1
<< v1 / s1 >> => res
--------------------------------
eval (v / s) m => $Vector3 res


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
fst t => res
------------------------
eval $first expr m => res

eval expr m => ($t t)
snd t => res
------------------------
eval $second expr m => res

eval v m => $Vector3 v1
<< v1.x >> => res
-----------------------------
eval (vectorx v) m => $f res

eval v m => $Vector3 v1
<< v1.y >> => res
-----------------------------
eval (vectory v) m => $f res

eval v m => $Vector3 v1
<< v1.z >> => res
-----------------------------
eval (vectorz v) m => $f res

<< x.Position >> => v
-------------------------------------
eval ($wrapper x) m => $Vector3 v

---------------------------------------------
eval ($wrapperSet x e) m => $wrapperSet x e



<< new Vector3(1.0,-3.0,0.0) >> => vx
<< new Vector3(0.0,1.0,0.0) >> => p
<< WrapperTest.Instantiate(p) >> => wt
m := Context <<ImmutableDictionary<string, Value>.Empty>> <<ImmutableDictionary<string, Value>.Empty>> <<ImmutableDictionary<string, Value>.Empty>>
eval ($wrapper wt + $Vector3 vx) m => res
--------------------------------------
test => res


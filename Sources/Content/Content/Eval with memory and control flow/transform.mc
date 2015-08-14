import System
import System.Collections.Immutable

Data "map" -> <<ImmutableDictionary<string, Value> >> : MapIntString                      Priority 10000
Func "run" -> MapIntString : Expr => EvalResult
Func MapIntString -> "add" -> <<string>> -> Value : Expr => MapIntString
Func MapIntString -> "lookup" -> <<string>> : Expr => Value

Func "eval" -> Expr -> MapIntString : Expr => EvalResult
Data Expr -> "gt" -> Expr : Expr                                                        Priority 10
Data Expr -> "+" -> Expr : Expr                                                         Priority 10
Data Expr -> "*" -> Expr : Expr                                                         Priority 20
Data Expr -> ";" -> Expr : Expr                   
Data Value -> "," -> MapIntString : EvalResult
Data "!" -> <<string>> : Variable                                                       Priority 10000
Data "?" -> <<bool>> : Value                                                            Priority 10000
Data "$" -> <<int>> : Value                                                             Priority 10000
Data "nil" : Value
Data Variable -> "assign" -> Expr : Expr                                                Priority 2

Data "if" -> Expr -> Then -> Expr -> Else -> Expr : Expr                                Priority 5
Data "then" : Then
Data "else" : Else
Data "while" -> Expr -> Do -> Expr : Expr                                               Priority 5
Data "do" : Do

MapIntString is Expr
Value is Expr
Variable is Expr


<<M.Remove(k)>> => M1
<<M1.Add(k,v)>> => M2
--------------------------
(map M) add k v => map M2

<<M.GetKey(k)>> => v
-----------------------
(map M) lookup k => v


M add "x" $10 => M1
M1 add "y" $20 => M2
M2 add "z" $-30 => M3
eval (!"x" assign (if (!"x" gt !"y") then (!"x" assign $-1; !"x") else (!"z" assign !"y" + !"z"; !"z")); !"x" * $2) M3 => res,M4
---------------------------------------------------------------------------------------------------------------------------------
run M => res,M4


---------------------
eval ($i) m => $i,m

---------------------
eval nil m => nil,m

m lookup v => res
------------------------
eval !v m => res,m

eval e m => res,m1
m1 add v res => m2
-------------------------------
eval (!v assign e) m => nil,m2

eval a m0 => nil,m1
eval b m1 => res,m2
------------------------
eval (a;b) m0 => res,m2

eval a m0 => $x,m1
eval b m1 => $y,m2
<<x+y>> => res
-------------------------
eval (a+b) m0 => $res,m2

eval a m0 => $x,m1
eval b m1 => $y,m2
<<x*y>> => res
-------------------------
eval (a*b) m0 => $res,m2

eval a m0 => $x,m1
eval b m1 => $y,m2
x > y
-----------------------------
eval (a gt b) m0 => ?true,m2

eval a m0 => $x,m1
eval b m1 => $y,m2
x <= y
------------------------------
eval (a gt b) m0 => ?false,m2

eval c m0 => ?true, m1
eval a m1 => res,m2
---------------------------------------
eval (if c then a else b) m0 => res,m2

eval c m0 => ?false, m1
eval b m1 => res,m2
---------------------------------------
eval (if c then a else b) m0 => res,m2


eval c m0 => ?true, m1
eval b m1 => nil,m2
eval (while c do b) m2 => res,m3
---------------------------------------
eval (while c do b) m0 => res,m3

eval c m0 => ?false, m1
---------------------------------------
eval (while c do b) m0 => nil,m1

import System
import System.Collections.Immutable

Keyword [] "map" [<<ImmutableDictionary<string, Value> >>] Priority 10000 Class Memory
Keyword [] "run" [Memory] Priority 0 Class Expr
Keyword [Memory] "add" [<<string>> Value] Priority 1 Class Expr
Keyword [Memory] "lookup" [<<string>>] Priority 1 Class Expr

Keyword [] "eval" [Expr Memory] Priority 1 Class Expr
Keyword [Expr] "gt" [Expr] Priority 10 Class Expr
Keyword [Expr] "+" [Expr] Priority 20 Class Expr
Keyword [Expr] "*" [Expr] Priority 30 Class Expr
Keyword [Expr] ";" [Expr] Priority 1 Class Expr
Keyword [Value] "," [Memory] Priority 1 Class Expr
Keyword [] "!" [<<string>>] Priority 10000 Class Variable
Keyword [] "?" [<<bool>>] Priority 10000 Class Value
Keyword [] "$" [<<int>>] Priority 10000 Class Value
Keyword [] "nil" [] Priority 10000 Class Value
Keyword [Variable] "assign" [Expr] Priority 2 Class Expr
Keyword [] "if" [Expr Then Expr Else Expr] Priority 5 Class Expr
Keyword [] "then" [] Priority 10000 Class Then
Keyword [] "else" [] Priority 10000 Class Else

Memory is Expr
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

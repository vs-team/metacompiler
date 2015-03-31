import System
import System.Collections.Immutable

Keyword [] "map" [<<ImmutableDictionary<string, int> >>] Priority 10000 Class MapIntString
Keyword [] "run" [MapIntString] Priority 0 Class Expr
Keyword [MapIntString] "add" [<<string>> <<int>>] Priority 1 Class Expr
Keyword [MapIntString] "lookup" [<<string>>] Priority 1 Class Expr

Keyword [] "eval" [Expr MapIntString] Priority 1 Class Expr
Keyword [Expr] "+" [Expr] Priority 10 Class Expr
Keyword [Expr] "*" [Expr] Priority 20 Class Expr
Keyword [Expr] ";" [Expr] Priority 1 Class Expr
Keyword [Value] "," [MapIntString] Priority 1 Class Expr
Keyword [] "!" [<<string>>] Priority 10000 Class Variable
Keyword [] "$" [<<int>>] Priority 10000 Class Value
Keyword [] "nil" [] Priority 10000 Class Value
Keyword [Variable] "assign" [Expr] Priority 2 Class Expr

MapIntString is Expr
Value is Expr
Variable is Expr


<<M.Remove(k)>> => M1
<<M1.Add(k,v)>> => M2
--------------------------
(map M) add k v => map M2

<<M.GetKey(k)>> => v
-----------------------
(map M) lookup k => $v


M add "x" 10 => M1
M1 add "y" 20 => M2
M2 add "z" -30 => M3
eval (!"x" assign !"x" + !"y" * $2; !"x" + !"z") M3 => res
-----------------------
run M => res


---------------------
eval ($i) m => $i,m

---------------------
eval nil m => nil,m

m lookup v => res
------------------------
eval !v m => res,m

eval e m => $res,m1
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

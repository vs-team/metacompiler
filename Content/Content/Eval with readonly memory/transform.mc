import System
import System.Collections.Immutable

Keyword [] "map" [<<ImmutableDictionary<string, int> >>] Priority 10000 Class MapIntString
Keyword [] "run" [MapIntString] Priority 0 Class Expr
Keyword [MapIntString] "add" [<<string>> <<int>>] Priority 1 Class Expr
Keyword [MapIntString] "lookup" [<<string>>] Priority 1 Class Expr

Keyword [] "eval" [Expr MapIntString] Priority 1 Class Expr
Keyword [Expr] "+" [Expr] Priority 10 Class Expr
Keyword [Expr] "*" [Expr] Priority 20 Class Expr
Keyword [] "!" [<<string>>] Priority 10000 Class Variable
Keyword [] "$" [<<int>>] Priority 10000 Class Value

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
eval (!"x" + !"y" * $2) M3 => res
----------------------------------
run M => res


---------------------
eval ($i) m => $i

m lookup v => res
------------------------
eval !v m => res

eval a m => $x
eval b m => $y
<<x+y>> => res
-------------------------
eval (a+b) m => $res

eval a m => $x
eval b m => $y
<<x*y>> => res
-------------------------
eval (a*b) m => $res

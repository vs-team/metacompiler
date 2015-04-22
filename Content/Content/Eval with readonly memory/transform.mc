import System
import System.Collections.Immutable

Data [] [] "map" [<<ImmutableDictionary<string, int> >>] Priority 10000 Type MapIntString
Func [] "run" [MapIntString] Priority 0 Type Expr => Value
Func [] "add" [MapIntString <<string>> <<int>>] Priority 1 Type Expr => MapIntString
Func [] "lookup" [MapIntString <<string>>] Priority 1 Type Expr => MapIntString

Func [] "eval" [Expr MapIntString] Priority 1 Type Expr => Value
Data [] [Expr] "+" [Expr] Priority 10 Type Expr
Data [] [Expr] "*" [Expr] Priority 20 Type Expr
Data [] [] "!" [<<string>>] Priority 10000 Type Variable
Data [] [] "$" [<<int>>] Priority 10000 Type Value

MapIntString is Expr
Value is Expr
Variable is Expr


<<M.Remove(k)>> => M1
<<M1.Add(k,v)>> => M2
--------------------------
add (map M) k v => map M2

<<M.GetKey(k)>> => v
-----------------------
lookup (map M) k => $v


add M "x" 10 => M1
add M1 "y" 20 => M2
add M2 "z" -30 => M3
eval (!"x" + !"y" * $2) M3 => res
----------------------------------
run M => res


---------------------
eval ($i) m => $i

lookup m v => res
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

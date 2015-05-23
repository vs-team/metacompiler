import System
import System.Collections.Immutable

Data "map" -> <<ImmutableDictionary<string, int> >> : MapIntString         Priority 10000
Func "run" -> MapIntString : Expr => Value                                 Priority 0 
Func "add" -> MapIntString -> <<string>> -> <<int>>: Expr => MapIntString  Priority 1 
Func "lookup" -> MapIntString -> <<string>> : Expr => Value                Priority 1 

Func "eval" -> Expr MapIntString : Expr => Value                           Priority 1
Data Expr -> "+" -> Expr : Expr                                            Priority 10
Data Expr -> "*" -> Expr : Expr                                            Priority 20
Data "!" -> <<string>> : Variable                                          Priority 10000
Data "$" -> <<int>> : Value                                                Priority 10000

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

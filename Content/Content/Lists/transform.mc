import System

Data [] [] "nil" [] Priority 0 Type ListInt
Data [] [<<int>>] ";" [ListInt] Priority 1000 Type ListInt
Func [] "$" [<<int>>] Priority 10000 Type Expr => IntValue

Data [] [ListInt] "contains" [<<int>>] Priority 100 Type Expr
Data [] [] "removeOdd" [ListInt] Priority 100 Type Expr

Func [] "add" [ListInt] Priority 100 Type Expr => Expr

Data [] [] "yes" [] Priority 0 Type Bool
Data [] [] "no" [] Priority 0 Type Bool


--------------
add nil => $0

add xs => $res
--------------------------
add x;xs => $<<x + res>>


---------------------
nil contains k => no

x == k
------------------------
x;xs contains k => yes

x != k
xs contains k => res
------------------------
x;xs contains k => res


---------------------
removeOdd nil => nil

<< x % 2 >> == 0
removeOdd xs => xs'
-----------------------
removeOdd x;xs => xs'

<< x % 2 >> == 1
removeOdd xs => xs'
--------------------------
removeOdd x;xs => x;xs'

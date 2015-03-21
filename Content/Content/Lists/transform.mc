import System

Keyword [] "nil" [] Priority 0 Class ListInt
Keyword [IntValue] ";" [ListInt] Priority 1000 Class ListInt
Keyword [] "$" [<<int>>] Priority 10000 Class IntValue

Keyword [ListInt] "contains" [<<int>>] Priority 100 Class Expr
Keyword [] "removeOdd" [ListInt] Priority 100 Class Expr

Keyword [] "add" [ListInt] Priority 100 Class Expr

Keyword [] "yes" [] Priority 0 Class Bool
Keyword [] "no" [] Priority 0 Class Bool


--------------
add nil => $0

add xs => $res
--------------------------
add $x;xs => $<<x + res>>


---------------------
nil contains k => no

x == k
------------------------
$x;xs contains k => yes

x != k
xs contains k => res
------------------------
$x;xs contains k => res


---------------------
removeOdd nil => nil

<< x % 2 >> == 0
removeOdd xs => xs'
-----------------------
removeOdd $x;xs => xs'

<< x % 2 >> == 1
removeOdd xs => xs'
--------------------------
removeOdd $x;xs => $x;xs'

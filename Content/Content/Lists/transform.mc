Keyword [] "nil" [] Priority 0 Class ListInt
Keyword [IntValue] ";" [ListInt] Priority 1000 Class ListInt
Keyword [] "$" [<<int>>] Priority 10000 Class IntValue

Keyword [ListInt] "contains" [<<int>>] Priority 100 Class Expr
Keyword [] "removeOdd" [ListInt] Priority 100 Class Expr

Keyword [] "yes" [] Priority 0 Class Bool
Keyword [] "no" [] Priority 0 Class Bool


---------------------
nil contains k => no

x == k
------------------------
$x;xs contains k => yes

x != k
xs contains k => res
------------------------
$x;xs contains k => res


x == k
------------------------
$x;xs contains k => yes


---------------------
removeOdd nil => nil

x % 2
removeOdd xs => xs'
------------------------
removeOdd $x;xs => xs'

x !% 2
removeOdd xs => xs'
------------------------
removeOdd $x;xs => $x;xs'

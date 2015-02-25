Keyword [] "nil" [] Priority 0 Class ListInt
Keyword [<<int>>] ";" [ListInt] Priority 1000 Class ListInt

Keyword [ListInt] "contains" [<<int>>] Priority 100 Class Expr

Keyword [] "yes" [] Priority 0 Class Bool
Keyword [] "no" [] Priority 0 Class Bool


---------------------
nil contains k => no

x == k
---------------------
x;xs contains k => yes

x != k
xs contains k => res
-----------------------
x;xs contains k => res

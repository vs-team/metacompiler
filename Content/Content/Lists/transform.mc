Keyword = "nil" LeftArguments = [] RightArguments = [] Priority = 0 Class = "ListInt"
Keyword = ";" LeftArguments = [<<int>>] RightArguments = [ListInt] Priority = 1000 Class = "ListInt"

Keyword = "contains" LeftArguments = [ListInt] RightArguments = [<<int>>] Priority = 100 Class = "Expr"

Keyword = "yes" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Bool"
Keyword = "no" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Bool"


---------------------
nil contains k => no

x == k
---------------------
x;xs contains k => yes

x != k
xs contains k => res
-----------------------
x;xs contains k => res

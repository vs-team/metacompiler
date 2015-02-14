Keyword = "nil" GenericArguments = ['a] LeftArguments = [] RightArguments = [] Priority = 0 Class = "List 'a"
Keyword = ";" GenericArguments = ['a] LeftArguments = ['a] RightArguments = [(List 'a)] Priority = 1000 Class = "List 'a"

Keyword = "contains" GenericArguments = ['a] LeftArguments = [(List 'a)] RightArguments = ['a] Priority = 100 Class = "Expr"

Keyword = "yes" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Bool"
Keyword = "no" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Bool"


---------------------
nil contains k => no

x == k
-----------------------
x;xs contains k => yes

x != k
xs contains k => res
-----------------------
x;xs contains k => res

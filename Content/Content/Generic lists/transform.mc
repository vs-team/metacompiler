Keyword = "$" LeftArguments = [] RightArguments = [<<int>>] Priority = 10000 Class = "Id"

Keyword = "nil" GenericArguments = [t] LeftArguments = [] RightArguments = [] Priority = 0 Class = "List[t]"
Keyword = ";" GenericArguments = [t] LeftArguments = [t] RightArguments = [List[t]] Priority = 1000 Class = "List[t]"

Keyword = "contains" GenericArguments = [a] LeftArguments = [List[a]] RightArguments = [a] Priority = 100 Class = "Expr"

Keyword = "yes" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Bool"
Keyword = "no" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Bool"

Keyword = "," LeftArguments = [Expr] RightArguments = [Expr] Priority = 100 Class = "Expr"
Keyword = "runTest1" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Expr"

Bool is Expr
Id is Expr
List[t] is Expr

l := ($1;($2;($3;nil)))
p := l contains $3
p => res
-----------------------
runTest1 => p, res

x == k
-----------------------
x;xs contains k => yes

---------------------------
nil contains k => no

x != k
xs contains k => res
-----------------------
x;xs contains k => res


Keyword = "$" LeftArguments = [] RightArguments = [<<int>>] Priority = 10000 Class = "Id"

Keyword = "nil" GenericArguments = [a] LeftArguments = [] RightArguments = [] Priority = 0 Class = "List[a]"
Keyword = ";" GenericArguments = [a] LeftArguments = [a] RightArguments = [List[a]] Priority = 1000 Class = "List[a]"

Keyword = "contains" GenericArguments = [a] LeftArguments = [List[a]] RightArguments = [a] Priority = 100 Class = "Expr"

Keyword = "yes" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Bool"
Keyword = "no" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Bool"

Keyword = "," LeftArguments = [Expr] RightArguments = [Expr] Priority = 100 Class = "Expr"
Keyword = "runTest1" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Expr"

Bool is Expr
Id is Expr
List[a] is Expr

---------------------------
nil contains k => no

x == k
-----------------------
x;xs contains k => yes

x != k
xs contains k => res
-----------------------
x;xs contains k => res


l := ($1;($2;($3;nil)))
p := l contains $3
p => res
-----------------------
runTest1 => p, res

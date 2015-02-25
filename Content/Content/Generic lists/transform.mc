Keyword [] "$" [<<int>>] Priority 10000 Class Id

Keyword [] ("nil"[t]) [] Priority 0 Class (List[t])
Keyword [t] (";"[t]) [List[t]] Priority 1000 Class (List[t])

Keyword [List[a]] ("contains"[a]) [a] Priority 100 Class Expr

Keyword [] "yes" [] Priority 0 Class Bool
Keyword [] "no" [] Priority 0 Class Bool

Keyword [Expr] "," [Expr] Priority 100 Class Expr
Keyword [] "runTest1" [] Priority 0 Class Expr

Bool is Expr
Id is Expr
List[a] is Expr
List[<<int>>] is Id

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


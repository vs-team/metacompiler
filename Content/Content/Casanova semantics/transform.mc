Keyword = "$m" LeftArguments = [] RightArguments = [<<System.Collections.Immutable.ImmutableDictionary<string, Expr>>>] Priority = 10000 Class = "Locals"
Keyword = "add" LeftArguments = [Locals] RightArguments = [<<string>> <<Expr>>] Priority = 1000 Class = "Locals"
Keyword = "lookup" LeftArguments = [Locals] RightArguments = [<<string>>] Priority = 1000 Class = "Expr"

Keyword = "$" LeftArguments = [] RightArguments = [<<System.String>>] Priority = 10000 Class = "Id"

Keyword = "$b" LeftArguments = [] RightArguments = [<<System.Boolean>>] Priority = 10000 Class = "BoolConst"
Keyword = ">" LeftArguments = [IntExpr] RightArguments = [IntExpr] Priority = 1100 Class = "BoolExpr"
Keyword = "=" LeftArguments = [IntExpr] RightArguments = [IntExpr] Priority = 1100 Class = "BoolExpr"
Keyword = "&&" LeftArguments = [BoolExpr] RightArguments = [BoolExpr] Priority = 1000 Class = "BoolExpr"
Keyword = "||" LeftArguments = [BoolExpr] RightArguments = [BoolExpr] Priority = 900 Class = "BoolExpr"

Keyword = "$i" LeftArguments = [] RightArguments = [<<System.Int32>>] Priority = 10000 Class = "IntConst"
Keyword = "+" LeftArguments = [IntExpr] RightArguments = [IntExpr] Priority = 900 Class = "IntExpr"

Keyword = "if" LeftArguments = [] RightArguments = [BoolExpr Then Expr Else Expr] Priority = 500 Class = "Expr"
Keyword = "then" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Then"
Keyword = "else" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Else"

Keyword = "eval" LeftArguments = [] RightArguments = [Locals Expr] Priority = 0 Class = "Expr"

Keyword = "runTest1" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Test"

Id inherits Expr
BoolConst inherits BoolExpr
BoolExpr inherits Expr
IntConst inherits IntExpr
IntExpr inherits Expr


v := <<M.GetKey(k)>>
---------------------
($m M) lookup k => v

M' := <<M.Add(k,v)>>
------------------------
($m M) add k v => $m M'

c != $b true
c != $b false
eval M c => c'
eval M (if c' then t else e) => res
------------------------------------
eval M (if c then t else e) => res

eval M t => res
-------------------------------------------
eval M (if ($b true) then t else e) => res

eval M e => res
--------------------------------------------
eval M (if ($b false) then t else e) => res

M lookup v => res
-------------------
eval M ($v) => res

M0 := $m <<System.Collections.Immutable.ImmutableDictionary<string, Expr>.Empty>>
M0 add "x" ($i 100) => M1
M1 add "y" ($i 1000) => M2
eval M2 (if ($b true) then $"x" else $"y") => res
------------------------------------------------------------
runTest1 => res

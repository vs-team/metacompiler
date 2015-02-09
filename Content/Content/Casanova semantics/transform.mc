Keyword = "$m" LeftArguments = [] RightArguments = [<<System.Collections.Immutable.ImmutableDictionary<string, Expr>>> <<float>>] Priority = 10000 Class = "Locals"
Keyword = "add" LeftArguments = [Locals] RightArguments = [<<string>> <<Expr>>] Priority = 1000 Class = "Locals"
Keyword = "lookup" LeftArguments = [Locals] RightArguments = [<<string>>] Priority = 1000 Class = "Expr"

Keyword = "$" LeftArguments = [] RightArguments = [<<string>>] Priority = 10000 Class = "Id"

Keyword = "$b" LeftArguments = [] RightArguments = [<<bool>>] Priority = 10000 Class = "BoolConst"
Keyword = ">" LeftArguments = [IntExpr] RightArguments = [IntExpr] Priority = 1100 Class = "BoolExpr"
Keyword = "=" LeftArguments = [IntExpr] RightArguments = [IntExpr] Priority = 1100 Class = "BoolExpr"
Keyword = "&&" LeftArguments = [BoolExpr] RightArguments = [BoolExpr] Priority = 1000 Class = "BoolExpr"
Keyword = "||" LeftArguments = [BoolExpr] RightArguments = [BoolExpr] Priority = 900 Class = "BoolExpr"

Keyword = "$i" LeftArguments = [] RightArguments = [<<int>>] Priority = 10000 Class = "IntConst"
Keyword = "*" LeftArguments = [IntExpr] RightArguments = [IntExpr] Priority = 910 Class = "IntExpr"
Keyword = "/" LeftArguments = [IntExpr] RightArguments = [IntExpr] Priority = 910 Class = "IntExpr"
Keyword = "+" LeftArguments = [IntExpr] RightArguments = [IntExpr] Priority = 900 Class = "IntExpr"
Keyword = "-" LeftArguments = [IntExpr] RightArguments = [IntExpr] Priority = 900 Class = "IntExpr"

Keyword = "if" LeftArguments = [] RightArguments = [BoolExpr Then Expr Else Expr] Priority = 500 Class = "Expr"
Keyword = "then" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Then"
Keyword = "else" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Else"

Keyword = "wait" LeftArguments = [] RightArguments = [<<float>>] Priority = 0 Class = "Expr"

Keyword = "unit" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Expr"

Keyword = ";" LeftArguments = [Expr] RightArguments = [Expr] Priority = -10 Class = "Expr"

Keyword = "eval" LeftArguments = [] RightArguments = [Locals Expr] Priority = -1000 Class = "Expr"

Keyword = "runTest1" LeftArguments = [] RightArguments = [] Priority = -10000 Class = "Test"

Id inherits Expr
BoolConst inherits BoolExpr
BoolExpr inherits Expr
IntConst inherits IntExpr
IntExpr inherits Expr


v := <<M.GetKey(k)>>
---------------------
($m M dt) lookup k => v

M' := <<M.Add(k,v)>>
------------------------------
($m M dt) add k v => $m M' dt

dt := 0.01
M := $m <<System.Collections.Immutable.ImmutableDictionary<string, Expr>.Empty>> dt
eval M (wait 0.01) => res
------------------------------------------------------------
runTest1 => res

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

  <<dt >= t>> == true
  --------------------------------
  eval ($m M dt) (wait t) => unit

  <<dt >= t>> == false
  t' := <<t - dt>>
  -----------------------------------
  eval ($m M dt) (wait t) => wait t'

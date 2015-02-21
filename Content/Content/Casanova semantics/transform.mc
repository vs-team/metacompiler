Keyword = "$m" LeftArguments = [] RightArguments = [<<System.Collections.Immutable.ImmutableDictionary<string, Expr>>>] Priority = 10000 Class = "Locals"
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

Keyword = "nil" LeftArguments = [] RightArguments = [] Priority = 500 Class = "ExprList"
Keyword = "," LeftArguments = [Expr] RightArguments = [ExprList] Priority = 910 Class = "ExprList"

Keyword = "nilr" LeftArguments = [] RightArguments = [] Priority = 500 Class = "ExprResultList"
Keyword = ":" LeftArguments = [ExprResult] RightArguments = [ExprResultList] Priority = 910 Class = "ExprResultList"


Keyword = "if" LeftArguments = [] RightArguments = [BoolExpr Then Expr Else Expr] Priority = 500 Class = "Expr"
Keyword = "then" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Then"
Keyword = "else" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Else"

Keyword = "wait" LeftArguments = [] RightArguments = [<<float>>] Priority = 0 Class = "Expr"
Keyword = "waitResult" LeftArguments = [] RightArguments = [<<float>>] Priority = 0 Class = "ExprResult"

Keyword = "yield" LeftArguments = [] RightArguments = [ExprList] Priority = 0 Class = "Expr"
Keyword = "yieldResult" LeftArguments = [] RightArguments = [ExprResultList] Priority = 0 Class = "ExprResult"

Keyword = ";" LeftArguments = [Expr] RightArguments = [Expr] Priority = -10 Class = "Expr"
Keyword = "setDt" LeftArguments = [] RightArguments = [<<float>>] Priority = -10 Class = "ExprResult"

Keyword = ";'" LeftArguments = [ExprResult] RightArguments = [Expr] Priority = -10 Class = "ExprResult"

Keyword = "eval" LeftArguments = [] RightArguments = [<<float>> Locals Expr] Priority = -1000 Class = "Expr"
Keyword = "evalMany" LeftArguments = [] RightArguments = [<<float>> Locals ExprList] Priority = -1000 Class = "Expr"
Keyword = "stepOrSuspend " LeftArguments = [] RightArguments = [<<float>> Locals ExprResult Expr] Priority = -1000 Class = "Expr"

Keyword = "runTest1" LeftArguments = [] RightArguments = [] Priority = -10000 Class = "Test"


Id is Expr
BoolConst is BoolExpr
BoolExpr is Expr
IntConst is IntExpr
IntExpr is Expr
IntConst is ExprResult


v := <<M.GetKey(k)>>
---------------------
($m M) lookup k => v

M' := <<M.Add(k,v)>>
------------------------
($m M) add k v => $m M'

dt := 0.02
M := $m <<System.Collections.Immutable.ImmutableDictionary<string, Expr>.Empty>>
eval dt M ((wait 0.01); (yield (($i 1),nil))) => res
---------------------------------------------------------------------------------
runTest1 => res

  c != $b true
  c != $b false
  eval dt M c => c'
  eval dt M (if c' then t else e) => res
  ---------------------------------------
  eval dt M (if c then t else e) => res

  eval dt M t => res
  ----------------------------------------------
  eval dt M (if ($b true) then t else e) => res

  eval dt M e => res
  -----------------------------------------------
  eval dt M (if ($b false) then t else e) => res

  M lookup v => res
  ----------------------
  eval dt M ($v) => res

  <<dt >= t>> == true
  dt' := <<dt - t>>
  --------------------------------------
  eval dt M (wait t) => setDt dt'

  <<dt < t>> == true
  t' := <<t - dt>>
  ------------------------------------
  eval dt M (wait t) => waitResult t'

  ------------------------------
  eval dt M ($i val) => $i val
 
  evalMany dt M (e,exprs) => vals
  -------------------------------------------------
  eval dt M (yield (e,exprs)) => yieldResult vals

 
   es := e,exprs
   eval dt M e => val
   evalMany dt M exprs => vals
   res := val : vals
   ----------------------------------
   evalMany dt M (e,exprs) => val:vals

   ---------------------------
   evalMany dt M nil => nilr


  eval dt M a => a'
  stepOrSuspend dt M a' b => res
  -----------------------------------
  eval dt M (a; b) => res

    eval dt' M b => res
    ----------------------------------------
    stepOrSuspend dt M (setDt dt') b => res

    -----------------------------------------------------------
    stepOrSuspend dt M (waitResult t) b => (waitResult t) ;' b

    ---------------------------------------------------------------
    stepOrSuspend dt M (yieldResult vals) b => (yieldResult vals) ;' b

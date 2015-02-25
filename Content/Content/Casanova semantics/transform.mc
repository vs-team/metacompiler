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

Keyword = "evalRule" LeftArguments = [] RightArguments = [<<float>> Locals <<string>> Expr] Priority = 1000 Class = "Entity"

Keyword = "nil" GenericArguments = [t] LeftArguments = [] RightArguments = [] Priority = 0 Class = "List[t]"
Keyword = "," GenericArguments = [t] LeftArguments = [t] RightArguments = [List[t]] Priority = 1000 Class = "List[t]"

Keyword = "if" LeftArguments = [] RightArguments = [BoolExpr Then Expr Else Expr] Priority = 500 Class = "Expr"
Keyword = "then" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Then"
Keyword = "else" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Else"

Keyword = "wait" LeftArguments = [] RightArguments = [<<float>>] Priority = 0 Class = "Expr"
Keyword = "waitResult" LeftArguments = [] RightArguments = [<<float>>] Priority = 0 Class = "ExprResult"

Keyword = "yield" LeftArguments = [] RightArguments = [List[Expr]] Priority = 0 Class = "Expr"
Keyword = "yieldResult" LeftArguments = [] RightArguments = [List[ExprResult]] Priority = 0 Class = "ExprResult"

Keyword = ";" LeftArguments = [Expr] RightArguments = [Expr] Priority = -10 Class = "Expr"
Keyword = "setDt" LeftArguments = [] RightArguments = [<<float>>] Priority = -10 Class = "ExprResult"

Keyword = ";'" LeftArguments = [ExprResult] RightArguments = [Expr] Priority = -10 Class = "ExprResult"

Keyword = "eval" LeftArguments = [] RightArguments = [<<float>> Locals Expr] Priority = -1000 Class = "Expr"
Keyword = "evalMany" LeftArguments = [] RightArguments = [<<float>> Locals List[Expr]] Priority = -1000 Class = "Expr"
Keyword = "stepOrSuspend " LeftArguments = [] RightArguments = [<<float>> Locals ExprResult Expr] Priority = -1000 Class = "Expr"

Keyword = "runTest1" LeftArguments = [] RightArguments = [] Priority = -10000 Class = "Test"


Id is Expr
BoolConst is BoolExpr
BoolConst is ExprResult
BoolExpr is Expr
IntConst is IntExpr
IntConst is ExprResult
IntExpr is Expr
ExprResult is Expr


c != $b true
c != $b false
eval dt M c => c'
eval dt M (if c' then t else e) => res
---------------------------------------
eval dt M (if c then t else e) => res

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
   res := val,vals
   ----------------------------------
   evalMany dt M (e,exprs) => val,vals

   ---------------------------
   evalMany dt M nil => nil


  debug := <<EntryPoint.Print("---------")>>
  debug1 := <<EntryPoint.Print(a)>>
  eval dt M a => a1
  debug2 := <<EntryPoint.Print(a1)>>
  stepOrSuspend dt M a1 b => res
  debug3 := <<EntryPoint.Print(b)>>
  debug1000 := <<EntryPoint.Print("++++++++")>>
  -----------------------------------
  eval dt M (a; b) => res

    eval dt' M b => res
    ----------------------------------------
    stepOrSuspend dt M (setDt dt') b => res

    res := (waitResult t) ;' b
    <<EntryPoint.Print(res.ToString())>>
    -----------------------------------------------------------
    stepOrSuspend dt M (waitResult t) b => res

    ---------------------------------------------------------------
    stepOrSuspend dt M (yieldResult vals) b => (yieldResult vals) ;' b

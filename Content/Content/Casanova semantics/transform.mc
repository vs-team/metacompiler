Keyword [Locals] "add" [<<string>> <<ExprResult>>] Priority 1000 Class Locals
Keyword [Locals] "lookup" [<<string>>] Priority 1000 Class MemoryOp
Keyword [] "$m" [<<System.Collections.Immutable.ImmutableDictionary<string, ExprResult> >>] Priority 10000 Class Locals

Keyword [] "$" [<<string>>] Priority 10000 Class Id

Keyword [] "$b" [<<bool>>] Priority 10000 Class BoolConst
Keyword [IntExpr] ">" [IntExpr] Priority 1100 Class BoolExpr
Keyword [IntExpr] "=" [IntExpr] Priority 1100 Class BoolExpr
Keyword [BoolExpr] "&&" [BoolExpr] Priority 1000 Class BoolExpr
Keyword [BoolExpr] "||" [BoolExpr] Priority 900 Class BoolExpr

Keyword [] "$i" [<<int>>] Priority 10000 Class IntConst
Keyword [IntExpr] "*" [IntExpr] Priority 910 Class IntExpr
Keyword [IntExpr] "/" [IntExpr] Priority 910 Class IntExpr
Keyword [IntExpr] "+" [IntExpr] Priority 900 Class IntExpr
Keyword [IntExpr] "-" [IntExpr] Priority 900 Class IntExpr

Keyword [] "nil" [] Priority 500 Class ExprList
Keyword [Expr] ";" [ExprList] Priority 910 Class ExprList

Keyword [] "nilResult" [] Priority 500 Class ExprResultList
Keyword [ExprResult] "consResult" [ExprResultList] Priority 910 Class ExprResultList


Keyword [] "if" [BoolExpr Then ExprList Else ExprList] Priority 500 Class Expr
Keyword [] "then" [] Priority 0 Class Then
Keyword [] "else" [] Priority 0 Class Else

Keyword [] "wait" [<<float>>] Priority 0 Class Expr
Keyword [] "waitResult" [<<float>>] Priority 0 Class ExprResult

Keyword [] "yield" [ExprList] Priority 0 Class Expr
Keyword [] "yieldResult" [ExprResultList] Priority 0 Class ExprResult

Keyword [] "setDt" [<<float>>] Priority -10 Class ExprResult

Keyword [] "eval" [<<float>> Locals Expr] Priority -1000 Class Expr
Keyword [] "evalMany" [<<float>> Locals ExprList] Priority -1000 Class Expr
Keyword [] "stepOrSuspend" [<<float>> Locals ExprResult Expr] Priority -1000 Class Expr

Keyword [] "continueWith" [ExprList] Priority -1000 Class ExprResult
Keyword [] "updateFieldsAndContinueWith" [ExprResultList ExprList] Priority -1000 Class ExprResult


Keyword [] "runTest1" [] Priority -10000 Class Test


Id is Expr
BoolConst is BoolExpr
BoolConst is ExprResult
BoolExpr is Expr
IntConst is IntExpr
IntConst is ExprResult
IntExpr is Expr
ExprResult is Expr
ExprList is Expr


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
M := $m <<System.Collections.Immutable.ImmutableDictionary<string, ExprResult>.Empty>>
y1 := yield (($i 1);nil)
y2 := yield (($i 2);nil)
eval dt M (y1;(y2;nil)) => res
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

  --------------------------
  eval dt M nil => nilResult
 
  debug0 := <<EntryPoint.Print("Evaluating yield...")>>
  debug1 := <<EntryPoint.Print(e)>>
  debug1b := <<EntryPoint.Print(exprs)>>
  evalMany dt M (e;exprs) => vals
  debug2 := <<EntryPoint.Print(vals)>>
  -------------------------------------------------
  eval dt M (yield (e;exprs)) => yieldResult vals

   debug1 := <<EntryPoint.Print("Evaluating yield arguments...")>>
   eval dt M e => val
   debug2 := <<EntryPoint.Print(val)>>
   evalMany dt M exprs => vals
   debug3 := <<EntryPoint.Print(vals)>>
   res := val consResult vals
   ----------------------------------------------
   evalMany dt M (e;exprs) => val consResult vals

   -------------------------------
   evalMany dt M nil => nilResult


  eval dt M a => a'
  stepOrSuspend dt M a' b => res
  -----------------------------------
  eval dt M (a; b) => res

    eval dt' M b => res
    ----------------------------------------
    stepOrSuspend dt M (setDt dt') b => res


    res := continueWith((wait t);b) 
    -----------------------------------------------------------
    stepOrSuspend dt M (waitResult t) b => res

    res := updateFieldsAndContinueWith vals b
    ---------------------------------------------------------------
    stepOrSuspend dt M (yieldResult vals) b => res

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

Keyword [] "rule" [Domain ExprList] Priority 0 Class RuleResult

Keyword [] "updateFields" [Locals Domain ExprResultList] Priority 0 Class MemoryOp

Keyword [] "suspend" [] Priority 0 Class ExprResult

Keyword [] "updateRules" [<<float>> Locals RuleList RuleList] Priority -1000 Class RuleManager
Keyword [] "loopRules" [<<float>> Locals RuleList RuleList <<int>>] Priority -1000 Class RuleManager

Keyword [] "entity" [<<string>> Locals RuleList] Priority 0 Class Entity
Keyword [] "updateEntity" [<<float>> Entity <<int>>] Priority -1000 Class EntityManager

Keyword [] "nilRule" [] Priority 500 Class RuleList
Keyword [Rule] "consRule" [RuleList] Priority 910 Class RuleList 
Keyword [] "rule" [Domain ExprList] Priority 0 Class Rule
Keyword [] "updateResult" [Locals RuleList] Priority -1000 Class UpdateResult

Keyword [] "setDt" [<<float>>] Priority -10 Class ExprResult

Keyword [] "nilDomain" [] Priority 500 Class Domain
Keyword [<<string>>] "consDomain" [Domain] Priority 910 Class Domain

Keyword [] "eval" [<<float>> Locals Expr] Priority -1000 Class Expr
Keyword [] "evalMany" [<<float>> Locals ExprList] Priority -1000 Class Expr
Keyword [] "evalRule" [<<float>> Locals Domain ExprList ExprList] Priority -1000 Class RuleEvaluation
Keyword [] "stepOrSuspend" [<<float>> Locals ExprResult Expr] Priority -1000 Class Expr
Keyword [] "ruleResult" [Locals ExprList] Priority -1000 Class RuleResult

Keyword [] "suspendResult" [] Priority -1000 Class ExprResult
Keyword [] "continueWith" [ExprList] Priority -1000 Class ExprResult
Keyword [] "atomic" [] Priority -1000 Class ExprResult
Keyword [] "reEvaluate" [ExprList] Priority -1000 Class ExprResult
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

<<M.ContainsKey(k)>> == false
M' := <<M.Add(k,v)>>
------------------------
($m M) add k v => $m M'

<<M.ContainsKey(k)>> == true
M' := <<M.SetItem(k,v)>>
------------------------
($m M) add k v => $m M'



dt := 1.0
Me := $m <<System.Collections.Immutable.ImmutableDictionary<string, ExprResult>.Empty>>
Me add "F1" ($i 100) => Ml
Ml add "F2" ($i 100) => M
dom1 := "F1" consDomain nilDomain
dom2 := "F2" consDomain nilDomain
f1 := (($i 90);(($i 50);nil))
w1 := wait 3.0
w2 := wait 2.0
y1 := yield(($i 10);nil)
y2 := yield(($i 20);nil)
b1 := w1;(y1;nil)
b2 := w2;(y2;nil)
r1 := rule dom1 b1
r2 := rule dom2 b2
rs := r1 consRule (r2 consRule nilRule)
e := entity "E" M rs
updateEntity dt e 10 => res
debug := <<EntryPoint.Print("Done!")>>
------------------------------------------------------------
runTest1 => res

  loopRules dt fields rs rs updates => updateResult fields' rs'
  --------------------------------------------------------------------------
  updateEntity dt (entity name fields rs) updates => entity name fields' rs'

  <<i > 0>> == true
  outputString := <<"\n----------------\n" + (fields.ToString()) + "\n\n" + (rs.ToString()) + "\n----------------\n">>
  outputUpdate := <<EntryPoint.Print(outputString)>>
  sleeping := <<EntryPoint.Sleep(dt)>>
  updateRules dt fields startingRules rs => updateResult updatedFields updatedRules
  j := <<i - 1>>
  loopRules dt updatedFields startingRules updatedRules j => updateResult fs' rs'
  ----------------------------------------------------------------------------------
  loopRules dt fields startingRules rs i => updateResult fs' rs'

  ----------------------------------------------------------
  loopRules dt fields startingRules rs 0 => updateResult fields rs


  evalRule dt M domain sb block => ruleResult M' continuation
  updatedRule := rule domain continuation
  updateRules dt M' startingRules rs => updateResult M'' rs'
  ------------------------------------------------------------------------------------------
  updateRules dt M ((rule sd sb) consRule startingRules) ((rule domain block) consRule rs) => updateResult M'' (updatedRule consRule rs')

  ----------------------------------------------------------
  updateRules dt M rs nilRule => updateResult M nilRule

  eval dt M block => continueWith((wait t); b)
  ---------------------------------------------------------------------
  evalRule dt M domain startingBlock block => ruleResult M ((wait t);b) 

  eval dt M block => (updateFieldsAndContinueWith vals b)
  updateFields M domain vals => M'
  -----------------------------------------------------------
  evalRule dt M domain startingBlock block => ruleResult M' b

    M add field v => M'
    updateFields M' fields vals => M''
    -----------------------------------------------------------------------
    updateFields M (field consDomain fields) (v consResult vals) => M''

    ---------------------------------------
    updateFields M nilDomain nilResult => M

  eval dt M block => reEvaluate b
  evalRule dt M domain startingBlock b => res
  ---------------------------------------------------------------------------------
  evalRule dt M domain startingBlock block => res

  ---------------------------------------------------------------------
  evalRule dt M domain startingBlock nil => ruleResult M startingBlock

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
  -------------------------------
  eval dt M (wait t) => atomic

  <<dt < t>> == true
  t' := <<t - dt>>
  ------------------------------------
  eval dt M (wait t) => waitResult t'

  ------------------------------
  eval dt M ($i val) => $i val

  --------------------------
  eval dt M nil => nilResult
 
  evalMany dt M (e;exprs) => vals
  -------------------------------------------------
  eval dt M (yield (e;exprs)) => yieldResult vals

    eval dt M e => val
    evalMany dt M exprs => vals
    res := val consResult vals
    ----------------------------------------------
    evalMany dt M (e;exprs) => val consResult vals

    -------------------------------
    evalMany dt M nil => nilResult


  eval dt M a => a1
  stepOrSuspend dt M a1 b => res
  -----------------------------------
  eval dt M (a; b) => res

    res := reEvaluate b
    ----------------------------------------
    stepOrSuspend dt M atomic b => res

    res := reEvaluate b
    ----------------------------------------
    stepOrSuspend dt M nilResult b => res

    res := continueWith((wait t);b)
    -------------------------------------------
    stepOrSuspend dt M (waitResult t) b => res

    res := updateFieldsAndContinueWith vals b
    ----------------------------------------------
    stepOrSuspend dt M (yieldResult vals) b => res

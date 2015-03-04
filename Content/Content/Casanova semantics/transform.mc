Keyword [Locals] "add" [<<string>> <<ExprResult>>] Priority 1000 Class Locals
Keyword [Locals] "lookup" [<<string>>] Priority 1000 Class MemoryOp
Keyword [] "$m" [<<System.Collections.Immutable.ImmutableDictionary<string, ExprResult> >>] Priority 10000 Class Locals

Keyword [] "$" [<<string>>] Priority 10000 Class Id

Keyword [] "$b" [<<bool>>] Priority 10000 Class BoolConst
Keyword [IntExp] ">" [IntExp] Priority 1100 Class BoolExpr
Keyword [IntExp] "=" [IntExp] Priority 1100 Class BoolExpr
Keyword [BoolExpr] "&&" [BoolExpr] Priority 1000 Class BoolExpr
Keyword [BoolExpr] "||" [BoolExpr] Priority 900 Class BoolExpr

Keyword [] "$i" [<<int>>] Priority 10000 Class IntConst
Keyword [IntExp] "*" [IntExp] Priority 910 Class IntExp
Keyword [IntExp] "/" [IntExp] Priority 910 Class IntExp
Keyword [IntExp] "+" [IntExp] Priority 900 Class IntExp
Keyword [IntExp] "-" [IntExp] Priority 900 Class IntExp

Keyword [] "let" [Id Expr] Priority 500 Class Expr
Keyword [] "letResult" [Id ExprResult] Priority 0 Class ExprResult
Keyword [] "binding" [Id ExprResult ExprList] Priority -1000 Class ExprResult

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

Keyword [] "updateFields" [Locals Domain ExprResultList] Priority 0 Class MemoryOp

Keyword [] "suspend" [] Priority 0 Class ExprResult

Keyword [] "updateRules" [<<float>> Locals RuleList RuleList] Priority -1000 Class RuleManager
Keyword [] "loopRules" [<<float>> Locals RuleList RuleList <<int>>] Priority -1000 Class RuleManager

Keyword [] "entity" [<<string>> Locals RuleList] Priority 0 Class Entity
Keyword [] "updateEntity" [<<float>> Entity <<int>>] Priority -1000 Class EntityManager

Keyword [] "nilRule" [] Priority 500 Class RuleList
Keyword [Rule] "consRule" [RuleList] Priority 910 Class RuleList 
Keyword [] "rule" [Domain ExprList Locals] Priority 0 Class Rule
Keyword [] "updateResult" [Locals RuleList] Priority -1000 Class UpdateResult

Keyword [] "setDt" [<<float>>] Priority -10 Class ExprResult

Keyword [] "nilDomain" [] Priority 500 Class Domain
Keyword [<<string>>] "consDomain" [Domain] Priority 910 Class Domain

Keyword [] "eval" [<<float>> Locals Locals Expr] Priority -1000 Class Expr
Keyword [] "evalMany" [<<float>> Locals Locals ExprList] Priority -1000 Class Expr
Keyword [] "evalRule" [<<float>> Locals Locals Domain ExprList ExprList] Priority -1000 Class RuleEvaluation
Keyword [] "stepOrSuspend" [<<float>> Locals ExprResult Expr] Priority -1000 Class Expr
Keyword [] "ruleResult" [Locals Locals ExprList] Priority -1000 Class RuleResult

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
IntConst is IntExp
IntConst is ExprResult
IntExp is Expr
ExprResult is Expr
ExprList is Expr


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
iterations := 10
Me := $m <<System.Collections.Immutable.ImmutableDictionary<string, ExprResult>.Empty>>
Me add "F1" ($i 100) => Ml
Ml add "F2" ($i 100) => M
L1 := $m <<System.Collections.Immutable.ImmutableDictionary<string, ExprResult>.Empty>>
L2 := $m <<System.Collections.Immutable.ImmutableDictionary<string, ExprResult>.Empty>>
dom1 := "F1" consDomain nilDomain
dom2 := "F2" consDomain nilDomain
f1 := (($i 90);(($i 50);nil))
l1 := let $"x" $i 100
w1 := wait 2.0
w2 := wait 2.0
y1 := yield(($i 10 + $i 30);nil)
y2 := yield(($i 20);nil)
y3 := yield($"x";nil)
b1 := l1;(w1;(y1;(w1;(y3;nil))))
b2 := w2;(y2;nil)
r1 := rule dom1 b1 L1
r2 := rule dom2 b2 L2
rs := r1 consRule nilRule
e := entity "E" M rs
updateEntity dt e iterations => res
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


  evalRule dt M localVariables domain sb block => ruleResult M' localVariables' continuation
  updatedRule := rule domain continuation localVariables'
  updateRules dt M' startingRules rs => updateResult M'' rs'
  --------------------------------------------------------------------------------------------------
  updateRules dt M ((rule sd sb sl) consRule startingRules) ((rule domain block localVariables) consRule rs) => updateResult M'' (updatedRule consRule rs')

  ----------------------------------------------------------
  updateRules dt M rs nilRule => updateResult M nilRule

  eval dt M lv block => continueWith((wait t); b)
  ---------------------------------------------------------------------
  evalRule dt M lv domain startingBlock block => ruleResult M lv ((wait t);b) 

  eval dt M lv block => (updateFieldsAndContinueWith vals b)
  updateFields M domain vals => M'
  -------------------------------------------------------------------
  evalRule dt M lv domain startingBlock block => ruleResult M' lv b

    M add field v => M'
    updateFields M' fields vals => M''
    -----------------------------------------------------------------------
    updateFields M (field consDomain fields) (v consResult vals) => M''

    ---------------------------------------
    updateFields M nilDomain nilResult => M


  eval dt M lv block => binding ($varName) val b
  lv add varName val => lv'
  evalRule dt M lv' domain startingBlock b => res
  -----------------------------------------------------
  evalRule dt M lv domain startingBlock block => res

  eval dt M lv block => reEvaluate b
  evalRule dt M lv domain startingBlock b => res
  ---------------------------------------------------------------------------------
  evalRule dt M lv domain startingBlock block => res

  ---------------------------------------------------------------------
  evalRule dt M lv domain startingBlock nil => ruleResult M lv startingBlock

  eval dt M lv exp => exp'
  ---------------------------------------------
  eval dt M lv (let id exp) => letResult id exp'

  c != $b true
  c != $b false
  eval dt M lv c => c'
  eval dt M lv (if c' then t else e) => res
  ---------------------------------------
  eval dt M lv (if c then t else e) => res

  eval dt M lv t => res
  ----------------------------------------------
  eval dt M lv (if ($b true) then t else e) => res

  eval dt M lv e => res
  -----------------------------------------------
  eval dt M lv (if ($b false) then t else e) => res

  eval dt M lv e1 => ($i res1)
  eval dt M lv e2 => ($i res2)
  res := <<res1 + res2>>
  -----------------------------------------------
  eval dt lv M (e1 + e2) => ($i res)

  eval dt M lv e1 => ($i res1)
  eval dt M lv e2 => ($i res2)
  res := <<res1 - res2>>
  --------------------------------
  eval dt M lv (e1 - e2) => ($i res)

  <<dt >= t>> == true
  -------------------------------
  eval dt M lv (wait t) => atomic

  <<dt < t>> == true
  t' := <<t - dt>>
  ------------------------------------
  eval dt M lv (wait t) => waitResult t'

  ------------------------------
  eval dt M lv ($i val) => $i val

  lv lookup id => val
  -------------------------
  eval dt M lv ($ id) => val

  --------------------------
  eval dt M lv nil => nilResult
 
  evalMany dt M lv (e;exprs) => vals
  -------------------------------------------------
  eval dt M lv (yield (e;exprs)) => yieldResult vals

    eval dt M lv e => val
    evalMany dt M lv exprs => vals
    res := val consResult vals
    ----------------------------------------------
    evalMany dt M lv (e;exprs) => val consResult vals

    -------------------------------
    evalMany dt M lv nil => nilResult


  eval dt M lv a => a1
  stepOrSuspend dt M a1 b => res
  -----------------------------------
  eval dt M lv (a; b) => res

    res := reEvaluate b
    ----------------------------------------
    stepOrSuspend dt M atomic b => res

    res := binding id exp b
    ------------------------------------------------
    stepOrSuspend dt M (letResult id exp) b => res

    res := reEvaluate b
    ----------------------------------------
    stepOrSuspend dt M nilResult b => res

    res := continueWith((wait t);b)
    -------------------------------------------
    stepOrSuspend dt M (waitResult t) b => res

    res := updateFieldsAndContinueWith vals b
    ----------------------------------------------
    stepOrSuspend dt M (yieldResult vals) b => res

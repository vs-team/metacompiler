include Content.CNV3.Basics.mc


import System.Collections.Immutable
import System.Threading

Data "=" : EQ                                                                                 Priority 5
Data "then" : Then
Data "else" : Else

Data "nop" : stmt                                                                             Priority 100
Data stmt -> ";" -> stmt : stmt                                                               Priority 50
Data "wait" -> Expr : stmt                                                                    Priority 100
Data "when" -> Expr : stmt                                                                    Priority 100
Data "let" -> ID -> EQ -> Expr : Let                                                          Priority 100
Data "if" -> Expr -> Then -> stmt -> Else -> stmt : stmt                                      Priority 100
Data "while" -> Expr -> stmt : stmt                                                           Priority 100
Data "for" -> ID -> In -> Expr -> stmt : stmt                                                 Priority 100
Data "yield" -> List[Expr] : stmt                                                             Priority 100
Data "rule" -> List[<<string>>] -> stmt -> stmt -> <<ImmutableDictionary<string, Value> >>  -> <<float>> : Rule     Priority 20

Data "unit" : Unit
Data "State" List[Rule] -> <<ImmutableDictionary<string, Value> >> -> <<ImmutableDictionary<string, Value> >> : GameState   Priority 10
Func "evalRule" -> Rule -> <<ImmutableDictionary<string, Value> >> -> <<ImmutableDictionary<string, Value> >> : ExecutionRule  => ExecutionResult   Priority 10
Func "tick" -> List[Rule] -> List[Rule] -> <<ImmutableDictionary<string, Value> >> -> <<ImmutableDictionary<string, Value> >> -> <<float>> : ticker => GameState                      Priority 10
Func "loopRules" -> List[Rule] -> List[Rule] -> <<int>> -> <<ImmutableDictionary<string, Value> >> -> <<ImmutableDictionary<string, Value> >> -> <<float>> : RuleLooper => GameState  Priority 10

Func "addStmt" -> stmt -> stmt : AddStmt => stmt

Data "Done" -> ctxt : ExecutionResult
Data "Suspend" -> stmt -> ctxt : ExecutionResult                              Priority 7
Data "Yield" -> stmt -> List[Value] -> ctxt : ExecutionResult
Data "Resume" -> stmt -> ctxt : ExecutionResult
Data "Atomic" -> stmt -> ctxt : ExecutionResult

Func "eval_s" -> stmt -> stmt -> ctxt -> <<float>> : Execution => ExecutionResult
Func "evalQueryBindings" -> List[Let] -> ctxt : BindingsEvaluator => ctxt  Priority 10
Func "evalYield" -> List[Expr] -> ctxt : YieldEvaluation => List[Value]
Func <<ImmutableDictionary<string, Value> >> -> "add" -> <<string>> -> Value : DictionaryOp => <<ImmutableDictionary<string, Value> >>    Priority 10
Func "updateFields" -> List[<<string>>] -> List[Value] -> ctxt : MemoryOp => ctxt   Priority 10

Data "from" -> ID -> In -> Expr -> List[Let] -> Where -> Select : Expr           Priority 100
Data "in" : In                                                                   Priority 1000
Data "where" -> Expr : Where                                                     Priority 1000
Data "select" -> Expr: Select                                                    Priority 1000

Func "testStatement" : Test => GameState

Let is stmt



------------------------------
evalQueryBindings nil m => m

eval expr (Context locals fields globals) => val
locals add id val => updatedLocals
evalQueryBindings bs (Context updatedLocals fields globals) => res
----------------------------------------------------------------------------------
evalQueryBindings ((let $ id = expr)::bs) (Context locals fields globals) => res


--------------------------------------------------------------------------------------------
eval (from ($ id) in ($l nil) bindings (where condition) (select se)) m => $l nil



eval listExpr (Context locals fields globals) => $l (x::xs)
locals add id x => updatedLocals
evalQueryBindings bindings (Context updatedLocals fields globals) => memoryAfterBindings
eval condition memoryAfterBindings => $b false
eval from $ id in $l xs bindings (where condition) (select se) (Context locals fields globals) => res
--------------------------------------------------------------------------------------------------
eval (from $ id in listExpr bindings (where condition) (select se)) (Context locals fields globals) => res


eval listExpr (Context locals fields globals) => $l (x::xs)
locals add id x => updatedLocals
evalQueryBindings bindings (Context updatedLocals fields globals) => memoryAfterBindings
eval condition memoryAfterBindings => $b true
eval se memoryAfterBindings => sv
eval from $ id in $l xs bindings (where condition) (select se) (Context locals fields globals) => $l res
---------------------------------------------------------------------------------------------------
eval (from $ id in listExpr bindings (where condition) (select se)) (Context locals fields globals) => $l (sv::res)


<<d.SetItem(k,v)>> => dict
------------------------
d add k v => dict


eval cond ctxt => $b true
--------------------------------------------------------------------------
eval_s (if cond then b else c) k ctxt dt => Atomic b;k ctxt

eval cond ctxt => $b false
--------------------------------------------------------------------------
eval_s (if cond then b else c) k ctxt dt => Atomic c;k ctxt



eval cond ctxt => $b true
--------------------------------------------------------------------
eval_s (while cond b) k ctxt dt => Atomic b;((while cond b);k) ctxt

eval cond ctxt => $b false
--------------------------------------------------------------------
eval_s (while cond b) k ctxt dt => Atomic k ctxt


eval expr ctxt => ($l nil)
------------------------------------------
eval_s (for v in expr b) k ctxt dt => Atomic k ctxt

eval expr (Context locals e w) => ($l (x :: xs))
locals add var x => updatedLocals
----------------------------------------------------------------
eval_s (for ($ var) in expr b) k (Context locals e w) dt => Atomic b;((for ($ var) in ($l xs) b);k) (Context updatedLocals e w)


eval (b) (Context locals e w) => c
<<locals.SetItem(a, c)>> => res
---------------------------------
eval_s (let ($a) = b) k (Context locals e w) dt => Atomic k (Context res e w)

eval expr ctxt => ($f t)
<<t <= dt>> == false
----------------------------------
eval_s (wait expr) k ctxt dt => Suspend (wait $f (<<t - dt>>));k ctxt

eval expr ctxt => ($f t)
<<t <= dt>> == true
----------------------------------
eval_s (wait expr) k ctxt dt => Resume k ctxt


eval expr ctxt => ($b true)
---------------------------------------------
eval_s (when expr) k ctxt dt => Atomic k ctxt

eval expr ctxt => ($b false)
------------------------------------------
eval_s (when expr) k ctxt dt => Suspend (when expr);k ctxt


-------------------------
evalYield nil ctxt => nil

eval expr ctxt => v
evalYield exprs ctxt => vs
-------------------------------------------
evalYield (expr :: exprs) ctxt => v :: vs


evalYield exprs ctxt => values
------------------------------------------------------
eval_s (yield exprs) k ctxt dt => Yield k values ctxt


a != nop
---------------------
addStmt a b => a;b

-------------------
addStmt nop nop => nop

addStmt b k => cont
eval_s a cont ctxt dt => res
-------------------------------
eval_s (a;b) k ctxt dt => res


-------------------------------
eval_s nop nop ctxt dt => Done ctxt

eval_s b k (Context locals fields globals) dt => Atomic z (Context newLocals newFields newGlobals)
evalRule (rule dom z nop newLocals dt) newFields newGlobals => res
-------------------------------------------------------------------
evalRule (rule dom b k locals dt) fields globals  => res

eval_s b k (Context locals fields globals) dt => Done context
------------------------------------------------
evalRule (rule dom b k locals dt) fields globals => Done context

eval_s b k (Context locals fields globals) dt => Suspend ks context
-------------------------------------------------------------------
evalRule (rule dom b k locals dt) fields globals  => Suspend ks context

eval_s b k (Context locals fields globals) dt => Resume ks context
--------------------------------------------------------------
evalRule (rule dom b k locals dt) fields globals  => Resume ks context

eval_s b k (Context locals fields globals) dt => Yield ks values context
updateFields dom values context  => updatedContext
--------------------------------------------------------------------------------------
evalRule (rule dom b k locals dt) fields globals => Yield ks values updatedContext


----------------------------------------
updateFields nil nil context => context

eval y c => $Vector3 v
<< x.Position = v >> => u
updateFields fs vs c => updatedContext
-------------------------------------------------------------------------------------
updateFields (f :: fs) (($wrapperSet x y) :: vs) c => updatedContext

e add f v => updatedEntity
updateFields fs vs (Context l updatedEntity w) => updatedContext
-------------------------------------------------------------------------------------
updateFields (f :: fs) (v :: vs) (Context l e w) => updatedContext

evalRule r fields globals => Done (Context newLocals newFields newGlobals)
tick originals rs newFields newGlobals dt => (State updatedRules updatedFields updatedGlobals)
st := State (original::updatedRules) updatedFields updatedGlobals
-------------------------------------------------------------------------------------
tick (original::originals) (r::rs) fields globals dt => st

evalRule (rule dom body k locals delta) fields globals => Suspend (s;cont) (Context newLocals newFields newGlobals)
r := rule dom s cont newLocals dt
tick originals rs newFields newGlobals dt => (State updatedRules updatedFields updatedGlobals)
st := State (r::updatedRules) updatedFields updatedGlobals
------------------------------------------------------
tick (original::originals) ((rule dom body k locals delta)::rs) fields globals dt => st


evalRule (rule dom body k locals delta) fields globals => Yield cont values (Context newLocals newFields newGlobals)
r := rule dom cont nop newLocals dt
tick originals rs newFields newGlobals dt => (State updatedRules updatedFields updatedGlobals)
st := State (r::updatedRules) updatedFields updatedGlobals
------------------------------------------------------
tick (original::originals) ((rule dom body k locals delta)::rs) fields globals dt => st

evalRule (rule dom body k locals delta) fields globals => Resume cont (Context newLocals newFields newGlobals)
r := rule dom cont nop newLocals dt
tick originals rs newFields newGlobals dt => (State updatedRules updatedFields updatedGlobals)
st := State (r::updatedRules) updatedFields updatedGlobals
------------------------------------------------------
tick (original::originals) ((rule dom body k locals delta)::rs) fields globals dt => st


------------------------------------------------------------
tick nil nil fields globals dt => (State nil fields globals)



tick original rs fields globals dt => (State nrs newFields newGlobals)
st := State nrs newFields newGlobals
steps > 0
<<Console.WriteLine(st)>>
<<Thread.Sleep((int)(dt * 1000))>>
<<Console.WriteLine("-------------------------")>>
loopRules original nrs <<steps - 1>> newFields newGlobals dt => s
-----------------------------------------------------------
loopRules original rs steps fields globals dt => s

tick original rs fields globals dt => s
<<Thread.Sleep((int)(dt * 1000))>>
----------------------------------------------
loopRules original rs 0 fields globals dt => s


vx := $Vector3 <<new Vector3(1.0,0.0,0.0)>>
vy := $Vector3 << new Vector3(0.0,1.0,0.0) >>
<< new Vector3(0.0,0.0,1.0) >> => p
<< WrapperTest.Instantiate(p) >> => wt
s1 := yield (($wrapperSet wt ($"Base" + vx))::nil)
s2 := when ((vectorx $"Base") lt $f 30.0)
<<ImmutableDictionary<string, Value>.Empty>> add "Base" ($wrapper wt) => dict
c := Context <<ImmutableDictionary<string, Value>.Empty>> dict <<ImmutableDictionary<string, Value>.Empty>>
r1 := rule ("Base" :: nil) (s1;s2) nop <<ImmutableDictionary<string, Value>.Empty>> 0.1
tick (r1::nil) (r1::nil) dict <<ImmutableDictionary<string, Value>.Empty>> 0.1  => res
--------------------------
testStatement => res



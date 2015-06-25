include Content.CNV3.Basics.mc
include Content.CNV3.Tuples.mc
include Content.GenericLists.transform.mc

import System.Collections.Immutable
import System.Threading

Data "=" : EQ
Data "then" : Then
Data "else" : Else

Data "nop" : stmt                                                                             Priority 100
Data stmt -> ";" -> stmt : stmt                                                               Priority 50
Data "wait" -> <<float>> : stmt                                                               Priority 100
Data "let" -> ID -> EQ -> Expr : stmt                                                         Priority 100
Data "if" -> Expr -> Then -> stmt -> Else -> stmt : stmt                                      Priority 100
Data "while" -> Expr -> stmt : stmt                                                           Priority 100
Data "yield" -> List[Expr] : stmt                                                             Priority 100
Data "rule" List[<<string>>] -> stmt -> stmt -> ctxt -> <<float>> : Rule                                          Priority 20

Data "unit" : Unit
Func "run" : runnable => List[ExecutionResult]
Func "evalRule" -> Rule : ExcecutionRule => ExecutionResult                                   Priority 10
Func "tick" -> List[Rule] -> <<float>> : ticker => List[ExecutionResult]                      Priority 10
Func "loopRules" -> List[Rule] -> List[Rule] -> <<int>> -> <<float>> : RuleLooper => List[ExecutionResult]  Priority 10

Func "addStmt" -> stmt -> stmt : AddStmt => stmt


Func "rebuild" -> List[Rule] -> List[Rule] -> List[ExecutionResult] -> <<float>> : Rebuild => List[Rule]                  Priority 10

Data "Done" -> ctxt : ExecutionResult
Data "Suspend" -> stmt -> ctxt : ExecutionResult                              Priority 7
Data "Yield" -> stmt -> List[Value] -> ctxt : ExecutionResult
Data "Resume" -> stmt -> ctxt : ExecutionResult
Data "Atomic" -> stmt -> ctxt : ExecutionResult

Func "eval_s" -> stmt -> stmt -> ctxt -> <<float>> : Execution => ExecutionResult
Func "evalYield" -> List[Expr] -> ctxt : YieldEvaluation => List[Value]
Func <<ImmutableDictionary<string, Value> >> -> "add" -> <<string>> -> Value : DictionaryOp => <<ImmutableDictionary<string, Value> >>    Priority 10
Func "updateFields" -> List[<<string>>] -> List[Value] -> ctxt : MemoryOp => ctxt   Priority 10

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


eval (b) (Context locals entity world) => c
<<locals.SetItem(a, c)>> => res
---------------------------------
eval_s (let ($a) = b) k (Context locals entity world) dt => Atomic k (Context res entity world)

<<t <= dt>> == false
----------------------------------
eval_s (wait t) k ctxt dt => Suspend wait <<t - dt>>;k ctxt

<<t <= dt>> == true
----------------------------------
eval_s (wait t) k ctxt dt => Resume k ctxt


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
eval_s a (cont) ctxt dt => res
-------------------------------
eval_s (a;b) k ctxt dt => res


-------------------------------
eval_s nop nop ctxt dt => Done ctxt

eval_s b k ctxt dt => Atomic z c
evalRule (rule dom z nop c dt) => res
-------------------------------
evalRule (rule dom b k ctxt dt)  => res

eval_s b k ctxt dt => Done context
-------------------------------
evalRule (rule dom b k ctxt dt)  => Done context

eval_s b k ctxt dt => Suspend ks context
-------------------------------
evalRule (rule dom b k ctxt dt)  => Suspend ks context

eval_s b k ctxt dt => Resume ks context
-------------------------------
evalRule (rule dom b k ctxt dt)  => Resume ks context

eval_s b k (Context locals entity world) dt => Yield ks values context
updateFields dom values context  => updatedContext
--------------------------------------------------------------------------------------
evalRule (rule dom b k (Context locals entity world) dt)  => Yield ks values updatedContext


----------------------------------------
updateFields nil nil context => context

entity add f v => updatedEntity
updateFields fs vs (Context l updatedEntity world) => updatedContext
-------------------------------------------------------------------------------------
updateFields (f :: fs) (v :: vs) (Context l entity world) => updatedContext

evalRule a => b
tick as dt => res
------------------------------------------------------
tick a::as dt => b::res

---------------
tick nil dt => nil

---------------------------
rebuild nil nil nil dt => nil

rebuild q rs b dt => res
-------------------------
rebuild ((rule dom body nop context delta)::q) (r::rs) ((Done cont)::b) dt => (rule dom body nop cont dt)::res

rebuild ms q b dt => res
-------------------------
rebuild (m::ms) ((rule dom body k context delta)::q) ((Suspend (s;cont) updatedContext)::b) dt => (rule dom s cont updatedContext dt)::res

rebuild ms q b dt => res
-------------------------
rebuild (m::ms) ((rule dom body k context delta)::q) ((Yield cont values updatedContext)::b) dt => (rule dom cont nop updatedContext dt)::res

rebuild ms q b dt => res
-------------------------
rebuild (m::ms) ((rule dom body k context delta)::q) ((Resume cont updatedContext)::b) dt => (rule dom cont nop updatedContext dt)::res



tick rs dt => temp
rebuild original rs temp dt => nrs
steps > 0
loopRules original nrs <<steps - 1>> dt => res
--------------------------------------------
loopRules original rs steps dt => res

tick rs dt => fin
--------------------------
loopRules original rs 0 dt => fin


p12 := if ($b true) then (wait 4.0;yield (($i 5)::($i 0)::nil)) else (wait 2.0)
p1 := wait 3.0
p2 := wait 2.0
p3 := let $"x" = $i 10
p4 := yield (($"Test" + $i 5)::($"X" + $i 1)::nil)
p5 := while ($b true) (wait 1.0;p4)
<<ImmutableDictionary<string, Value>.Empty>> add "Test" ($i 0) => dd
dd add "X" ($i 1) => dict
temporaryrule := rule ("Test" :: "X" :: nil) (p5) nop (Context (<<ImmutableDictionary<string, Value>.Empty>>) dict (<<ImmutableDictionary<string, Value>.Empty>>)) 0.016
typorule := rule ("test" :: nil) (p2) nop (Context (<<ImmutableDictionary<string, Value>.Empty>>) dict (<<ImmutableDictionary<string, Value>.Empty>>)) 0.016
loopRules (temporaryrule::nil) (temporaryrule::nil) 4 0.016 => res
---------------------------------------------------------------------------------------------------------
run => res

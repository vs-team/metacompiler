include Content.CNV3.Basics.mc
include Content.CNV3.Tuples.mc


import System.Collections.Immutable
import System.Threading

Data "=" : EQ
Data "then" : Then
Data "else" : Else

Data "nop" : stmt                                                                             Priority 100
Data stmt -> ";" -> stmt : stmt                                                               Priority 50
Data "wait" -> Expr : stmt                                                                    Priority 100
Data "when" -> Expr : stmt                                                                    Priority 100
Data "let" -> ID -> EQ -> Expr : stmt                                                         Priority 100
Data "if" -> Expr -> Then -> stmt -> Else -> stmt : stmt                                      Priority 100
Data "while" -> Expr -> stmt : stmt                                                           Priority 100
Data "for" -> ID -> Expr -> stmt : stmt                                                       Priority 100
Data "yield" -> List[Expr] : stmt                                                             Priority 100
Data "rule" List[<<string>>] -> stmt -> stmt -> <<ImmutableDictionary<string, Value> >>  -> <<float>> : Rule     Priority 20

Data "unit" : Unit
Func "run" : runnable => List[ExecutionResult]
Func "evalRule" -> Rule -> <<ImmutableDictionary<string, Value> >> -> <<ImmutableDictionary<string, Value> >> : ExecutionRule  => ExecutionResult   Priority 10
Func "tick" -> List[Rule] -> <<ImmutableDictionary<string, Value> >> -> <<ImmutableDictionary<string, Value> >> -> <<float>> : ticker => List[ExecutionResult]                      Priority 10
Func "loopRules" -> List[Rule] -> List[Rule] -> <<int>> -> <<float>> : RuleLooper => List[ExecutionResult]  Priority 10

Func "addStmt" -> stmt -> stmt : AddStmt => stmt


Func "rebuild" -> List[Rule] -> List[Rule] -> List[ExecutionResult] -> <<float>> : Rebuild => List[Rule]                 Priority 10

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


eval expr ctxt => ($l nil)
------------------------------------------
eval_s (for v expr b) k ctxt dt => Atomic k ctxt

eval expr (Context locals entity world) => ($l (x :: xs))
locals add var x => updatedLocals
----------------------------------------------------------------
eval_s (for ($ var) expr b) k (Context locals entity world) dt => Atomic b;((for ($ var) ($l xs) b);k) (Context updatedLocals entity world)


eval (b) (Context locals entity world) => c
<<locals.SetItem(a, c)>> => res
---------------------------------
eval_s (let ($a) = b) k (Context locals entity world) dt => Atomic k (Context res entity world)

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

entity add f v => updatedEntity
updateFields fs vs (Context l updatedEntity world) => updatedContext
-------------------------------------------------------------------------------------
updateFields (f :: fs) (v :: vs) (Context l entity world) => updatedContext

evalRule a fields globals => Yield ks values (Context newLocals newFields newGlobals)
tick as newFields newGlobals dt => res
------------------------------------------------------
tick (a::as) fields globals dt => b::res

---------------------------------
tick nil fields globals dt => nil

---------------------------
rebuild nil nil nil dt => nil

rebuild q rs b dt => res
---------------------------
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
<<Thread.Sleep((int)(dt * 1000))>>
<<Console.WriteLine(nrs)>>
loopRules original nrs <<steps - 1>> dt => res
--------------------------------------------
loopRules original rs steps dt => res

tick rs dt => nrs
<<Thread.Sleep((int)(dt * 1000))>>
--------------------------
loopRules original rs 0 dt => nrs


p12 := if ($b true) then (wait $f 4.0;yield (($i 5)::($i 0)::nil)) else (wait $f 2.0)
xs := $l (($i 1) :: ($i 2) :: ($i 3) :: nil)
p1 := wait $f 3.0
p2 := wait $f 2.0
p3 := let $"x" = $i 10
p4 := yield (($"Test" + $i 5)::($"X" + $i 1)::nil)
p5 := while ($b true) (wait $f 1.0;p4)
p6 := yield (($"Test" + $i 1)::($"X" + $i 1)::nil)
p7 := for $"x" xs (p1;p6;p2)
p8 := when $"X" gt $i 1
p9 := yield (($"Test" + $i 1000)::nil)
<<ImmutableDictionary<string, Value>.Empty>> add "Test" ($i 0) => dd
dd add "X" ($i 1) => dict
ra := rule ("Test" :: "X" :: nil) (p2;p6) nop (Context (<<ImmutableDictionary<string, Value>.Empty>>) dict (<<ImmutableDictionary<string, Value>.Empty>>)) 1.0
rb := rule ("test" :: nil) (p8;p3;p2) nop (Context (<<ImmutableDictionary<string, Value>.Empty>>) dict (<<ImmutableDictionary<string, Value>.Empty>>)) 1.0
loopRules (ra::rb::nil) (ra::rb::nil) 6 1.0 => res
---------------------------------------------------------------------------------------------------------
run => res

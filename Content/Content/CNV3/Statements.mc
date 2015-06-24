include Content.CNV3.Basics.mc
include Content.CNV3.Tuples.mc
include Content.GenericLists.transform.mc

import System.Collections.Immutable

Data "=" : EQ
Data "then" : Then
Data "else" : Else

Data "nop" : stmt                                                                             Priority 100
Data stmt -> ";" -> stmt : stmt                                                               Priority 50
Data "wait" -> <<float>> : stmt                                                               Priority 100
Data "let" -> ID -> EQ -> Expr : stmt                                                         Priority 100
Data "if" -> Expr -> Then -> stmt -> Else -> stmt : stmt                                      Priority 100

Data "rule" stmt -> stmt -> ctxt -> <<float>> : Rule                                          Priority 20

Data "unit" : Unit
Func "run" : runnable => List[ExecutionResult]
Func "evalRule" -> Rule : ExcecutionRule => ExecutionResult                                   Priority 10
Func "tick" -> List[Rule] -> <<float>> : ticker => List[ExecutionResult]                      Priority 10
Func "loopRules" -> List[Rule] -> List[Rule] -> <<int>> -> <<float>> : RuleLooper => List[ExecutionResult]  Priority 10

Func "addStmt" -> stmt -> stmt : AddStmt => stmt


Func "rebuild" -> List[Rule] -> List[Rule] -> List[ExecutionResult] -> <<float>> : Rebuild => List[Rule]                  Priority 10

Data "Done" -> ctxt : ExecutionResult
Data "Suspend" -> stmt -> ctxt : ExecutionResult                              Priority 7
Data "Yield" -> stmt -> List[ID] -> List[Value] -> ctxt : ExecutionResult
Data "Resume" -> stmt -> ctxt : ExecutionResult
Data "Atomic" -> stmt -> ctxt : ExecutionResult

Data "Context" -> locals -> entity -> world : ctxt

Data "$e" : entity
Data "$w" : world
Data "$l" -> <<ImmutableDictionary<string, Value> >> : locals

Func "eval_s" -> stmt -> stmt -> ctxt -> <<float>> : Execution => ExecutionResult


eval ($b a) (locals) => $b true
--------------------------------------------------------------------------
eval_s (if ($b a) then b else c) k (Context ($l locals) entity world) dt => Atomic b;k (Context ($l locals) entity world)

eval ($b a) (locals) => $b false
--------------------------------------------------------------------------
eval_s (if ($b a) then b else c) k (Context ($l locals) entity world) dt => Atomic c;k (Context ($l locals) entity world)


eval (b) (locals) => c
<<locals.SetItem(a, c)>> => res
<<Console.Write("let ")>>
<<Console.WriteLine(k.ToString())>>
---------------------------------
eval_s (let ($a) = b) k (Context ($l locals) entity world) dt => Atomic k (Context ($l res) entity world)

<<t <= dt>> == false
----------------------------------
eval_s (wait t) k ctxt dt => Suspend wait <<t - dt>>;k ctxt

<<t <= dt>> == true
----------------------------------
eval_s (wait t) k ctxt dt => Resume k ctxt

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

<<Console.WriteLine("--------------")>>
eval_s b k ctxt dt => Atomic z c
<<Console.Write("Atomic ")>>
<<Console.WriteLine(z)>>
eval_s z nop c dt => res 
-------------------------------
evalRule (rule b k ctxt dt)  => res

eval_s b k ctxt dt => Done context
-------------------------------
evalRule (rule b k ctxt dt)  => Done context

eval_s b k ctxt dt => Suspend ks context
-------------------------------
evalRule (rule b k ctxt dt)  => Suspend ks context

eval_s b k ctxt dt => Resume ks context
<<Console.WriteLine(b)>>
eval_s ks nop context dt => res
-------------------------------
evalRule (rule b k ctxt dt)  => res

eval_s b k ctxt dt => Yield ks ids values context
-------------------------------
evalRule (rule b k ctxt dt)  => Yield ks ids values context

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
rebuild ((rule body nop context delta)::q) (r::rs) ((Done cont)::b) dt => (rule body nop cont dt)::res

rebuild ms q b dt => res
-------------------------
rebuild (m::ms) ((rule body k context delta)::q) ((Suspend (wait t;cont) updatedcontext)::b) dt => (rule (wait t) cont updatedcontext dt)::res

tick rs dt => temp
rebuild original rs temp dt => nrs
steps > 0 
loopRules original nrs <<steps - 1>> dt => res
--------------------------------------------
loopRules original rs steps dt => res

tick rs dt => fin
--------------------------
loopRules original rs 0 dt => fin


p12 := if ($b true) then (wait 16.0;wait 7.0) else (wait 2.0)
p1 := wait 3.0
p2 := wait 2.0
p3 := let $"x" = $i 10 
temporaryrule := rule (p1;p3;p12) nop (Context ($l <<ImmutableDictionary<string, Value>.Empty>>) $e $w) 1.0
typorule := rule (p2) nop (Context ($l <<ImmutableDictionary<string, Value>.Empty>>) $e $w) 1.0
loopRules (temporaryrule::typorule::nil) (temporaryrule::typorule::nil) 3 1.0 => res
-----------------------------------------------------------------------------------------------------------
run => res

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
Func "loopRules" -> List[Rule] -> <<int>> -> <<float>> : RuleLooper => List[ExecutionResult]  Priority 10

Func "rebuild" -> List[ExecutionResult] -> <<float>> : Rebuild => List[Rule]                  Priority 10

Data "Done" -> ctxt : ExecutionResult
Data "Suspend" -> stmt -> ctxt : ExecutionResult                              Priority 7
Data "Yield" -> stmt -> ctxt : ExecutionResult
Data "Resume" -> stmt -> ctxt : ExecutionResult
Data "Atomic" -> ctxt : ExecutionResult
Data "None" : ExecutionResult

Data "Context" -> locals -> entity -> world : ctxt

Data "$e" : entity
Data "$w" : world
Data "$l" -> <<ImmutableDictionary<string, Value> >> : locals

Func "eval_s" -> stmt -> stmt -> ctxt -> <<float>> : Execution => ExecutionResult


eval ($b a) (locals) => $b true
eval_s b k (Context ($l locals) entity world) dt => res
--------------------------------------------------------------------------
eval_s (if ($b a) then b else c) k (Context ($l locals) entity world) dt => res

eval ($b a) (locals) => $b false
eval_s c k (Context ($l locals) entity world) dt => res
--------------------------------------------------------------------------
eval_s (if ($b a) then b else c) k (Context ($l locals) entity world) dt => res


eval (b) (locals) => c
<<locals.SetItem(a, c)>> 
---------------------------------
eval_s (let ($a) = b) k (Context ($l locals) entity world) dt => Atomic (Context ($l locals) entity world)

<<t <= dt>> == true
<<System.Console.WriteLine("wait 0")>>
<<System.Console.WriteLine(k.ToString())>>
----------------------------------
eval_s (wait t) k ctxt dt => Resume k ctxt

k !=nop
a != nop
<<t <= dt>> == false
<<System.Console.WriteLine("a2 " + t.ToString())>>
<<System.Console.WriteLine("b2: " + a.ToString())>>
<<System.Console.WriteLine("c2: " + k.ToString())>>
-----------------------------------------------------------------------------------------
eval_s (wait t;a) k (Context locals entity world) dt => Suspend ((wait <<t - dt>>);a;k) (Context locals entity world)

k == nop
a !=nop
<<t <= dt>> == false
<<System.Console.WriteLine("a1: " + t.ToString())>>
<<System.Console.WriteLine("b1: " + a.ToString())>>
<<System.Console.WriteLine("c1: " + k.ToString())>>
-----------------------------------------------------------------------------------------
eval_s (wait t;a) k (Context locals entity world) dt => Suspend ((wait <<t - dt>>);a) (Context locals entity world)

k == nop
<<t <= dt>> == false
<<System.Console.WriteLine("a0: " + k.ToString())>>
<<System.Console.WriteLine("b0: " + t.ToString())>>
-----------------------------------------------------------------------------------------
eval_s (wait t;nop) k (Context locals entity world) dt => Suspend ((wait <<t - dt>>)) (Context locals entity world)

b != nop
eval_s a (b;k) ctxt dt => res
-------------------------------
eval_s (a;b) k ctxt dt => res

-------------------------------
eval_s nop nop ctxt dt => Done ctxt

k != nop
eval_s k nop ctxt dt => res
-------------------------------
eval_s nop k ctxt dt => res


eval_s body k context dt => Done ctxt
-------------------------------
evalRule rule body k context dt => Done ctxt

eval_s body k (Context locals entity world) dt => Suspend a updatedLocals
-------------------------------------------------------------------------
evalRule rule body k (Context locals entity world) dt => Suspend a updatedLocals


evalRule (rule body k (Context locals entity world) deltaTime) => Done context
r := rule body k (Context locals entity world) deltaTime
newa := rule body nop context dt
<<System.Console.WriteLine("tick2")>>
tick (newa::b) dt => res
----------------------------------------------------------------------
tick ((rule body k (Context locals entity world) deltaTime)::b) dt => res

evalRule (rule body k (Context locals entity world) deltaTime) => Resume newCont context
newa := rule newCont nop context dt
<<System.Console.WriteLine("tick1")>>
tick (newa::b) dt => res
----------------------------------------------------------------------
tick ((rule body k (Context locals entity world) deltaTime)::b) dt => res

evalRule (rule body k (Context locals entity world) deltaTime) => Suspend newCont context
newa := rule newCont nop context dt
<<System.Console.WriteLine("tick0")>>
tick (newa::b) dt => res
----------------------------------------------------------------------
tick ((rule body k (Context locals entity world) deltaTime)::b) dt => res



evalRule a => z
<<System.Console.WriteLine("tick_base")>>
tick b dt => res
---------------------
tick (a::b) dt => z::res

<<System.Console.WriteLine("xxxx")>>
------------------------
tick (nil) dt => nil


rebuild as dt => newrules
body := (wait t);k
r := rule body nop ctxt dt
------------------------------------------------------------------
rebuild ((Suspend ((wait t);k) ctxt)::as) dt => (r::newrules)

-----------------------
rebuild nil dt => nil

<<System.Console.WriteLine("Fellow")>>
tick rs dt => res
-----------------------
loopRules rs 0 dt => res

steps > 0
<<System.Console.WriteLine(steps)>>
tick rs dt => res
rebuild res dt => newrs
loopRules newrs <<steps - 1>> dt => rest
--------------------------------
loopRules rs steps dt => rest

p12 := if ($b true) then ((wait 3.0);if ($b true) then (wait 16.0) else (wait 200.0)) else (wait 5.0)
p1 := wait 3.0
p2 := wait 2.0
p3 := let $"x" = $i 10 
temporaryrule := rule (p1;p2) nop (Context ($l <<ImmutableDictionary<string, Value>.Empty>>) $e $w) 1.0
typorule := rule (p2) nop (Context ($l <<ImmutableDictionary<string, Value>.Empty>>) $e $w) 1.0
loopRules (temporaryrule::nil) 0 1.0 => res
-----------------------------------------------------------------------------------------------------------
run => res

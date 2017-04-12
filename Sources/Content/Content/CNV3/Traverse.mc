include Content.CNV3.Statements.mc
include Content.CNV3.Imported.mc
include Content.CNV3.Basics.mc

import UnityEngine
import System.Collections.Immutable
import System.Collections.Generic
import System.Threading


Data "entity" -> List[<<string>>] -> <<ImmutableDictionary<string, Value> >> -> List[Rule] -> List[Rule] : Entity                  Priority 10
Data "traverseResult" -> Value -> <<ImmutableDictionary<string, Value> >> : TraverseResult
Func "traverse" -> Value -> <<ImmutableDictionary<string, Value> >> -> <<float>> : Traverse => TraverseResult                             Priority 10
Func "runTraverse" -> <<float>> : runnable => TraverseResult
Func "loopTraverse" -> Value -> <<float>> -> <<int>> : runnable => TraverseResult
Func "run" -> <<float>> : runnable => GameState

Entity is Value


--------------------------------------------------------
traverse ($f x) globals dt => traverseResult ($f x) globals

--------------------------------------------------------
traverse ($b x) globals dt => traverseResult ($b x) globals

--------------------------------------------------------
traverse ($i x) globals dt => traverseResult ($i x) globals

--------------------------------------------------------
traverse ($s x) globals dt => traverseResult ($s x) globals

traverse x globals dt => traverseResult x1 g1
traverse y g1 dt => traverseResult y1 g2
----------------------------------------------------------------
traverse ($t (x,y)) globals dt => traverseResult ($t (x1,y1)) g2



traverse x globals dt => traverseResult x1 g1
traverse ($l xs) g1 dt => traverseResult ($l xs1) g2
--------------------------------------------------------------
traverse ($l (x :: xs)) globals dt => traverseResult ($l (x1::xs1)) g2




--------------------------------------------------------------------------------------------
traverse (entity nil fields original rs) globals dt => traverseResult (entity nil fields original rs) globals


tick original rs fields globals dt => State nrs newFields newGlobals
debug := State nrs newFields newGlobals
<<Console.WriteLine(debug)>>
<<newFields.GetKey(f)>> => v
traverse v newGlobals dt => traverseResult v1 g1
newFields add f v1 => updatedFields
traverse (entity fs updatedFields original nrs) g1 dt => res
------------------------------------------------------------------------------------
traverse (entity f::fs fields original rs) globals dt => res


n > 0
traverse (entity fieldNames fields original rs) <<ImmutableDictionary<string, Value>.Empty>> dt => traverseResult (entity names updatedFields uo urules) g1
w1 := entity names updatedFields uo urules
<<Console.WriteLine(w1)>>
<<Thread.Sleep((int)(dt * 1000))>>
loopTraverse (entity fieldNames updatedFields uo urules) dt <<n - 1>> => res
-----------------------------------------------------
loopTraverse (entity fieldNames fields original rs) dt n => res

traverse w <<ImmutableDictionary<string, Value>.Empty>> dt => res
-------------------------------------
loopTraverse w dt 0 => res

<<ImmutableDictionary<string, Value>.Empty>> add "Position" ($f 3.5) => dict
s1 := yield (($"Position" + ($f 0.5))::nil)
s2 := yield (($"Velocity" - ($f 1.5))::nil)
r1 := rule ("Position"::nil) s1 nop <<ImmutableDictionary<string, Value>.Empty>> dt
w := entity ("Position"::nil) dict (r1::nil) (r1::nil)
loopTraverse w dt 4 => res
------------------------------------------------------------------------------------------
runTraverse dt => res

p := $Vector3 <<Vector3.one>>
vx := $Vector3 <<new Vector3(1.0,0.0,0.0)>>
vx1 := $Vector3 <<new Vector3(<<-1.0>>,0.0,0.0)>>
vy := $Vector3 << new Vector3(0.0,1.0,0.0) >>
s1 := when ((vectorx $"Position") lt $f 30.0)
s2 := yield (vx::nil)
s3 := when ((vectorx $"Position") gt $f 0.0)
s4 := yield (vx1::nil)
s5 := yield (($"Position" + $"Velocity")::nil)
<<ImmutableDictionary<string, Value>.Empty>> add "Position" p => dict1
dict1 add "Velocity" vx => dict
r1 := rule ("Velocity"::nil) (s1;s2) nop <<ImmutableDictionary<string, Value>.Empty>> dt
r2 := rule ("Velocity"::nil) (s3;s4) nop <<ImmutableDictionary<string, Value>.Empty>> dt
r3 := rule ("Position"::nil) s5 nop <<ImmutableDictionary<string, Value>.Empty>> dt
loopRules (r1::r2::r3::nil) (r1::r2::r3::nil) 10 dict <<ImmutableDictionary<string, Value>.Empty>> dt => res
--------------------------------------------------------------------------------------------------
run dt => res

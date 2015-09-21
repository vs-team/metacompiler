include Content.CNV3.Statements.mc
include Content.CNV3.Imported.mc

import UnityEngine
import System.Collections.Immutable
import System.Collections.Generic

Data "entity" -> List[<<string>>] -> <<ImmutableDictionary<string, Value> >> -> List[Rule] -> List[Rule] : Entity                  Priority 10
Data "worldEntity" -> List[<<string>>] -> <<ImmutableDictionary<string, Value> >> -> List[Rule] -> List[Rule] : World                                  Priority 10
Data "traverseResult" -> Value -> <<ImmutableDictionary<string, Value> >> : TraverseResult
Func "traverse" -> Value -> <<ImmutableDictionary<string, Value> >> -> <<float>> : Traverse => TraverseResult                             Priority 10
Func "run" -> <<float>> : runnable => GameState

Entity is Value
World is Entity


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
<<newFields.GetKey(f)>> => v
traverse v newGlobals dt => traverseResult v1 g1
newFields add f v1 => updatedFields
traverse (entity fs updatedFields original rs) g1 dt => res
------------------------------------------------------------------------------------
traverse (entity f::fs fields original rs) globals dt => res


traverse (entity fieldNames fields original rs) fields dt => traverseResult (entity updatedNames updatedFields updatedOriginal updatedRules) newGlobals
traverse (worldEntity updatedNames newGlobals updatedOriginal updatedRules) <<ImmutableDictionary<string, Value>.Empty>> dt => res
--------------------------------------------------------------------------------------
traverse (worldEntity fieldNames fields original rs) <<ImmutableDictionary<string, Value>.Empty>> dt => res

vx := $Vector3 <<new Vector3(1.0,0.0,0.0)>>
vy := $Vector3 << new Vector3(0.0,1.0,0.0) >>
s1 := yield (($"Position" + vx)::nil)
s2 := when ((vectorx $"Position") lt $f 30.0)
<<ImmutableDictionary<string, Value>.Empty>> add "Position" ($Vector3 << new Vector3(0.0,0.0,1.0) >>) => dict
r1 := rule ("Position" :: nil) (s1;s2) nop <<ImmutableDictionary<string, Value>.Empty>> dt
loopRules (r1::nil) (r1::nil) 100 dict <<ImmutableDictionary<string, Value>.Empty>> dt => res
--------------------------------------------------------------------------------------------------
run dt => res

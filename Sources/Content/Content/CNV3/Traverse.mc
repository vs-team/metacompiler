include Content.CNV3.Statements.mc

import System.Collections.Immutable
import System.Collections.Generic

Data "entity" -> List[<<string>>] -> <<ImmutableDictionary<string, Value> >> -> List[Rule] -> List[Rule] : Entity                  Priority 10
Data "worldEntity" -> <<ImmutableDictionary<string, Value> >> -> List[Rule] -> List[Rule] : World                                  Priority 10
Data "traverseResult" -> Value -> <<ImmutableDictionary<string, Value> >> : TraverseResult
Func "traverse" -> Value -> <<ImmutableDictionary<string, Value> >> -> <<float>> : Traverse => TraverseResult                             Priority 10
Func "run" : runnable => GameState

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
--------------------------------------------------------
traverse ($t x y) globals dt => traverseResult ($t x1 y1) g2

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


traverse (entity fieldNames fields original rs) fields dt => traverseResult (entity updatedNames updatedFields updatedOriginal updatedRules) newGlobals dt
traverse (worldEntity updatedNames newGlobals updatedOriginal updatedRules) <<ImmutableDictionary<string, Value>.Empty>> dt =>  
--------------------------------------------------------------------------------------
traverse (worldEntity fieldNames fields original rs) <<ImmutableDictionary<string, Value>.Empty>> dt => res



p12 := if ($b true) then (wait $f 4.0;yield (($i 5)::($i 0)::nil)) else (wait $f 2.0)
xs := $l (($i 1) :: ($i 2) :: ($i 3) :: nil)
p1 := wait $f 3.0
p2 := wait $f 2.0
p3 := let $"x" = $i 10
p4 := yield (($"Test" + $i 5)::($"X" + $"x")::nil)
p5 := while ($b true) (wait $f 1.0;p4)
p6 := yield (($"Test" + $i 1)::($"X" + $i 1)::nil)
p7 := for $"x" in xs (p1;p6;p2)
p8 := when $"X" gt $i 1
p9 := yield (($"Test" + $i 1000)::nil)
subq := from $"y" in ($l (($i 1)::($i 3)::($i 5)::nil)) nil where $"y" lt $i 5 select $"y"
subl := let $"subq" = subq
q := from $"x" in ($l (($i 1)::($i 2)::($i 3)::nil)) (subl::nil) where $"x" gt $i 1 select ($"x" ++ $"subq")
p10 := let $"q" = q
p11 := yield (($"Test")::($"q")::nil)
<<ImmutableDictionary<string, Value>.Empty>> add "Test" ($i 0) => dd
dd add "X" ($i 1) => dict
r1 := rule ("Test" :: "X" :: nil) (p1;p3;p4) nop <<ImmutableDictionary<string, Value>.Empty>> 1.0
r2 := rule ("Test" :: nil) (p8;p3;p2) nop <<ImmutableDictionary<string, Value>.Empty>> 1.0
loopRules (r1::nil) (r1::nil) 4 dict <<ImmutableDictionary<string, Value>.Empty>> 1.0 => res
--------------------------------------------------------------------------------------------------
run => res

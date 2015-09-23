include Content.CNV3.Statements.mc

import UnityEngine
import System.Collections.Immutable
import System.Collections.Generic

Func "run" -> <<float>> : runnable => GameState

vx := $Vector3 <<new Vector3(1.0,0.0,0.0)>>
<< new Vector3(0.0,0.0,1.0) >> => p
<< WrapperTest.Instantiate(p) >> => wt
s1 := yield (($wrapperSet wt ($"Base" + vx))::nil)
s2 := when ((vectorx $"Base") lt $f 30.0)
<<ImmutableDictionary<string, Value>.Empty>> add "Base" ($wrapper wt) => dict
r1 := rule ("Base" :: nil) (s1;s2) nop <<ImmutableDictionary<string, Value>.Empty>> dt
tick (r1::nil) (r1::nil) dict <<ImmutableDictionary<string, Value>.Empty>> dt => res
--------------------------------------------------------------------------------------------------
run dt => res


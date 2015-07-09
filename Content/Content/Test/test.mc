include Content.PeanoNumbers.transform.mc
include Content.GenericLists.transform.mc
include Content.CNV3.Tuples.mc

import System
import System.Collections.Immutable


Func "debug" : Debug => <<float>>

t := 1.0,(2.0,3.0)
l := 5.0::(7.0::(10.0::nil))
p := s(s(s(s(z))))
fst t => f
length l => lg
toNum p => n
-------------------------------
debug => <<f + lg + n>>

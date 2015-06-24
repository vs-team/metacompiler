include Content.Lists.transform.mc

import System



Data "lin" : ListInt Priority 0 
Data ListInt -> "snoc" -> <<int>> : ListInt         Priority 1000 Associativity Left 

Func "dda" -> ListInt : Expr => <<int>>             Priority 100 


-------------
dda lin => 0

dda xs => res
--------------------------
dda xs snoc x => <<x + res>>

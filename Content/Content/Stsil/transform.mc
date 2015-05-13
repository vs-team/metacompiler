import System

Data "lin" : ListInt Priority 0 
Data ListInt -> "snoc" -> <<int>> : ListInt         Priority 1000 Associativity Left 
Func "$" -> <<int>> : Expr => IntValue              Priority 10000 

Func "dda" -> ListInt : Expr => Expr                Priority 100 


--------------
dda lin => $0

dda xs => $res
--------------------------
dda xs snoc x => $<<x + res>>


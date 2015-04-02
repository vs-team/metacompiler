import System

Data [] [] "lin" [] Priority 0 Type ListInt
Data [] [ListInt] "snoc" [<<int>>] Priority 1000 Type ListInt Associativity Left 
Func [] "$" [<<int>>] Priority 10000 Type Expr => IntValue 

Func [] "dda" [ListInt] Priority 100 Type Expr => Expr 


--------------
dda lin => $0

dda xs => $res
--------------------------
dda xs snoc x => $<<x + res>>


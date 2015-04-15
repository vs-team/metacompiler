import System

Data [] [] "nil" [] Priority 0 Type ListInt
Data [] [<<int>>] ";" [ListInt] Priority 1000 Type ListInt
Data [] [] "$" [<<int>>] Priority 10000 Type IntValue

Func [] "contains" [ListInt <<int>>] Priority 100 Type Expr => Bool
Func [] "removeOdd" [ListInt] Priority 100 Type Expr => ListInt

Func [] "add" [ListInt] Priority 100 Type Expr => IntValue
Func [] "plus" [ListInt <<int>>] Priority 100 Type Expr => ListInt
Func [] "length" [ListInt] Priority 100 Type Expr => IntValue

Data [] [] "yes" [] Priority 0 Type Bool
Data [] [] "no" [] Priority 0 Type Bool


----------------
length nil => $0

length xs => $y
--------------------------
length x;xs => $<<1 + y>>


--------------
add nil => $0

add xs => $res
--------------------------
add x;xs => $<<x + res>>


-------------------
plus nil k => nil

plus xs k => xs'
<<x+k>> => x'
----------------------
plus x;xs k => x';xs'


---------------------
contains nil k => no

x == k
------------------------
contains x;xs k => yes

x != k
contains xs k => res
------------------------
contains x;xs k => res


---------------------
removeOdd nil => nil

<< x % 2 >> == 0
removeOdd xs => xs'
-----------------------
removeOdd x;xs => xs'

<< x % 2 >> == 1
removeOdd xs => xs'
--------------------------
removeOdd x;xs => x;xs'

import System

      

Data "nil" : ListInt
Data <<int>> -> ";" -> ListInt : ListInt                                Priority 1000
Data "$" -> <<int>> : IntValue                                          Priority 10000

Func "contains" -> ListInt -> <<int>> : Expr => Bool                    Priority 100 
Func "removeOdd" -> ListInt : Expr => ListInt                           Priority 100  

Func "add" -> ListInt : Expr => IntValue                                Priority 100 
Func "plus" -> ListInt -> <<int>> : Expr => ListInt                     Priority 100
Func "length" -> ListInt : Expr => IntValue                             Priority 100 

Data ListInt -> "," -> ListInt : ListIntPair                            Priority 900
Func "split" -> ListInt : Expr => ListIntPair                           Priority 100 
Func "merge" -> ListInt -> ListInt : Expr => ListInt                    Priority 100
Func "mergeSort" -> ListInt : Expr => ListInt                           Priority 100 

Data "yes" : Bool                                                        
Data "no" :   Bool                                                      


---------------------
split nil => nil,nil

---------------------------
split x;nil => (x;nil),nil

split xs => l,r
----------------------------
split x;y;xs => (x;l),(y;r)


---------------------
merge nil nil => nil

-----------------------
merge x;xs nil => x;xs

-----------------------
merge nil y;ys => y;ys

x <= y
merge xs y;ys => res
-------------------------
merge x;xs y;ys => x;res

x > y
merge x;xs ys => res
-----------------------
merge x;xs y;ys => y;res

---------------------
mergeSort nil => nil

-------------------------
mergeSort x;nil => x;nil

split x;y;xs => l,r
mergeSort l => l'
mergeSort r => r'
merge l' r' => res
----------------------
mergeSort x;y;xs => res


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

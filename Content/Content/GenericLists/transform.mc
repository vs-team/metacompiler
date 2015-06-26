import System      

Data[a b] a -> "," -> b : Pair[a b]                                     Priority 900

Data[a] a -> "::" -> List[a] : List[a]                                   Priority 1000
Data[a] "nil" : List[a]

Func "removeOdd" -> List[<<int>>] : ListOperator => List[<<int>>]
Func[a] "length" -> List[a] : ListOperator => <<int>>
Func[a] List[a] -> "append" -> List[a] : ListOperator => List[a]

----------------
length nil => 0

length xs => y
--------------------------
length x::xs => <<1 + y>>


---------------------
nil append ys => ys

xs append ys => zs
-----------------------------
(x :: xs) append ys => x :: zs


---------------------
removeOdd nil => nil

<< x % 2 >> == 0
removeOdd xs => xs'
-----------------------
removeOdd x::xs => xs'

<< x % 2 >> == 1
removeOdd xs => xs'
--------------------------
removeOdd x::xs => x::xs'

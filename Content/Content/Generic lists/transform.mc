import System      

Data[a b] a -> "," -> b : Pair[a b]                                     Priority 900

Data[a] a -> ";" -> List[a] : List[a]                                   Priority 1000
Data[a] "nil" : List[a]

Func "removeOdd" -> List[<<int>>] : Expr => List[<<int>>]

Func[a] "length" -> List[a] : Expr => <<int>>

Func "run" : Expr => Pair[<<int>> List[<<int>>]]


----------------
length nil => 0

length xs => y
--------------------------
length x;xs => <<1 + y>>


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


l1 := "x";"y";"z";nil
l2 := 1;2;3;4;5;nil
length l1 => x
removeOdd l2 => y
----------------------
run => x,y

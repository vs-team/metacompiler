Data "Z" -> pnum
Data "S" -> pnum -> pnum

Func pnum -> "add" -> pnum -> pnum
Func "main" -> pnum


x -> Z
-------
x add y -> y


x -> S a 
a add y -> b
S b -> res
------
x add y -> res


Z -> a0
S a0 -> a1
S a1 -> a2
S a2 -> a3
a2 add a3 -> res
-----
main -> res
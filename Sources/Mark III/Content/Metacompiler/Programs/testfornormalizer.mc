Func int -> "add" -> int -> int
Func int -> "sub" -> int -> int
Func "test" -> int -> int

x add y -> res
-----
x sub y -> res


a -> c
b -> d
c sub d -> res2
res2 -> res3
c add d -> res4
-------------
a add b -> res3
Func int -> "add" -> int -> int
Func int -> "sub" -> int -> int
Func "test" -> int -> int

x add y -> res
res -> testres
x -> bla
y -> bal
bla sub bal -> dump
-----
x sub y -> testres


a -> c
b -> d
c sub d -> res2
res2 -> res3
c add d -> res4
-------------
a add b -> res3
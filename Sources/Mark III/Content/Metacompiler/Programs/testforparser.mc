Func int -> "add" -> int -> int
Func int -> "sub" -> int -> int

Func int -> "+" -> int -> int
Func int -> "-" -> int -> int

Func "run" -> int

Data 'a -> "," -> 'b -> 'a * 'b

x + y == x + y
x + y -> res
---
x add y -> res


x - y -> res
----
x sub y -> res

x add y -> res
res sub c -> res2
--------
run -> res2


------
run -> d,e

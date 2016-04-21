Data "Z" -> pnum
Data "S" -> pnum -> pnum

Func pnum -> "add" -> pnum -> pnum


a -> Z
-------
a add b -> b

a -> S c
c add b -> d
S d -> res
----
a add b -> res

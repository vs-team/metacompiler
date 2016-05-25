Data "Z" -> pnum
Data "S" -> pnum -> pnum

Func pnum -> "add" -> pnum -> pnum
Func pnum -> "sub" -> pnum -> pnum

Func "main" -> pnum

a -> Z
-------
a add b -> b

a -> S c
c add b -> d
S d -> res
----
a add b -> res

b -> Z
------
a sub b -> a

a -> Z
----
a sub b -> Z

a -> S c
b -> S d
c sub d -> res
------
a sub b -> res


Z -> n0
S n0 -> n1
S n1 -> n2
S n2 -> n3
S n3 -> n4
n2 add n4 -> a6
a6 sub n3 -> s3
-------
main -> s3

Data "niks" -> niks
Func "main" -> res
Func "buildcol" -> System.Single -> System.Single -> System.Single -> niks
Func "buildrow" -> System.Single -> System.Single -> niks

Data "img" -> System.Single -> System.Single -> img
Func "mulimg" -> img -> img -> img
Func "addimg" -> img -> img -> img
Func "range" -> img -> System.Int64
Func "iter" -> System.Int64 -> img -> img -> System.String

a -> img r i
System.Single. * r i -> mul
3.0 -> ra
mul < ra
1 -> res
-----------
range a -> res

a -> img r i
System.Single. * r i -> mul
3.0 -> ra
mul >= ra
0 -> res
----------
range a -> res

a -> img ar ac
b -> img br bc
System.Single. * ar br -> ra
System.Single. * ar bc -> cb
System.Single. * br ac -> cc
System.Single. * ac bc -> rd
-1.0 -> neg
System.Single. * rd neg -> rneg
System.Single. + ra rneg -> rres
System.Single. + cb cc -> cres
img rres cres -> res
---------
mulimg a b -> res

a -> img ar ac
b -> img br bc
System.Single. + ar br -> ares
System.Single. + ac bc -> bres
img ares bres -> res
------------
addimg a b -> res

5 -> lim
i < lim
1 -> n1
mulimg n n -> mulres
addimg mulres start -> ares
range ares -> ra
ra == n1
System.Int64. + i n1 -> in
iter in ares start -> res
-----------
iter i n start -> res

5 -> lim
i < lim
mulimg n n -> mulres
addimg mulres start -> ares
range ares -> ra
0 -> n0
ra == n0
"_" -> res
-----------
iter i n start -> res

5 -> lim
i >= lim
"M" -> res
-----------
iter i n start -> res

b < e
80.0 -> step
3.0 -> space
System.Single. / space step -> n
System.Single. + b n -> bn
0 -> i
0.0 -> d0
img d0 d0 -> begin
img r b -> start
iter i begin start -> st
System.String.print st -> nop
buildcol r bn e -> c
niks -> res
-----------
buildcol r b e -> res

b >= e
niks -> res
------------
buildcol r b e -> res

b < e
60.0 -> step
-1.5 -> colb
1.5 -> cole
3.25 -> space
-1.0 -> neg
System.Single. / space step -> n
System.Single. + b n -> bn
buildcol b colb cole -> cols
buildrow bn e -> r
niks -> res
-----------
buildrow b e -> res

b >= e
niks -> res
-----------
buildrow b e -> res

-2.25 -> b
1.0 -> e
buildrow b e -> res
-----
main -> res

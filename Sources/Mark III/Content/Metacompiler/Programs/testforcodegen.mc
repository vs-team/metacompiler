Data "bla" -> System.Int64 -> num
Func "main" -> string
Func "con" -> System.Int64 -> System.Int64 -> string

a < b
"less" -> r
----------
con a b -> r

a > b
"greater" -> r
----------
con a b -> r


1 -> a
2 -> b
System.Int64. + a b -> c
con c b -> r
-----
main -> r

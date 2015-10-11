Data "Some" a -> a -> Option a
Data "None" a -> Option a
Data a -> "," a b -> b -> a * b

Func "tryPositive" -> int -> Option int

x < 0
--
tryPositive x -> Some x

x >= 0
--
tryPositive x -> None


Func "tryNegative" -> int -> Option int

x < 0
--
tryNegative x -> Some x

x >= 0
--
tryNegative x -> None


Func "run" -> int -> int -> (Option int) * (Option int)

tryPositive x -> a
tryNegative y -> b
--
run x y -> a, b

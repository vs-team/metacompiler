
TypeFunc "test" => #a => #b => #a * #b 


TypeAlias "ali" => #a => #a * #b

Func "add" -> int -> int -> int

Func "main" -> int


test a b => res
-----
test a b => res

add x y -> res
----
add x y -> res



0 -> res
-----
main -> res
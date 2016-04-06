
TypeFunc "test" => #a => #b => #a * #b 


TypeAlias "ali" => #a => #a * #b

Func "add" -> int -> int -> int


add x y  -> res
----
add x y-> res
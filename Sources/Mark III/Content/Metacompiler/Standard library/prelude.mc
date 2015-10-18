Data "unit" -> Unit
Data "Some" -> 'a -> Option 'a
Data "None" -> Option 'a
Data "::" -> 'a -> List 'a
Data "empty" -> List 'a
Data 'a -> "," -> 'b -> 'a * 'b
Data "A" -> 'a -> 'a | 'b
Data "B" -> 'b -> 'a | 'b


Func ('b->'c) -> "<-" -> ('a->'b) -> ('a -> 'c)

f x -> y
g y -> z
----------------
(f <- g) x -> z

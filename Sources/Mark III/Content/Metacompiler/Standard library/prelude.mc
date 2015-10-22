Data "unit" -> Unit
Data "Some" -> 'a -> Option 'a
Data "None" -> Option 'a
Data "::" -> 'a -> List 'a
Data "empty" -> List 'a
Data 'a -> "," -> 'b -> 'a * 'b
Data "A" -> 'a -> 'a | 'b
Data "B" -> 'b -> 'a | 'b
Func ('b->'c) -> "<.>" -> ('a->'b) -> ('a -> 'c)
Data "then" -> Then
Data "else" -> Else
Func "if" -> Boolean -> Then -> ('a -> 'b) -> Else -> ('a -> 'b) -> ('a -> 'b)


(f <.> g) x -> g(f(x))


(if True then f else g) x -> f x

(if False then f else g) x -> g x

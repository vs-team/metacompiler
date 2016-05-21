import System

Data "unit" -> Unit
TypeAlias #a => "*" => #b => #c
'a * 'b => tuple<'a 'b>
TypeAlias #a => "|" => #b => #c
'a | 'b => pipe<'a 'b>
Data 'a -> "," -> 'b -> 'a * 'b    #> 5
Data "Left" -> 'a -> 'a | 'b       #> 5
Data "Right" -> 'b -> 'a | 'b      #> 5

Data "then" -> Then
Data "else" -> Else

Func "if" -> Boolean^System -> Then -> 'a -> Else -> 'a -> 'a
if True^builtin then f else g -> f
if False^builtin then f else g -> g

Data "with" -> With

Func "match" -> ('a | 'b) -> With -> ('a -> 'c) -> ('b -> 'c) -> 'c
match (Left x) with f g -> f x

match y with g h -> res
--------------------
match (Right y) with f (g h) -> res

match (Right y) with f g -> g y

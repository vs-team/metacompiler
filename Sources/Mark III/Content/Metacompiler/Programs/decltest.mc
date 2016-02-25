Data 'a -> "," -> 'b -> 'a * 'b
Data 'a -> "tupletuple" -> 'b -> 'c * ('a * 'b)
Data 'a -> "bar" -> 'b -> 'a | 'b  #> 123L

Data "tripletuple" -> 'a * 'b * 'c
Data 'a -> "bartuple" -> 'b -> 'a * 'b | c * d  #>321R 
Data 'a -> "bartupcor" -> 'b -> ('a * 'b) | ('c * 'd)

Data 'a -> "bla" -> ('b -> 'c) -> 'a * ('b -> 'c)

Data "wtf" -> (('a -> ('b -> 'c)) -> (('a -> 'b) -> 'c))

Data "vla" -> int  #>456L

Func 'a -> "functest" -> 'b -> 'c

Func int -> "add" -> int -> int

Data int -> "int" -> int -> int * int | int

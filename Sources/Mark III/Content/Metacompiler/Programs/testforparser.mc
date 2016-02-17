Data 'a -> "tuple" -> 'b -> 'a * 'b
Data 'a -> "bar" -> 'b -> 'a | 'b  #> 123L

Data 'a -> 'c -> "bartuple" -> 'b -> 'd -> 'a * 'b | c * d  #>321R 
Data 'a -> 'c -> "bartupcor" -> 'b -> 'd -> ('a * 'b) | ('c * 'd)

Data 'a -> "bla" -> ('b -> 'c) -> 'a * ('b -> 'c)

Data "wtf" -> (('a -> ('b -> 'c)) -> (('a -> 'b) -> 'c)) 'd

Data "vla" -> int  #>456L

Data "app" -> 'a 'b 'c

Func 'a -> "functest" -> 'b -> 'c

Func int -> "add" -> int -> int
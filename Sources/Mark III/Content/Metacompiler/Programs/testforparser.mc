Data "A" -> int -> int 

Data "B" -> gen<a b> -> gen<c d>

Func "C" -> (lama -> lamb) -> (lamc -> lamd)

Func "D" -> (lam<a b> -> lam<c d>) -> test<e f>

TypeAlias test => "A" => int => int 

TypeAlias "B" => gen<a b> => gen<c d>

TypeFunc "C" => (lama -> lamb) => (lamc -> lamd)

bla => test
------------
TypeFunc "D" => (lam<a b> -> lam<c d>) => test<e f>

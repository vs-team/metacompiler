import System.Collections.Immutable

Data[a b] a -> "," -> b : Tuple[a b]                         Priority 250
Func[a b] "first" -> Tuple[a b] : TupleOperator => a         Priority 10


-----------------
first (x,y) => x

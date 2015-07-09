import System.Collections.Immutable

Data[a b] a -> "," -> b : Tuple[a b]                         Priority 100
Func[a b] "fst" -> Tuple[a b] : TupleOperator => a
Func[a b] "snd" -> Tuple[a b] : TupleOperator => b


----------------
fst (x,y) => x

------------------
snd (x,y) => y

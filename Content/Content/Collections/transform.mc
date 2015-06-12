import System.Collections.Immutable

Data[a] "$t" -> a : Singleton[a]                                        Priority 1000
Data[a b] Singleton[a] -> "," -> b : Tuple[a b]                         Priority 250
Func[a b] "first" -> Tuple[a b] : TupleOperator => a                    Priority 10


-----------------
first ($t x,y) => x


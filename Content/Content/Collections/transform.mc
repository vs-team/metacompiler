Data[a] "$t" -> a : Tuple[a]                             Priority 1000
Data[a] Tuple[a] -> "," -> Tuple[a] : Tuple[a]           Priority 250

Func[a] "run" -> Tuple[a] : Main => Tuple[a]             Priority 0

-------------
run t => t 


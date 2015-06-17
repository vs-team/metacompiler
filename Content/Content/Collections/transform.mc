import System.Collections.Immutable

Data[a b] a -> "," -> b : Tuple[a b]                         Priority 250
Func[a b] "fst" -> Tuple[a b] : TupleOperator => a
Func[a b] "snd" -> Tuple[a b] : TupleOperator => b

Func "run" -> Tuple[<<ImmutableDictionary<string, int> >> <<ImmutableDictionary<string, int> >>] : TupleOperator => <<ImmutableDictionary<string, int> >>
Func "run1" -> Tuple[<<int>> <<int>>] : TupleOperator => <<int>>
Func "run2" -> Tuple[<<int>> <<ImmutableDictionary<string, int> >>] : TupleOperator => Tuple[<<int>> <<ImmutableDictionary<string, int> >>]
Func "run3" -> Tuple[<<int>> Tuple[<<ImmutableDictionary<string, int> >> <<ImmutableDictionary<string, int> >>]] : TupleOperator => Tuple[<<int>> <<ImmutableDictionary<string, int> >>]
Func "run4" : TupleOperator => Tuple[<<float>> <<float>>]

----------------
fst (x,y) => x

----------------
snd (x,y) => y

fst t => x
snd t => y
-----------------
run t => y

fst t => x
snd t => y
-----------------
run1 t => <<x+y>>

fst t => x
snd t => y
<<x+1>> => x1
<<y.Add("k",x1)>> => y1
-------------------------
run2 t => (x1,y1)

fst t => x
snd t => t1
snd t1 => y
<<x+1>> => x1
<<y.Add("k",x1)>> => y1
-------------------------
run3 t => (x1,y1)

--------------------
run4 => (1.0,2.0)

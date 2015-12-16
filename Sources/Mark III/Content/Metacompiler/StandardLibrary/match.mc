import prelude

Data "with" -> With

TypeFunc "match" => * => Module

match ('a | 'b) => MatchT ('a | 'b) {
    TypeFunc "Head" => *
    Head => 'a

    TypeFunc "Tail" => *
    Tail => 'b
    
    Func "do" -> 'a -> With -> (Head -> 'b) -> (Tail -> 'b) -> 'b
    do (Left x) with f g -> f x
    do (Right y) with f g -> g y
  }

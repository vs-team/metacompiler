import prelude

Data "with" -> With

TypeFunc "matchOr" => * => Module

matchOr ('a | 'b) => MatchT ('a | 'b) {
    TypeFunc "Head" => *
    Head => 'a
    TypeFunc "Tail" => *
    Tail => 'b
    
    Func "matchOr" => 'a => with => (Head => 'b) => (Tail => 'b) => 'b
    do (Left x) with f g => f x
    do (Right y) with f g => g y
  }

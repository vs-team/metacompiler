import prelude

Data "with" -> With

TypeFunc "match" => * => Module
match ('a | 'b) => Module ('a | 'b) {
  TypeFunc "Head" => *
  Head => 'a

  TypeFunc "Tail" => *
  Tail => 'b

  Func "do" -> 'a -> With -> (Head -> 'b) -> (Tail -> 'b) -> 'b
  do (Left x) with f g -> f x

  do y with g h -> res
  --
  do (Right y) with f (g h) -> res

  do (Right y) with f g -> g y
}

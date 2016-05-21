import prelude

Data "with" -> With

TypeFunc "match" => #a => Module
match ('a | 'b) => Module ('a | 'b) {
  TypeFunc "Head" => #a
  Head => 'a

  TypeFunc "Tail" => #a
  Tail => 'b

  Func "do" -> 'c -> With -> (Head -> 'd) -> (Tail -> 'd) -> 'd
  do (Left x) with f g -> f x

  do y with g h -> res
  --------------------
  do (Right y) with f (g h) -> res

  do (Right y) with f g -> g y
}

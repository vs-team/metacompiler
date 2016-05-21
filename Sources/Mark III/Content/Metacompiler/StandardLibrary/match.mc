import prelude

Data "with" -> With

TypeFunc "match" => #a => Module
match ('a | 'b) => Module ('a | 'b) {
  TypeFunc "Head" => #a
  Head => 'a

  TypeFunc "Tail" => #a
  Tail => 'b

  Func "doMatch" -> 'c -> With -> (Head -> 'd) -> (Tail -> 'd) -> 'd
  doMatch (Left x) with f g -> f x

  doMatch y with g h -> res
  --------------------
  doMatch (Right y) with f (g h) -> res

  doMatch (Right y) with f g -> g y
}

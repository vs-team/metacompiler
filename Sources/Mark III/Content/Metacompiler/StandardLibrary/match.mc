Data "with" => With

TypeFunc "Match" => * => Signature

Match 'a => Signature {
  TypeFunc Head => *
  TypeFunc Tail => *

  TypeFunc "match" => 'a => With => (Head => 'b) => (Tail => 'b) => 'b
}

 
TypeFunc "match" => * => Match

match ('a | 'b) => Match ('a | 'b) {
  Head => 'a
  Tail => 'b

  match (Left x) f g => f x
  match (Right y) f g => g y
}

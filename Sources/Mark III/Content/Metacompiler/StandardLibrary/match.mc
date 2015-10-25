Data "with" => With

Signature "Match" 'a {
  TypeFunc Head => *
  TypeFunc Tail => *

  TypeFunc "match" => 'a => With => (Head => 'b) => (Tail => 'b) => 'b
}

 
ModuleFunc "match" => * => Match

match ('a | 'b) => Module (Match ('a | 'b)) {
  Head => 'a
  Tail => 'b

  match (Left x) f g => f x
  match (Right y) f g => g y
}

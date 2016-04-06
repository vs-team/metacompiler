TypeFunc "MonoidAdd" => #a => Module
MonoidAdd 'a => Module {
  Func 'a -> "+" -> 'a -> 'a #> 60
  Func "identityAdd" -> 'a
}

TypeFunc "MonoidMul" => #a => Module
MonoidMul 'a => Module {
  Func 'a -> "*" -> 'a -> 'a #> 70
  Func "identityMul" -> 'a
}

TypeFunc "GroupAdd" => #a => Module
GroupAdd 'a => Module {
  inherit MonoidAdd 'a
  Func 'a -> "-" -> 'a -> 'a #> 60
}

TypeFunc "GroupMul" => #a => Module
GroupMul 'a => Module {
  inherit MonoidMul 'a
  Func 'a -> "/" -> 'a -> 'a #> 70
}

TypeFunc "Number" => #a => Module
Number 'a => Module {
  inherit GroupAdd 'a
  inherit GroupMul 'a
}

TypeFunc "Vector" => #a => Module
Vector 'a => Module {
  inherit GroupAdd 'a
  inherit MonoidMul 'a
}

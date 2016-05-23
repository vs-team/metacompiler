TypeFunc "MonoidAdd" => Type => Module
MonoidAdd 'a => Module {
  Func 'a -> "+" -> 'a -> 'a #> 60
  Func "identityAdd" -> 'a
}

TypeFunc "MonoidMul" => Type => Module
MonoidMul 'a => Module {
  Func 'a -> "*" -> 'a -> 'a #> 70
  Func "identityMul" -> 'a
}

TypeFunc "GroupAdd" => Type => Module
GroupAdd 'a => Module {
  inherit MonoidAdd 'a
  Func 'a -> "-" -> 'a -> 'a #> 60
}

TypeFunc "GroupMul" => Type => Module
GroupMul 'a => Module {
  inherit MonoidMul 'a
  Func 'a -> "/" -> 'a -> 'a #> 70
}

TypeFunc "Number" => Type => Module
Number 'a => Module {
  inherit GroupAdd 'a
  inherit GroupMul 'a
}

TypeFunc "Vector" => Type => Module
Vector 'a => Module {
  inherit GroupAdd 'a
  inherit MonoidMul 'a
}

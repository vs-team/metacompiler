TypeFunc "MonoidAdd" => * => Module
MonoidAdd 'a => Module {
  Func 'a -> "+" -> 'a -> 'a #> 60
  Func "identityAdd" -> 'a
}

TypeFunc "MonoidMul" => * => Module
MonoidMul 'a => Module {
  Func 'a -> "*" -> 'a -> 'a #> 70
  Func "identityMul" -> 'a
}

TypeFunc "GroupAdd" => * => Module
GroupAdd 'a => Module {
  inherit MonoidAdd 'a
  Func 'a -> "-" -> 'a -> 'a #> 60
}

TypeFunc "GroupMul" => * => Module
GroupMul 'a => Module {
  inherit MonoidMul 'a
  Func 'a -> "/" -> 'a -> 'a #> 70
}

TypeFunc "Number" => * => Module
Number 'a => Module {
  inherit GroupAdd 'a
  inherit GroupMul 'a
}

TypeFunc "Vector" => * => Module
Vector 'a => Module {
  inherit GroupAdd 'a
  inherit MonoidMul 'a
}

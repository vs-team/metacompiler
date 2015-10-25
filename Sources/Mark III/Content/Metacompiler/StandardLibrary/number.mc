Signature "Number" 'a {
  TypeFunc "Num" => 'a

  Func 'a -> "+" -> 'a -> 'a
  Func 'a -> "-" -> 'a -> 'a
  Func "zero" -> 'a

  Func 'a -> "*" -> 'a -> 'a
  Func 'a -> "/" -> 'a -> 'a
  Func "one" -> 'a
}

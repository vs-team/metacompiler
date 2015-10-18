Data "unit" -> Unit
Data "Some" a -> a -> Option a
Data "None" a -> Option a
Data "left" a b -> a -> Either a b
Data "right" a b -> b -> Either a b
Data a -> "," a b -> b -> a * b

Module "Monad" m {
  Func m a -> ">>=" a b -> (a -> m b) -> m b
  Func "return" a -> m a
}

Module "Number" a {
  Func a -> "+" -> a -> a
  Func a -> "-" -> a -> a
  Func "zero" -> a

  Func a -> "*" -> a -> a
  Func a -> "/" -> a -> a
  Func "one" -> a
}

Func "run" -> Unit
run -> unit

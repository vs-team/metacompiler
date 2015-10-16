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

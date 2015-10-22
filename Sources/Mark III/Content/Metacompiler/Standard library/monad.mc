import Prelude

Class "Monad" m
  {
    ArrowFunc m a -> ">>=" a b -> (a -> m b) -> m b
    Func "return" a -> m a
  }

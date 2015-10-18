import Prelude

Class "Monad" m
  {
    Func m a -> ">>=" a b -> (a -> m b) -> m b
    Func "return" a -> m a
  }
Class "Monad" m
  {
    ArrowFunc m a -> ">>=" a b -> (a -> m b) -> m b
    Func "return" a -> m a
  }

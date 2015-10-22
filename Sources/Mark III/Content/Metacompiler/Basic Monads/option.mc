import Prelude

Monad Option
  {
    None >>= k -> None

    Some x >>= k -> k x

    return x -> Some x
  }

import Prelude

Monad Option
  {
    None >>= k -> None

    k x -> y
    ------------------
    Some x >>= k -> y

    return x -> Some x
  }

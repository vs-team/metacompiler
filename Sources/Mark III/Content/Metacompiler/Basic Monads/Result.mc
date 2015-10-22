import Monad

Data "Done" -> 'a -> Result 'a
Data "Error" -> String  -> Result 'a

Instance "res" Monad (Result 'a)
  {
    return x -> Done x
    
    (Done x) >>= k -> k x

    (Error s) >>= k -> Error s


    Func "fail" -> String -> Result 'a
    Func "try" -> (Result 'a) -> ('a -> 'b) (String -> 'b)

    fail s -> Error(s)

    try (Done x) p q -> p x

    try (Error e) p q -> q e
  }

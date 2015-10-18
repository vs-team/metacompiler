import Monad

Data "Done" -> 'a -> Result 'a
Data "Error" -> String  -> Result 'a

Instance "res" Monad (Result 'a)
  {
    return x -> Done x
    
    k x -> out
    ----------------------
    (Done x) >>= k -> out

    (Error s) >>= k -> Error s


    Func "fail" -> String -> Result 'a
    Func "try" (Result 'a) ('a -> 'b) (String -> 'b)

    fail s -> Error(s)

    p x -> y
    --
    try (Done x) p q -> y

    q e -> y
    --
    try (Error e) p q -> y
  }

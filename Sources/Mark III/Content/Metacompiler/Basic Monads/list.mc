import Prelude

Func "map" -> List 'a -> ('a -> 'b) -> List 'b
Func "filter" -> List 'a -> ('a -> Boolean) -> List 'a
Func List 'a -> "@" -> List 'a -> List 'a

empty @ l -> l

(x :: xs) @ l -> x :: (xs @ l)


map empty f -> empty

map (x :: xs) f -> (map (f x)) :: xs


filter empty p -> empty

(if p x 
   (\unit -> x :: (filter xs p))
   (\unit -> (filter xs p))) unit -> res
-- 
filter (x :: xs) p -> res


Monad List
  {
    empty >>= k -> empty

    (x::xs) >>= k -> (k x) @ (xs >>= k)

    return x -> x :: empty
  }

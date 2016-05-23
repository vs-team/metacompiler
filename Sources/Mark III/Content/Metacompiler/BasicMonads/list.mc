import prelude
import monad

TypeAlias "List" => Type => Unit | (Type * (List Type))

TypeAlias Type -> "::" -> List Type -> Right (Type * (List Type))
TypeAlias "empty" -> Left Unit


Func List 'a -> "@" -> List 'a -> List 'a  #> 200
empty @ l -> l
(x :: xs) @ l -> x :: (xs @ l)


Func "map" -> List 'a -> ('a -> 'b) -> List 'b
map empty f -> empty
map (x :: xs) f -> (f x) :: (map xs)


Func "filter" -> List 'a -> ('a -> Boolean) -> List 'a
filter empty p -> empty

(if p x then
  (x :: (filter xs p))
  else
  (filter xs p)) -> res
-------------------------
filter (x :: xs) p -> res


TypeAlias "ListT" => (Type => Type) => Type => Type
ListT 'M 'a => 'M(List 'a)

TypeFunc "list" => Monad => Monad
list 'M => Monad(ListT MCons^'M) {
  {lm >>= l
    (match l with
      (\empty -> return^'M empty)
      (\(x :: xs) ->
        {x >>=^'M y
          return^'M k x} -> z
        xs >>= k -> zs
        (z @ zs)))} -> res
  -------------------------------
  lm >>= k -> res

  return x -> return^'M(x :: empty)
}

import prelude

TypeFunc "List" => * => *
List 'a => Unit | ('a * (List 'a))

Func 'a -> "::" -> List 'a -> List 'a
x :: xs -> Right(x,xs)

Func "empty" -> List 'a
empty -> Left Unit


Func "map" -> List 'a -> ('a -> 'b) -> List 'b
Func "filter" -> List 'a -> ('a -> Boolean) -> List 'a
Func List 'a -> "@" -> List 'a -> List 'a

empty @ l -> l
(x :: xs) @ l -> x :: (xs @ l)


map empty f -> empty
map (x :: xs) f -> (map (f x)) :: xs


filter empty p -> empty

(if p x then 
  (x :: (filter xs p)) 
  else 
  (filter xs p)
) -> res
--
filter (x :: xs) p -> res


TypeFunc "ListT" => (* => *) => * => *
ListT 'M 'a => 'M(List 'a)

TypeFunc "list" => Monad => Monad 

list M => Monad(ListT MCons^M) {
    lm >>=^M l
    (match l with
      (\empty -> return^M empty)
      (\(x :: xs) -> 
        k x >>=^M y
        ((return^M xs) >>= k) >>=^M ys
      return^M (y @ ys)
      )
    ) => res
    --
    lm >>= k => res

    return x => return^M(x :: empty)
  }

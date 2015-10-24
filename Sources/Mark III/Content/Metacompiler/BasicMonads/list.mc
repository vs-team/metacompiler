import Prelude

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
  (filter xs p)) -> res
-- 
filter (x :: xs) p -> res


TypeFunc "ListT" => (* => *) => * => *
ListT 'M 'a => List('M 'a)

ModuleFunc "list" => Monad 'M => Monad (ListT 'M)

list M => Module {
    MCons => ListT 'M
  
    empty >>= k => empty

    xm >>=^M x
    --
    (xm :: xms) >>= k => (k x) @ (xms >>= k)

    return x => x :: empty
  }

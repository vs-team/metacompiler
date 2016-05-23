import prelude
import monad

TypeAlias "Id" => Type => Type
Id 'a => 'a

TypeFunc "id" => Monad
id => Monad(Id) {
  x >>= k -> k x
  return x -> x
}

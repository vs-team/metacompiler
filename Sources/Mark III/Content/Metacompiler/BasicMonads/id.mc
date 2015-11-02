import Prelude

TypeFunc "Id" => * => *
Id 'a => 'a

TypeFunc "id" => Monad

id => Monad Id {
    x >>= k => k x
    return x => x
    lift f a => f a
    lift2 f a b => f a b
  }

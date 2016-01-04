import prelude

TypeAlias "Id" => * => *
Id 'a => 'a

TypeFunc "id" => Monad 
id => Monad(Id) {
    x >>= k -> k x
    return x -> x
  }

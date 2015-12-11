import prelude

TypeAlias "Id" => * => *
Id 'a => 'a

TypeFunc "id" => Monad Id

id => {
    x >>= k => k x
    return x => x
  }

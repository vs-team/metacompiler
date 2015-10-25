import Prelude

TypeFunc "Id" => * => *
Id 'a => 'a

ModuleFunc "id" => Monad

id => Module (Monad Id) {
    x >>= k => k x
    return x => x
  }

import prelude
import tryableMonad
import monad

TypeFunc "result" => Monad => TryableMonad

result 'M => either('M [String]){
    inherit e
  }

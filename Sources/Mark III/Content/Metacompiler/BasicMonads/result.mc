import prelude
import tryableMonad
import monad
import either
import list

TypeFunc "result" => Monad => TryableMonad

result 'M => either('M (List String)){
    inherit e
  }

import prelude
import tryableMonad
import monad
import either
import list

TypeFunc "result" => Monad => TryableMonad
result 'M => either(MCons^'M (List String)){
  inherit e
}

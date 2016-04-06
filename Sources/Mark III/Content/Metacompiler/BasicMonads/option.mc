import prelude
import either
import tryableMonad

TypeFunc "Option" => #a => #b
Option 'a => Unit | 'a

Func "Some" -> 'a -> Option 'a
Some x -> Right x

Func "None" -> Option 'a
None -> Left Unit

TypeFunc "option" => TryableMonad
option => either Option None {
  return
}

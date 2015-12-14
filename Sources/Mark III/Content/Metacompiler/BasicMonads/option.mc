import prelude
import either

TypeFunc "Option" => * => *
Option 'a => Unit | 'a

Func "Some" -> 'a -> Option 'a
Some x -> Right x

Func "None" -> Option 'a
None -> Left Unit

TypeFunc "option" => Monad
option => either id Unit

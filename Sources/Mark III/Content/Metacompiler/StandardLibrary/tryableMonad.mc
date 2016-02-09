import prelude
import monad

TypeFunc "TryableMonad" => ( * => * ) => Module
TryableMonad 'M => Monad(MCons^M) {
  inherit 'M

  Func "try" ('a -> MCons^'M 'b) => ('e -> MCons^'M 'b) => MCons^'M 'a => MCons^'M 'b

  Data "e" -> String

  $$ return the monad of the tryable monad, this way you can use the tryable monad as a the normal monad
  Func "getMonad" -> MCons^'M
  getMonad -> 'M

  $$ return the tryable monad, this way you can use the tryable monad
  Func "Tryable" -> 'a -> 'a
  Tryable -> a -> a
}

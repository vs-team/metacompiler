import prelude
import monad

TypeFunc "TryableMonad" => (#a => #b) => Module
TryableMonad 'M => Monad(MCons^'M) {
  inherit 'M

  Func "try" ('a -> MCons^'M 'b) => ('e -> MCons^'M 'b) => MCons^'M 'a => MCons^'M 'b

  $$ return the monad of the tryable monad, this way you can use the tryable monad as a the normal monad
  Func "getMonad" -> MCons^'M
  getMonad -> 'M
}

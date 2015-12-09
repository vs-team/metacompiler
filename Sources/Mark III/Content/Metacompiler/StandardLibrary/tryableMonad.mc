import Prelude

TypeFunc "TryableMonad" => ( * => * ) => Signature

$$ Monad M => in
$$ ---------------
TryableMonad M => Signature {
    inherit M

    TypeFunc "try" ('a -> MCons^M 'b) => ('e -> MCons^M 'b) => MCons^M 'a => MCons^M 'b
    
    Data "e" -> String

    $$ return the monad of the tryable monad, this way you can use the tryable monad as a the normal monad
    TypeFunc "Monad" => MCons^M
    Monad => M

    $$ return the tryable monad, this way you can use the tryable monad 
    TypeFunc "Tryable" => MCons
    Tryable => MCons
  }

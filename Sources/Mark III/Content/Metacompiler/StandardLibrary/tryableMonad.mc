import Prelude

TypeFunc "TryableMonad" => ( * => * ) => Signature

Monad M => in
---------------
TryableMonad M => {
    inherit in

    'e => 'a
    tryable 
  }

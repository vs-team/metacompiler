import Prelude

Signature "Monad" 'M {
    TypeFunc "MCons" => *
    MCons => 'M

    TypeFunc 'M 'a => ">>=" => ('a => 'M 'b) => 'M 'b
    TypeFunc "return" 'a => 'M 'a
  }

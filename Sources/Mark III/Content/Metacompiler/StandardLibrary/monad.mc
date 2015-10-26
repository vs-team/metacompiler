import Prelude

TypeFunc "Monad" => (* => *) => Signature

Monad 'M => Signature {
    TypeFunc "MCons" => *
    MCons => 'M

    TypeFunc 'M 'a => ">>=" => ('a => 'M 'b) => 'M 'b
    TypeFunc "return" => 'a => 'M 'a
  }

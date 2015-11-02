import Prelude

TypeFunc "Monad" => (* => *) => Signature

Monad 'M => Signature {
    TypeFunc "MCons" => *
    MCons => 'M

    TypeFunc 'M 'a => ">>=" => ('a => 'M 'b) => 'M 'b
    TypeFunc "return" => 'a => 'M 'a
    TypeFunc "lift" => ('a => 'b ) => 'M 'a => 'M 'b  
    TypeFunc "lift2" => ('a => 'b => 'c) => 'M 'a => 'M 'b => 'M 'c 
  }

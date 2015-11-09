import prelude

TypeFunc "Monad" => (* => *) => Signature

Monad 'M => Signature {
    TypeFunc "MCons" => *
    MCons => 'M
    
    TypeFunc 'M 'a => ">>=" => ('a => 'M 'b) => 'M 'b
    TypeFunc "return" => 'a => 'M 'a
    TypeFunc "returnFrom" => 'a => 'a
    TypeFunc "lift" => ('a => 'b ) => 'M 'a => 'M 'b
    TypeFunc "lift2" => ('a => 'b => 'c) => 'M 'a => 'M 'b => 'M 'c

    returnFrom a => a
    
    a >>= a'
    --
    lift f a => return(f a')

    a >>= a'
    b >>= b'
    --
    lift2 f a b => return(f a' b')
  }

import prelude

TypeFunc "Monad" => (* => *) => Signature

Monad 'M => Signature {
    TypeFunc "MCons" => *
    MCons => 'M

    TypeFunc 'M 'a => ">>=" => ('a => 'M 'b) => 'M 'b   #> 10 L
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

    $$ TypeFunc "liftM" => (M' 'a -> M' 'b) => M M' 'a => M M' 'b
    TypeFunc "liftM" => (* -> *) => * => *

    Na >>= a
    f a => b
    lift^Na(return^Na b) => res
    --
    liftM f Na => res
  }

import prelude

TypeFunc "Monad" => (* => *) => Module

Monad M => Module {
    TypeArrowFunc 'M 'a => ">>=" => ('a => 'M 'b) => 'M 'b   #> 10 L
    TypeFunc "return" => 'a => 'M 'a

    TypeFunc "MCons" => *
    MCons => M

    TypeFunc "returnFrom" => 'a => 'a
    returnFrom a => a

    TypeFunc "lift" => ('a => 'b ) => 'M 'a => 'M 'b
    a >>= a'
    --
    lift f a => return(f a')

    TypeFunc "lift2" => ('a => 'b => 'c) => 'M 'a => 'M 'b => 'M 'c
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

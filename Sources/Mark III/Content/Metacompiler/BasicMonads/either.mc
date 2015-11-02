import Prelude
import Match

TypeFunc "EitherT" => (* => *) => * => * => *
EitherT 'M 'b 'a => 'M('b | 'a)

TypeFunc "either" => Monad => * => Monad

either M 'b => Monad (EitherT MCons^M 'b) {
    TypeFunc "try" => MCons 'a => ('a -> MCons^M 'c) => ('b -> MCons^M 'c) => MCons^M 'c
    TypeFunc "fail" => 'b => MCons 'a
    
    return x => return^M(Right x)
    fail x => return^M(Left x)
    
    pm >>=^M y
    (match y with 
    (\x -> q x) 
    (\y -> p y)) => res
    --
    try pm p q => res
    
    pm >>= k => try pm k fail
  
    Ma >>=^Ma a
    --
    lift f Ma => return^M(f a)

    Ma >>=^Ma a
    Mb >>=^Mb b
    --
    lift2 f Ma Mb => return^M(f a b)
  }

import prelude
import match

TypeFunc "EitherT" => (* => *) => * => * => *
EitherT 'M 'e 'a => 'M('a | 'e)

TypeFunc "either" => Monad => * => Monad

either M 'a => Monad (EitherT MCons^M 'a) {
    TypeFunc "try" => ('a -> MCons^M 'b) => ('e -> MCons^M 'b) => MCons 'a => MCons^M 'b
    TypeFunc "fail" => 'e => MCons 'b
    
    return x => return^M(Left x)
    fail x => return^M(Right x)
    
    pm >>=^M y
    (match y with 
      (\Left x -> k x) 
      (\Right e -> err e)
    ) => res
    --
    try pm k err => res

    pm >>= k => try pm k fail
  }

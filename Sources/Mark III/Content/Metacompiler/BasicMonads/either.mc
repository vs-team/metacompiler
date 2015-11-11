import prelude
import match

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
      (\x -> k x) 
      (\y -> err y)
    ) => res
    --
    try pm k err => res

    pm >>= k => try pm k fail
  }

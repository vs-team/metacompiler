import prelude
import match

TypeFunc "EitherT" => (* => *) => * => * => *
EitherT 'M 'e 'a => 'M('a | 'e)

TypeFunc "either" => Monad => * => TryableMonad

either M e => TryableMonad(EitherT MCons^M 'e) {
    TypeFunc "fail" => 'e => MCons 'b
    fail x => return^M(Right (e + x))
    
    pm >>= k => try pm k fail

    return x => return^M(Left x)
    
    pm >>=^M y
    (match y with 
      (\Left x -> k x) 
      (\Right e -> err e)
    ) => res
    --
    try pm k err => res

  }

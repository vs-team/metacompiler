import Prelude
import Match

TypeFunc "EitherT" => (* => *) => * => * => *
EitherT 'M 'b 'a => 'M('b | 'a)

ModuleFunc "either" => Monad => * => Monad

either M 'b => Module Monad (EitherT MCons^M 'b) {
    TypeFunc "try" => MCons 'a => ('a -> MCons^M 'c) => ('b -> MCons^M 'c) => MCons^M 'c
    TypeFunc "fail" => 'b => MCons 'a

    return x => return^M(Right x)
    fail x => return^M(Left x)

    pm >>=^M y
    (match y with
     (\Left x -> q x)
     (\Right y -> p y)) => res
    --
    try pm p q => res

    pm >>= k => try pm k fail
  }

import prelude
import match
import monad
import tryableMonad

TypeAlias "EitherT" => (* => *) => * => * => *
EitherT 'M 'e 'a => 'M('a | 'e)

TypeFunc "either" => Monad => * => TryableMonad
either 'M 'e => TryableMonad(EitherT MCons^'M 'e) {
  Func "fail" -> 'e -> MCons 'b
  fail x -> return^'M(Right (e + x))
  
  pm >>= k -> try pm k fail

  return x -> return^M(Left x)
  
  pm >>=^'M y
  (do^(match(MCons 'a)) y with 
    (\x -> k x) 
    (\e -> err e)
  ) => res
  --
  try pm k err -> res

}

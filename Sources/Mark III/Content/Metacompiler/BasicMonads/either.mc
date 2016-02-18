import prelude
import match
import monad
import tryableMonad
import id

TypeAlias "EitherT" => (* => *) => * => * => *
EitherT 'M 'e 'a => 'M('a | 'e)

TypeFunc "either" => Monad => * => TryableMonad
either 'M 'e => TryableMonad(EitherT MCons^'M 'e) {
  Func "fail" -> 'e -> MCons 'b
  fail x -> return^'M(Right (e + x))

  pm >>= k -> try pm k fail

  return x -> either(return^'M(Left x))

  $$ pm >>=^'M y
  lift pm return^id -> y
  (do^(match(MCons 'a)) y with
    (\x -> k x)
    (\e -> err e)) -> res
  return^'M res -> res'
  --
  try pm k err -> res
}

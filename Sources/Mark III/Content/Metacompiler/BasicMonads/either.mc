import prelude
import monad
import tryableMonad

TypeAlias "EitherT" => (Type => Type) => Type => Type => Type
EitherT 'M ['e] 'a => 'M('a | 'e)

TypeFunc "either" => Monad => Type => TryableMonad
either 'M ['e] => TryableMonad(EitherT MCons^'M ['e]) {
  Func "fail" -> 'e -> MCons 'b
  fail e -> return^'M(Right (Right^'M :: e))

  pm >>= k -> try pm k fail

  return x -> (return^'M(Left x))

  {pm >>=^'M y
    (match y with
      (\x -> k x)
      (\e -> err e)) -> z
    return^'M z} -> res
  ---------------------
  try pm k err -> res
}

import prelude
import monad

TypeAlias "StateT" => (Type => Type) => Type => Type => Type
StateT 'M 's 'a => ('s -> 'M('a * 's))

TypeFunc "state" => Monad => Type => Monad
state 'M 's => Monad(StateT MCons^'M 's) {
  (\ s ->
    {p s >>=^'M x
      x -> (x',s')
      k x' s'}) -> res
  ---------------------
  p >>= k -> res

  return x -> (\ s -> return^'M(x,s))

  Func "getState" -> MCons 's
  getState -> (\ s -> return^'M(s,s))

  Func "setState" -> 's -> MCons Unit
  setState s -> (\ unit -> return^'M(unit,s))
}

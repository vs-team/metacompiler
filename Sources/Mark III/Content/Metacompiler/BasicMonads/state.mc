import Prelude

TypeFunc "StateT" => (* => *) => * => * => *
StateT 'M 's 'a => ('s -> 'M('a*'s))

TypeFunc "state" => Monad => * => Monad

state M 's => Monad(StateT MCons^M 's) {
    p s >>=^M (x,s')
    --
    (p >>= k) s => k x s'

    return x s => return^M(x,s)

    TypeFunc "getState" => MCons 's
    getState s => return^M(s,s)

    TypeFunc "setState" => 's => MCons Unit
    setState s' s => return^M(Unit,s')
  }

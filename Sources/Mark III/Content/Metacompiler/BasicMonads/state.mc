import prelude

TypeFunc "StateT" => (* => *) => * => * => *
StateT 'M 's 'a => ('s -> 'M('a*'s))

TypeFunc "state" => Monad => * => Monad

state M s => Monad(StateT MCons^M 's) {
    p s >>=^M (x,s')
    --
    (p >>= k) s => k x s'

    return x s => return^M(x,s)

    TypeFunc "getState" => MCons 's
    getState s => return^M(s,s)

    TypeFunc "setState" => 's => MCons Unit
    setState s' => return^M(Unit,s')

    TypeFunc "liftM" => (M 'a -> M 'b) => state M 'a => state M 'b
    a >>= a'
    f a' => b
    setState b a => res
    --
    liftM f a => res
  }

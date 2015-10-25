import Prelude

TypeFunc "StateT" => (* => *) => * => * => *
StateT 'M 's 'a => ('s -> 'M('a*'s))

ModuleFunc "state" => Monad => * => Monad

state M 's => Module (Monad(StateT MCons^M 's)) {
    MCons => StateT MCons^M 's

    p s >>=^M (x,s')
    --
    (p >>= k) s => k x s'

    return x => return^M x

    TypeFunc "getState" => MCons 's
    getState s => (s,s)

    TypeFunc "setState" => 's => MCons Unit
    setState s' s => (Unit,s')
  }

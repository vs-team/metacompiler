import Prelude

TypeFunc "StateT" => (* => *) => * => * => *
StateT 'M 's 'a => ('s -> 'M('a*'s))

ModuleFunc "state" => Monad 'M => 's => Monad(StateT 'M 's)

state M 's => Module {
    MCons => StateT 'M 's

    p s >>=^M (x,s')
    --
    (p >>= k) s => k x s'

    return x => return^M x

    TypeFunc "getState" => MCons 's
    getState s => (s,s)

    TypeFunc "setState" => 's => MCons Unit
    setState s' s => (Unit,s')
  }

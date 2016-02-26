import prelude

TypeFunc "Monad" => (* => *) => Module
Monad 'M => Module {
  ArrowFunc 'M 'a -> ">>=" -> ('a -> 'M 'b) -> 'M 'b   #> 10 L
  Func "return" -> 'a -> 'M 'a

  Func "MCons" -> *
  MCons -> 'M

  Func "returnFrom" -> 'a -> 'a
  returnFrom a -> a

  Func "lift" -> ('a -> 'b ) -> 'M 'a -> 'M 'b
  {a >>= a'
     return f a'} -> res
  ----------------------
  lift f a -> res

  Func "lift2" -> ('a -> 'b -> 'c) -> 'M 'a -> 'M 'b -> 'M 'c
  {a >>= a'
     b >>= b'
       return f a' b'} -> res
  ---------------------------
  lift2 f a b -> res

  Func "liftM" => (MCons^'M 'a -> MCons^M' 'b) => 'M (MCons^'M 'a) => 'M (Mcons^'M 'b)
  {M >>=^'M a
    f a -> b
    return^'M b} -> res
  ---------------------
  liftM f M -> res
}

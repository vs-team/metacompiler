﻿Data "unit" -> Unit
Data "Some" -> 'a -> Option 'a
Data "None" -> Option 'a
Data "left"  -> 'a -> Either 'a 'b
Data "right" -> 'b -> Either 'a 'b
Data 'a -> "," -> 'b -> 'a * 'b

Module "Monad" m
  [
    Func m a -> ">>=" a b -> (a -> m b) -> m b
    Func "return" a -> m a
  ]

Instance Monad Option
  {
    None >>= k -> None

    k x -> y
    ------------------
    Some x >>= k -> y

    return x -> Some x
  }

Module "Number" a
  [
    Func a -> "+" -> a -> a
    Func a -> "-" -> a -> a
    Func "zero" -> a

    Func a -> "*" -> a -> a
    Func a -> "/" -> a -> a
    Func "one" -> a
  ]

Instance Number Int
  {
    x + y -> <<x+y>>

    x - y -> <<x-y>>

    zero -> <<0>>

    x * y -> <<x*y>>

    x / y -> <<x/y>>

    one -> <<1>>
  }

Instance Number Float
  {
    x + y -> <<x+y>>

    x - y -> <<x-y>>

    zero -> <<0.0>>

    x * y -> <<x*y>>

    x / y -> <<x/y>>

    one -> <<1.0>>
  }

InstanceFunc "toOpt" a => Number a => Number(Option a)

toOpt n => Number(Option a)
  {
    Func "apply" -> (a -> a -> a) -> Option a -> Option a -> Option a

    opt_x >>= x
    opt_y >>= y
    op x y -> z
    return z -> opt_z
    -----------------------
    apply op opt_x opt_y -> opt_z

    apply (+) opt_x opt_y -> opt_z
    -------------------------------
    opt_x + opt_y -> opt_z

    apply (-) opt_x opt_y -> opt_z
    -------------------------------
    opt_x - opt_y -> opt_z

    apply (*) opt_x opt_y -> opt_z
    -------------------------------
    opt_x * opt_y -> opt_z

    apply (/) opt_x opt_y -> opt_z
    -------------------------------
    opt_x / opt_y -> opt_z

    zero -> z
    ---------------
    zero -> Some z

    one -> o
    ---------------
    one -> Some o
  }

InstanceFunc "toPair" a b => Number a => Number b => Number(a,b)

toPair n_a => n_b => Number(a,b)
  {
    Func "apply" -> (a -> a -> a) -> (b -> b -> b) -> a,b -> a,b -> a,b

    op_a a1 a2 -> a3
    op_b b1 b2 -> b3
    -------------------------------------------
    apply op_a op_b (a1,b1) (a2,b2) -> (a3,b3)

    apply (+) (+) pair_x pair_y -> pair_z
    --------------------------------------
    pair_x + pair_y -> pair_z

    apply (-) (-) pair_x pair_y -> pair_z
    --------------------------------------
    pair_x - pair_y -> pair_z

    apply (*) (*) pair_x pair_y -> pair_z
    --------------------------------------
    pair_x * pair_y -> pair_z

    apply (/) (/) pair_x pair_y -> pair_z
    --------------------------------------
    pair_x / pair_y -> pair_z

    zero -> z_a
    zero -> z_b
    ----------------
    zero -> z_a,z_b

    one -> o_a
    one -> o_b
    ---------------
    one -> o_a,o_b
  }


Func "run" -> Unit

run -> unit

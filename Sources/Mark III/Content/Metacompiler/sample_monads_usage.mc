import Prelude
import BasicMonads

ModuleFunc "sample_monads_usage" => Number => Module

option id => opt
state opt (N*N) => s
--
sample_monads_usage N => Module {
  TypeFunc "Stmt" => * => *
  Mem 'a => MCons^s 'a

  Func "fst" -> Stmt Num^N
  getState >>=^s (x,y)
  --
  fst -> return^s x

  Func "snd" -> Stmt Num^N
  getState >>=^s (x,y)
  --
  snd -> return^s y

  Func Num^N -> "//" -> Num^N -> Stmt Num^N

  zero^N -> z
  (if y <> z then (return^opt (x /^N y))
   else fail^opt) -> res
  --
  x // y -> return^s res

  Func "run" -> Stmt Num^N

  fst >>=^s x
  snd >>=^s y
  x // y >>=^s z
  --
  run -> return^s z
}


Func "main" -> Int -> Int -> (Option Int)

run^(sample_monads_usage int) (x,y) -> (res,x',y')
--
main x y -> res

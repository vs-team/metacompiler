import Prelude

TypeFunc "Option" => * => *
Option 'a => Unit | 'a

TypeFunc "OptionT" => (*=>*) => * => *
OptionT 'M 'a => Option ('M 'a)

ModuleFunc "option" => Monad => Monad

either M Unit => e
--
"option" M => Module (Monad(OptionT MCons^M)) {
  inherit e

  Func "Some" -> 'a -> OptionT MCons^M 'a
  Some x -> Right(return^M x)

  Func "None" -> ResultT MCons^M 'a
  None -> Left Unit

  Func "fail" -> ResultT MCons^M 'a
  fail -> None
}

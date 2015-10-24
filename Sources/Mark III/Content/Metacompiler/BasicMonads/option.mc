import Prelude

TypeFunc "Option" => * => *
Option 'a => Unit | 'a

TypeFunc "OptionT" => (*=>*) => * => *
OptionT 'M 'a => Option ('M 'a)

ModuleFunc "option" => Monad 'M => Monad (OptionT 'M)

either M Unit => e
--
"option" M => Module {
  inherit e

  Func "Some" -> 'a -> OptionT 'M 'a
  Some x -> Right(return^M x)

  Func "None" -> ResultT 'M 'a
  None -> Left Unit

  Func "fail" -> ResultT 'M 'a
  fail -> None
}

import Prelude

TypeFunc "Option" => * => *
Option 'a => Unit | 'a

Func "Some" -> 'a -> Option 'a
Some x -> Right x

Func "None" -> Option 'a
None -> Left Unit

TypeFunc "OptionT" => (*=>*) => * => *
OptionT 'M 'a => 'M(Option 'a)

ModuleFunc "option" => Monad => Monad

either M Unit => e
--
"option" M => Module (Monad(OptionT MCons^M)) {
  inherit e
}

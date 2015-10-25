import Prelude

TypeFunc "ResultT" => (*=>*) => * => *
ResultT 'M 'a => String | ('M 'a)

ModuleFunc "result" => Monad => Monad

either M String => e
--
"result" M => Module (Monad (ResultT MCons^M)) {
  inherit e

  Func "Done" -> 'a -> ResultT MCons^M 'a
  Done x -> Right(return^M x)

  Func "Error" -> String  -> ResultT MCons^M 'a
  Error -> Left

  Func "fail" -> String -> ResultT MCons^M 'a
  fail -> Error
}

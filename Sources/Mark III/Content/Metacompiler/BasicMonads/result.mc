import Prelude

TypeFunc "ResultT" => (*=>*) => * => *
ResultT 'M 'a => String | ('M 'a)

ModuleFunc "result" => Monad 'M => Monad (ResultT 'M)

either M String => e
--
"result" M => Module {
  inherit e

  Func "Done" -> 'a -> ResultT 'M 'a
  Done x -> Right(return^M x)

  Func "Error" -> String  -> ResultT 'M 'a
  Error -> Left

  Func "fail" -> String -> ResultT 'M 'a
  fail -> Error
}

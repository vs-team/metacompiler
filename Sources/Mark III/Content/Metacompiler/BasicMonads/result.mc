import Prelude

TypeFunc "ResultT" => (*=>*) => * => *
ResultT 'M 'a => 'M(String | 'a)

TypeFunc "result" => Monad => Monad

either M String => e
----------------------
result M => Monad (ResultT MCons^M) {
  inherit e

  Func "fail" -> String -> ResultT MCons^M 'a
  fail msg -> fail^e msg
}
 

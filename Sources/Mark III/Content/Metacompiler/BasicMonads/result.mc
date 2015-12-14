import prelude
import tryableMonad
import monad

TypeAlias "ResultT" => (*=>*) => * => *
ResultT 'M 'a => M('a | String )

TypeFunc "result" => Monad => TryableMonad

either 'M [String] => e
----------------------
result 'M => TryableMonad (ResultT MCons^'M) {
    inherit e

    Func "fail" -> String -> ResultT MCons^'M 'a
    fail msg -> fail^e msg
  }

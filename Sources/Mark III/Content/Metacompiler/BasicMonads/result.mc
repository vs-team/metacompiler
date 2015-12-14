import prelude
import tryableMonad
import Monad

TypeAlias "ResultT" => (*=>*) => * => *
ResultT 'M 'a => M('a | String )

TypeFunc "result" => Monad => TryableMonad

either 'M msg => e
----------------------
result 'M => TryableMonad (ResultT MCons^'M) {
    inherit e

    Func "fail" -> String -> ResultT MCons^'M 'a
    fail msg -> fail^e msg
  }

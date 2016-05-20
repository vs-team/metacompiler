
Func "lambdatest" -> a -> res #> R 123

Func "priass" -> a -> res #> 123 R
Func "ass" -> a -> res #> R
Func "pri" -> a -> res #> 456

Func "none" -> a -> res

a -> b
(\ a -> b) -> lama
(\ c ->
   c -> d
   d -> e
   e) -> res
----------
lambdatest a -> res
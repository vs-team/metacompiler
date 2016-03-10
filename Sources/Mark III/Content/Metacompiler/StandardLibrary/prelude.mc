import number
import System

Data "unit" -> Unit
Data 'a -> "," -> 'b -> 'a * 'b    #> 5
Data "Left" -> 'a -> 'a | 'b       #> 5
Data "Right" -> 'b -> 'a | 'b      #> 5
Data "test" -> Int^builtin -> 'a -> Test<'a>

Data "then" -> Then
Data "else" -> Else

Func "if" -> Boolean -> Then -> 'a -> Else -> 'a -> 'a
if True^builtin then f else g -> f
if False^builtin then f else g -> g

TypeFunc "int" => Number
int => Number System^Int {
  x + y -> add^builtin x y
  x - y -> sub^builtin x y
  identityAdd -> 0

  x * y -> mul^builtin x y
  x / y -> divsigned^builtin x y
  identityMul -> 1
}

TypeFunc "float" => Number
float => Number System^Float {
  x + y -> fadd^builtin x y
  x - y -> fsub^builtin x y
  identityAdd -> 0.0

  x * y -> fmul^builtin x y
  x / y -> fdivsigned^builtin x y
  identityMul -> 1.0
}

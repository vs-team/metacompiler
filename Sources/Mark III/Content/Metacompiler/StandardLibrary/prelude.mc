import number
import System

Data "unit" -> Unit
Data 'a -> "," -> 'b -> 'a * 'b    #> 5
Data "Left" -> 'a -> 'a | 'b       #> 5
Data "Right" -> 'b -> 'a | 'b      #> 5
Data "test" -> Int32^System -> 'a -> Test<'a>

Data "then" -> Then
Data "else" -> Else

Func "if" -> Boolean^builtin -> Then -> 'a -> Else -> 'a -> 'a
if True^builtin then f else g -> f
if False^builtin then f else g -> g


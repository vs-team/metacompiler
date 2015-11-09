import number

Data "Unit" -> Unit
Data 'a -> "," -> 'b -> 'a * 'b
Data "Left" -> 'a -> 'a | 'b
Data "Right" -> 'b -> 'a | 'b
Data "True" -> Boolean
Data "False" -> Boolean

Data "then" -> Then
Data "else" -> Else
TypeFunc "if" => Boolean => Then => 'a => Else => 'a => 'a

if True then f else g => f

if False then f else g => g

TypeFunc "int" => Number

int => Number Int {
    Num => Int
    
    x + y -> IntPlus^Primitives x y
    x - y -> IntMinus^Primitives x y
    zero -> 0
    
    x * y -> IntTimes^Primitives x y
    x / y -> IntDividedBy^Primitives x y
    one -> 1
  }

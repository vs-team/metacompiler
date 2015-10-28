import Number

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

  x + y -> Primitives.IntPlus x y
  x - y -> Primitives.IntMinus x y
  zero -> 0

  x * y -> Primitives.IntTimes x y
  x / y -> Primitives.IntDividedBy x y
  one -> 1
}

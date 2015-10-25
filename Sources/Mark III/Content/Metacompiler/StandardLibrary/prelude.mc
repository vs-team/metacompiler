import Number

Data "Unit" -> Unit
Data 'a -> "," -> 'b -> 'a * 'b
Data "Left" -> 'a -> 'a | 'b
Data "Right" -> 'b -> 'a | 'b

Data "then" -> Then
Data "else" -> Else
TypeFunc "if" => Boolean => Then => 'a => Else => 'a => 'a

(if True then f else g) x => f x

(if False then f else g) x => g x


ModuleFunc "int" => Number Int

int => Module {
  Num => Int

  x + y -> Primitives.IntPlus x y
  x - y -> Primitives.IntMinus x y
  zero -> 0

  x * y -> Primitives.IntTimes x y
  x / y -> Primitives.IntDividedBy x y
  one -> 1
}

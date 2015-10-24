Data "Unit" -> Unit
Data 'a -> "," -> 'b -> 'a * 'b
Data "Left" -> 'a -> 'a | 'b
Data "Right" -> 'b -> 'a | 'b

Data "then" -> Then
Data "else" -> Else
TypeFunc "if" => Boolean => Then => 'a => Else => 'a => 'a

(if True then f else g) x => f x

(if False then f else g) x => g x

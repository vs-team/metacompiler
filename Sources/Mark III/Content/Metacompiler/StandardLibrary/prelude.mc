import number

Data "Unit" -> Unit                #> 12345 R
Data 'a -> "," -> 'b -> 'a * 'b    #> 1234  R
Data "Left" -> 'a -> 'a | 'b       #> 123   R
Data "Right" -> 'b -> 'a | 'b      #> 12    R
Data "True" -> Boolean             #> 1     R
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
import number

Data "Unit" -> Unit                }> 10 
Data 'a -> "," -> 'b -> 'a * 'b    #> 20 R
Data "Left" -> 'a -> 'a | 'b       #> 20 
Data "Right" -> 'b -> 'a | 'b      #> 20 
Data "True" -> Boolean             #> 10 
Data "False" -> Boolean            #> 10 

Data "then" -> Then                #> 30 R
Data "else" -> Else                #> 30 R
TypeFunc "if" => Boolean => Then => 'a => Else => 'a => 'a  #> 30 R

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

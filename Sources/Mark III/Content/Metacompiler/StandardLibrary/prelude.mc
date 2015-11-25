import number

Data "Unit" -> Unit                
Data 'a -> "," -> 'b -> 'a * 'b    #> 5 
Data "Left" -> 'a -> 'a | 'b       #> 5 
Data "Right" -> 'b -> 'a | 'b      #> 5 
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
    
    x + y -> IntAdd^primitives x y
    x - y -> IntSub^primitives x y
    zero -> 0
    
    x * y -> IntMul^primitives x y
    x / y -> IntDiv^primitives x y
    one -> 1
  }

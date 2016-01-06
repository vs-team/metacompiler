import number
import boolean

Data "Unit" -> Unit                
Data 'a -> "," -> 'b -> 'a * 'b    #> 5 
Data "Left" -> 'a -> 'a | 'b       #> 5 
Data "Right" -> 'b -> 'a | 'b      #> 5 

Boolean => Boolean {
    True -> TrueBoolean^system
    False -> FalseBoolean^system
  }

Data "then" -> Then                
Data "else" -> Else                

Func "if" -> Boolean -> Then -> 'a -> Else -> 'a -> 'a  
if True then f else g -> f
if False then f else g -> g

TypeFunc "int" => Number 
int => Number Int^system {
    x + y -> IntAdd^system x y
    x - y -> IntSub^system x y
    zero -> 0
    
    x * y -> IntMul^system x y
    x / y -> IntDiv^system x y
    one -> 1
  }

TypeFunc "float" => Number 
float => Number Float^system {
    x + y -> FloatAdd^system x y
    x - y -> FloatSub^system x y
    zero -> 0
    
    x * y -> FloatMul^system x y
    x / y -> FloatDiv^system x y
    one -> 1
  }

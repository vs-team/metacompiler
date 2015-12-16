import number

Data "Unit" -> Unit                
Data 'a -> "," -> 'b -> 'a * 'b    #> 5 
Data "Left" -> 'a -> 'a | 'b       #> 5 
Data "Right" -> 'b -> 'a | 'b      #> 5 
Data "True" -> Boolean             
Data "False" -> Boolean             

Data "then" -> Then                
Data "else" -> Else                

Func "if" -> Boolean -> Then -> 'a -> Else -> 'a -> 'a  
if True then f else g -> f
if False then f else g -> g

TypeFunc "int" => Number 

int => Number Int^primitive {
    x + y -> IntAdd^primitive x y
    x - y -> IntSub^primitive x y
    zero -> 0
    
    x * y -> IntMul^primitive x y
    x / y -> IntDiv^primitive x y
    one -> 1
  }

TypeFunc "float" => Number 

float => Number Float^primitive {
    x + y -> FloatAdd^primitive x y
    x - y -> FloatSub^primitive x y
    zero -> 0
    
    x * y -> FloatMul^primitive x y
    x / y -> FloatDiv^primitive x y
    one -> 1
  }

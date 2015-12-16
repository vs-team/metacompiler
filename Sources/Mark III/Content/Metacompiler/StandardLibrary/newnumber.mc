TypeFunc "MonoidAdd" => * => Module
MonoidMul 'a => Module {
    Func 'a -> "+" -> 'a -> 'a #> 60
    Func "identity" -> 'a 
  }

TypeFunc "MonoidMul" => * => Module
MonoidMul 'a => Module {
    Func 'a -> "*" -> 'a -> 'a #> 70
    Func "identity" -> 'a
  }

TypeFunc "GroupAdd" => * => MonoidAdd => Module
Ring 'a 'M => Module{
	  inherit 'M 'a
	  Func 'a -> "-" -> 'a -> 'a #> 60
  }

TypeFunc "GroupMul" => * => MonoidMul => Module
Ring 'a 'M => Module{
	  inherit 'M 'a
  	Func 'a -> "/" -> 'a -> 'a #> 70
  }

TypeFunc "Number" => * => GroupAdd => GroupMul => Module
Number 'a 'A 'M => Module{
  	inherit 'A 'a
	  inherit 'M 'a
  }

TypeFunc "Vector" => * => GroupAdd => MonoidMul=> Module
Vector 'a 'A 'M => Module{
    inherit 'A 'a
    inherit 'M 'a
  } 

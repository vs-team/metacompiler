TypeFunc "Number" => * => Signature

Number 'a => Signature {
    TypeFunc "Num" => *

    Func 'a -> "+" -> 'a -> 'a #> 60  
    Func 'a -> "-" -> 'a -> 'a #> 60
    Func "zero" -> 'a

    Func 'a -> "*" -> 'a -> 'a #> 70
    Func 'a -> "/" -> 'a -> 'a #> 70
    Func "one" -> 'a 
  }
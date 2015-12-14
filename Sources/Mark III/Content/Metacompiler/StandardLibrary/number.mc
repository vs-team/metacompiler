TypeFunc "Number" => * => Module

Number 'a => Module {
    TypeFunc "Num" => *
    Num => 'a

    Func 'a -> "+" -> 'a -> 'a #> 60  
    Func 'a -> "-" -> 'a -> 'a #> 60
    Func "zero" -> 'a

    Func 'a -> "*" -> 'a -> 'a #> 70
    Func 'a -> "/" -> 'a -> 'a #> 70
    Func "one" -> 'a 
  }

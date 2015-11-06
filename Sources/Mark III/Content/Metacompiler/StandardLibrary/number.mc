TypeFunc "Number" => * => Signature

Number 'a => Signature {
    TypeFunc "Num" => *
    
    Func 'a -> "+" -> 'a -> 'a
    Func 'a -> "-" -> 'a -> 'a
    Func "zero" -> 'a
    
    Func 'a -> "*" -> 'a -> 'a
    Func 'a -> "/" -> 'a -> 'a
    Func "one" -> 'a
  }

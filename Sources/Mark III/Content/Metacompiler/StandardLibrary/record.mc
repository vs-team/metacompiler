TypeFunc "empty" => Record
empty => Record [] {
  fields -> Unit
  cons -> Unit
  make () -> ()
}

TypeFunc "field" => String => * => Record => Record
field l f rs => Record {
  fields -> f x fields^rs
  cons -> f x cons^rs
  make (x,xs) -> (x, (make^rs xs))
  label -> l
  field -> f
  rest -> rs

  Func "getter" -> String -> Record 
  getter l' rs -> (cons -> getter l' rest^rs)

  l' -> label^rs
  --
  getter l' rs -> (cons -> fields^rs)

  Func "get" -> String -> cons^(getter l rs)
  get l' -> get^rs l'
  get l -> fst^cons

  Func "set" -> * -> cons^(setter l f rs)
  set x -> fst^cons
}

TypeFunc "Record" => Module
Record => Module {
  Func "fields" -> *
  Func "cons" -> *
  Func "make" -> fields -> cons
  Func "label" -> String
  Func "field" -> *
  Func "rest" -> Record
}

TypeFunc "Setter" => Module
Setter => Module { cons => * {
  Func "setter" -> String -> Record -> Setter

  l' -> label^rs
  --
  setter l' rs -> {cons -> fields^rs}

  setter l' rs -> {cons -> setter l' rest^rs}
}

TypeFunc "empty" => Record
empty => Record [] {
  fields -> Unit
  cons -> Unit
  make () -> ()
  first -> Unit
}

TypeFunc "field" => String => * => Record => Record
field l f rs => Record {
  fields -> f x fields^rs
  cons -> f x cons^rs
  make (x,xs) -> (x, (make^rs xs))
  label -> l
  field -> f
  rest -> rs
  first -> cons

  Func "getter" -> String -> Record 
  getter l' rs -> (cons -> getter l' rest^rs)

  l' -> label^rs
  --
  getter l' rs -> (cons -> fields^rs)

  Func "get" -> String -> cons^(getter l rs)
  get l' -> get^rs l'
  get l -> first^cons

  $$fix setter
  Func "setter" -> String -> Record 
  setter l' rs -> (cons -> setter l' rest^rs)

  l' -> label^rs
  --
  setter l' rs -> (cons -> fields^rs)

  Func "set" -> String -> cons^(setter l rs)
  set l' -> set^rs l'
  set l -> fst^cons

  Func "set" -> * -> cons^(setter l f rs)
  set x -> first^cons
}

TypeFunc "Record" => Module
Record => Module {
  Func "fields" -> *
  Func "cons" -> *
  Func "make" -> fields -> cons
  Func "label" -> String
  Func "field" -> *
  Func "rest" -> Record
  Func "first" -> Record
}

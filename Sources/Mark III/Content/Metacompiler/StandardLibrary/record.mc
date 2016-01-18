TypeFunc "empty" => Record
empty => Record [] {
  fields -> Unit
  cons -> Unit
  make () -> ()
}

TypeFunc "field" => String => * => Record => Record
field l f Ms => Record {
  fields -> f x fields^Ms
  cons -> f x cons^Ms

  make (x,xs) -> (x, (make^Ms xs))
  label -> l
  field -> f
  rest -> Ms

  Func "get" -> cons^(get ta l f Ms)

  l -> l'
  --
  get l' -> fst^cons

  I l' -> I^M l'
  get l' -> get l

  $$ do set stuff
  Func "set" -> String -> cons
  set l -> cons
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

TypeFunc "Getter" => Module
Getter => Module { cons => * {
  Func getter -> Str_p -> Record -> Getter

  l' -> label^M
  --
  getter l' M -> {
    cons -> fields^M
  }

  getter l' M -> {
    cons -> getter l' rest^M
  }
}

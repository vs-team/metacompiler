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

  Func "get" -> cons^(getter l f rs)
  get -> get^rs l

  l -> l'
  --
  get l' -> fst^cons

  $$ do set stuff
  Func "set" -> cons
  set -> set^rs l
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
  getter l' rs -> {cons -> fields^rs}

  getter l' rs -> {cons -> getter l' rest^rs}
}

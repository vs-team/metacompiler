import prelude

TypeFunc "Record" => Module
Record => Module {
  TypeFunc "fields" => *
  TypeFunc "cons" => *
  TypeFunc "field" => *
  Func "make" -> fields -> cons
  Func "label" -> String
  Func "rest" -> Record
  Func "first" -> Record
}

TypeFunc "empty" => Record
empty => Record {
  fields -> Unit
  cons -> Unit
  field -> Unit
  make () -> ()
  label -> Unit
  rest -> Unit
  first -> Unit
}

TypeFunc "record" => String => * => Record => Record
record l f rs => Record {
  fields -> (f fields^rs)
  cons -> (f cons^rs)
  field -> f

  make (x,xs) -> (x, (make^rs xs))
  make x -> x

  label -> l
  rest -> rs
  first -> record (label field rest)

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


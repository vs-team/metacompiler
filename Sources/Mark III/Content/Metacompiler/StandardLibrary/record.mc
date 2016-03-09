import prelude

TypeFunc "Record" => Module
Record => Module {
  Func "fields" -> 'a
  Func "field" -> 'a
  Func "label" -> String
  Func "rest" -> Record

  Func "get" -> String -> Record -> Record
  (if (l = label^rs)
    rs
    else
    get l rest^rs) -> res
  -----------------------
  get l rs -> res

  Func "set" -> String -> 'a -> Record -> Record
  (if (l = label^rs)
    record l f rs
    else
    set l f rest^rs) -> res
  -------------------------
  set l f' rs -> res
}

TypeFunc "empty" => Record
empty => Record {
  fields -> Unit
  field -> Unit
  label -> Unit
  rest -> Unit
}

TypeFunc "record" => String => * => Record => Record
record l f rs => Record {
  fields -> (f,fields^rs)
  field -> f
  label -> l
  rest -> rs
}


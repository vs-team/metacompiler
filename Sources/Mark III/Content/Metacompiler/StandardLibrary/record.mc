import prelude

TypeFunc "Record" => Module
Record => Module {
  TypeFunc "cons"   => *
  TypeFunc "fields" => *
  TypeFunc "field"  => *
  TypeFunc "label"  => String
  TypeFunc "rest"   => Record

  TypeFunc "FieldType" => String => Record => *
  FieldType l r => field^(get l r)

  TypeFunc "get" => String => Record => Record
  (if (l = label^rs) then
    rs
  else
    get l rest^rs) => res
  -----------------------
  get l rs => res

  FieldType 'l 'rs => ft
  ----------------------
  TypeFunc "set" => 'l => 'rs => ft => cons^'rs

  (if (l = label^rs) then
    record l f rs
  else
    set l f rest^rs) => res
  -------------------------
  set l f' rs => res
}

TypeFunc "empty" => Record
empty => Record {
  cons   => Unit
  fields => Unit
  field  => Unit
  label  => Unit
  rest   => Unit
}

TypeFunc "record" => String => * => Record => Record
record l f rs => Record {
  cons   => (l,f,rs)
  fields => (f,fields^rs)
  field  => f
  label  => l
  rest   => rs
}

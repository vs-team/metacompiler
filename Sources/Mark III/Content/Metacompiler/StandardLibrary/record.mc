import prelude

TypeFunc "Record" => Module
Record => Module {
  TypeFunc "cons"   => #a
  TypeFunc "fields" => #a
  TypeFunc "field"  => #a
  TypeFunc "label"  => String
  TypeFunc "rest"   => Record

  TypeFunc "FieldType" => String => Record => #a
  FieldType l r => field^(get l r)

  TypeFunc "get" => String => Record => Record
  (if (l = label^rs) then
    rs
  else
    get l rest^rs) => res
  -----------------------
  get l rs => res

  TypeFunc "set" => 'l => #a => 'rs => cons^'rs
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

TypeFunc "record" => String => #a => Record => Record
record l f rs => Record {
  cons   => (l,f,rs)
  fields => (f,fields^rs)
  field  => f
  label  => l
  rest   => rs
}

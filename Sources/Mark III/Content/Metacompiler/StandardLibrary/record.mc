import prelude

TypeFunc "Record" => Module
Record => Module {
  TypeFunc "Label"  => String
  TypeFunc "Field"  => *
  TypeFunc "Rest"   => Record

  TypeFunc "get" => String => Record => Record
  (if (l = label^rs) then
    rs
  else
    get l rest^rs) => res
  -----------------------
  get l rs => res

  TypeFunc "set" => String => * => Record => Record
  (if (l = label^rs) then
    record l f rs
  else
    set l f rest^rs) => res
  -------------------------
  set l f rs => res
}

TypeFunc "EmptyRecord" => Record
EmptyRecord => Record{
  Field => Unit
  Label => Unit
  Rest => Unit
}

TypeFunc "RecordEntry" => String => * => Record => Record
RecordEntry label field rest => Record{
  Field => field
  Label => label
  Rest => rest
}

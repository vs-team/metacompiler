import prelude

TypeFunc "Record" => Module
Record => Module {
  TypeFunc "Label"  => String
  TypeFunc "Field"  => Type
  TypeFunc "Rest"   => Record

  TypeFunc "getType" => String => Record => Type
  (if (l = Label^rs) then
    Field^rs
  else
    getType l rest^rs) => res
  -----------------------
  getType l rs => res

  TypeFunc "setType" => String => Type => Record => Record
  (if (l = label^rs) then
    setType l f rs
  else
    setType l f rest^rs) => res
  -------------------------
  setType l f rs => res
}

TypeFunc "EmptyRecord" => Record
EmptyRecord => Record{
  Field => Unit
  Label => Unit
  Rest => Unit
}

TypeFunc "RecordEntry" => String => Type => Record => Record
RecordEntry label field rest => Record {
  Field => field
  Label => label
  Rest => rest

  TypeFunc "Repr" => Field * Repr^Rest
  TypeFunc "get" => s:String => (Repr -> (GetType s this))
  Func "cons" -> Field -> Repr^Rest -> Repr
  cons x xs -> (x,xs)

  (if (l = rs^Label) then
    x
  else
    get^rest l xs)
  -------------------
  get l (x,xs)
 }



 ------------------------------
 get^ship "Shield" ship => res

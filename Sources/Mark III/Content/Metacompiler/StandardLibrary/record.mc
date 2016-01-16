TypeFunc "empty" => Record
empty => Record [] {
  fields = Unit
  Cons = Unit
  make () = ()
}

TypeFunc "field" => String => * => Record => Record
field l f Ms => Record {
  fields => f x fields^Ms
  cons => f x cons^Ms
  make (x,xs) -> (x, (make^Ms xs))
  get => cons^(get ta l f Ms)
  label => l
  field => f
  rest => Ms

  l => l'
  --
  get l' => fst^cons

  I l' => I^M l'
  get l' => get l
  $$ do set stuff
}

TypeFunc "Record" => Module
Record => Module{
  fields -> *
  cous -> *
  make -> fields -> cous
  label => String
  field => *
  Rest => Record
}

TypeFunc "Getter" => Module
Getter => { Cons => * {
  Func getter => Str_p => Record => Getter

  l' => label^M
  --
  getter l' M => {
    Cons => fields^M
  }

  getter l' M => {
    Cons => getter l' Rest^M
  }
}

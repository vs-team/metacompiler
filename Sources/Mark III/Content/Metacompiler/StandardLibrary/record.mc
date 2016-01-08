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
    res^t => Ms
    l => l'
    --
    get l' => fs^(t cons)
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


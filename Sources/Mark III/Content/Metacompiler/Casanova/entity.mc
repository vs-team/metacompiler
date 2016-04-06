import prelude

TypeFunc "Updatable" => #a => Module
Updatable t => Module{
  TypeFunc "Cons" => Cons^t
  Func "update" -> Cons -> float^prelude -> Cons
}

TypeFunc "Rule" => Updatable => Module
Rule e => Module {
  Func "apply" -> Cons^e -> float^prelude -> Cons^e
}

TypeFunc "RuleEntity" => Updatable t => Rule t => Updatable r
RuleEntity e r => Updatable {
  update x dt -> update^e(apply^r x dt) dt
}

TypeFunc "TupleUpdatable" => Updatable t1 => Updatable t2 => Updatable (t1 #a t2)
TupleUpdatable e1 e2 => Updatable{
  TypeFunc "Cons" => Cons^t1 #a Cons^t2

  update^e1 x dt -> x1
  update^e2 y dt -> x2
  --------------------------
  update (x,y) dt -> (x1,x2)
}

TypeFunc "UnionUpdatable" => Updatable t1 => Updatable t2 => Updatable (t1 | t2)
UnionEntity e1 e2 => Updatable {

  TypeFunc "Cons" => Cons^t1 | Cons^t2


  do^match x with (\x -> update^t1 x dt) (\y -> update^t2 y dt) -> y
  ----------------------------------------------------------------------
  update x dt -> y
}

TypeFunc "IdUpdatable" => #a => Updatable
IdUpdatable t => Updatable {
  TypeFunc "Cons" -> Cons^t

  update x dt -> x
}

TypeFunc "EntityField" => Module
EntityField => Module{
  TypeFunc "Cons" => #a
  TypeFunc "Fields" => #a
  TypeFunc "Field" => #a
  TypeFunc "Label" => String
  TypeFunc "Rest" => EntityField

  Func "update" -> Cons -> 'a -> Cons
  update Empty dt -> Empty

  TypeFunc "get" => String => EntityField => EntityField
  (if (l = label^rs) then
    rs
  else
    get l rest^rs) => res
  -----------------------
  get l rs => res

  TypeFunc "set" => String => #a => EntityField => EntityField
  (if (l = label^rs) then
    Entity l f rs
  else
    set l f rest^rs) => res
  -------------------------
  set l f' rs => res
}

TypeFunc "Empty" => EntityField {
  Cons => unit
  Fields => unit
  Field => unit
  Label => unit
  Rest => unit
}

TypeFunc "Entity" => String => #a => EntityField => EntityField
Entity label field rest => EntityField{
  Cons => (label,field,rest)
  Fields => (field,Fields^rest)
  Field => field
  Label => label
  Rest => rest
}

TypeFunc "UpdatableEntity" => Entity => Entity
UpdatableEntity e => Entity (Label^e Field^e Rest^e) {

  e -> l f r
  update^f Cons^f dt -> f1
  update^r Cons^r dt -> r1
  Entity label f1 r1 -> res
  -----------------------------------------
  update e dt -> res
}

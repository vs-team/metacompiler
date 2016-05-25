import prelude
import System

TypeFunc "Updatable" => #a => Module
Updatable t => Module{
  TypeFunc "Cons" => Cons^t
  Func "update" -> Cons -> float^System -> Cons
}

TypeFunc "Rule" => #a => Module
Rule e => Module {
  TypeFunc "apply" -> Cons^e -> float^System -> Cons^e
}

TypeFunc "TupleUpdatable" => Updatable => Updatable => Updatable
TupleUpdatable e1 e2 => e{
  TypeFunc "Cons" => Cons^e1 * Cons^e2

  update^e1 x dt -> x1
  update^e2 y dt -> x2
  --------------------------
  update (x,y) dt -> (x1,x2)
}

TypeFunc "UnionUpdatable" => Updatable => Updatable => Updatable
UnionEntity e1 e2 => e{
  TypeFunc "Cons" => Cons^e1 | Cons^e2

  do^match x with (\x -> update^e1 x dt) (\y -> update^e2 y dt) -> y
  ----------------------------------------------------------------------
  update x dt -> y
}

TypeFunc "IdUpdatable" => #a => Updatable
IdUpdatable t => e{
  TypeFunc "Cons" -> Cons^t

  update x dt -> x
}

TypeFunc "EntityField" => Module
EntityField => Module{
  TypeFunc "Cons" => #a
  Cons => (label,field,rest)
  TypeFunc "Fields" => #b
  Fields => (field,Fields^rest)
  TypeFunc "Field" => #c
  TypeFunc "Label" => String
  TypeFunc "Rest" => EntityField

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

TypeFunc "Empty" => EntityField
Empty => EntityField{
  Field => unit
  Label => unit
  Rest => unit
}

TypeFunc "Entity" => String => #a => EntityField => EntityField
Entity label field rest => EntityField{
  Field => field
  Label => label
  Rest => rest
}

TypeFunc "UpdatableEntity" => Entity => Entity
UpdatableEntity e => e{
  TypeFunc "update" -> Cons -> 'a -> Cons
  update Empty dt -> Empty
}

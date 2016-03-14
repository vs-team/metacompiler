import prelude

TypeFunc "Entity" => * => Module
Entity t => Module{
  TypeFunc "Cons" => Cons^t
  Func "update" -> Cons -> Num float32^System -> Cons
}

TypeFunc "Rule" => Entity => Module
Rule e => Module {
  Func "apply" -> Cons^e -> Num float32^System -> Cons^e
}

TypeFunc "RuleEntity" => Entity t => Rule t => Entity r
RuleEntity e r => Entity {
  update x dt -> update^e(apply^r x dt) dt
}

TypeFunc "TupleEntity" => Entity t1 => Entity t2 => Entity (t1 * t2) 
TupleEntity e1 e2 => Entity{
  TypeFunc "Cons" => Cons^t1 * Cons^t2


  update^e1 x dt -> x1
  update^e2 y dt -> x2
  --------------------------
  update (x,y) dt -> (x1,x2)
}

TypeFunc "UnionEntity" => Entity t1 => Entity t2 => Entity (t1 | t2)
UnionEntity e1 e2 => Entity {
  
  TypeFunc "Cons" => Cons^t1 | Cons^t2


  do^match x with (\x -> update^t1 x dt) (\y -> update^t2 y dt) -> y
  ----------------------------------------------------------------------
  update x dt -> y
}

TypeFunc "IdEntity" => Entity t => Entity t
IdEntity e => {
  TypeFunc "Cons" -> Cons^t

  update x dt -> x
}
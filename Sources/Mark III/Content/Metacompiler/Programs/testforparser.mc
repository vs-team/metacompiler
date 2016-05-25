
TypeFunc "test" => int => module

test a => bla {
  Data "S" -> pnum -> pnum
  Data "Z" -> pnum
  Func pnum -> "add" -> pnum -> pnum

  a -> Z
  -------
  a add b -> b

  a -> S c
  c add b -> d
  S d -> res
  ---------
  a add b -> res
  
}

Func "lamtest" -> bla -> vla

(\ (x):int (y):bool -> x) -> res
-------
lamtest a -> res
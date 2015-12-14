import prelude

TypeFunc "Convertible" => 'keys => 'values => Signature

Convertible keys values => Signature{
  TypeFunc "get" => keys => values
  TypeFunc "set" => k => av => bv
}

TypeFunc "convertible" => 'a => 'b => Convertible

convertible Unit Unit => Convertible{

  get a => ()

  set k v => convertible () ()
}

convertible ks vs => c
------------------
convertible (k, ks) (v, vs) => Convertible{

  ka == k
  -------------------
  get (ka, kas) => (v, get^c kas)

  ka != k
  ---
  get (ka, kas) => get^c kas


  ka == k
  set^c kas vas => c_res
  ---
  set (ka, kas) (va, vas) => convertible (k, ks) (va, get^c_res ks)

  ka != k
  set^c (ka, kas) values => c_res
  ------
  set (ka, kas) values => convertible (k, ks) (v, get^c_res ks)
}
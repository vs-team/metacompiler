TypeFunc "Convertible" => * => * => Signature

Convertible 'k 'v => Signature{
	TypeFunc "get" => 'k => 'v
	TypeFunc "set" => 'k => 'av => 'bv
}

TypeFunc "convertible" => 'a => 'b => Convertible

convertible Unit Unit => Convertible{
	
	get a => ()

	set k v => convertible () ()
}

convertible ks vs => c
--
convertible (k, ks) (v, vs) => Convertible{
	
	ka == k
	-------------------
	get (ka, kas) => (v, get^c kas)
	
	ka != k
	---
	get (ka, kas) => get^c kas


	ka == k
	set^c kas vas => c'
	---
	set (ka, kas) (va, vas) => convertible (k, ks) (va, get^c' ks)

	ka != k
	set^c (ka, kas) values => c'
	------
	set (ka, kas) values => convertible (k, ks) (v, get^c' ks)
}
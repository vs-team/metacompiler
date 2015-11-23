TypeFunc "Entity" => * => * => Signature

Entity ('k * 'v) 'r => Signature{
	TypeFunc C => Convertible
	TypeFunc R => Rules
}

TypeFunc "entity" => ('k * 'ks) => ('v * 'vs) => 'r => Entity

entity (k, ks) (v, vs) r => Entity{
	C => convertible (k, ks) (v, vs)
	R => r
}
TypeFunc "Updateable" => 'w => 'a => Signature

Updateable 'w 'a => Signature{
	TypeFunc "update" => ('w * 'a) => float => 'a
}

TypeFunc "updateable" => 'w => 'a => Updateable

updateable 'w 'f => flds
--
updateable 'w Entity => Updateable{
	
	apply w C^e R^e dt => (Convertible keys values)
	---------------------------
	update (w, e) dt => Entity(keys values R^e)
}

updateable 'w 'a => u_a
updateable 'w 'b => u_b
--
updateable 'w ('a * 'b) => {
	
	update^u_a (w, a) dt => a' 
	update^u_b (w, b) dt => b'
	---------------------------
	update (w, (a, b)) dt => (a', b')
}

updateable 'w 'a => u_a
updateable 'w 'b => u_b
--
updateable 'w ('a | 'b) => {
	update^u_a (w, x) dt => res
	-----------------------------
	update (w, Left x) dt => res

	update^u_b (w, y) dt => res
	-----------------------------
	update (w, Right y) dt => res
}

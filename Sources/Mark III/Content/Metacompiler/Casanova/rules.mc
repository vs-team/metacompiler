TypeFunc "Rules" => 'w => 'f => 'r => Signature

Rules 'w 'c 'r => Signature{
	TypeFunc "apply" -> 'w -> 'c -> float -> 'r -> 'c
}

TypeFunc "rules" => 'w => 'c => 'r => Rules

TypeFunc "rule" => ('f, 'fs) => ('w -> 'f -> float -> 'f) => Rule


rules 'w 'c Unit => Rules{

	apply w c dt () => c
}

rules 'w 'c (Rule, Rules) => Rules{

	get^c fields => func_fields
	func w func_fields dt => func_fields'
	set^c fields func_fields' => c'
	apply w c' rs dt => res
	---------------------------
	apply w c dt ((rule fields func), rs) => res
}


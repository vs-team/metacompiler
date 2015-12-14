TypeFunc "Rules" => 'w => 'c => 'r => Signature

Rules 'w 'c 'r => Signature{
	
	TypeFunc "apply" -> 'w -> 'c -> float -> 'r -> 'c
}

TypeFunc "rules" => 'w => 'c => 'r => Rules

TypeFunc Rule => ('fields) => (\'values, 'world, float -> 'values) => Signature

TypeFunc "rule" => ('fields) => (\'values, 'world, float -> 'values) => Rule

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

TypeFunc SuspendableRule => 'w => 'f => 'r => Monad


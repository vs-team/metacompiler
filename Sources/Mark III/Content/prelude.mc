Data "Unit" -> Unit
Data "left" -> 'a -> 'a | 'b
Data "right" -> 'b -> 'a | 'b
Data 'a -> "," -> 'b -> 'a * 'b

TypeFunc "Option" => * => *
Option 'a => 'a | Unit

Func "Some" 'a -> 'a -> Option 'a
Some -> Left

Func "None" 'a -> Option 'a
None -> Right Unit

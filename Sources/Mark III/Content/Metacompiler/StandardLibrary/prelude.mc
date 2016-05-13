import System

Data "unit" -> Unit
TypeAlias #a => "*" => #b => tuple<#a #b>
TypeAlias #a => "|" => #b => pipe<#a #b>
Data 'a -> "," -> 'b -> 'a * 'b    #> 5
Data "Left" -> 'a -> 'a | 'b       #> 5
Data "Right" -> 'b -> 'a | 'b      #> 5

Data "then" -> Then
Data "else" -> Else

Func "if" -> Boolean^System -> Then -> 'a -> Else -> 'a -> 'a
if True^builtin then f else g -> f
if False^builtin then f else g -> g


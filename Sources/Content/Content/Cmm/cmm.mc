import System.Collections.Immutable

Func "emptyDictionary" : DictionaryOp => SymbolTable
Func SymbolTable -> "add" -> Id -> Value : DictionaryOp => SymbolTable   Priority 100
Func SymbolTable -> "lookup" -> Id : DicitonaryOp => Value   Priority 100
Func SymbolTable -> "contains" -> Id : DictionaryOp => Answer Priority 100
Func "eval" -> TableList -> Expr : RuntimeOp => EvaluationResult Priority 0
Func SymbolTable -> "defineVariable" -> Id : MemoryOp => SymbolTable Priority 300
Func "updateTables" -> TableList -> TableList -> Id -> Expr : MemoryOp => EvaluationResult Priority 0
Func "program" -> SymbolTable -> ExprList : Program => SymbolTable  Priority 0
Func "runProgram" : Test => SymbolTable Priority -10000
Func "loopFor" -> TableList -> Expr -> Expr -> Expr : RuntimeOp => EvaluationResult Priority 0


Data "$i" -> <<int>> : Value Priority 300
Data "$d" -> <<double>> : Value Priority 300
Data "$s" -> <<string>> : Value Priority 300
Data "$b" -> <<bool>> : Value Priority 300
Data "$void" : Value Priority 300

Data "$" -> <<string>> : Id Priority 300

Data Expr -> "+" -> Expr : Expr Priority 100
Data Expr -> "-" -> Expr : Expr Priority 100
Data Expr -> "*" -> Expr : Expr Priority 100
Data Expr -> "/" -> Expr : Expr Priority 100

Data Expr -> "&&" -> Expr : Expr Priority 100
Data Expr -> "||" -> Expr : Expr Priority 100
Data "!" -> Expr : Expr Priority 100
Data Expr -> "equals" -> Expr : Expr Priority 100
Data Expr -> "neq" -> Expr: Expr Priority 100
Data Expr -> "ls" -> Expr : Expr Priority 100
Data Expr -> "leq" -> Expr : Expr Priority 100
Data Expr -> "grt" -> Expr : Expr Priority 100
Data Expr -> "geq" -> Expr : Expr Priority 100

Data "t_int" : Type Priority 500
Data "t_double" : Type Priority 500
Data "t_string" : Type Priority 500
Data "t_bool" : Type Priority 500

Data "variable" -> Type -> Id : Expr Priority 10
Data Id -> "=" -> Expr : Expr Priority 10
Data "then" : Then Priority 10
Data "else" : Else Priority 10
Data "if" -> Expr -> Then -> Expr -> Else -> Expr : Expr Priority 10
Data "while" -> Expr -> Expr : Expr Priority 10
Data "for" -> Expr -> Expr -> Expr -> Expr : Expr Priority 10


Data Expr -> ";" -> ExprList : ExprList Priority 5
Data "nop" : ExprList Priority 500

Data "evalResult" -> TableList -> Value : EvaluationResult Priority 0

Data "error" -> <<string>> : Error Priority 300

Data "Yes" : Answer                                                                                                         Priority 100
Data "No"  : Answer                                                                                                         Priority 100
Data "$m" << ImmutableDictionary<Id, Value> >> : SymbolTable                                                                      Priority 300
Data SymbolTable -> "nextTable" -> TableList : TableList              Priority 10
Data "nilTable" : TableList  Priority 500 

Value is Expr
Id is Expr
Error is Value
ExprList is Expr

-------------------------------------------
emptyDictionary => $m << ImmutableDictionary<Id,Value>.Empty >>

<<M.GetKey(k)>> => v
---------------------
($m M) lookup k => v

<<M.ContainsKey(k)>> == false
<<M.Add(k,v)>> => M'
------------------------
($m M) add k v => $m M'

<<M.ContainsKey(k)>> == true
<<M.SetItem(k,v)>> => M'
------------------------
($m M) add k v => $m M'

<<M.ContainsKey(k)>> == true
---------------------------
($m M) contains k => Yes

<<M.ContainsKey(k)>> == false
---------------------------
($m M) contains k => No


emptyDictionary => M
decl_fact := variable t_int $"fact"
decl_n := variable t_int $"n"
decl_j := variable t_int $"j"
assign1 := $"fact" = $i 1
assign2 := $"n" = $i 10
assign3 := $"j" = $i 1
l_block := ($"fact" = $"fact" * $"j") ; ($"j" = $"j" + $i 1 ; nop)
l := while ($"j" leq $"n") l_block
f := for ($"j" = $i 1) ($"j" leq $"n") ($"j" = $"j" + $i 1) ($"fact" = $"fact" * $"j")
code := decl_fact ; (decl_n ; (decl_j ; (assign1 ; (assign2 ; (assign3 ; (f ;nop))))))
program M code => M'
---------------------------------------------------------------------
runProgram => M'


eval (memory nextTable nilTable) code => evalResult (memory' nextTable nilTable) $void
-----------------------------------------------------------------------------------------
program memory code => memory'


------------------------------------------------
eval tables ($i v) => evalResult tables ($i v)

-----------------------------------------------
eval tables ($d v) => evalResult tables ($d v)

-----------------------------------------------
eval tables ($s v) => evalResult tables ($s v)

-----------------------------------------------
eval tables ($b v) => evalResult tables ($b v)

symbols contains ($name) => Yes
symbols lookup ($name) => val
------------------------------------------------------------------------------------
eval (symbols nextTable tables) ($name) => evalResult (symbols nextTable tables) val

symbols contains ($name) => No
eval tables ($name) => evalResult tables' val
------------------------------------------------------------------------------------
eval (symbols nextTable tables) ($name) => evalResult (symbols nextTable tables') val

eval tables expr1 => evalResult tables' ($i val1)
eval tables' expr2 => evalResult tables'' ($i val2)
<<val1 + val2>> => arithmeticResult
----------------------------------------------------------------------
eval tables expr1 + expr2 => evalResult tables'' ($i arithmeticResult)

eval tables expr1 => evalResult tables' ($i val1)
eval tables' expr2 => evalResult tables'' ($i val2)
<<val1 - val2>> => arithmeticResult
----------------------------------------------------------------------
eval tables expr1 - expr2 => evalResult tables'' ($i arithmeticResult)

eval tables expr1 => evalResult tables' ($i val1)
eval tables' expr2 => evalResult tables'' ($i val2)
<<val1 * val2>> => arithmeticResult
----------------------------------------------------------------------
eval tables expr1 * expr2 => evalResult tables'' ($i arithmeticResult)

eval tables expr1 => evalResult tables' ($i val1)
eval tables' expr2 => evalResult tables'' ($i val2)
<<val1 / val2>> => arithmeticResult
----------------------------------------------------------------------
eval tables expr1 / expr2 => evalResult tables'' ($i arithmeticResult)

eval tables expr1 => evalResult tables' ($d val1)
eval tables' expr2 => evalResult tables'' ($d val2)
<<val1 + val2>> => arithmeticResult
----------------------------------------------------------------------
eval tables expr1 + expr2 => evalResult tables'' ($d arithmeticResult)

eval tables expr1 => evalResult tables' ($d val1)
eval tables' expr2 => evalResult tables'' ($d val2)
<<val1 - val2>> => arithmeticResult
----------------------------------------------------------------------
eval tables expr1 - expr2 => evalResult tables'' ($d arithmeticResult)

eval tables expr1 => evalResult tables' ($d val1)
eval tables' expr2 => evalResult tables'' ($d val2)
<<val1 * val2>> => arithmeticResult
----------------------------------------------------------------------
eval tables expr1 * expr2 => evalResult tables'' ($d arithmeticResult)

eval tables expr1 => evalResult tables' ($d val1)
eval tables' expr2 => evalResult tables'' ($d val2)
<<val1 / val2>> => arithmeticResult
----------------------------------------------------------------------
eval tables expr1 / expr2 => evalResult tables'' ($d arithmeticResult)

eval tables expr1 => evalResult tables' ($s val1)
eval tables' expr2 => evalResult tables'' ($s val2)
<<val1 + val2>> => arithmeticResult
----------------------------------------------------------------------
eval tables expr1 + expr2 => evalResult tables'' ($s arithmeticResult)


eval tables expr1 => evalResult tables' ($b val1)
eval tables' expr2 => evalResult tables'' ($b val2)
<<val1 && val2>> => boolResult
----------------------------------------------------------------------
eval tables expr1 && expr2 => evalResult tables'' ($b boolResult)

eval tables expr1 => evalResult tables' ($b val1)
eval tables' expr2 => evalResult tables'' ($b val2)
<<val1 || val2>> => boolResult
----------------------------------------------------------------------
eval tables expr1 || expr2 => evalResult tables'' ($b boolResult)


eval tables expr => evalResult tables' ($b val)
<< !val >> => boolResult
--------------------------------------------------------
eval tables (!expr) => evalResult tables' ($b boolResult)

eval tables expr1 => evalResult tables' val1
eval tables' expr2 => evalResult tables'' val2
val1 == val2
--------------------------------------------------------
eval tables (expr1 equals expr2) => evalResult tables' ($b true)

eval tables expr1 => evalResult tables' val1
eval tables' expr2 => evalResult tables'' val2
val1 != val2
--------------------------------------------------------
eval tables (expr1 equals expr2) => evalResult tables' ($b false)


eval tables (expr1 equals expr2) => evalResult tables' ($b res)
<< !res >> => boolResult
-------------------------------------------------------------------
eval tables (expr1 neq expr2) => evalResult tables' ($b boolResult)


eval tables expr1 => evalResult tables' ($i val1)
eval tables' expr2 => evalResult tables'' ($i val2)
<< val1 < val2 >> => boolResult
---------------------------------------------------------
eval tables (expr1 ls expr2) => evalResult tables' ($b boolResult)

eval tables expr1 => evalResult tables' ($i val1)
eval tables' expr2 => evalResult tables'' ($i val2)
<< val1 <= val2 >> => boolResult
---------------------------------------------------------
eval tables (expr1 leq expr2) => evalResult tables' ($b boolResult)

eval tables expr1 => evalResult tables' ($i val1)
eval tables' expr2 => evalResult tables'' ($i val2)
<< val1 > val2 >> => boolResult
---------------------------------------------------------
eval tables (expr1 grt expr2) => evalResult tables' ($b boolResult)

eval tables expr1 => evalResult tables' ($i val1)
eval tables' expr2 => evalResult tables'' ($i val2)
<< val1 >= val2 >> => boolResult
---------------------------------------------------------
eval tables (expr1 geq expr2) => evalResult tables' ($b boolResult)

eval tables expr1 => evalResult tables' ($d val1)
eval tables' expr2 => evalResult tables'' ($d val2)
<< val1 < val2 >> => boolResult
---------------------------------------------------------
eval tables (expr1 ls expr2) => evalResult tables' ($b boolResult)

eval tables expr1 => evalResult tables' ($d val1)
eval tables' expr2 => evalResult tables'' ($d val2)
<< val1 <= val2 >> => boolResult
---------------------------------------------------------
eval tables (expr1 leq expr2) => evalResult tables' ($b boolResult)

eval tables expr1 => evalResult tables' ($d val1)
eval tables' expr2 => evalResult tables'' ($d val2)
<< val1 > val2 >> => boolResult
---------------------------------------------------------
eval tables (expr1 grt expr2) => evalResult tables' ($b boolResult)

eval tables expr1 => evalResult tables' ($d val1)
eval tables' expr2 => evalResult tables'' ($d val2)
<< val1 >= val2 >> => boolResult
---------------------------------------------------------
eval tables (expr1 geq expr2) => evalResult tables' ($b boolResult)


symbols defineVariable id => symbols'
---------------------------------------------------------------------------------------------
eval (symbols nextTable tables) (variable t id) => evalResult (symbols' nextTable tables) $void


  symbols add id $void => symbols'
  -------------------------------------
  symbols defineVariable id => symbols'

symbols contains id => Yes
eval globals expr => evalResult globals' val
symbols add id val => symbols'
-------------------------------------------------------------------------------------------------
updateTables globals (symbols nextTable tables) id expr => evalResult (symbols' nextTable tables) $void 

symbols contains id => No
updateTables globals tables id expr => evalResult tables' $void
-------------------------------------------------------------------------------------------------
updateTables globals (symbols nextTable tables) id expr => evalResult (symbols nextTable tables') $void


updateTables tables tables id expr => res
----------------------------------------
eval tables (id = expr) => res

eval tables condition => evalResult tables' ($b true)
emptyDictionary => table
eval (table nextTable tables) expr1 => evalResult (table' nextTable tables'') val
------------------------------------------------------------------------------------
eval tables (if condition then expr1 else expr2) => evalResult tables'' val

eval tables condition => evalResult tables' ($b false)
emptyDictionary => table
eval (table nextTable tables) expr2 => evalResult (table' nextTable tables'') val
------------------------------------------------------------------------------------
eval tables (if condition then expr1 else expr2) => evalResult tables'' val

eval tables condition => evalResult tables' ($b true)
emptyDictionary => table
eval (table nextTable tables) expr => evalResult (table' nextTable tables'') val
eval tables'' (while condition expr) => res
---------------------------------------------------
eval tables (while condition expr) => res

eval tables condition => evalResult tables' ($b false)
-----------------------------------------------------------
eval tables (while condition expr) => evalResult tables' $void

eval tables init => evalResult tables' $void
loopFor tables' condition step expr => res
-------------------------------------------------------
eval tables (for init condition step expr) => res

  eval tables condition => evalResult tables' ($b true)
  emptyDictionary => table
  eval (table nextTable tables) expr => evalResult (table' nextTable tables'') val
  eval tables'' step => evalResult tables3 val'
  loopFor tables3 condition step expr => res 
  ------------------------------------------------------------------
  loopFor tables condition step expr => res

  eval tables condition => evalResult tables' ($b false)
  ------------------------------------------------------------------
  loopFor tables condition step expr => evalResult tables' $void

--------------------------------------------------
eval tables nop => evalResult tables $void

eval tables a => evalResult tables' $void
eval tables' b => res
----------------------------------------
eval tables (a;b) => res


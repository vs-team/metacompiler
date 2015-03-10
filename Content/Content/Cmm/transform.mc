Keyword [] "emptyDictionary" [] Priority 100 Class DictionaryOp
Keyword [SymbolTable] "add" [Id Value] Priority 100 Class DictionaryOp
Keyword [SymbolTable] "lookup" [Id] Priority 100 Class DictionaryOp
Keyword [SymbolTable] "contains" [Id] Priority 100 Class DictionaryOp
Keyword [] "$m" [<< System.Collections.Immutable.ImmutableDictionary<Id, Value> >>] Priority 300 Class SymbolTable

Keyword [SymbolTable] "nextTable" [TableList] Priority 10 Class TableList
Keyword [] "nilTable" [] Priority 500 Class TableList

Keyword [SymbolTable] "defineVariable" [Id] Priority 300 Class MemoryOp
Keyword [] "updateTables" [TableList TableList Id Expr] Priority 0 Class MemoryOp

Keyword [] "$i" [<<int>>] Priority 300 Class Value
Keyword [] "$d" [<<double>>] Priority 300 Class Value
Keyword [] "$s" [<<string>>] Priority 300 Class Value
Keyword [] "$b" [<<bool>>] Priority 300 Class Value
Keyword [] "$void" [] Priority 300 Class Value

Keyword [] "$" [<<string>>] Priority 300 Class Id

Keyword [Expr] "+" [Expr] Priority 100 Class Expr
Keyword [Expr] "-" [Expr] Priority 100 Class Expr
Keyword [Expr] "*" [Expr] Priority 100 Class Expr
Keyword [Expr] "/" [Expr] Priority 100 Class Expr

Keyword [Expr] "&&" [Expr] Priority 100 Class Expr
Keyword [Expr] "||" [Expr] Priority 100 Class Expr
Keyword [] "!" [Expr] Priority 100 Class Expr
Keyword [Expr] "equals" [Expr] Priority 100 Class Expr
Keyword [Expr] "ls" [Expr] Priority 100 Class Expr
Keyword [Expr] "leq" [Expr] Priority 100 Class Expr
Keyword [Expr] "grt" [Expr] Priority 100 Class Expr
Keyword [Expr] "geq" [Expr] Priority 100 Class Expr

Keyword [] "t_int" [] Priority 500 Class Type
Keyword [] "t_double" [] Priority 500 Class Type
Keyword [] "t_string" [] Priority 500 Class Type
Keyword [] "t_bool" [] Priority 500 Class Type

Keyword [] "variable" [Type Id] Priority 10 Class Expr
Keyword [Id] "=" [Expr] Priority 10 Class Expr
Keyword [] "then" [] Priority 10 Class Then
Keyword [] "else" [] Priority 10 Class Else
Keyword [] "if" [Expr Then Expr Else Expr] Priority 10 Class Expr
Keyword [] "loop" [Expr Expr] Priority 10 Class Expr

Keyword [] "Yes" [] Priority 100 Class Answer
Keyword [] "No" [] Priority 100 Class Answer

Keyword [Expr] ";" [ExprList] Priority 5 Class ExprList
Keyword [] "nop" [] Priority 500 Class ExprList

Keyword [] "eval" [TableList Expr] Priority 0 Class RuntimeOp
Keyword [] "evalResult" [TableList Value] Priority 0 Class EvaluationResult

Keyword [] "error" [<<string>>] Priority 300 Class Error

Keyword [] "program" [SymbolTable ExprList] Priority 0 Class Program
Keyword [] "runProgram" [] Priority -10000 Class Test

Value is Expr
Id is Expr
Error is Value
ExprList is Expr


M := $m <<System.Collections.Immutable.ImmutableDictionary<Id, Value>.Empty>>
----------------------------------------------------------------------------
emptyDictionary => M

v := <<M.GetKey(k)>>
---------------------
($m M) lookup k => v

<<M.ContainsKey(k)>> == false
M' := <<M.Add(k,v)>>
------------------------
($m M) add k v => $m M'

<<M.ContainsKey(k)>> == true
M' := <<M.SetItem(k,v)>>
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
l := loop ($"j" leq $"n") l_block
code := decl_fact ; (decl_n ; (decl_j ; (assign1 ; (assign2 ; (assign3 ; (l ;nop))))))
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
arithmeticResult := <<val1 + val2>>
----------------------------------------------------------------------
eval tables expr1 + expr2 => evalResult tables'' ($i arithmeticResult)

eval tables expr1 => evalResult tables' ($i val1)
eval tables' expr2 => evalResult tables'' ($i val2)
arithmeticResult := <<val1 - val2>>
----------------------------------------------------------------------
eval tables expr1 - expr2 => evalResult tables'' ($i arithmeticResult)

eval tables expr1 => evalResult tables' ($i val1)
eval tables' expr2 => evalResult tables'' ($i val2)
arithmeticResult := <<val1 * val2>>
----------------------------------------------------------------------
eval tables expr1 * expr2 => evalResult tables'' ($i arithmeticResult)

eval tables expr1 => evalResult tables' ($i val1)
eval tables' expr2 => evalResult tables'' ($i val2)
arithmeticResult := <<val1 / val2>>
----------------------------------------------------------------------
eval tables expr1 / expr2 => evalResult tables'' ($i arithmeticResult)

eval tables expr1 => evalResult tables' ($d val1)
eval tables' expr2 => evalResult tables'' ($d val2)
arithmeticResult := <<val1 + val2>>
----------------------------------------------------------------------
eval tables expr1 + expr2 => evalResult tables'' ($d arithmeticResult)

eval tables expr1 => evalResult tables' ($d val1)
eval tables' expr2 => evalResult tables'' ($d val2)
arithmeticResult := <<val1 - val2>>
----------------------------------------------------------------------
eval tables expr1 - expr2 => evalResult tables'' ($d arithmeticResult)

eval tables expr1 => evalResult tables' ($d val1)
eval tables' expr2 => evalResult tables'' ($d val2)
arithmeticResult := <<val1 * val2>>
----------------------------------------------------------------------
eval tables expr1 * expr2 => evalResult tables'' ($d arithmeticResult)

eval tables expr1 => evalResult tables' ($d val1)
eval tables' expr2 => evalResult tables'' ($d val2)
arithmeticResult := <<val1 / val2>>
----------------------------------------------------------------------
eval tables expr1 / expr2 => evalResult tables'' ($d arithmeticResult)

eval tables expr1 => evalResult tables' ($s val1)
eval tables' expr2 => evalResult tables'' ($s val2)
arithmeticResult := <<val1 + val2>>
----------------------------------------------------------------------
eval tables expr1 + expr2 => evalResult tables'' ($s arithmeticResult)


eval tables expr1 => evalResult tables' ($b val1)
eval tables' expr2 => evalResult tables'' ($b val2)
boolResult := <<val1 && val2>>
----------------------------------------------------------------------
eval tables expr1 && expr2 => evalResult tables'' ($b boolResult)

eval tables expr1 => evalResult tables' ($b val1)
eval tables' expr2 => evalResult tables'' ($b val2)
boolResult := <<val1 || val2>>
----------------------------------------------------------------------
eval tables expr1 || expr2 => evalResult tables'' ($b boolResult)


eval tables expr => evalResult tables' ($b val)
boolResult := << !val >>
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

eval tables expr1 => evalResult tables' ($i val1)
eval tables' expr2 => evalResult tables'' ($i val2)
boolResult := << val1 < val2 >>
---------------------------------------------------------
eval tables (expr1 ls expr2) => evalResult tables' ($b boolResult)

eval tables expr1 => evalResult tables' ($i val1)
eval tables' expr2 => evalResult tables'' ($i val2)
boolResult := << val1 <= val2 >>
---------------------------------------------------------
eval tables (expr1 leq expr2) => evalResult tables' ($b boolResult)

eval tables expr1 => evalResult tables' ($i val1)
eval tables' expr2 => evalResult tables'' ($i val2)
boolResult := << val1 > val2 >>
---------------------------------------------------------
eval tables (expr1 grt expr2) => evalResult tables' ($b boolResult)

eval tables expr1 => evalResult tables' ($i val1)
eval tables' expr2 => evalResult tables'' ($i val2)
boolResult := << val1 >= val2 >>
---------------------------------------------------------
eval tables (expr1 geq expr2) => evalResult tables' ($b boolResult)

eval tables expr1 => evalResult tables' ($d val1)
eval tables' expr2 => evalResult tables'' ($d val2)
boolResult := << val1 < val2 >>
---------------------------------------------------------
eval tables (expr1 ls expr2) => evalResult tables' ($b boolResult)

eval tables expr1 => evalResult tables' ($d val1)
eval tables' expr2 => evalResult tables'' ($d val2)
boolResult := << val1 <= val2 >>
---------------------------------------------------------
eval tables (expr1 leq expr2) => evalResult tables' ($b boolResult)

eval tables expr1 => evalResult tables' ($d val1)
eval tables' expr2 => evalResult tables'' ($d val2)
boolResult := << val1 > val2 >>
---------------------------------------------------------
eval tables (expr1 grt expr2) => evalResult tables' ($b boolResult)

eval tables expr1 => evalResult tables' ($d val1)
eval tables' expr2 => evalResult tables'' ($d val2)
boolResult := << val1 >= val2 >>
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
eval tables'' (loop condition expr) => res
---------------------------------------------------
eval tables (loop condition expr) => res

eval tables condition => evalResult tables' ($b false)
-----------------------------------------------------------
eval tables (loop condition expr) => evalResult tables' $void

--------------------------------------------------
eval tables nop => evalResult tables $void

eval tables a => evalResult tables' $void
tables1 := tables'
eval tables' b => res
----------------------------------------
eval tables (a;b) => res


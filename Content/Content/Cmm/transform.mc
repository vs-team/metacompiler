Keyword [] "emptyDictionary" [] Priority 100 Class DictionaryOp
Keyword [SymbolTable] "add" [Id Value] Priority 100 Class DictionaryOp
Keyword [SymbolTable] "lookup" [Id] Priority 100 Class DictionaryOp
Keyword [SymbolTable] "contains" [Id] Priority 100 Class DictionaryOp
Keyword [] "$m" [<< System.Collections.Immutable.ImmutableDictionary<Id, Value> >>] Priority 300 Class SymbolTable

Keyword [SymbolTable] "nextTable" [TableList] Priority 10 Class TableList
Keyword [] "nilTable" [] Priority 500 Class TableList

Keyword [SymbolTable] "defineVariable" [Id] Priority 300 Class MemoryOp

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

Keyword [] "t_int" [] Priority 500 Class Type
Keyword [] "t_double" [] Priority 500 Class Type
Keyword [] "t_string" [] Priority 500 Class Type
Keyword [] "t_bool" [] Priority 500 Class Type

Keyword [] "variable" [Type Id] Priority 10 Class Expr
Keyword [Id] "=" [Expr] Priority 10 Class Expr
Keyword [] "then" [] Priority 10 Class Then
Keyword [] "else" [] Priority 10 Class Else
Keyword [] "if" [Expr Then Expr Else Expr] Priority 10 Class Expr

Keyword [] "Yes" [] Priority 100 Class Answer
Keyword [] "No" [] Priority 100 Class Answer

Keyword [Expr] ";" [ExprList] Priority 5 Class ExprList
Keyword [] "nop" [] Priority 500 Class ExprList

Keyword [] "eval" [TableList TableList Expr] Priority 0 Class RuntimeOp
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
sumAssign := $"x" = $b true
varDecl := variable t_int $"y"
varAssign1 := $"y" = $i 1 + $i 5
varAssign2 := $"y" = $i 1 + $i 10
varAssign3 := $"z" = $"y"
choice := if $"x" then (varDecl ; (varAssign1 ; (varAssign3 ; nop))) else (varDecl ; (varAssign2 ; (varAssign3 ; nop)))
code := variable t_bool $"x"; ( variable t_int $"z" ; (sumAssign ; (choice ; nop)))
program M code => M'
---------------------------------------------------------------------
runProgram => M'



eval (memory nextTable nilTable) (memory nextTable nilTable) code => evalResult (memory' nextTable nilTable) $void
debug0 := <<EntryPoint.Print("Done!")>>
-----------------------------------------------------------------------------------------
program memory code => memory'


------------------------------------------------
eval globals tables ($i v) => evalResult tables ($i v)

-----------------------------------------------
eval globals tables ($d v) => evalResult tables ($d v)

-----------------------------------------------
eval globals tables ($s v) => evalResult tables ($s v)

-----------------------------------------------
eval globals tables ($b v) => evalResult tables ($b v)

symbols contains ($name) => Yes
symbols lookup ($name) => val
------------------------------------------------------------------------------------
eval (symbols nextTable tables) ts ($name) => evalResult (symbols nextTable tables) val


symbols contains ($name) => No
eval tables ts ($name) => evalResult tables' val
------------------------------------------------------------------------------------
eval (symbols nextTable tables) ts ($name) => evalResult (symbols nextTable tables') val



eval globals tables expr1 => evalResult tables' ($i val1)
eval globals tables' expr2 => evalResult tables'' ($i val2)
arithmeticResult := <<val1 + val2>>
----------------------------------------------------------------------
eval globals tables expr1 + expr2 => evalResult tables'' ($i arithmeticResult)

eval globals tables expr1 => evalResult tables' ($i val1)
eval globals tables' expr2 => evalResult tables'' ($i val2)
arithmeticResult := <<val1 - val2>>
----------------------------------------------------------------------
eval globals tables expr1 - expr2 => evalResult tables'' ($i arithmeticResult)

eval globals tables expr1 => evalResult tables' ($i val1)
eval globals tables' expr2 => evalResult tables'' ($i val2)
arithmeticResult := <<val1 * val2>>
----------------------------------------------------------------------
eval globals tables expr1 * expr2 => evalResult tables'' ($i arithmeticResult)

eval globals tables expr1 => evalResult tables' ($i val1)
eval globals tables' expr2 => evalResult tables'' ($i val2)
arithmeticResult := <<val1 / val2>>
----------------------------------------------------------------------
eval globals tables expr1 / expr2 => evalResult tables'' ($i arithmeticResult)

eval globals tables expr1 => evalResult tables' ($d val1)
eval globals tables' expr2 => evalResult tables'' ($d val2)
arithmeticResult := <<val1 + val2>>
----------------------------------------------------------------------
eval globals tables expr1 + expr2 => evalResult tables'' ($d arithmeticResult)

eval globals tables expr1 => evalResult tables' ($d val1)
eval globals tables' expr2 => evalResult tables'' ($d val2)
arithmeticResult := <<val1 - val2>>
----------------------------------------------------------------------
eval globals tables expr1 - expr2 => evalResult tables'' ($d arithmeticResult)

eval globals tables expr1 => evalResult tables' ($d val1)
eval globals tables' expr2 => evalResult tables'' ($d val2)
arithmeticResult := <<val1 * val2>>
----------------------------------------------------------------------
eval globals tables expr1 * expr2 => evalResult tables'' ($d arithmeticResult)

eval globals tables expr1 => evalResult tables' ($d val1)
eval globals tables' expr2 => evalResult tables'' ($d val2)
arithmeticResult := <<val1 / val2>>
----------------------------------------------------------------------
eval globals tables expr1 / expr2 => evalResult tables'' ($d arithmeticResult)

eval globals tables expr1 => evalResult tables' ($s val1)
eval globals tables' expr2 => evalResult tables'' ($s val2)
arithmeticResult := <<val1 + val2>>
----------------------------------------------------------------------
eval globals tables expr1 + expr2 => evalResult tables'' ($s arithmeticResult)


eval globals tables expr1 => evalResult tables' ($b val1)
eval globals tables' expr2 => evalResult tables'' ($b val2)
boolResult := <<val1 && val2>>
----------------------------------------------------------------------
eval globals tables expr1 && expr2 => evalResult tables'' ($b boolResult)

eval globals tables expr1 => evalResult tables' ($b val1)
eval globals tables' expr2 => evalResult tables'' ($b val2)
boolResult := <<val1 || val2>>
----------------------------------------------------------------------
eval globals tables expr1 || expr2 => evalResult tables'' ($b boolResult)


eval globals tables expr => evalResult tables' ($b val)
boolResult := << !val >>
--------------------------------------------------------
eval globals tables (!expr) => evalResult tables' ($b boolResult)


symbols defineVariable id => symbols'
---------------------------------------------------------------------------------------------
eval globals (symbols nextTable tables) (variable t id) => evalResult (symbols' nextTable tables) $void


  symbols add id $void => symbols'
  -------------------------------------
  symbols defineVariable id => symbols'


symbols contains id => Yes
eval globals (symbols nextTable tables) expr => evalResult (symbols' nextTable tables') val
symbols' add id val => symbols''
---------------------------------------------------------------------------------------------
eval globals (symbols nextTable tables) (id = expr) => evalResult (symbols'' nextTable tables) $void

symbols contains id => No
eval globals tables (id = expr) => evalResult tables' $void
--------------------------------------------------------------------------------------------
eval globals (symbols nextTable tables) (id = expr) => evalResult (symbols nextTable tables') $void


eval globals tables condition => evalResult tables' ($b true)
emptyDictionary => table
eval globals (table nextTable tables) expr1 => evalResult (table' nextTable tables'') val
------------------------------------------------------------------------------------
eval globals tables (if condition then expr1 else expr2) => evalResult tables'' val

eval globals tables condition => evalResult tables' ($b false)
emptyDictionary => table
eval globals (table nextTable tables) expr2 => evalResult (table' nextTable tables'') val
------------------------------------------------------------------------------------
eval globals tables (if condition then expr1 else expr2) => evalResult tables'' val

--------------------------------------------------
eval globals tables nop => evalResult tables $void

eval globals tables a => evalResult tables' $void
tables1 := tables'
debug1 := <<EntryPoint.Print(tables1)>>
eval tables' tables' b => res
----------------------------------------
eval globals tables (a;b) => res


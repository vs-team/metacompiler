Keyword [] "emptyDictionary" [] Priority 100 Class DictionaryOp
Keyword [SymbolTable] "add" [Id Type] Priority 100 Class DictionaryOp
Keyword [SymbolTable] "lookup" [Id] Priority 100 Class DictionaryOp
Keyword [SymbolTable] "contains" [Id] Priority 100 Class DictionaryOp
Keyword [] "$m" [<< System.Collections.Immutable.ImmutableDictionary<Id, Type> >>] Priority 300 Class SymbolTable

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
Keyword [Expr] "neq" [Expr] Priority 100 Class Expr
Keyword [Expr] "ls" [Expr] Priority 100 Class Expr
Keyword [Expr] "leq" [Expr] Priority 100 Class Expr
Keyword [Expr] "grt" [Expr] Priority 100 Class Expr
Keyword [Expr] "geq" [Expr] Priority 100 Class Expr

Keyword [] "t_int" [] Priority 500 Class Type
Keyword [] "t_double" [] Priority 500 Class Type
Keyword [] "t_string" [] Priority 500 Class Type
Keyword [] "t_bool" [] Priority 500 Class Type
Keyword [] "t_void" [] Priority 500 Class Type

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

Keyword [] "error" [<<string>>] Priority 300 Class Error

Keyword [] "program" [ExprList] Priority 0 Class Program

Keyword [Expr] "::" [Type] Priority 8 class TypedExpr
Keyword [TypedExpr] "nextExpr" [TypedExprList] Priority 0 Class TypedExprList
Keyword [] "nilTypedExpr" [] Priority 0 Class TypedExprList 

Keyword [] "typeCheck" [TableList TypedExprList Expr] Priority 0 Class TypeChecker
Keyword [] "typeCheckResult" [TableList TypedExprList] Priority 0 Class TypeCheckResult

Keyword [] "runTypeCheck" [] Priority 0 Class Test

Value is Expr
Id is Expr
Error is TypeCheckResult
ExprList is Expr
TypedExprList is Expr
Program is Expr

M := $m <<System.Collections.Immutable.ImmutableDictionary<Id, Type>.Empty>>
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
code := nop
//typeCheck (M nextTable nilTable) nilTypedExpr (program code) => res
typeCheck (M nextTable nilTable) nilTypedExpr $b true => res
------------------------------------------------------------------
runTypeChecker => res


-------------------------------------------------------------------------------------------------
typeCheck tables typedExprs nop => typeCheckResult tables ((nop :: t_void) nextExpr typedExprs)

-------------------------------------------------------------------------------------------------
typeCheck tables typedExprs ($i val) => typeCheckResult tables (($i val :: t_int) nextExpr typedExprs)

-------------------------------------------------------------------------------------------------
typeCheck tables typedExprs ($d val) => typeCheckResult tables (($d val :: t_double) nextExpr typedExprs)

-------------------------------------------------------------------------------------------------
typeCheck tables typedExprs ($s val) => typeCheckResult tables (($s val :: t_string) nextExpr typedExprs)

-------------------------------------------------------------------------------------------------
typeCheck tables typedExprs ($b val) => typeCheckResult tables (($b val :: t_bool) nextExpr typedExprs)


typeCheck tables typedExprs code => typeCheckResult tables' typedExprs'
--------------------------------------------------------------------------------
typeCheck tables typedExprs (program code) => typeCheckResult tables' typedExprs'












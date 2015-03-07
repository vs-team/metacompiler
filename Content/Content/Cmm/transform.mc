Keyword [Locals] "add" [<<string>> <<Value>>] Priority 100 Class Locals
Keyword [Locals] "lookup" [<<string>>] Priority 100 Class MemoryOp
Keyword [] "$m" [<<System.Collections.Immutable.ImmutableDictionary<string, Value> >>] Priority 300 Class Locals

Keyword [] "$i" [<<int>>] Priority 300 Class IntConst

Keyword [] "integer" [] Priority 500 Class Type
Keyword [<<string>>] ":" [Type] Priority 300 Class VarDecl

Keyword [] ""

Keyword [] "variables" [VarDeclList] Priority 0 Class Variables
Keyword [] "end" [VarDeclList] Priority 500 Class End
Keyword [VarDecl] "nextVar" [] Priority 10 Class  VarDeclList

Keyword [] "var" [<<string>>] Priority 100 Class Id
Keyword [Id] "=" [Expr] Priority 10 Class Statement

Keyword [Statement] ";" [StmList] Priority 10 Class StmList

Keyword [] "program" [VarDeclList StmList] Priority 0 Class Program
Keyword [] "runProgram" [] Priority -10000 Class Test

End is VarDeclList
End is StmList
IntConst is Expr
IntConst is Value


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




M := $m <<System.Collections.Immutable.ImmutableDictionary<string, Value>.Empty>>
M add "var" ($i 3) => M'
------------------------
runProgram => M'

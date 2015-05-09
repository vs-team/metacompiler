Data [] [] "basicExpression" [Keyword Extension Imported DebugInfo TypeInfo] Priority 0 Type BasicExpression
Data [] [] "keyword" [Keyword DebugInfo TypeInfo] Priority 0 Type Keyword
Data [] [] "application" [Bracket BasicExpressionList] Priority 0 Type Application
Data [] [] "nilExpr" [] Priority 300 Type BasicExpressionList
Data [] [BasicExpression] "nextExpr" [BasicExpressionList] Priority 100 Type BasicExpressionList
Data [] [] "imported" [Imported DebugInfo TypeInfo] Priority 0 Type Imported
Data [] [] "extension" [Extension DebugInfo TypeInfo] Priority 0 Type Extension
Data [] [] "di" [<<int>> <<string>> <<int>> DebugInfo DebugInfo] Priority 0 Type DebugInfo
Data [] [] "ti" [<<string>>] Priority 0 Type TypeInfo
Data [] [] "_implicit" [] Priority 10 Type Bracket
Data [] [] "square" [] Priority 10 Type SquareBracket
Data [] [] "curly" [] Priority 10 Type CurlyBracket
Data [] [] "angle" [] Priority 10 Type AngleBracket
Data [] [] "regular" [] Priority 10 Type RegularBracket
Data [] [] "data" [] Priority 10 Type Data
Data [] [] "function" [] Priority 10 Type Function
Data [] [] "generic" [] Priority 10 Type Generic
Data [] [] "_null" [] Priority 10 Type Null
Func [] "identity" [BasicExpression] Priority 0 Type Identity => BasicExpression
Data [] [] "test" [] Priority 0 Type DebugProgram

Keyword is BasicExpression
Application is BasicExpression
Imported is BasicExpression
Extension is BasicExpression

SquareBracket is Bracket
CurlyBracket is Bracket
AngleBracket is Bracket
RegularBracket is Bracket
Data is Bracket
Function is Bracket
Generic is Bracket

Null is TypeInfo
Null is DebugInfo


----------------
identity x => x

x := application _implicit nilExpr
identity x => res
-------------------------
test => res


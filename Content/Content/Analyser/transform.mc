Data [] [] "basicExpression" [Keyword BasicExpression Imported DebugInfo TypeInfo] Priority 0 Type BasicExpression
Data [] [] "keyword" [Keyword DebugInfo TypeInfo] Priority 0 Type BasicExpression
Data [] [] "application" [Bracket BasicExpressionList] Priority 0 Type BasicExpression
Data [] [] "nilExpr" [] Priority 300 Type BasicExpressionList
Data [] [BasicExpression] "nextExpr" [BasicExpressionList] Priority 100 Type BasicExpressionList
Data [] [] "imported" [Imported DebugInfo TypeInfo] Priority 0 Type Imported
Data [] [] "extension" [BasicExpression DebugInfo TypeInfo] Priority 10 Type BasicExpression
Data [] [] "di" [<<int>> <<int>> <<string>>] Priority 10 Type DebugInfo
Data [] [] "ti" [<<string>>] Priority 10 Type TypeInfo
Data [] [] "_implicit" [] Priority 10 Type Bracket
Data [] [] "square" [] Priority 10 Type Bracket
Data [] [] "curly" [] Priority 10 Type Bracket
Data [] [] "angle" [] Priority 10 Type Bracket
Data [] [] "regular" [] Priority 10 Type Bracket
Data [] [] "data" [] Priority 10 Type Bracket
Data [] [] "function" [] Priority 10 Type Bracket
Data [] [] "generic" [] Priority 10 Type Bracket
Data [] [] "_null" [] Priority 10 Type Null
Func [] "identity" [BasicExpression] Priority 0 Type Identity => BasicExpression
Data [] [] "main" [] Priority 0 Type Main

Data [] [] "sequence" [] Priority 10 Type Keyword
Data [] [] "smallerThan" [] Priority 10 Type Keyword
Data [] [] "smallerOrEqual" [] Priority 10 Type Keyword
Data [] [] "greaterThan" [] Priority 10 Type Keyword
Data [] [] "notDivisible" [] Priority 10 Type Keyword
Data [] [] "Divisible" [] Priority 10 Type Keyword
Data [] [] "greaterOrEqual" [] Priority 10 Type Keyword
Data [] [] "equals" [] Priority 10 Type Keyword
Data [] [] "notequals" [] Priority 10 Type Keyword
Data [] [] "doubleArrow" [] Priority 10 Type Keyword
Data [] [] "fractionLine" [] Priority 10 Type Keyword
Data [] [] "nesting" [] Priority 10 Type Keyword
Data [] [] "definedAs" [] Priority 10 Type Keyword
Data [] [] "inlined" [] Priority 10 Type Keyword
Data [] [] "custom" [<<string>>] Priority 10 Type Keyword


Imported is BasicExpression
Null is TypeInfo
Null is DebugInfo


res := keyword (custom "myKeyword")  (di 3 5 "testFile") _null
----------------------------------------------------------------------
main => res


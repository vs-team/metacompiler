Data [] [] "nil" [] Priority 0 Type BinTreeInt
Func [] "node" [BinTreeInt <<int>> BinTreeInt] Priority 1010 Type Expr => BinTreeInt
Data [] [BinTreeInt] "add" [<<int>>] Priority 100 Type Expr
Data [] [BinTreeInt] "contains" [<<int>>] Priority 100 Type Expr

Data [] [Expr] "," [Expr] Priority 1010 Type Expr
Func [] "$" [<<bool>>] Priority 100 Type Expr => BoolExpr

Data [] [] "run" [] Priority 0 Type Expr

BoolExpr is Expr

nil add 10 => t1
t1 add 5 => t2
t2 add 7 => t2b
t2b add 15 => t3
t3 add 1 => t4
t4 add 16 => t
t contains 7 => res
--------------------------
run => (t contains 7),res

  -----------------------------
  nil add k => node nil k nil

  x == k
  -------------------------------------
  (node l k r) add x => node l k r

  x < k
  l add x => l'
  -----------------------------------
  (node l k r) add x => node l' k r

  x > k
  r add x => r'
  --------------------------------------
  (node l k r) add x => (node l k r')


  -------------------------
  nil contains k => $false

  x == k
  ----------------------------------
  (node l k r) contains x => $true

  x < k
  l contains x => res
  --------------------------------
  (node l k r) contains x => res

  x > k
  r contains x => res
  --------------------------------
  (node l k r) contains x => res


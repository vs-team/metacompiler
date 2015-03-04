Keyword [] "nil" [] Priority 0 Class BinTreeInt
Keyword [] "node" [BinTreeInt <<int>> BinTreeInt] Priority 1010 Class BinTreeInt
Keyword [BinTreeInt] "add" [<<int>>] Priority 100 Class Expr
Keyword [BinTreeInt] "contains" [<<int>>] Priority 100 Class Expr

Keyword [Expr] "," [Expr] Priority 1010 Class Expr
Keyword [] "$" [<<bool>>] Priority 100 Class BoolExpr

Keyword [] "run" [] Priority 0 Class Expr

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


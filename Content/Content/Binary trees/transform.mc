Keyword = "nil" LeftArguments = [] RightArguments = [] Priority = 0 Class = "BinTreeInt"
Keyword = "+-" LeftArguments = [BinTreeInt] RightArguments = [<<int>>] Priority = 1010 Class = "BinTreeIntLeft"
Keyword = "-+" LeftArguments = [BinTreeIntLeft] RightArguments = [BinTreeInt] Priority = 1000 Class = "BinTreeInt"

Keyword = "add" LeftArguments = [BinTreeInt] RightArguments = [<<int>>] Priority = 100 Class = "Expr"
Keyword = "contains" LeftArguments = [BinTreeInt] RightArguments = [<<int>>] Priority = 100 Class = "Expr"

Keyword = "," LeftArguments = [Expr] RightArguments = [Expr] Priority = 1010 Class = "Expr"
Keyword = "$" LeftArguments = [] RightArguments = [<<bool>>] Priority = 100 Class = "BoolExpr"

Keyword = "run" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Expr"

BoolExpr is Expr

nil add 10 => t1
t1 add 5 => t2
t2 add 15 => t3
t3 add 1 => t4
t4 add 16 => t
t contains 7 => res
--------------------
run => (t contains 7),res

  -----------------------------
  nil add k => nil +- k -+ nil

  x == k
  -------------------------------------
  (l +- k -+ r) add x => (l +- k -+ r)

  <<x < k>> == true
  l add x => l'
  --------------------------------------
  (l +- k -+ r) add x => (l' +- k -+ r)

  <<x > k>> == true
  r add x => r'
  --------------------------------------
  (l +- k -+ r) add x => (l +- k -+ r')


  -------------------------
  nil contains k => $false

  x == k
  ----------------------------------
  (l +- k -+ r) contains x => $true

  <<x < k>> == true
  l contains x => res
  --------------------------------
  (l +- k -+ r) contains x => res

  <<x > k>> == true
  r contains x => res
  --------------------------------
  (l +- k -+ r) contains x => res


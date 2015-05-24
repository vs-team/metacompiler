Data "nil" : BinTreeInt                                                       Priority 0
Data "node" -> BinTreeInt -> <<int>> -> BinTreeInt : BinTreeInt               Priority 1010
Func BinTreeInt -> "add" -> <<int>> : Expr => BinTreeInt                      Priority 100
Func BinTreeInt -> "contains" -> <<int>> : Expr => <<bool>>                   Priority 100

Data Expr -> "," -> <<bool>> : Expr                                           Priority 1010

Func "run" : Expr => Expr                                                     Priority 0

BinTreeInt is Expr


nil add 10 => t1
t1 add 5 => t2
t2 add 7 => t2b
t2b add 15 => t3
t3 add 1 => t4
t4 add 16 => t
t contains 7 => res
arg := t contains 7
--------------------------
run => arg,res

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
  nil contains k => false

  x == k
  ----------------------------------
  (node l k r) contains x => true

  x < k
  l contains x => res
  --------------------------------
  (node l k r) contains x => res

  x > k
  r contains x => res
  --------------------------------
  (node l k r) contains x => res


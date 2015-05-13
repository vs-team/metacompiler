Data "$" -> <<int>> : IntValue          Priority 9
Data  "z" : Num                         Priority 3
Data "s" -> Num : Num                   Priority 4  
Func Expr -> "+" -> Expr : Expr => Num  Priority 1 
Func Expr -> "*" -> Expr : Expr => Num  Priority 2  
Func "eval" -> Expr : Expr => Num        
Func "toNum" -> Num : IntValue          
Func "run" : Expr                       

Num is Expr
IntValue is Expr


-----------
z + a => a

a + b => c
-----------------
s(a) + b => s(c)

-----------
z * a => z

a * b => c
c + b => d
--------------
s(a) * b => d


--------
eval z => z

---------------
eval(s a) => s a

eval a => a'
eval b => b'
a' + b' => c
--------------
eval (a + b) => c

eval a => a'
eval b => b'
a' * b' => c
--------------
eval (a * b) => c

toNum a => $res
---------------
toNum (s(a)) => $<<res + 1>>

----------------
toNum z => $0

eval(ssz * ssz * ssz + sz) => n
toNum n => res
--------------------------------
run => res


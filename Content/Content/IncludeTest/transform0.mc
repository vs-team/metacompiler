Keyword [] "z" []         Priority 2  Class Num
Keyword [] "s" [Num]      Priority 3  Class Num
Keyword [Expr] "+" [Expr] Priority 0  Class Expr
Keyword [Expr] "*" [Expr] Priority 1  Class Expr

Num is Expr

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

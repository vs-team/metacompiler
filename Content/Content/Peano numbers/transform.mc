Data [] [] "z" []         Priority 2  Type Num
Func [] "s" [Num]      Priority 3  Type Expr => Num
Data [] [Expr] "+" [Expr] Priority 0  Type Expr
Data [] [Expr] "*" [Expr] Priority 1  Type Expr
Func [] "!" [Expr]     Priority 1  Type Expr => Expr

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


--------
!z => z

---------------
!(s a) => s a

!a => a'
!b => b'
a' + b' => c
--------------
!(a + b) => c

!a => a'
!b => b'
a' * b' => c
--------------
!(a * b) => c

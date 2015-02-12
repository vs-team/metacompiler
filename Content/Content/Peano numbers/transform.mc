Keyword = "z" LeftArguments = [] RightArguments = [] Priority = 2 Class = "Num"
Keyword = "s" LeftArguments = [] RightArguments = [Num] Priority = 3 Class = "Num"

Keyword = "+" LeftArguments = [Expr] RightArguments = [Expr] Priority = 0 Class = "Expr"
Keyword = "*" LeftArguments = [Expr] RightArguments = [Expr] Priority = 1 Class = "Expr"

Keyword = "!" LeftArguments = [] RightArguments = [Expr] Priority = 1 Class = "Expr"

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

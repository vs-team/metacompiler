Keyword = "+" LeftArguments = [Expr] RightArguments = [Expr] Priority = 0 Class = "Expr"
Keyword = "*" LeftArguments = [Expr] RightArguments = [Expr] Priority = 1 Class = "Expr"
Keyword = "z" LeftArguments = [] RightArguments = [] Priority = 2 Class = "Expr"
Keyword = "s" LeftArguments = [] RightArguments = [Expr] Priority = 3 Class = "Expr"

-------
z => z

a => a'
------------
s a => s a'

a => a'
------------
z + a => a'

a => a'
b => b'
a' + b' => c
------------------
(s(a)) + b => s(c)

-----------
z * a => z

a => a'
b => b'
a' * b' => c
c => c'
c' + b' => d
----------------
(s(a)) * b => d


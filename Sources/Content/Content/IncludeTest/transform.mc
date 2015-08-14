include Content.IncludeTest.transform0.mc

Keyword [] "!" [Expr]     Priority 1  Class Expr

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

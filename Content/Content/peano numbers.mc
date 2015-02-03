Keyword = "+" LeftAriety = 1 RightAriety = 1 Priority = 0
Keyword = "*" LeftAriety = 1 RightAriety = 1 Priority = 1
Keyword = "z" LeftAriety = 0 RightAriety = 0 Priority = 2
Keyword = "s" LeftAriety = 0 RightAriety = 1 Priority = 3

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


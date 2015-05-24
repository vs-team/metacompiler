import System

Data "$i" -> <<int>> : Value                          Priority 300
Data "$s" -> <<string>> : Key                         Priority 300
Data "entry" -> Key -> Value : ListElem               Priority 200
Data "nothing" : ListElem                             Priority 300


Data "nil" : List
Data ListElem -> ";" -> List : List                   Priority 190 

Data "empty" : BTree
Data "node" -> List : BTree                           Priority 150

Func "isLeaf" -> List : Expr => <<bool>>
Func "split" -> List  : Expr => BTree
Func List -> "insertSort" -> ListElem -> BTree : Expr => List
Func ListElem -> "insertInto" -> List : Expr => List
Func "merge" -> BTree -> List : Expr => List

Func BTree -> "insert" -> ListElem : Expr => BTree
Func BTree -> "find" -> Key : Expr => ListElem
Func "lookup" -> BTree -> Key : Expr => ListElem           Priority -10 

Func "main" : Expr => ListElem


BTree is ListElem



---------------------------------
isLeaf (empty;rest) => <<true>>

l != empty
---------------------------------
isLeaf (l;rest) => <<false>>

------------------------------------------
split l;(k;(r;nil)) => node l;(k;(r;nil))

res := node l;(k1;(m;(k2;(r;nil))))
------------------------------------------------------
split l;(k1;(m;(k2;(r;nil)))) => res

------------------------------------------------------------------------
split l;(k1;(l_m;(k2;(r_m;(k3;(r;nil)))))) => node l;(k1;(l_m;(k2;(r_m;(k3;(r;nil))))))

l' := node l;(k1;(l_m;nil))
r' := node m;(k3;(r_m;(k4;(r;nil))))
----------------------------------------------------------------------------
split l;(k1;(l_m;(k2;(m;(k3;(r_m;(k4;(r;nil)))))))) => node l';(k2;(r';nil))


------------------------------------
l;nil insertSort (entry $s k $i v) r => l;(entry $s k $i v;(r;nil))

<< System.String.Compare(x,k,false) >> == 0
----------------------------------------
(l;(entry $s x $i v1;xs)) insertSort (entry $s k $i v) r => l;(entry $s k $i v;xs)

<< System.String.Compare(x,k,false) >> == 1
----------------------------------------
(l;(entry $s x $i v1;xs)) insertSort (entry $s k $i v) r => l;(entry $s k $i v;(r;(entry $s x $i v1;xs)))

<< System.String.Compare(x,k,false) >> == -1
xs insertSort (entry $s k $i v) r => xs'
------------------------------------
(l;(entry $s x $i v1;xs)) insertSort (entry $s k $i v) r => l;(entry $s x $i v1;xs')


---------------------------------------
merge (node l;(k;(r;nil))) xs => l;(k;(r;xs))

--------------------------------------------------
merge (node l;(k1;(m;(k2;rs)))) xs => (node l;(k1;(m;(k2;rs))));xs


l insert k => l'
merge l' nil => l''
---------------------------
k insertInto l;nil => l''

<< System.String.Compare(x,k,false) >> == 1
l insert (entry $s k $i v) => l'
merge l' (entry $s x $i v1;xs) => l''
---------------------------------------------
(entry $s k $i v) insertInto (l;(entry $s x $i v1;xs)) => l''

<< System.String.Compare(x,k,false) >> == 0
------------------------------------
(entry $s k $i v) insertInto (l;(entry $s x $i v1;xs)) => l;(entry $s k $i v;xs)

<< System.String.Compare(x,k,false) >> == -1
(entry $s k $i v) insertInto xs => xs'
------------------------------------
(entry $s k $i v) insertInto (l;(entry $s x $i v1;xs)) => l;(entry $s x $i v1;xs')


isLeaf l => yes
l insertSort kv empty => l1
split l1 => res
-------------------------------
(node l) insert kv => res

isLeaf l => no
kv insertInto l => l'
split l' => res
----------------------------------------
(node l) insert kv => res

----------------------------------------------
empty insert kv => node (empty;(kv;(empty;nil)))




lookup tree k => kv
------------------------
tree find k => kv

----------------------------
lookup empty key => nothing


<< System.String.Compare(key,k,false) >> == 0
--------------------------------------------------------------------
lookup node(l;(entry $s k $i v;(r;nil))) ($s key) => entry $s k $i v

<< System.String.Compare(key,k,false) >> == -1
lookup l ($s key) => kv 
--------------------------------------------------------------------
lookup node(l;(entry $s k $i v;(r;nil))) ($s key) => kv

<< System.String.Compare(key,k,false) >> == 1
lookup r ($s key) => kv 
--------------------------------------------------------------------
lookup node(l;(entry $s k $i v;(r;nil))) ($s key) => kv

<< System.String.Compare(key,k1,false) >> == 0
--------------------------------------------------------------------
lookup node(l;(entry $s k1 $i v1;(m;(k2;(r;nil))))) ($s key) => entry $s k1 $i v1

<< System.String.Compare(key,k2,false) >> == 0
--------------------------------------------------------------------
lookup node(l;(k1;(m;(entry $s k2 $i v2;(r;nil))))) ($s key) => entry $s k2 $i v2

<< System.String.Compare(key,k1,false) >> == -1
lookup l ($s key) => kv
--------------------------------------------------------------------
lookup node(l;(entry $s k1 $i v1;(m;(entry $s k2 $i v2;(r;nil))))) ($s key) => kv

<< System.String.Compare(key,k1,false) >> == 1
<< System.String.Compare(key,k2,false) >> == -1
lookup m ($s key) => kv
--------------------------------------------------------------------
lookup node(l;(entry $s k1 $i v1;(m;(entry $s k2 $i v2;(r;nil))))) ($s key) => kv

<< System.String.Compare(key,k2,false) >> == 1
lookup r ($s key) => kv
--------------------------------------------------------------------
lookup node(l;(entry $s k1 $i v1;(m;(entry $s k2 $i v2;(r;nil))))) ($s key) => kv

<< System.String.Compare(key,k1,false) >> == 0
--------------------------------------------------------------------
lookup node(l;(entry $s k1 $i v1;(l_m;(k2;(r_m;(k3;(r;nil))))))) ($s key) => entry $s k1 $i v1

<< System.String.Compare(key,k2,false) >> == 0
--------------------------------------------------------------------
lookup node(l;(k1;(l_m;(entry $s k2 $i v2;(r_m;(k3;(r;nil))))))) ($s key) => entry $s k2 $i v2

<< System.String.Compare(key,k3,false) >> == 0
--------------------------------------------------------------------
lookup node(l;(k1;(l_m;(k2;(r_m;(entry $s k3 $i v3;(r;nil))))))) ($s key) => entry $s k3 $i v3

<< System.String.Compare(key,k1,false) >> == -1
lookup l ($s key) => kv
-----------------------------------------------------------------------------------------------
lookup node(l;(entry $s k1 $i v1;(l_m;(entry $s k2 $i v2;(r_m;(entry $s k3 $i v3;(r;nil))))))) ($s key) => kv

<< System.String.Compare(key,k1,false) >> == 1
<< System.String.Compare(key,k2,false) >> == -1
lookup l_m ($s key) => kv
-----------------------------------------------------------------------------------------------
lookup node(l;(entry $s k1 $i v1;(l_m;(entry $s k2 $i v2;(r_m;(entry $s k3 $i v3;(r;nil))))))) ($s key) => kv

<< System.String.Compare(key,k2,false) >> == 1
<< System.String.Compare(key,k3,false) >> == -1
lookup r_m ($s key) => kv
-----------------------------------------------------------------------------------------------
lookup node(l;(entry $s k1 $i v1;(l_m;(entry $s k2 $i v2;(r_m;(entry $s k3 $i v3;(r;nil))))))) ($s key) => kv

<< System.String.Compare(key,k3,false) >> == 1
lookup r ($s key) => kv
-----------------------------------------------------------------------------------------------
lookup node(l;(entry $s k1 $i v1;(l_m;(entry $s k2 $i v2;(r_m;(entry $s k3 $i v3;(r;nil))))))) ($s key) => kv


empty insert (entry $s "aab" $i 10) => t1
t1 insert (entry $s "bce" $i 5) => t2
t2 insert (entry $s "l" $i 7) => t2b
t2b insert (entry $s "k" $i 15) => t3
t3 insert (entry $s "a" $i 1) => t4
t4 insert (entry $s "w" $i 16) => t5
t5 insert (entry $s "z" $i 100) => t6
t6 insert (entry $s "b" $i 3) => t7
t7 insert (entry $s "ax" $i 1) => t8
t8 insert (entry $s "c" $i 12) => t9
//<<Console.WriteLine(t9)>>
t9 find ($s "b") => kv
--------------------------
main => kv


Keyword [] "yes" [] Priority 0 Class BoolExpr
Keyword [] "no" [] Priority 0 Class BoolExpr

Keyword [] "$" [<<int>>] Priority 300 Class ListElem
Keyword [] "$s" [<<string>>] Priority 300 Class ListElem

Keyword [] "nil" [] Priority 0 Class List
Keyword [ListElem] ";" [List] Priority 100 Class List

Keyword [] "empty" [] Priority 0 Class BTree
Keyword [] "node" [List] Priority 0 Class BTree

Keyword [] "isLeaf" [List] Priority 0 Class Expr
Keyword [] "split" [List] Priority 0 Class Expr
Keyword [List] "insertSort" [ListElem BTree] Priority 0 Class Expr
Keyword [<<int>>] "insertInto" [List] Priority 0 Class Expr
Keyword [] "merge" [BTree List] Priority 0 Class Expr

Keyword [] "generateString" [BTree] Priority 0 Class PrintUtils
Keyword [] "prettyPrint" [BTree] Priority 0 Class PrintUtils
Keyword [] "toString" [BTree] Priority 0 Class PrintUtils

Keyword [BTree] "insert" [<<int>>] Priority 0 Class Expr
Keyword [BTree] "contains" [<<int>>] Priority 0 Class Expr

Keyword [] "main" [] Priority 0 Class Expr

BTree is ListElem


---------------------------------
isLeaf (empty;rest) => yes

l != empty
---------------------------------
isLeaf (l;rest) => no

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
l;nil insertSort ($k) r => l;($k;(r;nil))

x == k
----------------------------------------
(l;($x;xs)) insertSort ($k) r => l;($k;(r;($x;xs)))

x > k
----------------------------------------
(l;($x;xs)) insertSort ($k) r => l;($k;(r;($x;xs)))

x < k
xs insertSort ($k) r => xs'
------------------------------------
(l;($x;xs)) insertSort ($k) r => l;($x;xs')


---------------------------------------
merge (node l;(k;(r;nil))) xs => l;(k;(r;xs))

--------------------------------------------------
merge (node l;(k1;(m;(k2;rs)))) xs => (node l;(k1;(m;(k2;rs))));xs


l insert k => l'
merge l' nil => l''
---------------------------
k insertInto l;nil => l''

x > k
l insert k => l'
merge l' ($x;xs) => l''
-------------------------------
k insertInto (l;($x;xs)) => l''

x == k
------------------------------------
k insertInto (l;($x;xs)) => l;($k;xs)

x < k
k insertInto xs => xs'
------------------------------------
k insertInto (l;($x;xs)) => l;($x;xs')


isLeaf l => yes
l insertSort ($k) empty => l1
split l1 => res
-------------------------------
(node l) insert k => res

isLeaf l => no
k insertInto l => l'
split l' => res
-------------------------------
(node l) insert k => res

----------------------------------------------
empty insert k => node (empty;($ k;(empty;nil)))



s := <<"n2 = (" + (k.ToString()) + ")">>
---------------------------------------------------------
generateString (node l;(k;(r;nil))) => ($s s)


s := <<"n3 = (" + (k1.ToString()) + "," + (k2.ToString()) + ")">>
-----------------------------------------------------------
generateString (node l;(k1;(m;(k2;(r;nil))))) => ($s s)


s := <<"n4 = (" + (k1.ToString()) + "," + (k2.ToString()) + "," + (k3.ToString()) + ")">>
-----------------------------------------------------------
generateString (node l;(k1;(l_m;(k2;(r_m;(k3;(r;nil))))))) => ($s s)


generateString (node l;(k;(r;nil))) => ($s s1)
toString l => ($s s2)
toString r => ($s s3)
s := << s1 + "\n--------\n" + s2 + "  " + s3 >>
-------------------------------------------------------
toString (node l;(k;(r;nil))) => ($s s)

generateString (node l;(k1;(m;(k2;(r;nil))))) => ($s s1)
toString l => ($s s2)
toString m => ($s s3)
toString r => ($s s4)
s := << s1 + "\n--------\n" + s2 + "  " + s3 + "  " + s4 >>
-------------------------------------------------------
toString (node l;(k1;(m;(k2;(r;nil))))) => ($s s)

generateString (node l;(k1;(l_m;(k2;(r_m;(k3;(r;nil))))))) => ($s s1)
toString l => ($s s2)
toString l_m => ($s s3)
toString r_m => ($s s4)
toString r => ($s s5)
s := << s1 + "\n--------\n" + s2 + "  " + s3 + "  " + s4 + "  " + s5 >>
-------------------------------------------------------
toString (node l;(k1;(l_m;(k2;(r_m;(k3;(r;nil))))))) => ($s s)


-----------------------------
toString empty => ($s "()")


toString n => ($s s)
output := <<EntryPoint.Print(s)>>
----------------------------------------------
prettyPrint n => empty


empty insert 10 => t1
t1 insert 5 => t2
t2 insert 7 => t2b
t2b insert 15 => t3
t3 insert 1 => t4
t4 insert 16 => t
prettyPrint t4 => empty
--------------------------
main => empty


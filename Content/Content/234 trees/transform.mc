Keyword [] "yes" [] Priority 0 Class BoolExpr
Keyword [] "no" [] Priority 0 Class BoolExpr

Keyword [] "$" [<<int>>] Priority 0 Class ListElem

Keyword [] "nil" [] Priority 0 Class List
Keyword [ListElem] ";" [List] Priority 100 Class List

Keyword [] "empty" [] Priority 0 Class BTree
Keyword [] "node" [List] Priority 0 Class BTree

Keyword [] "isLeaf" [BTree] Priority 0 Class Expr
Keyword [] "split" [List] Priority 0 Class Expr
Keyword [List] "insertSort" [ListElem BTree] Priority 0 Class Expr
Keyword [ListElem] "insertInto" [List] Priority 0 Class Expr

Keyword [BTree] "insert" [<<int>>] Priority 0 Class Expr
Keyword [BTree] "contains" [<<int>>] Priority 0 Class Expr

Keyword [] "main" [] Priority 0 Class Expr

BTree is ListElem


---------------------------------
isLeaf (node empty;rest) => yes

l != empty
---------------------------------
isLeaf (node l;rest) => no


------------------------------------------
split (l;k;r;nil) => node l;k;r;nil

------------------------------------------------------
split (l;k1;m;k2;r;nil) => node l;k1;m;k2;r;nil

------------------------------------------------------------------------
split (l;k1;l_m;k2;r_m;k3;r;nil) => node l;k1;l_m;k2;r_m;k3;r;nil

l' := node l;k1;l_m;nil
r' := node m;k3;r_m;k4;r;nil
-----------------------------------------------------------------
split (l;k1;l_m;k2;m;k3;r_m;k4;r;nil) => node l';k2;r';nil


------------------------------------
l;nil insertSort k r => l;$k;r;nil

x > k
----------------------------------------
l;$x;xs insertSort k r => l;$k;r;$x;xs

x <= k
xs insertSort k r => xs'
------------------------------------
l;$x;xs insertSort k r => l;$x;xs'


isLeaf l => yes
l insertSort k empty => l'
split l' => splitRes
-------------------------------
(node l) insert k => splitRes

isLeaf l => no
// insertInto adds k to the appropriate node
// if a 2-node comes up as the result then it insertionSorts it with the other elements of l and then splits it
// if a more than 2-node comes up as the result then it just puts it back into l
k insertInto l => l'
split l' => splitRes
-------------------------------
(node l) insert k => splitRes

----------------------------------------------
empty insert k => node (empty;$ k;empty;nil)


empty insert 10 => t1
t1 insert 5 => t2
t2 insert 7 => t2b
t2b insert 15 => t3
t3 insert 1 => t4
t4 insert 16 => t
t contains 7 => res
--------------------------
main => (t contains 7),res


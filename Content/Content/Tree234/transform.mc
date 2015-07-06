Keyword [] "node2" [Entry Node Node] Priority 0 Class Node
Keyword [] "node3" [Entry Entry Node Node Node] Priority 0 Class Node
Keyword [] "node4" [Entry Entry Entry Node Node Node Node] Priority 0 Class Node
Keyword [] "nothing" [] Priority 100 Class Nothing
Keyword [] "dictionary" [Node] Priority 0 Class Dictionary

Keyword [] "$i" [<<int>>] Priority 20 Class Value
Keyword [] "$s" [<<string>>] Priority 20 Class Value
Keyword [] "$" [<<int>> Value] Priority 10 Class Entry

Keyword [Dictionary] "get" [<<int>>] Priority 0 Class DictionaryOperator
Keyword [Dictionary] "add" [Entry] Priority 0 Class DictionaryOperator
Keyword [Dictionary] "insert" [Node Entry] Priority 0 Class DisctionaryOperator
Keyword [] "traverse" [<<int>> Node] Priority 0 Class DictionaryOperator
Keyword [] "prettyPrint" [Dictionary] Priority 0 Class DictionaryOperator 
Keyword [] "toString" [Node] Priority 0 Class DicitonaryOperator
Keyword [] "generateString" [Node] Priority 0 Class DictionaryOperator
Keyword [] "insertionResult" [Node Entry] Priority 0 Class DictionaryOperator
Keyword [] "split" [Node] Priority 0 Class DictionaryOperator

Keyword [] "runProgram" [] Priority 0 Class Program


Nothing is Node
Nothing is Entry


--------------------------------
traverse key nothing => nothing


key == k
----------------------------------------------------------
traverse key (node2 ($ k v) child1 child2) => ($ k v)

<<key < k>> == true
traverse key child1 => entry
-----------------------------------------------------
traverse key (node2 ($ k v) child1 child2) => entry

<<key > k>> == true
traverse key child2 => entry
-----------------------------------------------------
traverse key (node2 ($ k v) child1 child2) => entry

key == k1
-------------------------------------------------------------------------
traverse key (node3 ($ k1 v1) ($ k2 v2) child1 child2 child3) => ($ k1 v1)

key == k2
-------------------------------------------------------------------------
traverse key (node3 ($ k1 v1) ($ k2 v2) child1 child2 child3) => ($ k2 v2)

<<key < k1>> == true
traverse key child1 => entry
---------------------------------------------------------------------------
traverse key (node3 ($ k1 v1) ($ k2 v2) child1 child2 child3) => entry

<<(key > k1) && (key < k2)>> == true
traverse key child2 => entry
---------------------------------------------------------------------------
traverse key (node3 ($ k1 v1) ($ k2 v2) child1 child2 child3) => entry

<<key > k2>> == true
traverse key child3 => entry
---------------------------------------------------------------------------
traverse key (node3 ($ k1 v1) ($ k2 v2) child1 child2 child3) => entry


key == k1
-------------------------------------------------------------------------------------------
traverse key (node4 ($ k1 v1) ($ k2 v2) ($ k3 v3) child1 child2 child3 child4) => ($ k1 v1)

key == k2
-------------------------------------------------------------------------------------------
traverse key (node4 ($ k1 v1) ($ k2 v2) ($ k3 v3) child1 child2 child3 child4) => ($ k2 v2)

key == k3
-------------------------------------------------------------------------------------------
traverse key (node4 ($ k1 v1) ($ k2 v2) ($ k3 v3) child1 child2 child3 child4) => ($ k3 v3)


<<key < k1>> == true
traverse key child1 => entry
----------------------------------------------------------------------------------------
traverse key (node4 ($ k1 v1) ($ k2 v2) ($ k3 v3) child1 child2 child3 child4) => entry

<<key > k1 && key < k2>> == true
traverse key child2 => entry
----------------------------------------------------------------------------------------
traverse key (node4 ($ k1 v1) ($ k2 v2) ($ k3 v3) child1 child2 child3 child4) => entry

<<key > k2 && key < k3>> == true
traverse key child3 => entry
----------------------------------------------------------------------------------------
traverse key (node4 ($ k1 v1) ($ k2 v2) ($ k3 v3) child1 child2 child3 child4) => entry

<<key > k3>> == true
traverse key child4 => entry
----------------------------------------------------------------------------------------
traverse key (node4 ($ k1 v1) ($ k2 v2) ($ k3 v3) child1 child2 child3 child4) => entry


traverse key root => entry
--------------------------------------
(dictionary root) get key => entry


s := <<"(" + (k1.ToString()) + "," + (v1.ToString()) + ")  ">>
---------------------------------------------------------
generateString (node2 ($ k1 v1) child1 child2) => ($s s)


s := <<"(" + (k1.ToString()) + "," + (v1.ToString()) + ")  (" + (k2.ToString()) + "," + (v2.ToString()) + ")">>
-----------------------------------------------------------
generateString (node3 ($ k1 v1) ($ k2 v2) child1 child2 child3) => ($s s)


s := <<"(" + (k1.ToString()) + "," + (v1.ToString()) + ")  (" + (k2.ToString()) + "," + (v2.ToString()) + ")  (" + (k3.ToString()) + "," + (v3.ToString()) + ")">>
-----------------------------------------------------------
generateString (node4 ($ k1 v1) ($ k2 v2) ($ k3 v3) child1 child2 child3 child4) => ($s s)


generateString (node2 ($ k1 v1) child1 child2) => ($s s1)
toString child1 => ($s s2)
toString child2 => ($s s3)
s := << s1 + "\n" + s2 + "  " + s3 >>
-------------------------------------------------------
toString (node2 ($ k1 v1) child1 child2) => ($s s)

generateString (node3 ($ k1 v1) ($ k2 v2) child1 child2 child3) => ($s s1)
toString child1 => ($s s2)
toString child2 => ($s s3)
toString child3 => ($s s4)
s := << s1 + "\n" + s2 + "  " + s3 + "  " + s4 >>
-------------------------------------------------------
toString (node3 ($ k1 v1) ($ k2 v2) child1 child2 child3) => ($s s)

generateString (node4 ($ k1 v1) ($ k2 v2) ($ k3 v3) child1 child2 child3 child4) => ($s s1)
toString child1 => ($s s2)
toString child2 => ($s s3)
toString child3 => ($s s4)
toString child4 => ($s s5)
s := << s1 + "\n" + s2 + "  " + s3 + "  " + s4 + "  " + s5 >>
-------------------------------------------------------
toString (node4 ($ k1 v1) ($ k2 v2) ($ k3 v3) child1 child2 child3 child4) => ($s s)


-----------------------------
toString nothing => ($s "")


toString root => ($s s)
output := <<EntryPoint.Print(s)>>
----------------------------------------------
prettyPrint (dictionary root) => nothing


k == k1
updatedNode := (node2 ($ k v) child1 child2)
---------------------------------------------------------------------------------------
d insert (node2 ($ k1 v1) child1 child2) ($ k v) => insertionResult updatedNode nothing

k == k1
updatedNode := (node3 ($ k v) ($ k2 v2) child1 child2 child3)
--------------------------------------------------------------------------------------------------
d insert (node3 ($ k1 v1) ($ k2 v2) child1 child2 child3) ($ k v) => insertionResult updatedNode nothing

k == k2
updatedNode := (node3 ($ k1 v1) ($ k v) child1 child2 child3)
--------------------------------------------------------------------------------------------------
d insert (node3 ($ k1 v1) ($ k2 v2) child1 child2 child3) ($ k v) => insertionResult updatedNode nothing

k == k1
updatedNode := (node4 ($ k v) ($ k2 v2) ($ k3 v3) child1 child2 child3 child4)
--------------------------------------------------------------------------------------------------
d insert (node4 ($ k1 v1) ($ k2 v2) ($ k3 v3) child1 child2 child3 child4) ($ k v) => insertionResult updatedNode nothing

k == k2
updatedNode := (node4 ($ k1 v1) ($ k v) ($ k3 v3) child1 child2 child3 child4)
--------------------------------------------------------------------------------------------------
d insert (node4 ($ k1 v1) ($ k2 v2) ($ k3 v3) child1 child2 child3 child4) ($ k v) => insertionResult updatedNode nothing

k == k3
updatedNode := (node4 ($ k1 v1) ($ k2 v2) ($ k v) child1 child2 child3 child4)
--------------------------------------------------------------------------------------------------
d insert (node4 ($ k1 v1) ($ k2 v2) ($ k3 v3) child1 child2 child3 child4) ($ k v) => insertionResult updatedNode nothing

<<k < k1>> == true
updatedNode := (node3 ($ k v) ($ k1 v1) nothing nothing nothing)
-----------------------------------------------------------------------------------------
d insert (node2 ($ k1 v1) nothing nothing) ($ k v) => insertionResult updatedNode nothing

<<k > k1>> == true
updatedNode := (node3 ($ k1 v1) ($ k v) nothing nothing nothing)
-------------------------------------------------------------------------
d insert (node2 ($ k1 v1) nothing nothing) ($ k v) => insertionResult updatedNode nothing


<<k < k1>> == true
updatedNode := (node4 ($ k v) ($ k1 v1) ($ k2 v2) nothing nothing nothing nothing)
--------------------------------------------------------------------------------
d insert (node3 ($ k1 v1) ($ k2 v2) nothing nothing nothing) ($ k v) => insertionResult updatedNode nothing

<<k > k1 && k < k2>> == true
updatedNode := (node4 ($ k1 v1) ($ k v) ($ k2 v2) nothing nothing nothing nothing)
--------------------------------------------------------------------------------
d insert (node3 ($ k1 v1) ($ k2 v2) nothing nothing nothing) ($ k v) => insertionResult updatedNode nothing

<<k > k2>> == true
updatedNode := (node4 ($ k1 v1) ($ k2 v2) ($ k v) nothing nothing nothing nothing)
--------------------------------------------------------------------------------
d insert (node3 ($ k1 v1) ($ k2 v2) nothing nothing nothing) ($ k v) => insertionResult updatedNode nothing


child1 := (node2 ($ k1 v1) child1 child2)
----------------------------------------------------------------------------
split (node4 ($ k1 v1) ($ k2 v2) ($ k3 v3) child1 child2 child3 child4) => 


(node4 ($ k1 v1) ($ k2 v2) ($ k3 v3) child1 child2 child3 child4) == root
split (node4 ($ k1 v1) ($ k2 v2) ($ k3 v3) child1 child2 child3 child4) => 
-----------------------------------------------------------------------------------------------
(dictionary root) insert (node4 ($ k1 v1) ($ k2 v2) ($ k3 v3) child1 child2 child3 child4) => insertionResult updatedNode nothing


(dictionary root) insert root entry => insertionResult updatedNode nothing
---------------------------------------------------
(dictionary root) add entry => (dictionary newRoot)



n1 := node3 ($ 1 ($i 3)) ($ 2 ($i 1)) (nothing) (nothing) (nothing)
root := node2 ($ 3 ($i 3)) (n1) (nothing)
t := dictionary root
prettyPrint t => nothing
t get 2 => res
----------------------------------------------------------------
runProgram => res





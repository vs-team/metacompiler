Func [] "$" [<<string>>] Priority 10 Type Expr => Id
Func [] "\" [Id Dot Term] Priority 9 Type Expr => Term
Data [] [Term] "|" [Term] Priority 8 Type Term
Data [] [] "->" [] Priority 0 Type Dot

Data [] [Term] "as" [Term] Priority 6 Type Where
Data [] [Term] "with" [Where] Priority 5 Type With

Id is Term

---------
$x => $x

---------------
\$x -> t => \$x -> t

-----------------
$x | u => $x | u

u | v => v'
v' | w => res
-------------------
(u | v) | w => res

t with ($x as u) => t'
-----------------------------------
\$x -> t | u => t'

  x == y
  ---------------------
  $y with $x as u => u

  x != y
  ----------------------
  $y with $x as u => $y

  x == y
  ----------------------------
  \$x -> t with $y as u => \$x -> t

  x != y
  t with $y as u => t'
  -----------------------------
  \$x -> t with $y as u => \$x -> t'

  t with $x as v => t'
  u with $x as v => u'
  --------------------------------
  (t | u) with $x as v => t' | u'

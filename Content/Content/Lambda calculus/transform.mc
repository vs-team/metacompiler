Keyword [] "$" [<<string>>] Priority 10 Class Id
Keyword [] "\" [Id Dot Term] Priority 9 Class Term
Keyword [Term] "|" [Term] Priority 8 Class Term
Keyword [] "->" [] Priority 0 Class Dot

Keyword [Term] "as" [Term] Priority 6 Class Where
Keyword [Term] "with" [Where] Priority 5 Class With

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

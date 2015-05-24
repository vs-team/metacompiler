Func "$" -> <<string>> : Id => Term                 Priority 10
Func "\" -> Id -> Dot -> Term : Term => Term        Priority 9
Func Term -> "|" -> Term : Term => Term             Priority 8 
Data "->" : Dot

Data Term -> "as" -> Term : Where                   Priority 6
Func Term -> "with" -> Where : With => Term         Priority 5

Func "run" : Expr => Term

Id is Term


(\$"x" -> $"x") | $"y" => res
------------------------------
run => res

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

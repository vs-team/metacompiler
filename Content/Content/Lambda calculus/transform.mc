Data "$" -> <<string>> : Id                         Priority 10
Data "\" -> Id -> Dot -> Term : Term                Priority 9
Data Term -> "|" -> Term : Term                     Priority 8 
Data "->" : Dot

Func "eval" -> Term : Expr => Term

Data Term -> "as" -> Term : Where                   Priority 6
Func Term -> "with" -> Where : With => Term

Func "run" : Expr => Term

Id is Term


eval (\$"x" -> $"x") | $"y" => res
------------------------------
run => res

---------
eval $x => $x

---------------
eval \$x -> t => \$x -> t

-----------------
eval $x | u => $x | u

eval u | v => v'
eval v' | w => res
-------------------
eval (u | v) | w => res

t with ($x as u) => t'
-----------------------------------
eval \$x -> t | u => t'

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

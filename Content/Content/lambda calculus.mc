Keyword = "$" LeftArguments = [] RightArguments = [<<System.String>>] Priority = 10 Class = "Term"
Keyword = "\" LeftArguments = [] RightArguments = [Term Dot Term] Priority = 9 Class = "Term"
Keyword = "|" LeftArguments = [Term] RightArguments = [Term] Priority = 8 Class = "Term"
Keyword = "." LeftArguments = [] RightArguments = [] Priority = 0 Class = "Dot"

Keyword = ":=" LeftArguments = [Term] RightArguments = [Term] Priority = 6 Class = "Where"
Keyword = "with" LeftArguments = [Term] RightArguments = [Where] Priority = 5 Class = "With"



-------------
$x => $x

-----------------------
\$x.t => \$x.t

---------------------------
$x | u => $x | u

u | v => v'
v' | w => res
-------------------
(u | v) | w => res

t with $x := u => t'
---------------------------
\$x.t | u => t'

  x == y
  -------------------------------
  $y with $x := u => u

  x != y
  ------------------------------------
  $y with $x := u => $y

  x == y
  --------------------------------------------
  \$x.t with $y := u => \$x.t

  x != y
  t with $y := u => t'
  -----------------------------------------------
  \$x.t with $y := u => \$x.t'

  t with $x := v => t'
  u with $x := v => u'
  ----------------------------------------
  (t | u) with $x := v => t' | u'


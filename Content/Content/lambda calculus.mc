Keyword = "." LeftArguments = [] RightArguments = [] Priority = 0 Class = "Dot"
Keyword = "|" LeftArguments = [Term] RightArguments = [Term] Priority = 1 Class = "Term"
Keyword = "\" LeftArguments = [] RightArguments = [Term Dot Term] Priority = 2 Class = "Term"
Keyword = "$" LeftArguments = [] RightArguments = [<<System.String>>] Priority = 1 Class = "Term"
Keyword = "with" LeftArguments = [Term] RightArguments = [Where] Priority = 4 Class = "With"
Keyword = ":=" LeftArguments = [Term] RightArguments = [Term] Priority = 5 Class = "Where"



-------------
$x => $x

-----------------------
\($x).t => \($x).t

---------------------------
($x) | u => ($x) | u

t with (($x) := u) => t'
---------------------------
(\($x).t) | u => t'

  x == y
  -------------------------------
  ($y) with (($x) := u) => u

  x != y
  ------------------------------------
  ($y) with (($x) := u) => ($y)

  x == y
  --------------------------------------------
  (\($x).t) with (($y) := u) => \($x).t

  x != y
  t with (($y) := u) => t'
  -----------------------------------------------
  (\($x).t) with (($y) := u) => (\($x).t')

  t with (($x) := v) => t'
  u with (($x) := v) => u'
  ----------------------------------------
  (t | u) with (($x) := v) => t' | u'


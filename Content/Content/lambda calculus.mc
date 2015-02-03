Keyword = "id" LeftAriety = 0 RightAriety = 1 Priority = 0
Keyword = ">>" LeftAriety = 1 RightAriety = 1 Priority = 1
Keyword = "\" LeftAriety = 0 RightAriety = 3 Priority = 2
Keyword = "." LeftAriety = 0 RightAriety = 0 Priority = 3
Keyword = "with" LeftAriety = 1 RightAriety = 1 Priority = 4
Keyword = ":=" LeftAriety = 1 RightAriety = 1 Priority = 5



-------------
id x => id x

-----------------------
\(id x).t => \(id x).t

---------------------------
(id x) >> u => (id x) >> u

t with ((id x) := u) => t'
---------------------------
(\(id x).t) >> u => t'

  << x.Equals(y) >> => << true >>
  -------------------------------
  (id y) with ((id x) := u) => u

  << x.Equals(y) >> => << false >>
  ------------------------------------
  (id y) with ((id x) := u) => (id y)

  << x.Equals(y) >> => << true >>
  --------------------------------------------
  (\(id x).t) with ((id y) := u) => \(id x).t

  << x.Equals(y) >> => << false >>
  t with ((id y) := u) => t'
  -----------------------------------------------
  (\(id x).t) with ((id y) := u) => (\(id x).t')

  t with ((id x) := v) => t'
  u with ((id x) := v) => u'
  ----------------------------------------
  (t >> u) with ((id x) := v) => t' >> u'

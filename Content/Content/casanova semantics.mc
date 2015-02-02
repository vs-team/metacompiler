Keyword = "," LeftAriety = 1 RightAriety = 1 Priority = 25
Keyword = ";" LeftAriety = 1 RightAriety = 1 Priority = 26
Keyword = "->" LeftAriety = 1 RightAriety = 1 Priority = 27
Keyword = "exprEval" LeftAriety = 0 RightAriety = 2 Priority = 47
Keyword = "assignFields" LeftAriety = 0 RightAriety = 3 Priority = 48
Keyword = "rulesEval" LeftAriety = 0 RightAriety = 2 Priority = 49
Keyword = "ruleEval" LeftAriety = 0 RightAriety = 2 Priority = 49
Keyword = "rule" LeftAriety = 0 RightAriety = 3 Priority = 50
Keyword = "cont" LeftAriety = 0 RightAriety = 1 Priority = 51
Keyword = "yield" LeftAriety = 0 RightAriety = 1 Priority = 52
Keyword = "wait" LeftAriety = 0 RightAriety = 1 Priority = 53
Keyword = "unit" LeftAriety = 0 RightAriety = 0 Priority = 54
Keyword = "id" LeftAriety = 0 RightAriety = 1 Priority = 55
Keyword = "if" LeftAriety = 0 RightAriety = 5 Priority = 100
Keyword = "-" LeftAriety = 1 RightAriety = 1 Priority = 500
Keyword = "+" LeftAriety = 1 RightAriety = 1 Priority = 501
Keyword = "/" LeftAriety = 1 RightAriety = 1 Priority = 502
Keyword = "*" LeftAriety = 1 RightAriety = 1 Priority = 503
Keyword = "arith_op" LeftAriety = 0 RightAriety = 3 Priority = 504
Keyword = "bool_op" LeftAriety = 0 RightAriety = 3 Priority = 505
Keyword = "then" LeftAriety = 0 RightAriety = 0 Priority = 1000
Keyword = "else" LeftAriety = 0 RightAriety = 0 Priority = 1001
Keyword = "true" LeftAriety = 0 RightAriety = 0 Priority = 1002
Keyword = "false" LeftAriety = 0 RightAriety = 0 Priority = 1003
Keyword = ":=" LeftAriety = 0 RightAriety = 0 Priority = 1002


M, expr => res
---------------------------
exprEval M expr => M', res

  M, c => c'
  M, if c' then a else b => res
  -------------------------------
  M, (if c then a else b) => res

    M, a => res
    ----------------------------------
    M, (if true then a else b) => res

    M, b => res
    -----------------------------------
    M, (if false then a else b) => res

  M[s] => res
  -------------
  M, id s => res

  M, x => x'
  M', y => y'
  << x' op y' >> => res
  --------------------------
  M, (bool_op op x y) => res

  M, x => x'
  M', y => y'
  << x' op y' >> => res
  ---------------------------
  M, (arith_op op x y) => res

  M, a => unit
  M', b => res
  -----------------------
  M, (a; b) => M', res

  M, a => M', cont(a)
  ---------------------------
  M, (a; b) => M', cont(a; b)

  M, c => M', true
  ----------------------
  M, wait c => M', unit

  M, c => M', false
  ------------------------------------
  M, wait c => M', cont((wait c); unit)

  M, a => M', res
  ----------------------------------------
  M, yield a => M', cont((yield res); unit)



M, r => M', r'
-----------------------------
ruleEval M r => M', r'

  M[f -> res] => M'
  ---------------------------
  assignFields M f res => M'

  M[f -> x] => M'
  assignFields M' fs xs => M''
  --------------------------------------
  assignFields M (f, fs) (x, xs) => M''

  M, b => unit
  -----------------------------------------------------
  M, (rule FS := (b, b0)) => M', (rule FS := (b0, b0))

  M, b => cont((yield res); b')
  assignFields M FS res => M'
  -------------------------------------------------
  M, rule FS := (b, b0) => M', rule FS := (b', b0)

  M, b => cont((wait c); b')
  ----------------------------------------------------
  M, (rule FS := (b, b0)) => M, (rule FS := (b', b0))

  -----------------------------
  assignFields M unit res => M


M, rs => M', rs'
--------------------------------
rulesEval M rs => M', rs'

  -------------
  M, unit => M

  ruleEval M r => M', r'
  rulesEval M' rs => M'', rs'
  -------------------------------------
  (M, r); rs => (M'', r'); rs'


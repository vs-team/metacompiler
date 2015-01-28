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
  M, s => res

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
  M, wait c => M', cont(wait c; unit)

  M, a => M', res
  ----------------------------------------
  M, yield a => M', cont(yield res; unit)



M, rule => M', rule'
-----------------------------
ruleEval M rule => M', rule'

  M[f -> res] => M'
  ---------------------------
  assignFields M f res => M'

  M[f -> x] => M'
  assignFields M' fs xs => M''
  --------------------------------------
  assignFields M (f, fs) (x, xs) => M''

  M, b => unit
  -----------------------------------------------
  M, rule FS = (b, b0) => M', rule FS = (b0, b0)

  M, b => cont(yield res; b')
  assignFields M FS res => M'
  -----------------------------------------------
  M, rule FS = (b, b0) => M', rule FS = (b', b0)

  M, b => cont(wait c; b')
  ----------------------------------------------
  M, rule FS = (b, b0) => M, rule FS = (b', b0)

  -----------------------------
  assignFields M unit res => M


M, rules => M', rules'
--------------------------------
rulesEval M rules => M', rules'

  -------------
  M, unit => M

  ruleEval M rule => M', rule'
  rulesEval M' rules => M'', rules'
  -------------------------------------
  M, rule; rules => M'', rule'; rules'


import System

Func "run" : Expr => Value                      Priority 0 

Func "eval" -> Expr : Expr => Value             Priority 1
Func "flip" -> Value : Expr => Value
Func "shortcutOnTrue" -> Value -> Expr : Expr => Value
Func "shortcutOnFalse" -> Value -> Expr : Expr => Value
Data Expr -> "|" -> Expr : Expr                 Priority 10
Data Expr -> "&" -> Expr : Expr                 Priority 20
Data "!" -> Expr : Expr                         Priority 30

Data "TRUE" : Value
Data "FALSE" : Value


Value is Expr


eval (FALSE | (TRUE & !FALSE)) => res
-----------------------------
run => res

  ------------------
  eval TRUE => TRUE

  ------------------
  eval FALSE => FALSE


  eval a => y
  flip y => y'
  -------------------
  eval !a => y'
    
    -------------------
    flip TRUE => FALSE

    -------------------
    flip FALSE => TRUE


  eval a => y
  shortcutOnTrue y b => y'
  -------------------
  eval (a|b) => y'

    ------------------------
    shortcutOnTrue TRUE b => TRUE

    eval b => y
    ------------------------
    shortcutOnTrue FALSE b => y


  eval a => y
  shortcutOnFalse y b => y'
  -------------------
  eval (a&b) => y'

    ---------------------------------
    shortcutOnFalse FALSE b => FALSE

    eval b => y
    ----------------------------
    shortcutOnFalse TRUE b => y

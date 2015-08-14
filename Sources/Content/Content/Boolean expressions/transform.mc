import System

Func "run" : Expr => Value                      Priority 0 

Func "eval" -> Expr : Expr => Value             Priority 1
Data Expr -> "|" -> Expr : Expr                 Priority 10
Data Expr -> "&" -> Expr : Expr                 Priority 20
Data "!" -> Expr : Expr                         Priority 30

Data "TRUE" : Value
Data "FALSE" : Value


Value is Expr


eval (FALSE | TRUE & !FALSE) => res
-----------------------------
run => res

  ------------------
  eval TRUE => TRUE

  ------------------
  eval FALSE => FALSE


  eval a => TRUE
  -------------------
  eval !a => FALSE

  eval a => FALSE
  -------------------
  eval !a => TRUE


  eval a => TRUE
  -------------------
  eval (a|b) => TRUE

  eval a => FALSE
  eval b => y
  -------------------
  eval (a|b) => y


  eval a => FALSE
  -------------------
  eval (a&b) => FALSE

  eval a => TRUE
  eval b => y
  -------------------
  eval (a&b) => y

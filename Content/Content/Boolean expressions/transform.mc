import System

Func [] "run" [] Priority 0 Type Expr => Value

Func [] "eval" [Expr] Priority 1 Type Expr => Value
Data [] [Expr] "|" [Expr] Priority 10 Type Expr
Data [] [Expr] "&" [Expr] Priority 20 Type Expr
Data [] [] "!" [Expr] Priority 30 Type Expr
Data [] [] "TRUE" [] Priority 10000 Type Value
Data [] [] "FALSE" [] Priority 10000 Type Value

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

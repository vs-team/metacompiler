Keyword = "overflow" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Num"
Keyword = "nil" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Num"
Keyword = "d0" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Digit"
Keyword = "d1" LeftArguments = [] RightArguments = [] Priority = 0 Class = "Digit"
Keyword = "," LeftArguments = [Num] RightArguments = [Digit] Priority = 10 Class = "Num"

Keyword = "+" LeftArguments = [Num] RightArguments = [Num] Priority = 5 Class = "Expr"
Keyword = "addRest" LeftArguments = [] RightArguments = [Num Num Digit] Priority = 5 Class = "Expr"

Num is Expr


addRest a b d0 => c
--------------------
a + b => c

  --------------------------------
  addRest nil nil d1 => overflow

  --------------------------
  addRest nil nil d0 => nil

  addRest a b d0 => res
  -------------------------------
  addRest a,d0 b,d0 d0 => res,d0

  addRest a b d0 => res
  -------------------------------
  addRest a,d0 b,d0 d1 => res,d1

  addRest a b d0 => res
  -------------------------------
  addRest a,d0 b,d1 d0 => res,d1

  addRest a b d1 => res
  -------------------------------
  addRest a,d0 b,d1 d1 => res,d0

  addRest a b d0 => res
  -------------------------------
  addRest a,d1 b,d0 d0 => res,d1

  addRest a b d1 => res
  -------------------------------
  addRest a,d1 b,d0 d1 => res,d0

  addRest a b d1 => res
  -------------------------------
  addRest a,d1 b,d1 d0 => res,d0

  addRest a b d1 => res
  -------------------------------
  addRest a,d1 b,d1 d1 => res,d1


Keyword [] "overflow" [] Priority 0 Class Num
Keyword [] "nil" [] Priority 0 Class Num
Keyword [] "d0" [] Priority 0 Class Digit
Keyword [] "d1" [] Priority 0 Class Digit
Keyword [Num] "," [Digit] Priority 10 Class Num

Keyword [Num] "+" [Num] Priority 5 Class Expr
Keyword [] "addCarry" [Num Num Digit] Priority 5 Class Expr
Keyword [] "addDigits" [Digit Digit Digit] Priority 5 Class Expr

Num is Expr


addCarry a b d0 => c
--------------------
a + b => c
  
  --------------------------------
  addCarry nil nil d1 => overflow

  --------------------------
  addCarry nil nil d0 => nil

  addDigits da db dr => (nil,dr',d)
  addCarry a b dr' => res
  ----------------------------------
  addCarry a,da b,db dr => res,d

    ----------------------------------
    addDigits d0 d0 d0 => (nil,d0),d0

    ----------------------------------
    addDigits d0 d0 d1 => (nil,d0),d1

    ----------------------------------
    addDigits d0 d1 d0 => (nil,d0),d1

    ----------------------------------
    addDigits d0 d1 d1 => (nil,d1),d0

    ----------------------------------
    addDigits d1 d0 d0 => (nil,d0),d1

    ----------------------------------
    addDigits d1 d0 d1 => (nil,d1),d0

    ----------------------------------
    addDigits d1 d1 d0 => (nil,d1),d0

    ----------------------------------
    addDigits d1 d1 d1 => (nil,d1),d1


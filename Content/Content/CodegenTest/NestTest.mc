Func "Count1" -> <<int>> : Expr => <<int>>
Func "Count2" -> <<int>> : Expr => <<int>>
Func "Count3" -> <<int>> : Expr => <<int>>

Count2 x => res
------------------
Count1 x => res

  Count3 x => res
  ---------------
  Count2 x => res


    -------------
    Count3 x => x

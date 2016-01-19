Data double -> "," -> double -> double*double
Func "solve_quadratic" -> double -> double -> double -> double*double
Func "run" -> () -> double

b*b-4*a*c -> d
d > 0.0
-b - (sqrt d) -> x
a*x*x+b*x+c -> y
--------------
solve_quadratic a b c -> x,y

b*b-4*a*c -> d
d > 0.0
-b + (sqrt d) -> x
a*x*x+b*x+c -> y
--------------
solve_quadratic a b c -> x,y

b*b-4*a*c -> d
d = 0.0
-b -> x
a*x*x+b*x+c -> y
--------------
solve_quadratic a b c -> x,y

 2.0 -> a
-4.0 -> b
-5.0 -> c
---------
run -> solve_quadratic a b c

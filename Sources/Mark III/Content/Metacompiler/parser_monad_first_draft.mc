import Prelude
import BasicMonads


TypeFunc "parser" => * => * => result

result M => res
state(res,ctxt) => state^ctxt
state(state^ctxt,[char]) => st
--
parser M char ctxt => Monad( Mcons^st ) {
    inherit st 

    Func "try" -> Mcons 'a -> ('a -> Mcons 'b) -> (String -> Mcons 'b) -> Mcons 'b
    ArrowFunc Mcons 'a -> "\_/" -> Mcons 'b -> Mcons ('a | 'b)  #> 60
    ArrowFunc Mcons 'c -> ">>" -> Mcons 'd -> Mcons 'd          #> 10
    Func "firstSuccesful" -> [Mcons 'a] -> Mcons 'a
    ArrowFunc Mcons 'a -> "\/" -> Mcons 'a -> Mcons 'a          #> 60
  ++Func "nothing" -> Mcons Unit
    Func "repeat" -> Mcons 'c -> Mcons ['c]
    Func "step" -> Mcons 'a
    Func "eof" -> Mcons Unit

  ++Func "ignore" -> Mcons 'c -> Mcons Unit
    Func "lookahead" -> Mcons 'c -> Mcons 'c
  ++Func "fail" -> String -> Mcons 'c
  ++Func "getBuffer" -> Mcons 'a
  ++Func "getContext" -> Mcons 'b
  ++Func "setContext" -> 'b -> Mcons Unit


    lift^st( lift^ctxt( try^res (p chars ctxt) k err ) => res
    --
    (try p k err) chars ctxt -> res


    --
    try p1 p2 -> res

    (match try p1 with
      (\x -> return(A(x)))
      (\e1 -> 
        (match try p2 with
          (\y -> return(B(y)))
          (\e2 -> fail (e1+e2)))) >>= res
    --
    (p1 \_/ p2) chars ctxt -> res
    
    p >>= Unit
    k >>= out
    --
    p >> k -> out
  
    (ps 
      (\[] -> fail "firstSuccesful failed."
      (\p::ps ->
        (try p
	  (\
	  (\res
    --
    firstSuccesful (p::ps) -> res
    
    (try p1
      (\x -> return x)
      (\e1 -> 
        (try p2
          (\y -> return y)
          (\e2 -> fail (e1+e2)))) >>= res
    --
    (p1 \/ p2) -> res

    ++nothing chars ctxt -> return chars ctxt
  
    ((p >>= x
      repeat p >>= xs
      --
      return (x :: xs)) \/ 
      (nothing >> (return empty))) >>= out
    --
    repeat p -> out

    (a
      (\ (c :: cs) -> return c cs
      (\ empty -> fail "Error: unexpected eof." empty) -> res
    --
    step a -> res

    (a
      (\ eof (c :: cs) -> fail "Error: expected eof." (c :: cs)
      (\ eof empty -> return Unit empty)) -> res
    --
    eof a -> res
  
    p >>= x
    --
  ++ignore p -> return Unit

    ((try^res (p chars ctxt)) 
      (\(Done(x) -> return^res(Done(x))) 
      (\e -> fail^res e)) >>= res
    --
    lookahead p chars ctxt -> res
  
  ++fail msg chars ctxt -> Error(msg)

  ++getBuffer chars ctxt -> return^parser(chars,ctxt,chars)
  
  ++getContext chars ctxt -> return^parser(chars,ctxt,ctxt)
  
  ++setContext ctxt' chars ctxt -> Done(Unit,chars,ctxt')
  }

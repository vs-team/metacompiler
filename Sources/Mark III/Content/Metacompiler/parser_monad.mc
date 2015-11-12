import Prelude
import BasicMonads


TypeFunc "parser" => * => * => result

result M => res
state(res,ctxt) => state^ctxt
state(state^ctxt,[char]) => st
--
parser M char ctxt => Monad( Mcons^st ) {
    inherit st 

    Func "retRes" -> 'char -> 'ctxt -> 'result -> Mcons
    Func "try" -> Mcons 'a -> ('a -> Mcons 'b) -> (String -> Mcons 'b) -> Mcons 'b
    ArrowFunc Mcons 'a -> "\_/" -> Mcons 'b -> Mcons ('a | 'b)  #> 60
    ArrowFunc Mcons 'c -> ">>" -> Mcons 'd -> Mcons 'd          #> 10
    Func "firstSuccesful" -> [Mcons 'a] -> Mcons 'a
    ArrowFunc Mcons 'a -> "\/" -> Mcons 'a -> Mcons 'a          #> 60
    Func "nothing" -> 'char -> 'ctxt -> Mcons
    Func "repeat" -> Mcons 'c -> Mcons ['c]
    Func "step" -> Mcons 'a
    Func "eof" -> Mcons Unit
    Func "ignore" -> Mcons 'c -> Mcons Unit
    Func "lookahead" -> Mcons 'c -> Mcons 'c
    Func "fail" -> 'char -> 'ctxt -> String  -> Mcons
    Func "getBuffer" -> 'char -> 'ctxt -> Mcons
    Func "getContext" -> 'char -> 'ctxt -> Mcons
    Func "setContext" -> 'char -> 'ctxt -> Mcons


 $*
    lift^st(return^st(
      lift^state^ctxt(return^state^ctxt(
        lift^res(return^res(result)),
      ctxt)),
    chars)) => res'
 *$
    return^res result = result'
    return^state^ctxt(result,ctxt) => ctxt'
    return^st(ctxt',chars) => res'
    --
    retRes chars ctxt result -> res'

    lift^st( lift^ctxt( try^res (p chars ctxt) k err ) => res
    --
    (try p k err) chars ctxt -> res


  $$--
  $$try p1 p2 -> res

    (match try p1 with
      (\lift^st(lift^st_ctxt(e1,p1e)) -> 
        (match try p2 with
          (\lift^st(lift^st_ctxt(e2,p2e)) ->
	    fail (e1+e2))
          (\y -> return(y))
	)
      (\x -> return(x))
    ) >>= res
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

    nothing chars ctxt -> retRes chars ctxt Unit
  
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
    ignore p -> return Unit

    ((try^res (p chars ctxt)) 
      (\(Done(x) -> return^res(Done(x))) 
      (\e -> fail^res e)) >>= res
    --
    lookahead p chars ctxt -> res
  
    fail chars ctxt msg -> retRes(chars,ctxt,msg)

    getBuffer chars ctxt -> retRes(chars,ctxt,chars)
  
    getContext chars ctxt -> retRes(chars,ctxt,ctxt)

    setContext chars ctxt -> retRes(chars,ctxt,Unit)
  }

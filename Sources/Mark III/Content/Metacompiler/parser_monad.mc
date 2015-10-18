import Prelude
import Monads

TypeName "Parser" 'char 'ctxt 'result = List 'char -> 'ctxt -> Result ('char * 'ctxt * 'result)

Instance "prs" Monad (Parser 'char 'ctxt)
  {
    return^res (res,chars,ctxt) >>= out
    -----------------------------------
    return res chars ctxt -> out

    p chars ctxt >>=^res (res,chars',ctxt')
    k res chars' ctxt' >>= out
    ----------------------------------------
    (p >>= k) chars ctxt -> out

    Func "nothing" -> Parser 'a 'b Unit
    Func "fail" -> String -> Parser 'a 'b 'c
    Func Parser 'a 'b 'c -> ">>" -> Parser 'a 'b 'd -> Parser 'a 'b 'd
    Func (Parser 'chars 'ctxt 'a) -> "\/" -> (Parser 'chars 'ctxt 'a) -> (Parser 'chars 'ctxt 'a)
    Func (Parser 'char 'ctxt 'a) -> "\_/" -> (Parser 'char 'ctxt 'b) -> (Parser 'char 'ctxt ('a | 'b)
    Func "repeat" -> Parser 'a 'b 'c -> Parser 'a 'b (List 'c)
    Func "step" -> Parser 'a 'b 'a
    Func "eof" -> Parser 'a 'b Unit
    Func "ignore" -> Parser 'a 'b 'c -> Parser 'a 'b Unit
    Func "lookahead" -> Parser 'a 'b 'c -> Parser 'a 'b 'c
    Func "getBuffer" -> Parser 'a 'b 'a
    Func "getContext" -> Parser 'a 'b 'b
    Func "setContext" -> 'b -> Parser 'a 'b Unit

    nothing chars ctxt -> Done((),chars,ctxt)

    (try <- (p1 chars ctxt)) 
      return^res 
      (\e1 -> (try <- (p2 chars ctxt)) 
      return^res (\e2 -> fail (e1+e2))) >>= out
    --------------------------------------------------------------
    (p1 \/ p2) chars ctxt -> out

    fail msg chars ctxt -> Error(msg)

    getBuffer chars ctxt -> Done(chars,chars,ctxt)

    getContext chars ctxt -> Done(ctxt,chars,ctxt)

    setContext ctxt' chars ctxt -> Done(unit,chars,ctxt')

    p >>= unit
    k >>= out
    --
    p >> k -> out

    { (try <- (p1 chars ctxt)) 
      (\(chars',ctxt',res) -> return^res(A res,chars',ctxt')) 
      (\e1 -> (try <- (p2 chars ctxt)) 
      (\(chars',ctxt',res) -> return^res(B res,chars',ctxt')) 
      (\e2 -> fail^res (e1+e2))) } >>= out
    --------------------------------------------------------------
    (p1 \_/ p2) chars ctxt -> out

    { (p >>= x
      repeat p >>= xs
      return (x :: xs)) \/ 
      (nothing >> (return empty)) } >>= out
    --
    repeat p -> out

    return c -> out
    --
    step (c :: cs) ctxt -> out

    fail "Error: unexpected eof." -> out
    --
    step (empty) ctxt-> out

    fail "Error: expected eof." -> out
    --
    eof (c :: cs) ctxt-> out

    return unit >>= out
    --
    eof empty ctxt -> out

    p >>= x
    return unit >>= out
    --
    ignore p -> out

    { (try <- (p chars ctxt)) 
      (\(chars',ctxt',res) -> return^res(res,chars,ctxt)) 
      (\e -> fail^res e) } >>= out
    --
    lookahead p chars ctxt -> out
  }

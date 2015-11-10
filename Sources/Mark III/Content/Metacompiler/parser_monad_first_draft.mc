import Prelude
import BasicMonads

TypeFunc "ParserT" => (* => * => *) => * => * => *

ParserT 'M 'char 'ctxt 'res => 
  (['char]) -> 'ctxt -> result( state( state( result('res) * ['char]) * 'ctxt))


TypeFunc "parser" => * => * => result

parser char ctxt => Monad( ResultT state( state( result(res) * [char'] ) * ctxt') ) {
  
  return chars ctxt res -> Done( return^state( return^state( return^Mcons^res( res ),chars),ctxt))

  p chars ctxt >>=^result (outM)
  outM >>=^state (charsM,ctxt')
  charsM >>=^state (chars',resM)
  resM >>=^result (res)
  --
  (p >>= k) chars ctxt -> k res chars' ctxt'

  Func "nothing" -> Mcons^parser Unit
  Func "fail" -> String -> Mcons^parser 'c
  Func Mcons^parser 'c -> ">>" -> Mcons^parser 'd -> Mcons^parser 'd
  Func (Mcons^parser 'a) -> "\/" -> (Mcons^parser 'a) -> (Mcons^parser 'a)
  Func (Mcons^parser 'a) -> "\_/" -> (Mcons^parser 'b) -> (Mcons^parser ('a | 'b)
  Func "repeat" -> Mcons^parser 'c -> Mcons^parser (List 'c)
  Func "step" -> Mcons^parser 'a
  Func "eof" -> Mcons^parser Unit
  Func "ignore" -> Mcons^parser 'c -> Mcons^parser Unit
  Func "lookahead" -> Mcons^parser 'c -> Mcons^parser 'c
  Func "getBuffer" -> Mcons^parser 'a
  Func "getContext" -> Mcons^parser 'b
  Func "setContext" -> 'b -> Mcons^parser Unit
  Func "try" -> (Mcons^parser 'a) -> (a -> Mcons^parser 'b) -> (String -> Mcons^parser 'b) -> (Mcons^parser 'b)

  nothing chars ctxt -> Done( return^state( return^state( return^result(),chars),ctxt))

  fail msg chars ctxt -> Error(msg)

  (try^res (p chars ctxt)
    (\Done(x) -> 
	  x >>=^x (charsM,ctxt')
      charsM >>=^state (chars',resM)
      resM >>=^result (res)
	  k res chars' ctxt')
    (\e -> return^parser(chars,ctxt,(err+e)) ) -> res
  --
  (try p k err) chars ctxt -> res

  (try p1
    (\x -> return x)
    (\e1 -> 
      (try p2
        (\y -> return y)
        (\e2 -> fail (e1+e2)))) >>= res
  --
  (p1 \/ p2) -> res

  getBuffer chars ctxt -> return^parser(chars,ctxt,chars)

  getContext chars ctxt -> return^parser(chars,ctxt,ctxt)

  setContext ctxt' chars ctxt -> Done(Unit,chars,ctxt')

  p >>= Unit
  k >>= out
  --
  p >> k -> out

  (try p1
    (\x -> return^parser(A(x)))
    (\e1 -> 
      (try p2
        (\y -> return^parser(B(y)))
        (\e2 -> fail (e1+e2)))) >>= res
  --
  (p1 \_/ p2) chars ctxt -> res

  ((p >>= x
    repeat p >>= xs
    --
    return (x :: xs)) \/ 
    (nothing >> (return empty))) >>= out
  --
  repeat p -> out

  step (c :: cs) -> return c cs

  step empty -> fail "Error: unexpected eof." empty

  eof (c :: cs) -> fail "Error: expected eof." (c :: cs)

  eof empty -> return^parser(Unit empty)

  p >>= x
  --
  ignore p -> return Unit

  ((try^res (p chars ctxt)) 
    (\(Done(x) -> return^res(Done(x))) 
    (\e -> fail^res e)) >>= res
  --
  lookahead p chars ctxt -> res
}

  
///////old parser\\\\\\\\\\\\\\\
Instance "prs" Monad (Parser 'char 'ctxt)
  {
    return res chars ctxt -> return^res (res,chars,ctxt)

    p chars ctxt >>=^res (res,chars',ctxt')
    ------------------------------------------
    (p >>= k) chars ctxt -> k res chars' ctxt'

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
    Func "try" -> (Parser 'chars 'ctxt 'a) -> (a -> Parser 'chars 'ctxt 'b) -> (String -> Parser 'chars 'ctxt 'b) -> (Parser 'chars 'ctxt 'b)

    nothing chars ctxt -> Done((),chars,ctxt)

    (try^res (p chars ctxt)
      (\(x,chars',ctxt') -> k x chars' ctxt')
      (\e -> err e chars ctxt)) -> res
    -------------------------------------------
    (try p k err) chars ctxt -> res

    (try p1
      (\x -> return x)
      (\e1 -> 
        (try p2
          (\y -> return y)
          (\e2 -> fail (e1+e2)))) >>= res
    --------------------------------------------
    (p1 \/ p2) -> res

    fail msg chars ctxt -> Error(msg)

    getBuffer chars ctxt -> Done(chars,chars,ctxt)

    getContext chars ctxt -> Done(ctxt,chars,ctxt)

    setContext ctxt' chars ctxt -> Done(Unit,chars,ctxt')

    p >>= Unit
    k >>= out
    --
    p >> k -> out

    (try p1
      (\x -> return (A(x)))
      (\e1 -> 
        (try p2
          (\y -> return (B(y)))
          (\e2 -> fail (e1+e2)))) >>= res
    --------------------------------------------------------------
    (p1 \_/ p2) chars ctxt -> res

    ((p >>= x
      repeat p >>= xs
      --
      return (x :: xs)) \/ 
      (nothing >> (return empty))) >>= out
    ---------------------------------------
    repeat p -> out

    step (c :: cs) -> return c cs

    step empty -> fail "Error: unexpected eof." empty

    eof (c :: cs) -> fail "Error: expected eof." (c :: cs)

    eof empty -> return Unit empty

    p >>= x
    --
    ignore p -> return Unit

    ((try^res (p chars ctxt)) 
      (\(chars',ctxt',res) -> return^res(res,chars,ctxt)) 
      (\e -> fail^res e)) >>= res
    --
    lookahead p chars ctxt -> res
  }

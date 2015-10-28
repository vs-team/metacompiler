import Prelude
import BasicMonads

TypeName "Parser" 'char 'ctxt 'result = List 'char -> 'ctxt -> Result ('char * 'ctxt * 'result)

TypeFunc "ParserT" => (* => * => *) => * => * => *

ParserT 'M 'char 'ctxt 'res => 
  (['char]) -> 'ctxt -> result( state( state( result(['char]) * 'ctxt) * 'ctxt))
  (['char]) -> 'ctxt -> result( state( state( result(['char]) * 's1) * 's2))





TypeFunc "parser" => state => state => result

char' = 
s1 = 
s2 = 
--
parser M char ctxt => Monad( ResultT state( state( result(char') * s1) * s2) ) {
  
  return chars ctxt res -> return^result (chars,ctxt,res)


  p chars ctxt >>=^res (res,chars',ctxt')
  ------------------------------------------
  (p >>= k) chars ctxt -> k res chars' ctxt'

}

  

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

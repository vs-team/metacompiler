module ExtractFromToken2

open ParserMonad
open Lexer2
open Common

let extract_position_from_token (token:Token) :Position =
  match token with  
  | Lexer2.Id(_,pos) -> pos
  | Lexer2.Keyword(_,pos) -> pos
  | Lexer2.Literal(_,pos) -> pos

let extract_keyword() :Parser<Token,_,Keyword*Position> =
  prs{
    let! next = step
    match next with
    | Keyword(k,p) -> return k,p
    | _ -> 
      let err = sprintf "keyword expected but found: %A . %A" next (extract_position_from_token next)
      return! fail (ParserError err)
  }

let extract_keyword_with_error(st:string) :Parser<Token,_,Keyword*Position> =
  prs{
    let! next = step
    match next with
    | Keyword(k,p) -> return k,p
    | _ -> 
      let err = sprintf "keyword expected: %s but found: %A. %A" st next (extract_position_from_token next)
      return! fail (ParserError err)
  }

let extract_id() :Parser<Token,_,string*Position> =
  prs{
    let! next = step
    match next with
    | Lexer2.Id(i,p) -> return i,p
    | _ -> 
      let err = sprintf "Id. %A" (extract_position_from_token next)
      return! fail (ParserError err)
  }

let extract_literal() :Parser<Token,_,Literal*Position> =
  prs{
    let! next = step
    match next with
    | Lexer2.Literal(lit,pos) -> return lit,pos
    | _ -> 
      let err = sprintf "literal. %A" (extract_position_from_token next)
      return! fail (ParserError err)
  }

let extract_string_literal() :Parser<Token,_,string*Position> =
  prs{
    let! next = step
    match next with
    | Lexer2.Literal(String(str),pos) -> return str,pos
    | _ -> 
      let err = sprintf "string literal. %A" (extract_position_from_token next)
      return! fail (ParserError err)
  }

let extract_int_literal() :Parser<Token,_,int*Position> =
  prs{
    let! next = step
    match next with
    | Lexer2.Literal(I32(i),pos) -> return i,pos
    | Lexer2.Literal(I64(i),pos) -> return (int(i),pos)
    | _ -> 
      let err = sprintf "int literal. %A" (extract_position_from_token next)
      return! fail (ParserError err)
  }

let check_id() (expected:string) :Parser<Token,_,_> =
  prs{
    let! id,pos = extract_id()
    if id = expected then return ()
    else
      let err = sprintf "id error. expected: %s but got %s" expected id
      return! fail (ParserError err) 

  }
  
let check_keyword() (expected:Keyword) :Parser<Token,_,_> =
  prs{
    let! k,p = extract_keyword_with_error (sprintf "%A" expected)
    let st = sprintf "Keyword: %A does not match expected: %A" k expected
    if k = expected then return () 
    else 
      let err = sprintf "%s. %A" st p
      return! fail (ParserError err)
  }

let check_condition() :Parser<Token,_,Predicate> =
  prs{
    let! exp,pos = extract_keyword()
    match exp with
    | Predicate Less         -> return Less        
    | Predicate LessEqual    -> return LessEqual   
    | Predicate Greater      -> return Greater     
    | Predicate GreaterEqual -> return GreaterEqual
    | Predicate Equal        -> return Equal       
    | Predicate NotEqual     -> return NotEqual    
    | _ -> return! fail (ParserError (sprintf "expected condition but got: %A" (exp,pos)))
  }

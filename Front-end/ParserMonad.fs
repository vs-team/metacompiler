module ParserMonad
#nowarn "40"

open Utilities

type Position = { Line : int }
  with member this.NextLine = { this with Line = this.Line + 1 }
       static member Zero = { Line = 1 }

type Error = Error of Position
  with 
    member this.Line = let (Error p) = this in p.Line
    static member Distinct (errors:List<Error>) = 
      errors |> Set.ofList |> Set.toSeq
      
(*
    A Parser consists of one function 'Parse', this function will return a tuple of a two lists.
    The first list contains a tuple of
    - A value of Type 'a. This value yields the result of applinyg 'Parse' to the buffer. This value will be referenced as the 'yielded value', if the parser 'yields a value' that value would be assigned to this value.
    - A list of characters. This list contains the remaining characters. The list can be seen as a buffer for uncomsumed input.
    - A context. The context is responsible for holding additional values needed during the parsing process. An example of this would be keywords with their appropriate names and values.
    - A Position. The position holds the current line index in the input buffer. The positions' line index is 'incremented' for every valid line break found during parsing.
    The second list is reponsible for keeping track of errors found during parsing. The list consists of the elements of type Position to track down the location of the error.

    A Parser is constructed by calling Parser.Make (f), here f must be a function of type List<char> -> 'ctxt -> Position -> List<'a * List<char> * 'ctxt * Position> * List<Error>.

    The Parser works by monadicly binding multiply Parsers to eachother. A simple example of a Parser trying to match a simple sequence of two characters 'i' and 'f' will look like this:

    p {
        let! c = character 'i'
        let! c' = character 'f'
        return ()
    }

    If character 'i' was satisfied then we will move on to the next expression ( It is also worth noting that the function character will consume one character from the input buffer ).
    If character 'i' was not satisfied ( or the input buffer was empty! ) then we would have not moved on to the next expression, the behaviour when a match can not be found is dependent on the function used.
    In this case, the function character will return the location of the error and return a Parser type containing this error.
    If character 'f' was satisfied then we will ParserBuilder.Return () this function will return a Parser with an empty yield and the remaining input buffer.

    It is possible to create a complex parser in this manner. 
*)


type Parser<'a, 'ctxt> = { Parse : List<char> -> 'ctxt -> Position -> List<'a * List<char> * 'ctxt * Position> * List<Error>}
  with
    static member Make(p:List<char> -> 'ctxt -> Position -> List<'a * List<char> * 'ctxt * Position> * List<Error>) : Parser<'a,'ctxt> = { Parse = p }
    static member (+) (p1:Parser<'a,'ctxt>, p2:Parser<'b,'ctxt>) : Parser<Either<'a,'b>,'ctxt> = 
     (fun buf ctxt p ->
        match p1.Parse buf ctxt p with
        | [],err1 ->
          match p2.Parse buf ctxt p with
          | [],err2 -> [],err1 @ err2
          | p2res,err2 ->
            [ for res,restBuf,ctxt',pos in p2res -> Second res, restBuf, ctxt', pos ], []
        | p1res,err1 ->
          [ for res,restBuf,ctxt',pos in p1res -> First res, restBuf, ctxt', pos ], []) |> Parser.Make
    static member (!!) (p:Parser<'a,'ctxt>) : Parser<'a,'ctxt> = 
      (fun buf ctxt pos -> 
        let all_res,err = p.Parse buf ctxt pos
        [
          for x,res_buf, ctxt', pos' in all_res do
          yield x, buf, ctxt', pos
        ], []) |> Parser.Make
and ParserBuilder() =
  member this.Bind(p:Parser<'a,'ctxt>, k:'a->Parser<'b,'ctxt>) : Parser<'b,'ctxt> =
   (fun buf ctxt pos ->
      let all_res,err = p.Parse buf ctxt pos
      let out =
        [
          for res,restBuf,ctxt',pos' in all_res do
          yield (k res).Parse restBuf ctxt' pos'
        ]
      out |> List.map fst |> List.concat, err @ (out |> List.map snd |> List.concat)) |> Parser.Make
  member this.Combine(p:Parser<'a,'ctxt>, k:Parser<'b,'ctxt>) : Parser<'b,'ctxt> =
   (fun buf ctxt pos ->
      let all_res,err = p.Parse buf ctxt pos
      let out =
        [
          for res,restBuf,ctxt',pos' in all_res do
          yield k.Parse restBuf ctxt' pos'
        ]
      out |> List.map fst |> List.concat, err @ (out |> List.map snd |> List.concat)) |> Parser.Make
  member this.Return(x:'a) : Parser<'a,'ctxt> =
    (fun buf ctxt pos -> [x,buf,ctxt,pos],[]) |> Parser.Make
  member this.ReturnFrom(x:Parser<'a,'ctxt>) : Parser<'a,'ctxt> = x
  member this.Yield(x:'a) : Parser<'a,'ctxt> = this.Return x
  member this.Zero() = this.Return()
  member this.Delay(f:unit -> Parser<'a,'ctxt>) = f()
  member this.Run(f:Parser<'a,'ctxt>) = f

let parser = ParserBuilder()
let p = parser  

(*
  Utility function to easily create a Parser type containing an error at it's current location.  
*)
let fail() = 
  (fun _ _ pos -> [],[Error pos]) |> Parser.Make 

(*
    The function character(c) will test if the next character in a Parser's input buffer matches the given character c.
    This function will return a Parser with an error if either:
    - The input buffer is empty
    - The given character c does not equal the first character in the Parser's input buffer
    
    p {
        let! c = character 't'
        return c
    }

    "test" -> c = 't'
    "foo3" -> error

    If the given character c matches the first character in the Parser's input buffer a Parser type will be returned containing the matched character c and the remaining buffer.
    Note that, if c is matched, this function will consume one character from the Parser's input buffer.
*)
let character(c:char) : Parser<char, 'ctxt> = 
 (fun buf ctxt (pos:Position) ->
  match buf with
  | x::cs when x = c -> 
    let pos' = 
      if x = '\n' then 
        pos.NextLine 
      else 
        pos
    [c, cs, ctxt, pos'],[]
  | _ -> [],[Error pos]) |> Parser.Make

(*
    The function word(s) will test if the given string s's characters match the next characters found in a Parser's input buffer.
    This function will return a Parser with an error if either:
    - The input buffer is empty
    - The next character in the given string s is not equal to the next character in the Parser's input buffer.
    
    p {
        let! w = word "foo"
        return w
    }

    "foo" -> w = 'foo'
    "bar" -> w = error

    If all characters in the string s are matched a Parser type will be returned yielding the matched string (note that this string will always be equal to the given string s).
    This function will consume s.Length characters from the Parser's input buffer if matched.
*)
let word (s:string) =
  let rec word (w:List<char>) : Parser<List<char>, 'ctxt> =
    p{
      match w with
      | x::xs ->
        let! c = character x
        let! cs = word xs
        return c::cs
      | [] -> 
        return []
    }
  word(s |> Seq.toList)

(*
    The function takeWhile(s:Parser) will consume characters from a Parser's input buffer as long as the given Parser s is matched.
    This function will return either nothing or something. Nothing will be returned if a match was never found. Something will be returned if atleast one match was found.
    An example of this would be to match all characters 's'

    p {
        let! c = takeWhile (character 's')
        return c
    }

    "ss33s" -> 'ss'
    "fooss" -> ''
    
    This function allows functions like word and character to be applied 'zero or more times'.
    The amount of input consumed by this function depends on the amount of input the given parser s's Parse function consumes.
*)

let rec takeWhile (s:Parser<'a,'ctxt>) : Parser<List<'a>, 'ctxt> =
  p{
    let something = 
      p{ let! c = s
         let! cs = takeWhile s
         return c::cs }
    let nothing = p { return [] }
    let! res = something + nothing
    match res with
    | First l -> return l
    | Second l -> return l
  }

(*
    The function character'(p:char->bool) will test if the next character in a Parser's input satisfies ( p(c) will return true ) the given function p.
    This function will return a Parser with an error if either:
    - The input buffer is empty
    - The next character in the Parser's input buffer does not satisfy the given function p.
    
    p {
        let! c = character' isAlpha
        return c
    }

    "hello3" -> 'h'
    "1world" -> error

    If the next character in the Parser's input buffer does satisfy the given function p a Parser type will be returned containing the matched character and the remaining buffer.
    Note that, if p is satisfied, this function will consume one character from the Parser's input buffer.
*)
let rec character' (p:char -> bool) : Parser<char, 'ctxt> =
 (fun buf ctxt (pos:Position) ->
    match buf with
    | c::cs when p c -> [c, cs, ctxt, if c = '\n' then pos.NextLine else pos],[]
    | buf -> [],[Error pos]) |> Parser.Make

(*
    The function takeWhile'(p:char->bool) will consume characters from a Parser's input buffer as long as the given function p is satisfied ( p(c) will return true ).
    This function will never return an error.

    This function will return all the characters from the Parser's input buffer as long as they satisfy the given function p, if p is not satisfied the process is stopped.

    An example of using this function would be to match all characters as long as they are digits, if a non-digit stop.

    p {
        let! c = takeWhile isDigit c
        return c
    }

    "123aade3" -> c = '123'
    "foo14443" -> c = ''
    
    This function allows functions like character' to be re-used 
    The amount of input consumed by this function is equal to the amount of times p was satisfied before the process was stopped.
*)
let rec takeWhile' (p:char -> bool) : Parser<List<char>,'ctxt> =
 (fun buf ctxt pos ->
    match buf with
    | c::cs when p c ->
      let all_res,err = (takeWhile' p).Parse cs ctxt pos
      [ for res,resBuf,ctxt',pos' in all_res -> c::res,resBuf,ctxt',pos' ],err
    | buf -> [[],buf,ctxt,pos],[]) |> Parser.Make

let isDigit c = c >= '0' && c <= '9'
let isAlpha c = (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')

let intLiteral() = 
  p{
    let! sign = character '-' + p{ return () }
    let! c = character' isDigit
    let! cs = takeWhile (character' isDigit)
    let s = new System.String(c::cs |> Seq.toArray)
    match sign with
    | First _ ->
      return -(s |> System.Int32.Parse)
    | _ ->
      return s |> System.Int32.Parse
  }

let floatLiteral() = 
  p{
    let! i = intLiteral()
    let! dot = character '.'
    let! c = character' isDigit
    let! cs = takeWhile (character' isDigit)
    let s = new System.String('0' :: '.' :: c::cs |> Seq.toArray)
    let f = s |> System.Double.Parse
    return float i + f
  }  

let identifier() =
  p{
    let! c = character' (fun c -> isAlpha c || c = '_')
    let! cs = takeWhile (character' (fun c -> isAlpha c || isDigit c || c = '-' || c = '_' || c = '\'' ))
    return new System.String(c::cs |> Seq.toArray)
  }

let rec longIdentifier() =
  p{
    let! id = identifier()
    let! dot = character '.' + p { return () }
    match dot with
    | First _ -> 
      let! rest = longIdentifier()
      return id + "." + rest
    | Second _ -> return id
  }

let stringLiteral() =
  p{
    let! q1 = character '\"'
    let! s = takeWhile' ((<>) '\"')
    let! q2 = character '\"'
    return s
  }

let tab() = 
  p{
    let! t = (character '\t') + (word "  ")
    return ()
  }

let eof() =
  (fun buf ctxt pos ->
     match buf with
     | [] -> [(),[],ctxt,pos],[]
     | _ -> [],[Error pos]) |> Parser.Make

let newline() = word "\r\n" + word "\n\r" + character '\n'

let blank_space() = takeWhile (character ' ')

let empty_line() = 
  p{
    let! bs = blank_space()
    let! nl = newline()
    return ()
  }

let rec empty_lines() = 
  p{
    let! el = empty_line() + p{ return () }
    match el with
    | First el ->
      let! els = empty_lines()
      return ()
    | Second _ ->
      return ()
  }

let getContext() =
  (fun buf ctxt pos -> [ctxt,buf,ctxt,pos],[]) |> Parser.Make

let setContext ctxt' =
  (fun buf ctxt pos -> [(),buf,ctxt',pos],[]) |> Parser.Make

let getPosition() =
  (fun buf ctxt pos -> [pos,buf,ctxt,pos],[]) |> Parser.Make

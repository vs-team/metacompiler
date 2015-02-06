module ParserMonad
#nowarn "40"

open Utilities

type Parser<'a, 'ctxt> = { Parse : List<char> -> 'ctxt -> List<'a * List<char> * 'ctxt> }
  with
    static member Make(p:List<char> -> 'ctxt -> List<'a * List<char> * 'ctxt>) : Parser<'a,'ctxt> = { Parse = p }
    static member (+) (p1:Parser<'a,'ctxt>, p2:Parser<'b,'ctxt>) : Parser<Either<'a,'b>,'ctxt> = 
     (fun buf ctxt ->
        match p1.Parse buf ctxt with
        | [] ->
          match p2.Parse buf ctxt with
          | [] -> []
          | p2res ->
            [ for res,restBuf,ctxt' in p2res -> Second res, restBuf, ctxt' ]
        | p1res ->
          [ for res,restBuf,ctxt' in p1res -> First res, restBuf, ctxt' ]) |> Parser.Make
    static member (!!) (p:Parser<'a,'ctxt>) : Parser<'a,'ctxt> = 
      (fun buf ctxt -> 
        [
          for x,res_buf, ctxt' in p.Parse buf ctxt do
          yield x, buf, ctxt'
        ]) |> Parser.Make
and ParserBuilder() =
  member this.Bind(p:Parser<'a,'ctxt>, k:'a->Parser<'b,'ctxt>) : Parser<'b,'ctxt> =
   (fun buf ctxt ->
      [
        let all_res = p.Parse buf ctxt
        for res,restBuf,ctxt' in all_res do
        yield! (k res).Parse restBuf ctxt'
      ]) |> Parser.Make
  member this.Combine(p:Parser<'a,'ctxt>, k:Parser<'b,'ctxt>) : Parser<'b,'ctxt> =
   (fun buf ctxt ->
      [
        let all_res = p.Parse buf ctxt
        for res,restBuf,ctxt' in all_res do
        yield! k.Parse restBuf ctxt'
      ]) |> Parser.Make
  member this.Return(x:'a) : Parser<'a,'ctxt> =
    (fun buf ctxt -> [x,buf,ctxt]) |> Parser.Make
  member this.ReturnFrom(x:Parser<'a,'ctxt>) : Parser<'a,'ctxt> = x
  member this.Yield(x:'a) : Parser<'a,'ctxt> = this.Return x
  member this.Zero() = this.Return()
  member this.Delay(f:unit -> Parser<'a,'ctxt>) = f()
  member this.Run(f:Parser<'a,'ctxt>) = f


let parser = ParserBuilder()
let p = parser  

let fail() = 
  (fun _ _ -> []) |> Parser.Make 

let character(c:char) : Parser<char, 'ctxt> = 
 (fun buf ctxt ->
  match buf with
  | x::cs when x = c -> [c, cs, ctxt]
  | _ -> []) |> Parser.Make

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

let rec character' (p:char -> bool) : Parser<char, 'ctxt> =
 (fun buf ctxt ->
    match buf with
    | c::cs when p c -> [c, cs, ctxt]
    | buf -> []) |> Parser.Make

let rec takeWhile' (p:char -> bool) : Parser<List<char>,'ctxt> =
 (fun buf ctxt ->
    match buf with
    | c::cs when p c ->
      [ for res,resBuf,ctxt' in (takeWhile' p).Parse cs ctxt -> c::res,resBuf,ctxt' ]
    | buf -> [[],buf,ctxt]) |> Parser.Make

let isDigit c = c >= '0' && c <= '9'
let isAlpha c = (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')

let intLiteral() = 
  p{
    let! c = character' isDigit
    let! cs = takeWhile (character' isDigit)
    let s = new System.String(c::cs |> Seq.toArray)
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

let longIdentifier() =
  p{
    let! c = character' (fun c -> isAlpha c || c = '_')
    let! cs = takeWhile (character' (fun c -> isAlpha c || isDigit c || c = '-' || c = '_' || c = '\'' || c = '.'))
    return new System.String(c::cs |> Seq.toArray)
  }

let stringLiteral() =
  p{
    let! q1 = character '\"'
    let! s = identifier()
    let! q2 = character '\"'
    return s
  }

let tab() = 
  p{
    let! t = (character '\t') + (word "  ")
    return ()
  }

let eof() =
  (fun buf ctxt ->
     match buf with
     | [] -> [(),[],ctxt]
     | _ -> []) |> Parser.Make
 
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
    let! el = empty_line()
    let! els = empty_lines() + p.Zero()
    return ()
  }

let getContext() =
  (fun buf ctxt -> [ctxt,buf,ctxt]) |> Parser.Make

let setContext ctxt' =
  (fun buf ctxt -> [(),buf,ctxt']) |> Parser.Make

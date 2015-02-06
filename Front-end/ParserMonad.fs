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
          yield x, buf, ctxt', pos'
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

let fail() = 
  (fun _ _ pos -> [],[Error pos]) |> Parser.Make 

let character(c:char) : Parser<char, 'ctxt> = 
 (fun buf ctxt (pos:Position) ->
  match buf with
  | x::cs when x = c -> [c, cs, ctxt, if c = '\n' then pos.NextLine else pos],[]
  | _ -> [],[Error pos]) |> Parser.Make

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
 (fun buf ctxt (pos:Position) ->
    match buf with
    | c::cs when p c -> [c, cs, ctxt, if c = '\n' then pos.NextLine else pos],[]
    | buf -> [],[Error pos]) |> Parser.Make

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
    let! el = empty_line()
    let! els = empty_lines() + p.Zero()
    return ()
  }

let getContext() =
  (fun buf ctxt pos -> [ctxt,buf,ctxt,pos],[]) |> Parser.Make

let setContext ctxt' =
  (fun buf ctxt pos -> [(),buf,ctxt',pos],[]) |> Parser.Make

let getPosition() =
  (fun buf ctxt pos -> [ctxt,buf,ctxt,pos],[]) |> Parser.Make

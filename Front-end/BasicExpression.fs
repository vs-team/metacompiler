module BasicExpression

type Bracket = Implicit | Square | Curly | Angle | SingleAngle | Regular
  with 
    static member FromChar = 
      function
      | '\000' -> Implicit
      | '(' -> Regular
      | '[' -> Square
      | '{' -> Curly
      | '≪' -> Angle
      | '<' -> SingleAngle
      | c -> failwithf "Unsupported bracket type %A" c


type BasicExpression<'k, 'e, 'i, 'di> =
  | Keyword of 'k
  | Application of Bracket * List<BasicExpression<'k, 'e, 'i, 'di>> * 'di
  | Imported of 'i * 'di
  | Extension of 'e
  with 
    override this.ToString() =
      match this with
      | Keyword k -> k.ToString()
      | Application(b,l,di) -> 
        let ls = l |> Seq.fold (fun s x -> s + " " + x.ToString()) ""
        match b with
        | Implicit -> sprintf "%s @ %s" ls (di.ToString())
        | Regular -> sprintf "(%s) @ %s" ls (di.ToString())
        | Square -> sprintf "[%s] @ %s" ls (di.ToString())
        | Curly -> sprintf "{%s} @ %s" ls (di.ToString())
        | Angle -> sprintf "<<%s>> @ %s" ls (di.ToString())
        | SingleAngle -> sprintf "<%s> @ %s" ls (di.ToString())
      | Imported(i,di) -> sprintf "%s @ %s" (i.ToString()) (di.ToString())
      | Extension e -> e.ToString()

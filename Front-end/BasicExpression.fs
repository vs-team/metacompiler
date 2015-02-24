module BasicExpression

(*
    Different Bracket types are to give context to an expression.
    If no an expression does not have a bracket then the type Implicit is used.
*)
type Bracket = Implicit | Square | Curly | Angle | Regular
  with 
    static member FromChar = 
      function
      | '\000' -> Implicit
      | '(' -> Regular
      | '[' -> Square
      | '{' -> Curly
      | '≪' -> Angle
      | c -> failwithf "Unsupported bracket type %A" c

(*
    A typical Expression can take form of a Keyword, an Application, an Import or an Extension.
    The Position of the Expression is stored in the 'di variable.
            
    An Application consists of list of expressions, these expressions in order contain:
        - An operation on the given expressions
        - The lefthand arguments
        - The righthand arguments

    Note that these arguments can be Applications themselves.

    A Keyword is a typical Keyword found in the ConcreteExpressionParser.

    An Extension functoins as an identifier.

    An Imported Expressions functionas as a Literal
*)
type BasicExpression<'k, 'e, 'i, 'di> =
  | Keyword of 'k * 'di
  | Application of Bracket * List<BasicExpression<'k, 'e, 'i, 'di>> * 'di
  | Imported of 'i * 'di
  | Extension of 'e * 'di
  with 
    member this.DebugInformation =
      match this with
      | Keyword(k,di) -> di
      | Application(b,l,di) -> di        
      | Imported(i,di) -> di
      | Extension(e,di) -> di
    override this.ToString() =
      match this with
      | Keyword(k,di) -> k.ToString()
      | Application(b,l,di) -> 
        let ls = l |> Seq.fold (fun s x -> s + " " + x.ToString()) ""
        match b with
        | Implicit -> sprintf "%s @ %s" ls (di.ToString())
        | Regular -> sprintf "(%s) @ %s" ls (di.ToString())
        | Square -> sprintf "[%s] @ %s" ls (di.ToString())
        | Curly -> sprintf "{%s} @ %s" ls (di.ToString())
        | Angle -> sprintf "<<%s>> @ %s" ls (di.ToString())
      | Imported(i,di) -> sprintf "%s @ %s" (i.ToString()) (di.ToString())
      | Extension(e,di) -> e.ToString()

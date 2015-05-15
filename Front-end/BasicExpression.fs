module BasicExpression

(*
    Different Bracket types are used to give context to an expression.
    If an expression does not have a bracket then the type Implicit is used.
*)
type Bracket = Implicit | Square | Curly | Angle | Regular | Generic
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

    An Extension functions as an identifier.
*)
type BasicExpression<'k, 'e, 'i, 'di, 'ti> =
  | Keyword of 'k * 'di * 'ti
  | Application of Bracket * List<BasicExpression<'k, 'e, 'i, 'di, 'ti>> * 'di * 'ti
  | Imported of 'i * 'di * 'ti
  | Extension of 'e * 'di * 'ti
  with 
    member this.TypeInformation =
      match this with
      | Keyword(k,di,ti) -> ti
      | Application(b,l,di,ti) -> ti
      | Imported(i,di,ti) -> ti
      | Extension(e,di,ti) -> ti
    member this.DebugInformation =
      match this with
      | Keyword(k,di,ti) -> di
      | Application(b,l,di,ti) -> di        
      | Imported(i,di,ti) -> di
      | Extension(e,di,ti) -> di
    override this.ToString() =
      match this with
      | Keyword(k,di,ti) -> sprintf "%s :: %s" (k.ToString()) (ti.ToString())
      | Application(b,l,di,ti) -> 
        let ls = l |> Seq.fold (fun s x -> s + " " + x.ToString()) ""
        match b with
        | Generic -> sprintf "<%s>" ls
        | Implicit -> sprintf "%s @ %s :: %s" ls (di.ToString()) (ti.ToString())
        | Regular -> sprintf "(%s) @ %s :: %s" ls (di.ToString()) (ti.ToString())
        | Square -> sprintf "[%s] @ %s :: %s" ls (di.ToString()) (ti.ToString())
        | Curly -> sprintf "{%s} @ %s :: %s" ls (di.ToString()) (ti.ToString())
        | Angle -> sprintf "<<%s>> @ %s :: %s" ls (di.ToString()) (ti.ToString())
      | Imported(i,di,ti) -> sprintf "%s @ %s :: %s" (i.ToString()) (di.ToString()) (ti.ToString())
      | Extension(e,di,ti) -> e.ToString() 
    member this.ToStringCompact =
      match this with
      | Keyword(k,di,ti) -> sprintf "%s" (k.ToString())
      | Application(b,l,di,ti) -> 
        let ls = l |> Seq.fold (fun s x -> s + x.ToStringCompact) ""
        match b with
        | Generic -> sprintf "<%s>" ls
        | Implicit -> sprintf "%s" ls
        | Regular -> sprintf "(%s)" ls
        | Square -> sprintf "[%s]" ls
        | Curly -> sprintf "{%s}" ls
        | Angle -> sprintf "<<%s>>" ls
      | Imported(i,di,ti) -> sprintf "%s" (i.ToString())
      | Extension(e,di,ti) -> e.ToString() 

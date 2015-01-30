module BasicExpression

type Bracket = Square | Curly | Angle | Regular
  with 
    static member FromChar = 
      function
      | '(' -> Regular
      | '[' -> Square
      | '{' -> Curly
      | '≪' -> Angle
      | c -> failwithf "Unsupported bracket type %A" c

type BasicExpression<'k, 'e, 'i> =
  | Keyword of 'k
  | Application of Bracket * List<BasicExpression<'k, 'e, 'i>>
  | Imported of 'i
  | Extension of 'e
  with 
    override this.ToString() =
      match this with
      | Keyword k -> k.ToString()
      | Application(b,l) -> 
        let ls = l |> Seq.fold (fun s x -> s + " " + x.ToString()) ""
        match b with
        | Regular -> "(" + ls + ")"
        | Square -> "[" + ls + "]"
        | Curly -> "{" + ls + "}"
        | Angle -> "<<" + ls + ">>"
      | Imported i -> i.ToString()
      | Extension e -> e.ToString()

module Utilities

let debug_log (x:'a) = 
  do printfn "%A" x
  x

type Either<'a,'b> = First of 'a | Second of 'b
  with 
    member e.Fold f g =
            match e with
            | First x -> f x
            | Second y -> g y


let matching_brackets open_bracket closed_bracket = 
  match open_bracket, closed_bracket with
  | '(', ')' -> true
  | '[', ']' -> true
  | '{', '}' -> true
  | '≪', '≫' -> true
  | '<', '>' -> true
  | _ -> false

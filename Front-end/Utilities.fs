module Utilities

let debug_log (x:'a) = 
  do printfn "%A" x
  x
(*
    The Either type is a simple data structure used to hold two types 'a and 'b.
*)
type Either<'a,'b> = First of 'a | Second of 'b
  with 
    (*
        The function Fold is a helper function to apply functions to Either's First and Second member.
        If this Either has a First element then apply to given function fo First, else Apply the given function g to Second.
    *)
    member e.Fold f g =
            match e with
            | First x -> f x
            | Second y -> g y



(*
    The matching_brackets function is a helper function to test for bracket equality between two open and closing brackets.
*)
let matching_brackets open_bracket closed_bracket = 
  match open_bracket, closed_bracket with
  | '(', ')' -> true
  | '[', ']' -> true
  | '{', '}' -> true
  | '≪', '≫' -> true
  | '<', '>' -> true
  | _ -> false


let transitiveClosure (l:List<'a * 'a>) =
  let mutable m = Map.empty 
  for i, o in l do 
    match m |> Map.tryFind i with
    | Some os -> m <- m.Add(i, os |> Set.add o)
    | None -> m <- m.Add(i, Set.singleton o)
  let mutable changed = true
  while changed do
    changed <- false
    for x in m do
      let i, os = x.Key, x.Value
      for o in os do
        match m |> Map.tryFind o with
        | Some os' ->
          let m_i = m.[i]
          let m_i' = m_i + os'
          if m_i.Count <> m_i'.Count then
            m <- m.Add(i,m_i')
            changed <- true
        | None -> 
          ()
      ()      
  m

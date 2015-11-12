module StateMonad

open ErrorMonad

type State<'s,'a> = 's -> Result<'a * 's>

let ret x = fun s -> ret(x,s)
let (>>=) p k =
  fun s -> 
    p s >>= (fun (x,s') -> k x s')

type StateBuilder() =
  member this.Return(x) = ret x
  member this.ReturnFrom(p) = p
  member this.Bind(p,k) = p >>= k
let st = StateBuilder()

let fail msg = fun s -> Error msg

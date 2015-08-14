module StateMonad

type State<'a, 's> = 's -> 'a*'s
    
type StateBuilder() =
     member this.Bind (p: State<'a,'s>, k : 'a -> State<'b,'s>) : State<'b, 's> =  
          (fun s -> 
            let a', s0 = p s
            k a' s0)
     member this.Return (x : 'a) = fun s -> x,s
     member this.ReturnFrom s = s 
     member this.Zero () = ()

let st = StateBuilder()

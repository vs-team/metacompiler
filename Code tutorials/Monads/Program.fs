type Option<'a> = 
  | Just of 'a 
  | None

let (>>=) (o:Option<'a>) (f:'a->Option<'b>) : Option<'b> =
  match o with
  | Just x -> f x
  | None -> None

let ret x = Just x

type OptionBuilder() =
  member this.Return(x) = ret x
  member this.ReturnFrom(o:Option<'a>):Option<'a> = o
  member this.Bind(o, f) = o >>= f
  member this.Zero() = None
let opt = OptionBuilder()

let (!) (f:'a -> 'b) =
  fun (i:Option<'a>) ->
    opt{
      let! i_v = i
      return f i_v
    }

let (!!) (op:'a -> 'b -> 'c) =
  fun (i:Option<'a>) (j:Option<'b>) ->
    opt{
      let! i_v = i
      let! j_v = j
      return op i_v j_v
    }
  
let i = Just 100
let j = Just -150
let (<+>) = !!(+)
let sum_lift =
  opt{
    return! i <+> j
  }

let sum =
  opt{
    let! i_v = i
    let! j_v = j
    if i_v + j_v > 0 then
      return i_v + j_v
  }

let sum_ugly = 
  opt.Bind(i, fun i_v ->
  opt.Bind(j, fun j_v -> 
  if i_v + j_v > 0 then opt.Return(i_v + j_v)
  else opt.Zero()))

[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    0 // return an integer exit code

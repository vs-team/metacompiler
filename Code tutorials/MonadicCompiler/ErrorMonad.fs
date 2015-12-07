module ErrorMonad

type Result<'a> = Result of 'a | Error of List<string>

let ret x = Result x
let fail m = Error m

let tryCatch (k:'a->Result<'b>) (err:List<string>->Result<'b>) (p:Result<'a>) : Result<'b> =
  match p with
  | Result x -> k x
  | Error e -> err e
  
let (>>=) p k = tryCatch k fail p

type ErrorBuilder() =
  member this.Return x = ret x
  member this.Bind(p,k) = p >>= k
let err = ErrorBuilder()

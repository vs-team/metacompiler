module ExceptionMonad

type Exception<'a> = Result of 'a
                   | Exception of string

let ret x = Result x

let (>>=) m k = 
  match m with
  | Exception s -> Exception s
  | Result res -> k res

type ExceptionBuilder() =
  member this.Return(x) = ret x
  member this.ReturnFrom(x) = x
  member this.Bind(m,k) = m >>= k

let exc = ExceptionBuilder()

let fail s :Exception<_> =
  Exception s

let UseOptionMonad op err=
  match op with
  | None -> Exception err
  | Some(x) -> Result x

let rec fold (ls:List<'a>) (f :Exception<'state> -> 'a -> Exception<'state>) 
  (state:Exception<'state>): Exception<'state> = 
  match state with
  | Result(_) ->
    match ls with
    | x::xs -> 
      let res = (f state x)
      fold xs f res
    | [] -> state
  | Exception s -> Exception s
    
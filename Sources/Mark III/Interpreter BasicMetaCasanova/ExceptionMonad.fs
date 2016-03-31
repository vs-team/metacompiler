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

let rec fold (f :Exception<'state> -> 'a -> Exception<'state>) (state:Exception<'state>)
  (ls:List<'a>): Exception<'state> = 
  match state with
  | Result(_) ->
    match ls with
    | x::xs -> 
      let res = (f state x)
      fold f res xs
    | [] -> state
  | Exception s -> Exception s

let rec map (f:'a -> Exception<'b>) (ls:List<'a>):Exception<List<'b>> =
  match ls with
  | x::xs -> 
    match f x with
    | Result res -> 
      match map f xs with
      | Result resxs -> Result (res::resxs)
      | Exception s -> Exception s
    | Exception s -> Exception s
  | [] -> Result []

let rec list_of_exeption_to_list (ls:List<Exception<'a>>)
  :Exception<List<'a>> =
  match ls with
  | x::xs ->
    match x with
    | Result(res) ->
      match list_of_exeption_to_list xs with
      | Result(resxs) -> Result(res::resxs)
      | Exception s -> Exception s 
    | Exception s -> Exception s 
  | [] -> Result []

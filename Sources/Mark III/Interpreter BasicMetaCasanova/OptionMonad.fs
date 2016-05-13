module OptionMonad

open ParserMonad

type OptionBuilder() =
  member this.Return (res:'a) : Option<'a> = Some(res)
  member this.Bind (p:Option<'a>,k:'a->Option<'b>):Option<'b> = Option.bind k p
  member this.ReturnFrom p = p
  
let opt = OptionBuilder()

let option_to_bool (op:Option<'a>) :bool =
  match op with
  | Some(_) -> true
  | None    -> false

let unpack_option (op:Option<'a>): 'a =
  match op with
  | Some a -> a
  | None   -> failwith "one of the options failed to unpack."

let try_unpack_list_of_option (oplist:List<Option<'a>>) :Option<List<'a>> =
  opt{
    if List.forall option_to_bool oplist then
      return List.collect (fun x -> [unpack_option x]) oplist
    else return! None
  }

let use_parser_monad (pars:Parser<'char,'ctxt,'res>)(input:List<'char>*'ctxt) :Option<'res> =
  let convert = 
    match pars (input) with
    | Done(res,_,_) -> Some(res)
    | Error (e,i) ->
      printfn "%A" (e,i)
      None 
  opt{return! convert}

let use_exception_monad (excep:ExceptionMonad.Exception<'a>) :Option<'a> =
  opt{
    match excep with
    | ExceptionMonad.Result(res) -> return res
    | ExceptionMonad.Exception s -> 
      printfn "%s" s
      return! None 
  }

let option_to_list (op:Option<'a>) :List<'a> = if op.IsSome then [op.Value] else []

let react_to_parser_error 
    (pars:Parser<'char,'ctxt,'res>)(input:List<'char>*'ctxt) 
    (err:ErrorType)(f:Option<'res>) :Option<'res> = 
  let convert = 
    match pars (input) with
    | Done(res,_,_) -> Some(res)
    | Error (err,_) -> f
    | Error (e,i) ->
      printfn "%A" (e,i)
      None 
  opt{return! convert}

let if_none (op1:Option<'a>) (st:string) :Option<'a> =
  match op1 with
  | Some(x) -> Some (x)
  | None -> 
    printfn "%s" st
    None
module Pipeline2

open Common
open ParserMonad
open Lexer2

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
    | Error e ->
      printfn "%A" e
      None 
  opt{return! convert}
//-----------------------------------------------

let t = System.Diagnostics.Stopwatch()

let find_correct_path (paths:List<string>)(name:string) :Option<string> =
  let condition = (fun x -> System.IO.File.Exists(x + name + ".mc"))
  opt{
    let! correct_path = List.tryFind condition paths
    return sprintf "%s%s.mc" correct_path name
  }

let read_file (path:string) :Option<List<char>> = 
  opt{ 
    if System.IO.File.Exists(path) then 
      return System.IO.File.ReadAllText(path) |> Seq.toList
    else return! None
  }

let timer (str:string) (op:Option<'a>) :Option<'a> =
  opt{
    do t.Restart()
    let! res = op
    do t.Stop()
    do printfn "%s in %d ms." str t.ElapsedMilliseconds
    return res
  }

let start_lexer (paths:List<string>) (file_name:string) :Option<Id*List<Token>> =
  opt{
    let! correct_path = find_correct_path paths file_name
    let  full_path = correct_path
    let! chars = read_file correct_path
    let! tokens = use_parser_monad (tokenize2 correct_path) (chars,Position.Zero)
    return (file_name,tokens)
  } |> (timer (sprintf "Done tokenization of file: [%s.mc] " file_name))

let start_lexpars (paths:List<string>) (file_name:List<string>) :Option<_> =   
  opt{
    let list_of_lexer_results = 
      List.collect (fun x -> [start_lexer paths x]) file_name
    let! lexer_res = try_unpack_list_of_option list_of_lexer_results
    return lexer_res
  }

let start (paths:List<string>) (file_name:List<string>) :Option<_> =
  opt{
    t.Start()
    let! lexpars_res = start_lexpars paths file_name
    return lexpars_res
  }
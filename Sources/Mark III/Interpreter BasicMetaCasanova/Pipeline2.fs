module Pipeline2

open Common
open ParserMonad
open Lexer2
open Parser2
open OptionMonad

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

let lex_files (paths:List<string>) (file_name:List<string>) :Option<_> =   
  opt{
    let list_of_lexer_results = 
      List.collect (fun x -> [start_lexer paths x]) file_name
    let! lexer_res = try_unpack_list_of_option list_of_lexer_results
    return lexer_res
  }

let start_parser (tok:List<Id*Token>) :Option<_> =
  opt{
    return ()
  }

let start (paths:List<string>) (file_name:List<string>) :Option<_> =
  opt{
    t.Start()
    let! lexpars_res = lex_files paths file_name
    return lexpars_res
  }
module Pipeline2

open CodegenInterface
open Common
open ParserMonad
open Lexer2
open ParserTypes
open DeclParser2
open OptionMonad
open TypeChecker
open ParserAST
open Parser
//open GlobalSyntaxCheck2
//open RuleParser2
//open RuleNormalizer2
//open DataNormalizer2
//open RuleTypeChecker2
//open InterfaceBuilder2

let t = System.Diagnostics.Stopwatch()

let find_correct_path (paths:List<string>)(name:string) :Option<string> =
  let condition = (fun x -> System.IO.File.Exists(x + name + ".mc"))
  opt{
    let correct_path = List.tryFind condition paths
    let! found_path =  
      if_none correct_path (sprintf "could not find file [%s] in given dir's " name)
    return sprintf "%s%s.mc" found_path name
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
    do System.Console.ForegroundColor <- System.ConsoleColor.Green
    do printf "Done "
    do System.Console.ForegroundColor <- System.ConsoleColor.Magenta
    do printf "in %d ms: "  t.ElapsedMilliseconds
    do System.Console.ResetColor()
    do printfn "%s " str
    return res
  }

let start_lexer (paths:List<string>) (file_name:string) :Option<string*List<Token>> =
  opt{
    let! correct_path = find_correct_path paths file_name
    let  full_path = correct_path
    let! chars = read_file correct_path
    let! tokens = use_parser_monad (tokenize2 correct_path) (chars,Position.Zero)
    return (file_name,tokens)
  } |> (timer (sprintf "tokenization of file: [%s.mc] " file_name))

let lex_files (paths:List<string>) (file_name:List<string>) :Option<List<string*List<Token>>> =   
  opt{
    let list_of_lexer_results = 
      List.collect (fun x -> [start_lexer paths x]) file_name
    return! try_unpack_list_of_option list_of_lexer_results
  }

let start_parser ((st,tok):(string*List<Token>)) :Option<string*Program> =
  opt{
    let! res = use_parser_monad parse_tokens (tok,([st],([],([],[]))))
    return st,res
  } |> (timer (sprintf "parsing of file: [%s.mc] " st))

let parse_tokens (tokens:List<string*List<Token>>) :Option<List<string*Program>> =
  opt{
    let ls = List.map (fun x -> start_parser x) tokens
    let! res = try_unpack_list_of_option ls
    return res
  }

let start_codegen (ctxt:fromTypecheckerWithLove) :Option<_> =
  Codegen.failsafe_codegen ctxt |> (timer (sprintf "Code gen "))

let start (paths:List<string>) (file_name:List<string>) :Option<_> =
  opt{
    t.Start()
    let! lex_res = lex_files paths file_name
    let! pars_res = parse_tokens lex_res

    let! code_res = start_codegen balltest.ball_func

    let! st,prog = List.tryHead pars_res
    let symbolTable = buildSymbols (fst (snd prog)) Map.empty
    //let symbolTable = buildSymbols (fst (snd tcTest)) Map.empty
    do System.IO.File.WriteAllText ("typeCheckerOutput.txt", sprintf "%A" symbolTable)
    do System.IO.File.WriteAllText ("out.cs",(sprintf "%s" code_res))

    return pars_res
  }
module Pipeline2

open CodegenInterface
open Common
open ParserMonad
open Lexer2
open ParserTypes
open DeclParser2
open OptionMonad
open GlobalSyntaxCheck2
open RuleParser2
open RuleNormalizer2
open DataNormalizer2
open RuleTypeChecker2
open InterfaceBuilder2
open ParserTypes

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

let start_parser ((id,tokens):string*List<Token>) :Option<string*ParseScope> =
  opt{
    let name = {Namespace = [id]; Name = id}
    let! res = use_parser_monad (parse_full_lines) (tokens,{ParseScope.Zero with Name = name})
    return (id,res)
  } |> (timer (sprintf "parseing of file: [%s.mc] " id))

let start_normalizer ((id,scp):string*ParseScope) :Option<string*List<NormalizedRule>*_> =
  opt{
    let! _,norrule = use_parser_monad (normalize_rules) ([id,scp],"")
    let! _,nordata = use_parser_monad (normalize_datas) ([id,scp],"")
    return (id,norrule,nordata)
  } |> (timer (sprintf "normalizing of file: [%s.mc] " id))

let start_typechecker ((id,norrule,parscp):string*List<NormalizedRule>*ParseScope) :Option<string*List<Id*rule>> =
  opt{
    let typctxt = {RuleCheckerCtxt.Zero with RuleDecl = parscp.FuncDecl; DataDecl = parscp.DataDecl}
    let! typedrules = use_exception_monad (type_check_rules norrule typctxt) 
    return (id,typedrules)
  } |> (timer (sprintf "rule type checking of file: [%s.mc] " id))


let start_codegen (ctxt:fromTypecheckerWithLove) :Option<_> =
  Codegen.failsafe_codegen ctxt |> (timer (sprintf "Code gen "))

let start (paths:List<string>) (file_name:List<string>) :Option<_> =
  opt{
    t.Start()
    let! lex_res = start_lexer paths (List.head file_name)
    let! st,pars_res = start_parser lex_res
    let! _,norrule,nordata = start_normalizer (st,pars_res)
    let! type_res = start_typechecker (st,norrule,pars_res)

    //let interp = Interpreter.eval_main balltest.ball_func
    let! code_res = start_codegen balltest.ball_func
    do System.IO.File.WriteAllText ("out.cs",(sprintf "%s" code_res))

    return lex_res
  }
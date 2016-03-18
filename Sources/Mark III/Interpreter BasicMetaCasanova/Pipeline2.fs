module Pipeline2

open CodegenInterface
open Common
open ParserMonad
open Lexer2
open DeclParser2
open OptionMonad
open GlobalSyntaxCheck2
open RuleParser2
open RuleNormalizer2

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

let start_lexer (paths:List<string>) (file_name:string) :Option<Id*List<Token>> =
  opt{
    let! correct_path = find_correct_path paths file_name
    let  full_path = correct_path
    let! chars = read_file correct_path
    let! tokens = use_parser_monad (tokenize2 correct_path) (chars,Position.Zero)
    return (file_name,tokens)
  } |> (timer (sprintf "tokenization of file: [%s.mc] " file_name))

let lex_files (paths:List<string>) (file_name:List<string>) :Option<List<Id*List<Token>>> =   
  opt{
    let list_of_lexer_results = 
      List.collect (fun x -> [start_lexer paths x]) file_name
    return! try_unpack_list_of_option list_of_lexer_results
  }

let start_global_syntax_check (tokens:Id*List<Token>) :Option<_> =
  opt{
    let id,tok = tokens
    let! res = use_parser_monad check_syntax (tok,Position.Zero)
    return () 
  } |> (timer (sprintf "checking tokens of file: [%s.mc] " ((fun (id,_)->id)tokens)))

let check_tokens (tokens:List<Id*List<Token>>) : Option<_> =
  opt{
    let list_of_checks =
       List.collect (fun x -> [start_global_syntax_check x]) tokens
    let! res = try_unpack_list_of_option list_of_checks
    return ()
  }

let start_decl_parser (tokens:Id*List<Token>) :Option<Id*DeclParseScope*List<Token>> =
  opt{
    let id,tok = tokens
    let! res = use_parser_monad parse_scope (tok,{DeclParseScope.Zero with CurrentNamespace = [id]})
    return id,res,tok
  } |> (timer (sprintf "parsing decls of file: [%s.mc] " ((fun (id,_)->id)tokens)))

let parse_tokens (tokens:List<Id*List<Token>>) : Option<List<Id*DeclParseScope*List<Token>>> =
  opt{
    let list_of_decl_res = 
      List.collect (fun x -> [start_decl_parser x]) tokens
    return! try_unpack_list_of_option list_of_decl_res
  }

let start_rule_parser (ctxt:List<Id*DeclParseScope*List<Token>>) 
  :Option<List<Id*List<RuleDef>*List<SymbolDeclaration>>> =
  opt{
    return! use_parser_monad (itterate (parse_rule_scope)) (ctxt,[])
  } |> (timer (sprintf "parsing Rules "))

let start_rule_normalizer (ctxt:List<Id*List<RuleDef>*List<SymbolDeclaration>>)
  :Option<List<Id*List<NormalizedRule>>> = 
  opt{
    let ctxt' = List.collect (fun (id,rule,_) -> [id,rule]) ctxt
    let! res = use_parser_monad (itterate (normalize_rules)) (ctxt',"")
    return res
  }

let start_codegen (ctxt:fromTypecheckerWithLove) :Option<_> =
  Codegen.failsafe_codegen ctxt |> (timer (sprintf "Code gen "))

let start (paths:List<string>) (file_name:List<string>) :Option<_> =
  opt{
    t.Start()
    let! lex_res = lex_files paths file_name
    do! check_tokens lex_res
    let! decl_pars_res = parse_tokens lex_res
    let! rule_pars_res = start_rule_parser decl_pars_res
    let! normalized_rule_res = start_rule_normalizer rule_pars_res
    let! code_res = start_codegen CodegenTest.list_test
    do System.IO.File.WriteAllText ("out.cs",(sprintf "%s" code_res)) 
    return normalized_rule_res
    //return (List.collect (fun (x,y,z) -> [x,y]) decl_pars_res)
    //return lex_res
  }
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

let start_global_syntax_check (tokens:string*List<Token>) :Option<_> =
  opt{
    let id,tok = tokens
    let! res = use_parser_monad check_syntax (tok,Position.Zero)
    return () 
  } |> (timer (sprintf "checking tokens of file: [%s.mc] " ((fun (id,_)->id)tokens)))

let start_parser (tokens:string*List<Token>) :Option<string*ParserScope> =
  opt{
    let id,tok = tokens
    let scp = {ParserScope.Zero with DeclsScp = {DeclParseScope.Zero with Name = {Namespace = [id]; Name = id}}}
    let! res = use_parser_monad parse_full_lines (tok,scp)
    return id,res
  } |> (timer (sprintf "parsing file: [%s.mc] " ((fun (id,_)->id)tokens)))

let parse_syntax (tokens:List<string*List<Token>>) :Option<List<string*ParserScope>> =
  opt{
    let res = List.map (fun (id,ls) -> start_parser (id,ls)) tokens
    let! res = try_unpack_list_of_option res
    return res
  }

let start_rule_normalizer (ctxt:List<string*ParserScope>)
  :Option<List<string*List<NormalizedRule>>> = 
  opt{
    let newctxt = List.map (fun (id,(pc:ParserScope)) -> (id,pc.RuleScp)) ctxt
    let! res = use_parser_monad (itterate (normalize_rules)) (newctxt,"")
    return res
  } //|> (timer (sprintf "Normalizing Rules "))

let start_data_normalizer (ctxt:List<string*DeclParseScope>)
  :Option<List<string*List<Id*CodegenInterface.data>>> = 
  opt{
    let! res = use_parser_monad (itterate (normalize_datas)) (ctxt,"")
    return res
  } |> (timer (sprintf "Normalizing Datas "))

let use_rule_typechecker 
  (set:List<NormalizedRule>*List<SymbolDeclaration>*List<SymbolDeclaration>)
  :Option<List<Id*CodegenInterface.rule>> =
  let (rule,ruledecl,datadecl) = set
  let ctxt = {RuleCheckerCtxt.Zero with RuleDecl = ruledecl; DataDecl = datadecl}
  use_exception_monad (type_check_rules rule ctxt)
    
let start_rule_typechecker (rule:(List<string*List<NormalizedRule>>)) 
  (decls:(List<string*ParserScope>))
  :Option<List<string*List<Id*CodegenInterface.rule>>> =
  opt{
    let datadecl = Map.ofList decls
    let res = rule |> List.map (fun (id,rule) ->
      opt{
        let! pc = datadecl.TryFind(id)
        let! res = use_rule_typechecker (rule,pc.DeclsScp.FuncDecl,pc.DeclsScp.DataDecl)
        return id,res
      }
      ) 
    let! res = try_unpack_list_of_option res
    return res
  } |> (timer (sprintf "type checking rules"))

let start_codegen (ctxt:fromTypecheckerWithLove) :Option<_> =
  Codegen.failsafe_codegen ctxt |> (timer (sprintf "Code gen "))

let start (paths:List<string>) (file_name:List<string>) :Option<_> =
  opt{
    t.Start()
    let! lex_res = lex_files paths file_name
    let! parser_res = parse_syntax lex_res
    let! normalized_rule_res = start_rule_normalizer parser_res
    let! normalized_data_res = 
      start_data_normalizer (List.map (fun (id,pc) -> id,pc.DeclsScp) parser_res)
    
    let! typed_rule_res = start_rule_typechecker normalized_rule_res parser_res
    let! interface_res = build_interface typed_rule_res normalized_data_res
    
    let dump = Interpreter.eval_main interface_res

    let! code_res = start_codegen balltest.ball_func
    do System.IO.File.WriteAllText ("out.cs",(sprintf "%s" code_res))
    //return typed_rule_res
    //return normalized_data_res
    //return List.collect(fun (x,y,_) -> [(x,y)]) normalized_rule_res
    //return List.collect(fun (_,y,_) -> [(y)]) rule_pars_res
    //return (List.collect (fun (x,y,z) -> [x,y]) decl_pars_res)
    return parser_res
  }
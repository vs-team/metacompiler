module PipeLine

open Lexer
open Parser
open LineSplitter
open ScopeBuilder
open Prioritizer
open TypeChecker
open System
open System.IO
open ParserMonad
open Common

let t = System.Diagnostics.Stopwatch()

type Scope = 
  {
    File_paths   : string list
    Input_files  : string list
    Parsed_files : string list
    Tokens  : Lexer.Token list option
    Parsed  : List<Parser.BasicExpression> option
    Lines   : List<LineSplitter.Line> option
    Scopes  : List<string*ScopeBuilder.Scope>
    TypedScopes : List<string*Prioritizer.TypedScope>
  }
  with 
    static member Zero =
      {
        File_paths  = []
        Input_files = []
        Parsed_files = []
        Tokens      = None
        Parsed      = None
        Lines       = None
        Scopes      = []     
        TypedScopes = []
      }

let rec find_correct_path (paths:List<string>)(name:string) :Option<string> =
  match paths with
  | x::xs ->
    if System.IO.File.Exists(x + name + ".mc") then 
      Some(x + name + ".mc")
    else 
      find_correct_path xs name
  | [] -> None

let update_paths : Parser<string,Scope,unit> =
  fun (paths,ctxt) ->
    let a,b = ctxt.Scopes.Head
    let import_list = b.ImportDeclaration @ paths
    let filterd_list = List.filter (fun s1 -> not (List.exists (fun s2 -> s1 = s2 ) ctxt.Parsed_files)) import_list
    Done((),filterd_list,ctxt)


let start_lexer : Parser<string,Scope,Token list option> = 
  fun (paths,ctxt) ->
  match paths with
  | x::xs -> 
    match (find_correct_path ctxt.File_paths x) with 
    | Some path ->
      t.Restart()
      let tokens = tokenize path ".lex_cache" ()
      match tokens with
      | Some tok ->
        do t.Stop()
        do printfn "%s" x
        do printfn "Done tokenization in %d ms." t.ElapsedMilliseconds
        do t.Restart()
        Done(tokens,paths,ctxt)
      | _ ->
        printfn "No tokens returned. Tokenizer failed."
        failwith ""
        Error PipeLineError
    | None -> Error PipeLineError 
  | [] -> Error PipeLineError

let start_parser : Parser<string,Scope,Parser.BasicExpression list option> = 
  fun (paths,ctxt) ->
  match paths with
  | x::xs -> 
    match (find_correct_path ctxt.File_paths x) with 
    | Some path ->
      match ctxt.Tokens with 
      | Some tok ->
        match parse path ".paren_cache" tok with
        | Some(parsing) ->
          do printfn "Done parsing in %d ms." t.ElapsedMilliseconds
          do t.Restart()
          Done(Some(parsing),paths,ctxt)
        | _ -> Error PipeLineError
      | _ -> Error PipeLineError
    | _ -> Error PipeLineError 
  | _ -> Error PipeLineError

let start_line_splitter : Parser<string,Scope,LineSplitter.Line list option> = 
  fun (paths,ctxt) ->
  match paths with
  | x::xs -> 
    match (find_correct_path ctxt.File_paths x) with 
    | Some path ->
      match ctxt.Parsed with 
      | Some pars ->  
        match split_lines path ".split_cache" pars with
        | Some line_blocks ->
            do printfn "Done line splitting in %d ms." t.ElapsedMilliseconds
            do t.Restart()
            Done (Some(line_blocks),paths,ctxt)
        | _ -> Error PipeLineError
      | _ -> Error PipeLineError
    | _ -> Error PipeLineError
  | _ -> Error PipeLineError

let start_scope_builder : Parser<string,Scope,List<string*ScopeBuilder.Scope>> = 
  fun (paths,ctxt) ->
    match ctxt.Lines with 
      | Some line_blocks ->
        let start_scope = {ScopeBuilder.Scope.Zero with CurrentNamespace = paths.Head}
        match build_scopes line_blocks start_scope with
          | Some scope ->
            do printfn "Done scope building in %d ms." t.ElapsedMilliseconds
            Done(((paths.Head,scope)::scope.Modules),paths,ctxt)
          | _ -> Error PipeLineError
      | _ -> Error PipeLineError

let start_typecheck : Parser<string,Scope,List<string*Prioritizer.TypedScope>> =
  fun(paths,ctxt) ->
    match ctxt.Scopes with 
    | scopes ->
      match (decls_check()) (scopes,[]) with
      | Done(res,b,c) -> Done(res,paths,ctxt)
      | Error (p) -> Error (p) 

let lift_parser p mk_new_ctxt : Parser<string,Scope,Unit> =
  prs{
    let! p_res = p
    let! ctxt = getContext
    do! setContext (ctxt |> mk_new_ctxt p_res)
  }

let lexer_p : Parser<string,Scope,Unit> = 
  lift_parser start_lexer
    (fun tokens ctxt -> { ctxt with Tokens = tokens})

let parser_p : Parser<string,Scope,Unit> = 
  lift_parser start_parser
    (fun parsed ctxt -> { ctxt with Parsed = parsed})

let line_splitter_p : Parser<string,Scope,Unit> = 
  lift_parser start_line_splitter
    (fun lines ctxt -> { ctxt with Lines = lines})

let scope_builder_p : Parser<string,Scope,Unit> = 
  lift_parser start_scope_builder 
    (fun scope ctxt -> { ctxt with Scopes = scope @ ctxt.Scopes})

let Type_checker_p : Parser<string,Scope,Unit> = 
  lift_parser start_typecheck
    (fun typecheck ctxt -> {ctxt with TypedScopes = typecheck})

let add_to_parsed_list : Parser<string,Scope,Unit> = 
  prs{
    let! ctxt = getContext
    let! paths = getBuffer
    do! setContext { ctxt with Parsed_files = paths.Head :: ctxt.Parsed_files}
  }

let front_end : Parser<string,Scope,_> = 
  prs{
    do! lexer_p
    do! parser_p
    do! line_splitter_p
    do! scope_builder_p
    do! add_to_parsed_list
    do! update_paths
  }

let compiler : Parser<string,Scope,List<string*Prioritizer.TypedScope>> = 
  t.Start()
  prs{
    let! u = front_end |> repeat
    do! Type_checker_p
    let! scope = getContext
    return scope.TypedScopes
  }

let start_compiler (input) (file_paths) =
  let scp = {Scope.Zero with File_paths = file_paths}
  match compiler (input,scp) with
  | Done(res,_,_) -> res
  | Error (p) -> failwith ""
  
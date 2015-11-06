module PipeLine

open Lexer
open Parser
open LineSplitter
open ScopeBuilder
open TypeChecker
open System
open System.IO
open ParserMonad
open Common

let t = System.Diagnostics.Stopwatch()

type Scope = 
  {
    Input_files : string list
    Tokens  : Lexer.Token list option
    Parsed  : List<Parser.BasicExpression> option
    Lines   : List<LineSplitter.Line> option
    Scopes  : ScopeBuilder.Scope list
  }
  with 
    static member Zero =
      {
        Input_files = []
        Tokens      = None
        Parsed      = None
        Lines       = None
        Scopes      = []     
      }

let file_paths = ["../../../Content/Metacompiler/StandardLibrary/";
                  "../../../Content/Metacompiler/BasicMonads/"]

let rec find_correct_path (paths:List<string>)(name:string) :Option<string> =
  match paths with
  | x::xs ->
    if System.IO.File.Exists(x + name) then 
      Some(x + name)
    else 
      find_correct_path xs name
  | [] -> None

let next_input : Parser<string,Scope,_> = 
  fun (paths,ctxt) ->
  match paths with 
  | x::xs -> Done((),xs,ctxt)
  | [] -> Error ""

let start_lexer : Parser<string,Scope,Token list option> = 
  fun (paths,ctxt) ->
  match paths with
  | x::xs -> 
    match (find_correct_path file_paths x) with 
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
        Error ""
    | None -> Error ""
  | [] -> Error ""

let start_parser : Parser<string,Scope,Parser.BasicExpression list option> = 
  fun (paths,ctxt) ->
  match paths with
  | x::xs -> 
    match (find_correct_path file_paths x) with 
    | Some path ->
      match ctxt.Tokens with 
      | Some tok ->
        match parse path ".paren_cache" tok with
        | Some(parsing) ->
          do printfn "Done parsing in %d ms." t.ElapsedMilliseconds
          do t.Restart()
          Done(Some(parsing),paths,ctxt)
        | _ -> Error ""
      | _ -> Error ""
    | _ -> Error ""
  | _ -> Error ""

let start_line_splitter : Parser<string,Scope,LineSplitter.Line list option> = 
  fun (paths,ctxt) ->
  match paths with
  | x::xs -> 
    match (find_correct_path file_paths x) with 
    | Some path ->
      match ctxt.Parsed with 
      | Some pars ->  
        match split_lines path ".split_cache" pars with
        | Some line_blocks ->
            do printfn "Done line splitting in %d ms." t.ElapsedMilliseconds
            do t.Restart()
            Done (Some(line_blocks),paths,ctxt)
        | _ -> Error ""
      | _ -> Error ""
    | _ -> Error ""
  | _ -> Error ""

let start_scope_builder : Parser<string,Scope,ScopeBuilder.Scope> = 
  fun (paths,ctxt) ->
    match ctxt.Lines with 
      | Some line_blocks ->
        match build_scopes line_blocks with
          | Some scope ->
            do printfn "Done scope building in %d ms." t.ElapsedMilliseconds
            Done(scope,paths,ctxt)
          | _ -> Error ""
      | _ -> Error ""      

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
    (fun scope ctxt -> { ctxt with Scopes = scope :: ctxt.Scopes})

let front_end : Parser<string,Scope,_> = 
  prs{
    do! lexer_p
    do! parser_p
    do! line_splitter_p
    do! scope_builder_p
    do! next_input
  }

let compiler : Parser<string,Scope,List<ScopeBuilder.Scope>> = 
  t.Start()
  prs{
    let! u = front_end |> repeat
    let! scope = getContext
    return scope.Scopes
  }

let start_compiler (input) =
  match compiler (input,Scope.Zero) with
  | Done(res,_,_) -> res
  | Error e -> failwith ""
  
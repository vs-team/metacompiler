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
    Scopes  : ScopeBuilder.Scope option list
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

let start_scope_builder : Parser<string,Scope,ScopeBuilder.Scope option> = 
  fun (paths,ctxt) ->
    match ctxt.Lines with 
      | Some line_blocks ->
        match build_scopes line_blocks with
          | Some scope ->
            do printfn "Done scope building in %d ms." t.ElapsedMilliseconds
            printfn "%A" scope        
            File.WriteAllText ("parser_output.txt",(sprintf "%A" scope)) 
            Done(Some(scope),paths,ctxt)
          | _ -> Error ""
      | _ -> Error ""      

let lexer_p : Parser<string,Scope,Unit> = 
  prs{
    let! tokens = start_lexer
    let! ctxt = getContext
    do! setContext { ctxt with Tokens = tokens}
  }

let parser_p : Parser<string,Scope,Unit> = 
  prs{
    let! parsed = start_parser
    let! ctxt = getContext
    do! setContext { ctxt with Parsed = parsed}
  }

let line_splitter_p : Parser<string,Scope,Unit> = 
  prs{
    let! lines = start_line_splitter
    let! ctxt = getContext
    do! setContext { ctxt with Lines = lines}
  }

let scope_builder : Parser<string,Scope,Unit> = 
  prs{
    let! scope = start_scope_builder
    let! ctxt = getContext
    do! setContext { ctxt with Scopes = scope :: ctxt.Scopes}
  }
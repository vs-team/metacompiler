open Lexer
open Parser
open LineSplitter
open ScopeBuilder
open System
open System.IO

open Common

[<EntryPoint>]
let main argv = 
  let t = System.Diagnostics.Stopwatch()
  //let input_path = "../../../Content/parser_test_prelude.mc"
  let input_path = "../../../Content/parser_test_def.mc"
  //let input_path = "../../../Content/parser_test_module.mc"
  //let input_path = "../../../Content/rules_only.mc"
  t.Start()
  let tokens = tokenize input_path ".lex_cache" ()
  match tokens with
  | Some tokens ->
    do t.Stop()
    do printfn "Done tokenization in %d ms." t.ElapsedMilliseconds
    do t.Restart()
//    printfn "%A" tokens
    match parse input_path ".paren_cache" tokens with
    | Some(parsing) ->
  //    printfn "%A" parsing
      do printfn "Done parsing in %d ms." t.ElapsedMilliseconds
      do t.Restart()
      match split_lines input_path ".split_cache" parsing with
      | Some line_blocks ->
          do printfn "Done line splitting in %d ms." t.ElapsedMilliseconds
    //      printfn "%A" line_blocks
          do t.Restart()
          match build_scopes line_blocks with
          | Some scopes ->
            do printfn "Done scope building in %d ms." t.ElapsedMilliseconds
            printfn "%A" scopes        
            //let writer = File.CreateText ("parser_output.txt")
            File.WriteAllText ("parser_output.txt",(sprintf "%A" scopes)) 
            //fprintfn writer "%A" scopes
          | None ->
            printfn "No scopes returned. Scope builder failed."
        | _ ->
          printfn "No line split returned. Line splitter failed."
    | _ ->
      printfn "No parsing returned. parser failed."
  | _ ->
    printfn "No tokens returned. Tokenizer failed."
  0

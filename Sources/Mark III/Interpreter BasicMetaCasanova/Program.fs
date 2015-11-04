open Lexer
open Parser
open LineSplitter
open ScopeBuilder
open TypeChecker
open System
open System.IO

open Common

[<EntryPoint>]
let main argv = 
  let t = System.Diagnostics.Stopwatch()
  let input_path = "../../../Content/Metacompiler/StandardLibrary/prelude.mc"
  //let input_path = "../../../Content/Metacompiler/StandardLibrary/number.mc"
  //let input_path = "../../../Content/Metacompiler/StandardLibrary/monad.mc"
  //let input_path = "../../../Content/Metacompiler/StandardLibrary/match.mc"
  //let input_path = "../../../Content/Metacompiler/BasicMonads/either.mc"
  //let input_path = "../../../Content/Metacompiler/BasicMonads/id.mc"
  //parser fails//
  //let input_path = "../../../Content/Metacompiler/BasicMonads/list.mc"
  //let input_path = "../../../Content/Metacompiler/BasicMonads/option.mc"
  //let input_path = "../../../Content/Metacompiler/BasicMonads/result.mc"
  let input_path = "../../../Content/Metacompiler/BasicMonads/state.mc"

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
          | Some scope ->
            do printfn "Done scope building in %d ms." t.ElapsedMilliseconds
            printfn "%A" scope        
            //let writer = File.CreateText ("parser_output.txt")
            File.WriteAllText ("parser_output.txt",(sprintf "%A" scope)) 
            //fprintfn writer "%A" scopes
            match TypeCheck scope with
            | Some scope ->
              do printfn "Done typechecking in %d ms." t.ElapsedMilliseconds
            | None ->
              do printfn "typechecking failed."                  
          | None ->
            printfn "No scopes returned. Scope builder failed."
        | _ ->
          printfn "No line split returned. Line splitter failed."
    | _ ->
      printfn "No parsing returned. parser failed."
  | _ ->
    printfn "No tokens returned. Tokenizer failed."
  0

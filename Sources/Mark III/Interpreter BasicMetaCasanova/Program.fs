open Lexer
open Parenthesizer
open LineSplitter
open ScopeBuilder

[<EntryPoint>]
let main argv = 
  let t = System.Diagnostics.Stopwatch()
  let input_path = @"..\..\..\Content\rules_only.mc"
  t.Start()
  let tokens = tokenize input_path ".lex_cache" ()
  match tokens with
  | Some tokens ->
    do t.Stop()
    do printfn "Done tokenization in %d ms." t.ElapsedMilliseconds
    do t.Restart()
//    printfn "%A" tokens
    match parenthesize input_path ".paren_cache" tokens with
    | Some(parenthesization) ->
  //    printfn "%A" parenthesization
      do printfn "Done parenthesization in %d ms." t.ElapsedMilliseconds
      do t.Restart()
      let line_blocks = split_lines input_path ".split_cache" parenthesization
      do printfn "Done line splitting in %d ms." t.ElapsedMilliseconds
//      printfn "%A" line_blocks
      do t.Restart()
      match build_scopes line_blocks with
      | Some scopes ->
        do printfn "Done scope building in %d ms." t.ElapsedMilliseconds
        printfn "%A" scopes        
      | None ->
        printfn "No scopes returned. Scope builder failed."
    | _ ->
      printfn "No parenthesization returned. Parenthesizer failed."
  | _ ->
    printfn "No tokens returned. Tokenizer failed."
  0

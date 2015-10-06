open AST
open Lexer
open Parenthesizer
open LineSplitter

[<EntryPoint>]
let main argv = 
  let t = System.Diagnostics.Stopwatch()
  let input_path = @"..\..\..\Content\prelude.mc"
  t.Start()
  let tokens = tokenize input_path
  match tokens with
  | Some tokens ->
    do t.Stop()
    do printfn "Done tokenization in %d ms." t.ElapsedMilliseconds
    do t.Restart()
//    printfn "%A" tokens
    match parenthesize tokens with
    | Some(parenthesization) ->
  //    printfn "%A" parenthesization
      do printfn "Done parenthesization in %d ms." t.ElapsedMilliseconds
      do t.Restart()
      let expression = split_lines parenthesization
      do printfn "Done line splitting in %d ms." t.ElapsedMilliseconds
      //printfn "%A" expression
      do printfn "Done parsing."
    | _ ->
      printfn "No parenthesization returned. Parenthesizer failed."
  | _ ->
    printfn "No tokens returned. Tokenizer failed."
  0

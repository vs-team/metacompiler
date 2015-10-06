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
    t.Stop()
    printfn "Done tokenization in %d ms." t.ElapsedMilliseconds
//    printfn "%A" tokens
//    System.Console.ReadLine() |> ignore
    match parenthesize tokens with
    | Some(parenthesization) ->
  //    printfn "%A" parenthesization
      let expression = split_lines parenthesization
      printfn "%A" expression
    | _ ->
      printfn "No parenthesization returned. Parenthesizer failed."
  | _ ->
    printfn "No tokens returned. Tokenizer failed."
  0

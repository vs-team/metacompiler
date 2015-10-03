open AST
open Lexer
open Parenthesizer

[<EntryPoint>]
let main argv = 
  let input_path = @"..\..\..\Content\lexer bmc.mc"
  let tokens = tokenize input_path
  match tokens with
  | Some tokens ->
//    printfn "%A" tokens
//    System.Console.ReadLine() |> ignore
    let expression = parenthesize tokens
    printfn "%A" expression
  | _ ->
    printfn "No tokens returned. Tokenizer failed."
  0

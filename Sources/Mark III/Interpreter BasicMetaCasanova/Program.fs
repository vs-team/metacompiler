open AST
open Lexer
open Parenthesizer

(*
ISSUES: 
- pos only makes sense within lexer; remove from monad
- indentation bracket is missing
*)

[<EntryPoint>]
let main argv = 
  let input_path = @"..\..\..\Content\lexer bmc.mc"
  let tokens = tokenize input_path
  match tokens with
  | Some tokens ->
//    printfn "%A" tokens
    let expression = parenthesize tokens
    printfn "%A" expression
  | _ ->
    printfn "No tokens returned. Tokenizer failed."
  0

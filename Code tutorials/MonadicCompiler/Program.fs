open ParseMonad
open ErrorMonad
open Lexer
open Parenthesizer
open Prioritizer
open Interpreter

[<EntryPoint>]
let main argv =
  let file = System.IO.File.ReadAllText "main.src" |> Seq.toList
  match lexer() file () with
  | Result(tokens,_,_) ->
    match parenthesize() tokens () with
    | Result(coarseAST,_,_) ->
      let ast = prioritize coarseAST
      match interpret ast Map.empty with
      | Result(x, m) ->
        printfn "%A" m
      | _ ->
        printfn "Interpreter failed"
    | _ ->
    printfn "Parenthesizer failed"
  | _ -> 
    printfn "Lexer failed"
  0

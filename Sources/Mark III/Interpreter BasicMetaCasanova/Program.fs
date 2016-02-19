open ParserMonad
open Lexer
open Lexer2
open Parser
open LineSplitter
open ScopeBuilder
open TypeChecker
open PipeLine
open Pipeline2
open System
open System.IO

open Common

[<EntryPoint>]
let main argv = 
  let input = [// "prelude"
               //;"number"
               //;"monad"
               //;"match"
               //;"either"
               //;"id"
               //;"list"
               //;"option"
               //;"result"
               //;"state"
               //"testforparser"
               "decltest"
              ]
  let file_paths = ["../../../Content/Metacompiler/StandardLibrary/";
                    "../../../Content/Metacompiler/BasicMonads/";
                    "../../../Content/Metacompiler/Programs/"]
  

  let res = 
    match start file_paths input with
    | Some(x) -> File.WriteAllText ("parser_output.txt",(sprintf "%A" x)) 
    | None    ->  printfn "The whole damn compiler failed."

  res
  0

  (*
  match start_compiler input file_paths with
  | Done(scope,_,_) ->
    File.WriteAllText ("parser_output.txt",(sprintf "%A" scope)) 
    0
  | Error e -> 
    printfn "%A" e
    0
  *)


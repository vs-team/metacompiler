open ParserMonad
open Lexer
open Lexer2
open Parser
open LineSplitter
open ScopeBuilder
open TypeChecker
open PipeLine
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
               //;
               "list"
               //;"option"
               //;"result"
               //;"state"
              ]
  let file_paths = ["../../../Content/Metacompiler/StandardLibrary/";
                    "../../../Content/Metacompiler/BasicMonads/"]
  
  let path = 
    match find_correct_path file_paths "list" with
    | Some(st) -> st
    | None -> failwith "no file"

  match tokenize2 path ([],Position.Zero) with
  | Done(scope,_,_) ->
    File.WriteAllText ("parser_output.txt",(sprintf "%A" scope)) 
    0
  | Error e -> 
    printfn "%A" e
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


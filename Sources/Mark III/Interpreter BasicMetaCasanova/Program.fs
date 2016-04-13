open ParserMonad
open Lexer2
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
               //"testfornormalizer"
               "typefunctest"
               //"testforcodegen"
               //"decltest"
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

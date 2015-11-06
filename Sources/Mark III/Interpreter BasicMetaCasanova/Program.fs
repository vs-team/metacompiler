open Lexer
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
  let input = ["prelude.mc"
               ;"number.mc"
               ;"monad.mc"
               ;"match.mc"
               ;"either.mc"
               ;"id.mc"
               ;"list.mc"
               ;"option.mc"
               ;"result.mc"
               ;"state.mc"
              ]
  let scope = start_compiler input
  //printfn "%A" scope        
  File.WriteAllText ("parser_output.txt",(sprintf "%A" scope)) 

  0


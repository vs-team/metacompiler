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
  let input = [ "prelude"
               ;"number"
               ;"monad"
               ;"match"
               ;"either"
               ;"id"
               ;"list"
               ;"option"
               ;"result"
               ;"state"
              ]
  let scope = start_compiler input
  //printfn "%A" scope        
  File.WriteAllText ("parser_output.txt",(sprintf "%A" scope)) 
  let typecheck = TypeCheck {ImportDeclaration=[];InheritDeclaration=[];FunctionDeclarations=[];TypeFunctionDeclarations=[];DataDeclarations=[];TypeFunctionRules=[];Rules=[]} Map.empty 
  0


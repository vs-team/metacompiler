open Lexer
open Parser
open LineSplitter
open ScopeBuilder

open Common

type Namespace = {
  Name     : string
  Position : Position
  Scope    : NamespaceScope
}

and NamespaceScope = {
  Namespaces : List<Namespace>
  FuncDecls  : List<SymbolDeclaration>
  DataDecls  : List<SymbolDeclaration>
  Rules      : List<Rule>
  Modules    : List<Module>
}

and Module = {
  Name     : string
  Args     : List<Type>
  Position : Position
  Scope    : ModuleScope
}

and ModuleScope = {
  FuncDecls : List<SymbolDeclaration>
  DataDecls : List<SymbolDeclaration>
}

[<EntryPoint>]
let main argv = 
  let t = System.Diagnostics.Stopwatch()
  let input_path = "../../../Content/parser_test_module.mc"
  t.Start()
  let tokens = tokenize input_path ".lex_cache" ()
  match tokens with
  | Some tokens ->
    do t.Stop()
    do printfn "Done tokenization in %d ms." t.ElapsedMilliseconds
    do t.Restart()
//    printfn "%A" tokens
    match parenthesize input_path ".paren_cache" tokens with
    | Some(parenthesization) ->
  //    printfn "%A" parenthesization
      do printfn "Done parenthesization in %d ms." t.ElapsedMilliseconds
      do t.Restart()
      let line_blocks = split_lines input_path ".split_cache" parenthesization
      do printfn "Done line splitting in %d ms." t.ElapsedMilliseconds
//      printfn "%A" line_blocks
      do t.Restart()
      match build_scopes line_blocks with
      | Some scopes ->
        do printfn "Done scope building in %d ms." t.ElapsedMilliseconds
        printfn "%A" scopes        
      | None ->
        printfn "No scopes returned. Scope builder failed."
    | _ ->
      printfn "No parenthesization returned. Parenthesizer failed."
  | _ ->
    printfn "No tokens returned. Tokenizer failed."
  0

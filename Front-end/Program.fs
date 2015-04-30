﻿open System
open Utilities
open ParserMonad
open BasicExpression
open ConcreteExpressionParserPrelude
open ConcreteExpressionParser
open CodeGenerator
open Microsoft.CSharp
open System.CodeDom.Compiler
open System.IO
open System.Runtime.Serialization
open System.Xml.Serialization

do System.Threading.Thread.CurrentThread.CurrentCulture <- System.Globalization.CultureInfo.GetCultureInfo("EN-US")

let numSteps = 1000

let runDeduction path =
  let originalFilePath = System.IO.Path.Combine(path, "transform.mc")
  let rules = System.IO.File.ReadAllText(originalFilePath)
  let title = System.IO.Path.GetFileName path
  let timer = System.Diagnostics.Stopwatch()
  let output = ref ""
  let addOutput s = output := sprintf "%s\n%s" (output.Value) s
  match (program()).Parse (rules |> Seq.toList) ConcreteExpressionContext.Empty (Position.Create path) with
  | First(x,_,ctxt,pos) -> 
    fun (input:string) ->
      let input = input.Trim([|'\r'; '\n'|]) + "\n"
//      do debug_expr <- true
      match expr().Parse (input |> Seq.toList) ctxt Position.Zero with
      | First(y,_,ctxt',pos') ->
        try
          let generatedPath = generateCode originalFilePath title x y ctxt
          let args = new System.Collections.Generic.Dictionary<string, string>()
          do args.Add("CompilerVersion", "v4.5")
          let csc = new CSharpCodeProvider()
          let parameters = new CompilerParameters([| "mscorlib.dll"; "System.dll"; "System.Runtime.dll"; "System.Core.dll"; "System.Collections.Immutable.dll" |], sprintf "%s.dll" title, true)
          do parameters.GenerateInMemory <- true
          do parameters.CompilerOptions <- @"/optimize+"
          let results = csc.CompileAssemblyFromFile(parameters, [|generatedPath|])
          if results.Errors.HasErrors then
            for error in results.Errors do
              if error.IsWarning |> not then
                do sprintf "%s at %d: %s" error.FileName error.Line error.ErrorText |> addOutput 
            if CompilerSwitches.flushCSFileOnError then
              do System.IO.File.WriteAllText(generatedPath, "")
          else
            let types = results.CompiledAssembly.GetTypes()
            let entryPoint = types |> Seq.find (fun t -> t.Name = "EntryPoint")
            let run = entryPoint.GetMethod("Run")
            let results = run.Invoke(null, [|false|]) :?> seq<obj> |> Seq.toList
            do timer.Start()
            for i = 1 to numSteps do
              do run.Invoke(null, [|false|]) :?> seq<obj> |> Seq.toList |> ignore
            do timer.Stop()
            for r in results do sprintf "%A" r  |> addOutput 
            do "\n" |> addOutput 
            do sprintf "Total elapsed time per iteration = %gms" (float timer.ElapsedMilliseconds / float numSteps)|> addOutput
        with
        | e ->
          e.Message |> addOutput  
        output.Value
      | Second errors -> 
        sprintf "Parse error(s) in program at\n%s." ([errors] |> Error.Distinct |> Seq.map (fun e -> e.Line |> string) |> Seq.reduce (fun s x -> sprintf "%s\n%s" s x)) |> addOutput
        output.Value
  | Second errors ->
    fun (input:string) ->
      sprintf "Parse error(s) in rules at lines %s." ([errors] |> Error.Distinct |> Seq.map (fun e -> e.Line |> string) |> Seq.reduce (fun s x -> sprintf "%s\n%s" s x)) |> addOutput
      output.Value


[<EntryPoint; STAThread>]
let main argv = 
  let samples = 
    [
//      "Generic lists", @"runTest1"

      //"Cmm", @"runProgram"
//      "Trees 234", @"main"

      "Peano numbers", "!(((s(s(z))) * (s(s(z)))) * (s(s(z)) + s(z)))"
      "Boolean expressions", "run"

      "Lambda calculus", "run"
      "Binary trees", "run"

      "Lists", "mergeSort 5;6;4;10;9;8;7;0;1;2;3;nil"

      "Lists", "mergeSort 5;6;4;10;9;8;7;0;1;2;3;nil"

      "Lists", "plus 0;1;2;3;nil 10"
      "Lists", "length 0;1;2;3;nil"
      "Lists", "contains 0;1;2;3;nil 2"
      "Lists", "removeOdd 0;1;2;3;nil"
      "Lists", "add 0;1;2;3;nil"

      "stsil", "dda lin snoc 3 snoc 2 snoc 1"
      "Lists", "add 3;2;1;nil"

      "Eval without memory", "run"
//      "Eval with readonly memory", "run (map <<ImmutableDictionary<string, int>.Empty>>)"
//      "Eval with memory", "run (map <<ImmutableDictionary<string, int>.Empty>>)"
//      "Eval with memory and control flow", "run (map <<ImmutableDictionary<string, Value>.Empty>>)"

//      "Casanova semantics", @"runTest1"
    ]

  for name,input in samples 
    do runDeduction (System.IO.Path.Combine([| "Content"; name|])) input |> printfn "%s"

//  do GUI.ShowGUI samples runDeduction

  0

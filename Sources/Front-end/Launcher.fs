module Launcher
open System
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
open AnalyserAST
open AssemblyPrecaching

let  readFiles (path : string) : List<string>= 
      let stringSeq = path.Split([|'\\'|])
      let stringList = stringSeq |> Seq.toList 
      let rec crawlFiles (s : List<string>) (currentDirect : string) =
        match s with
        | [] -> Directory.GetFiles(currentDirect)|> Seq.toList
        | x::xs -> 
          match Directory.Exists(x) with
          | true ->  crawlFiles xs (currentDirect+x)
          | false -> match File.Exists(x) with
                      | true -> [(x + currentDirect)]
                      | false -> failwith "The specified file and/or directory doesn't exist"
      crawlFiles (stringList) ("")
      

let runDeduction path =
        let originalFilePath = path
        let rules = System.IO.File.ReadAllText(originalFilePath)
        let title = System.IO.Path.GetFileName path
        let timer = System.Diagnostics.Stopwatch()
        let output = ref ""
        let addOutput s = output := sprintf "%s\n%s" (output.Value) s
        match (program()).Parse (rules |> Seq.toList) ConcreteExpressionContext.Empty (Position.Create originalFilePath) with
        | First(x,_,ctxt,pos) -> 
          fun (input:string) ->
            let input = input.Trim([|'\r'; '\n'|]) + "\n"
      //      do debug_expr <- true
            match expr().Parse (input |> Seq.toList) ctxt Position.Zero with
            | First(y,_,ctxt',pos') ->
              try
              let customDlls = ["UnityEngine.dll"; "System.Collections.Immutable.dll"]
              let defaultDlls = [ "mscorlib.dll"; "System.dll"; "System.Runtime.dll"; "System.Core.dll"] 
              let dllParam = Array.append (List.toArray defaultDlls) (List.toArray customDlls)
              let ctxt = { ctxt with AssemblyInfo = assemblyPrecache defaultDlls customDlls }
              if CompilerSwitches.useGraphBasedCodeGenerator then 
                GraphBasedCodeGenerator.generate originalFilePath title x y ctxt 
              let generatedPath = generateCode originalFilePath title x y ctxt
              let programToAnalyser = convert x
              //let customKeywordsToAnalyser = ctxt.CustomKeywords |> List.map(fun keyword -> convert keyword)
              let args = new System.Collections.Generic.Dictionary<string, string>()
              do args.Add("CompilerVersion", "v4.5")
              let csc = new CSharpCodeProvider()
              let parameters = new CompilerParameters(dllParam, sprintf "%s.dll" title, true)
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
                let results =
                  match run.Invoke(null, [|false|]) with
                  | :? seq<obj> as res -> res |> Seq.toList
                  | res -> [res]
                do timer.Start()
                for i = 1 to CompilerSwitches.numProfilerRuns do
                  match run.Invoke(null, [|false|]) with
                  | :? seq<obj> as res -> res |> Seq.toList |> ignore
                  | res -> ()
                do timer.Stop()
                for r in results do sprintf "%A" r  |> addOutput 
                do "\n" |> addOutput 
                if CompilerSwitches.numProfilerRuns > 0 then
                  do sprintf "Total elapsed time per iteration = %gms" (float timer.ElapsedMilliseconds / float CompilerSwitches.numProfilerRuns) |> addOutput
                else
                  do sprintf "Total elapsed time per iteration = 0ms" |> addOutput
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

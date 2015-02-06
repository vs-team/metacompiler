open System
open Utilities
open ParserMonad
open BasicExpression
open ConcreteExpressionParser
open CodeGenerator
open Microsoft.CSharp
open System.CodeDom.Compiler

let runDeduction path (input:string) =
  let input = input.Trim([|'\r'; '\n'|]) + "\n"
  let rules = System.IO.File.ReadAllText(System.IO.Path.Combine(path, "transform.mc"))
  let title = System.IO.Path.GetFileName path
  let timer = System.Diagnostics.Stopwatch()
  let output = ref ""
  let addOutput s = output := sprintf "%s\n%s" (output.Value) s
  match (program()).Parse (rules |> Seq.toList) ConcreteExpressionContext.Empty with
  | [] -> sprintf "Parse error in rules." |> addOutput 
  | (x,_,ctxt)::xs -> 
    match expr().Parse (input |> Seq.toList) ctxt with
    | [] -> sprintf "Parse error in expression %s." input |> addOutput 
    | (y,_,ctxt')::ys ->
      let src = generateCode title x y ctxt
      let args = new System.Collections.Generic.Dictionary<string, string>()
      do args.Add("CompilerVersion", "v4.5")
      let csc = new CSharpCodeProvider()
      let parameters = new CompilerParameters([| "mscorlib.dll"; "System.Core.dll" |], sprintf "%s.dll" title, true)
      do parameters.GenerateInMemory <- true
      let results = csc.CompileAssemblyFromSource(parameters, src)
      if results.Errors.HasErrors then
        for error in results.Errors
          do sprintf "%s" error.ErrorText |> addOutput 
      else
        let types = results.CompiledAssembly.GetTypes()
        let entryPoint = types |> Seq.find (fun t -> t.Name = "EntryPoint")
        let run = entryPoint.GetMethod("Run")
        do timer.Start()
        let results = run.Invoke(null, [|false|]) :?> seq<obj> |> Seq.toList
        do timer.Stop()
        for r in results do sprintf "%A" r  |> addOutput 
        do "\n" |> addOutput 
  do sprintf "Total elapsed time = %dms" timer.ElapsedMilliseconds |> addOutput
  output.Value


[<EntryPoint; STAThread>]
let main argv = 
  let ($) p a = p.Parse a

  //let casanova = System.IO.File.ReadAllText @"Content\casanova semantics.mc"
  let samples = 
    [
      "Lambda calculus", @"(\$""y"".$""y"" | \$""y"".$""y"") | ($""x"" | $""z"")" + "\n"
      "Peano numbers", "(s(s(z))) * (s(s(z)))\n"
    ]

  do GUI.ShowGUI samples runDeduction
  0

open System
open Utilities
open ParserMonad
open BasicExpression
open ConcreteExpressionParser
open CodeGenerator

[<EntryPoint; STAThread>]
let main argv = 
  let ($) p a = p.Parse a

  //let casanova = System.IO.File.ReadAllText @"Content\casanova semantics.mc"
  let samples = 
    [
      "Lambda calculus", @"(\$""y"".$""y"" | \$""y"".$""y"") | ($""x"" | $""z"")" + "\n"
      "Peano numbers", "(s(s(z))) * (s(s(z)))\n"
    ]

  do GUI.ShowGUI samples

//  let timer = System.Diagnostics.Stopwatch()
//  for title, rules, input in samples do
//    match (program()).Parse (rules |> Seq.toList) ConcreteExpressionContext.Empty with
//    | [] -> printfn "Parse error in rules."
//    | (x,_,ctxt)::xs -> 
//      match expr().Parse (input |> Seq.toList) ctxt with
//      | [] -> printfn "Parse error in expression %s." input
//      | (y,_,ctxt')::ys ->
//        let src = generateCode title x y ctxt
//        let args = new System.Collections.Generic.Dictionary<string, string>()
//        do args.Add("CompilerVersion", "v4.5")
//        let csc = new CSharpCodeProvider()
//        let parameters = new CompilerParameters([| "mscorlib.dll"; "System.Core.dll" |], sprintf "%s.dll" title, true)
//        let results = csc.CompileAssemblyFromSource(parameters, src)
//        if results.Errors.HasErrors then
//          for error in results.Errors
//            do printfn "%s" error.ErrorText
//        else
//          do printfn "Compilation succesful.\n"
//          let types = results.CompiledAssembly.GetTypes()
//          let entryPoint = types |> Seq.find (fun t -> t.Name = "EntryPoint")
//          let run = entryPoint.GetMethod("Run")
//          do timer.Start()
//          let results = run.Invoke(null, [|false|]) :?> seq<obj> |> Seq.toList
//          do timer.Stop()
//          for r in results do
//            do printfn "%A" r
//          do printfn "\n"
//  do printfn "Total elapsed time = %dms" timer.ElapsedMilliseconds
  0

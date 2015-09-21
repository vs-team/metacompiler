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

do System.Threading.Thread.CurrentThread.CurrentCulture <- System.Globalization.CultureInfo.GetCultureInfo("EN-US")

[<EntryPoint; STAThread>]
let main argv = 
  let samples = 
    [
//      "CNV3/Statements.mc", "run"
//        "CNV3/Traverse.mc", "run 0.1"
        "CNV3/Imported.mc", "testImported"
//      "Sequence/seq.mc", "evals bb"
//      "CNV3/Tuples.mc", "fst (1.0,2.0)"
//      "Test/test.mc", "debug"
//      "CNV3/Basics.mc", "test"

//      "CodegenTest/ListTest.mc", "length 5::(4::(3::(2::(1::nil))))"

//      "Boolean expressions/transform.mc", "run"
//      "Boolean expressions fast/transform.mc", "run"

//      "PeanoNumbers/transform.mc", "run"
//      "Lists/transform.mc", "plus 0;1;2;3;nil 10"
//      "Lists/transform.mc", "length 0;1;2;3;nil"
//      "Lists/transform.mc", "contains 0;1;2;3;nil 2"
//      "Lists/transform.mc", "removeOdd 0;1;2;3;nil"
//      "Lists/transform.mc", "add 0;1;2;3;nil"
//
//      "stsil/transform.mc", "dda lin snoc 3 snoc 2 snoc 1"
//      
//      "Eval without memory/transform.mc", "run"
//      "Eval with readonly memory/transform.mc", "run (map <<ImmutableDictionary<string, int>.Empty>>)"
//      "Eval with memory/transform.mc", "run (map <<ImmutableDictionary<string, int>.Empty>>)"
//      "Eval with memory and control flow/transform.mc", "run (map <<ImmutableDictionary<string, Value>.Empty>>)"
//
//      "Binary trees/transform.mc", "run"
//
//      "Trees 234/transform.mc", @"main"
//
//      "Lambda calculus/transform.mc", "run"
//
//      "GenericLists/transform.mc", @"length 0::1::2::3::nil"
//      "GenericLists/transform.mc", @"length ""0""::""1""::""2""::""3""::nil"
      
// not yet converted to new keyword syntax:


      //"Cmm", @"runProgram"
//      "Casanova semantics", @"runTest1"
    ]

  for name,input in samples 
    do Launcher.runDeduction (System.IO.Path.Combine([| @"../../../Content/Content"; name|])) input |> printfn "%s"

  0

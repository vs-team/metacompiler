module CodeGenerator

open TypeInference
open System
open Utilities
open ParserMonad
open BasicExpression
open TypeDefinition
open ConcreteExpressionParserPrelude
open ConcreteExpressionParser
open CodeGeneratorDeps

type TypedExpression   = BasicExpression<Keyword, Var, Literal, Position, Type>
type UntypedExpression = BasicExpression<Keyword, Var, Literal, Position, unit>

let generateCode (originalFilePath:string) (program_name:string)
                 (rules:UntypedExpression)
                 (program:UntypedExpression)
                 (ctxt:ConcreteExpressionContext) =
  
  match rules with
  | Application(Implicit, Keyword(Sequence, _, _) :: rules, pos, _) ->
    let mutable classes = Map.empty
    let mutable inheritanceRelationships = Map.empty
    for c,a in ctxt.AllInheritanceRelationships do
      match inheritanceRelationships |> Map.tryFind c with
      | Some i -> i.BaseInterfaces.Add a
      | None -> inheritanceRelationships <- inheritanceRelationships |> Map.add c { Name = c; BaseInterfaces = ResizeArray([a]) }
    for keyword in ctxt.CustomKeywords do
      let newClass = { GeneratedClass.Keyword   = keyword
                       GeneratedClass.BasicName = keyword.Name
                       GeneratedClass.GenericArguments = keyword.GenericArguments |> List.map fst
                       GeneratedClass.Interface = Keyword.ArgumentCSharpStyle keyword.BaseType cleanupWithoutDot
                       GeneratedClass.Parameters = ResizeArray()
                       GeneratedClass.Methods = Map.empty }
      for t,i in keyword.LeftArguments |> Seq.mapi (fun i p -> p,i+1) do
        newClass.Parameters.Add({ Name = sprintf "P%d" i; IsLeft = true; Type = t })
      for t,i in keyword.RightArguments |> Seq.mapi (fun i p -> p,i+1) do
        newClass.Parameters.Add({ Name = sprintf "P%d" (i + keyword.LeftArguments.Length); IsLeft = false; Type = t })
      classes <- classes.Add(keyword.Name,newClass)

    do process_rules classes [] rules ctxt

    let programTyped,_,_ = TypeInference.inferTypes program (Extension({ Name = "___tmp" }, Position.Zero, ())) [] ctxt

    let classes = classes
    let extensions = @"public static class Extensions { public static V GetKey<T, V>(this System.Collections.Immutable.ImmutableDictionary<T, V> self, T key) { return self[key]; } }"
    let interfaces = [ for k in ctxt.CustomKeywords -> Keyword.ArgumentCSharpStyle k.BaseType cleanupWithoutDot ] |> Seq.distinct |> Seq.toList 
    let inheritanceRelationships = inheritanceRelationships
    let interfacesCode = 
      [
        for i in interfaces do
          match inheritanceRelationships |> Map.tryFind (i) with
          | Some ir ->
            let explicitInterfaces = ir.BaseInterfaces
            yield sprintf "public interface %s : %s {}\n" i (explicitInterfaces |> Seq.reduce (fun s x -> s + ", " + x))
          | _ ->
            yield sprintf "public interface %s {}\n" i
      ] |> Seq.fold (+) ""
    let all_method_paths =
      seq{
        for c in classes -> c.Value.MethodPaths
      } |> Seq.reduce (+)
    let imports = 
        if ctxt.ImportedModules.Length > 0 then
            (ctxt.ImportedModules
                    |> List.map (fun x -> sprintf "using %s;\n" x)
                    |> List.reduce (fun x y -> x + y))
        else
            ""
    let prelude = sprintf "using System.Collections.Generic;\nusing System.Linq;\nnamespace %s {\n %s\n" (program_name.Replace(" ", "_")) extensions
    let programKeyword = ctxt.CustomKeywordsMap.[programTyped |> extractLeadingKeyword]
    let main = 
      let printedHead = "<head><link rel=\\\"stylesheet\\\" type=\\\"text/css\\\" href=\\\"style.css\\\"></head>\\n<ul class=\\\"tree\\\">\\n"
      let printedTail = "\\n</ul>"
      let OptionalPrintHead = if CompilerSwitches.printExpressionTree then sprintf "System.Console.WriteLine(\"%s\");" printedHead else ""
      let OptionalPrintTail = if CompilerSwitches.printExpressionTree then sprintf "System.Console.WriteLine(\"%s\");" printedTail else ""

      match programKeyword.Multeplicity with
      | KeywordMulteplicity.Single ->
        sprintf "public class EntryPoint {\n public static bool Print(string s) {System.Console.WriteLine(s); return true;}\n   \nstatic public object Run(bool printInput)\n{\n #line 1 \"input\"\n var p = %s;\nif(printInput) System.Console.WriteLine(p.ToString());\n %s\n var result = p.Run(); %s\n\nreturn result;\n}\n}\n" 
                (createElement ctxt programTyped |> fst) OptionalPrintHead OptionalPrintTail
      | KeywordMulteplicity.Multiple ->
        sprintf "public class EntryPoint {\n public static bool Print(string s) {System.Console.WriteLine(s); return true;}\n   static public IEnumerable<object> Run(bool printInput)\n{\n #line 1 \"input\"\n var p = %s;\nif(printInput) System.Console.WriteLine(p.ToString());\nforeach(var x in p.Run())\nyield return x;\n}\n}\n" 
                (createElement ctxt programTyped |> fst)
    [
      yield imports
      yield prelude
      yield "\n\n\n"
      yield interfacesCode
      yield "\n\n\n"
      for c in classes do
        let c = c.Value
        yield c.ToString(all_method_paths, ctxt, originalFilePath)
      yield "\n\n\n"
      yield main
      yield "\n}\n"
    ] |> Seq.fold (+) "" |> (fun txt -> System.IO.File.WriteAllText(program_name + ".cs", txt); program_name + ".cs")
  | _ -> failwithf "Cannot extract rules from input program @ %A" rules.DebugInformation

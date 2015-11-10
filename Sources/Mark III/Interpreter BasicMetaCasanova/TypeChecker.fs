module TypeChecker
open Common
open ScopeBuilder // Scope
open Parenthesizer

(*
Most Generic Fit
inline typefuncs
  order:
    typefuncs that return Signature
    other typefuncs (lazy: only when needed)
  everything must be inlined
  if can't solve, realize one level
  every level must solve at least one parameter
  keep track of every solved step to avoid type recursion (and for speed)
inline modules to set of functions for each instance
inline lambdas
  keep track of usage count. If too much, don't inline

typechecking algorithm
  treeify expressions
    symbol lookup
      find in scope, else find in parent (breadth-first, bottom import first)
*)

type UntypedScope = {
  Parents       : List<UntypedScope>
  FuncDecls     : Map<Id,SymbolDeclaration>
  TypeFuncDecls : Map<Id,SymbolDeclaration>
  DataDecls     : Map<Id,SymbolDeclaration>
  TypeFuncRules : Map<Id,List<Rule>>
  FuncRules     : Map<Id,List<Rule>>
}

let listToMapOfLists (lst:List<'k*'v>) :Map<'k,List<'v>> =
  lst |> List.fold 
    (fun acc (k,v)-> 
      match acc |> Map.tryFind k with
      | None   -> acc |> Map.add k [v]
      | Some x -> acc |> Map.add k (v::x))
    Map.empty
  |> Map.map (fun _ v -> List.rev v)

// TODO: add caching
let rec toUntypedScope (root:Scope) (scopes:Map<Id,Scope>) :UntypedScope = 
  let matchRules rules =
    rules
    |> List.map (fun (rule:Rule) -> 
       let bar = Parenthesize root.FunctionDeclarations rule.Input
       let baz = bar |> List.find (fun x -> match x with
                                            | Id (str,pos) -> root.FunctionDeclarations 
                                                              |> List.tryFind (fun e->str=e.Name)
                                                              |> Option.isSome
                                            | _ -> false)
       let name:Id = match baz with Id (str,_) -> str
       name,rule)
  {
    Parents       = root.ImportDeclaration |> List.map (fun x -> toUntypedScope scopes.[x] scopes)
    FuncDecls     = root.FunctionDeclarations     |> List.map (fun x -> x.Name,x) |> Map.ofList
    TypeFuncDecls = root.TypeFunctionDeclarations |> List.map (fun x -> x.Name,x) |> Map.ofList
    DataDecls     = root.DataDeclarations         |> List.map (fun x -> x.Name,x) |> Map.ofList
    TypeFuncRules = matchRules root.TypeFunctionRules |> listToMapOfLists
    FuncRules     = matchRules root.Rules             |> listToMapOfLists
  }

type TypeConstructors = Map<Id,Type>
type Type = Star       // type
          | Signature  // module
          | TypeId     of Id
          | BigArrow   of Type*Type
          | SmallArrow of Type*Type
          | Union      of Type*TypeConstructors


let test_decls = 
  let pos = { File="not_a_real_file.mc";Line=1;Col=1; }
  [
    {Name="+";LeftArgs=[[Id("int",pos)]];RightArgs=[[Id("int",pos)]];Return=[Id("int",pos)];Priority=30;Associativity=Left;Position=pos}
    {Name="*";LeftArgs=[[Id("int",pos)]];RightArgs=[[Id("int",pos)]];Return=[Id("int",pos)];Priority=40;Associativity=Left;Position=pos}
    {Name="^";LeftArgs=[[Id("int",pos)]];RightArgs=[[Id("int",pos)]];Return=[Id("int",pos)];Priority=50;Associativity=Right;Position=pos}
    {Name="sqrt";LeftArgs=[];RightArgs=[[Id("int",pos)]];Return=[Id("int",pos)];Priority=0;Associativity=Right;Position=pos}
    {Name="mod";LeftArgs=[];RightArgs=[[Id("int",pos)];[Id("int",pos)]];Return=[Id("int",pos)];Priority=0;Associativity=Right;Position=pos}
  ]

let test_exprs = 
  let pos = { File="not_a_real_file.mc";Line=1;Col=1; }
  [ [
      Literal(Int(2),{pos with Col=1})
      Id("^",        {pos with Col=2})
      Id("x",        {pos with Col=3})
      Id("+",        {pos with Col=4})
      Literal(Int(3),{pos with Col=5})
      Id("*",        {pos with Col=6})
      Literal(Int(5),{pos with Col=7})
      Id("^",        {pos with Col=8})
      Literal(Int(7),{pos with Col=9})
    ];[
      Id("sqrt",     {pos with Col=1})
      Id("mod",      {pos with Col=2})
      Literal(Int(2),{pos with Col=3})
      Id("+",        {pos with Col=4})
      Id("x",        {pos with Col=5})
      Literal(Int(3),{pos with Col=6})
      Id("*",        {pos with Col=7})
      Id("y",        {pos with Col=8})
    ];[
      Id("a",{pos with Col=1})
      Id("+",{pos with Col=2})
      Id("*",{pos with Col=3})
      Id("b",{pos with Col=4})
    ];[
      Application(Round,[
        Literal(Int(2),{pos with Col=1})
        Id("^",        {pos with Col=2})
      ])
      Literal(Int(3),  {pos with Col=3})
    ];[
      Application(Round,[
        Id("^",        {pos with Col=1})
        Literal(Int(2),{pos with Col=2})
      ])
      Literal(Int(3),  {pos with Col=3})
    ]
  ]

let TypeCheck (root:Scope) (scopes:Map<Id,Scope>) =
  do printfn "starting type checker (using dummy data)"
  do test_exprs |> List.iter (fun x-> do printfn "IN  %s" (prettyPrintExprs x)
                                      do printfn "OUT %s" (Parenthesize test_decls x |> prettyPrintExprs))
  None

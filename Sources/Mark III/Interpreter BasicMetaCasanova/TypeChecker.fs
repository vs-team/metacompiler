module TypeChecker
open Common
open ScopeBuilder // Scope
open Prioritizer

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

how to handle modules (signatures)
  translate signatures to templated functions
*)

type TypedScope = {
  Parents       : List<Id*TypedScope>
  FuncDecls     : Map<Id,SymbolDeclaration*Type>
  TypeFuncDecls : Map<Id,SymbolDeclaration*Type>
  DataDecls     : Map<Id,SymbolDeclaration*Type>
  TypeFuncRules : Map<Id,List<Rule>>
  FuncRules     : Map<Id,List<Rule>>
}

and TreeExpr = Abs of TreeExpr*MaybeType*Position
             | App of TreeExpr*TreeExpr*MaybeType*Position
             | Var of Id*MaybeType*Position
             | Lit of Literal*Type*Position

and Rule = {
  Input    :TreeExpr
  Output   :TreeExpr
  Premises :List<Premise>
}and Premise = Assignment  of TreeExpr*TreeExpr
             | Conditional of TreeExpr

and Type = Star       // type
         | Signature  // module
         | TypeId     of Id
         | BigArrow   of Type*Type
         | SmallArrow of Type*Type
         | Union      of Type*TypeConstructors
and TypeConstructors = Map<Id,Type>

and MaybeType = Conflict of List<TreeExpr*TreeExpr>
              | Known    of Type
              | Unknown

type Expr = Basic of BasicExpression | Tree of TreeExpr

// Question: does this work correctly with shadowed identifiers?
let rec selectDecls (exprs:List<BasicExpression>) (decls:List<SymbolDeclaration>) : List<SymbolDeclaration> =
  // recursively find names of operators and add them to the set
  let rec used_names exprs decls =
    exprs |> List.fold (fun (acc:Set<string>) (next:BasicExpression) -> 
      match next with 
      | Id(str,_) -> acc.Add(str)
      | Application(_,e) -> acc + (used_names e decls)
      | _ -> acc) Set.empty
  let set = used_names exprs decls
  // sort the set into a list according to the order in decls
  let lst = decls |> List.fold (fun acc next -> if set.Contains(next.Name) then next::acc else acc) []
  List.rev lst

let prioritize (exprs:List<BasicExpression>) (decls:List<SymbolDeclaration>) :List<Expr> =
   let decls = selectDecls exprs decls
   let exprs = exprs |> List.map Basic
   let rec prioritize (exprs:List<Expr>) = []
   prioritize exprs

let listToMapOfLists (lst:List<'k*'v>) :Map<'k,List<'v>> =
  lst |> List.fold 
    (fun acc (k,v)-> 
      match acc |> Map.tryFind k with
      | None   -> acc |> Map.add k [v]
      | Some x -> acc |> Map.add k (v::x))
    Map.empty
  |> Map.map (fun _ v -> List.rev v)

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
  let test = test_exprs |> List.map (fun x-> prioritize x test_decls)
  None

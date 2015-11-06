module TypeChecker
open Common
open ScopeBuilder // Scope
open Parenthesizer

(*
Most Generic fit
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

let rec toUntypedScope (root:Scope) (scopes:Map<Id,Scope>) :UntypedScope = 
  {
    Parents       = root.ImportDeclaration |> List.map (fun x -> toUntypedScope scopes.[x] scopes)
    FuncDecls     = root.FunctionDeclarations     |> List.map (fun x -> x.Name,x) |> Map.ofList
    TypeFuncDecls = root.TypeFunctionDeclarations |> List.map (fun x -> x.Name,x) |> Map.ofList
    DataDecls     = root.DataDeclarations         |> List.map (fun x -> x.Name,x) |> Map.ofList
    TypeFuncRules = Map.empty
    FuncRules     =
      let foo = 
        root.Rules 
        |> List.map (fun (rule:Rule) -> 
          let bar = Parenthesize root.FunctionDeclarations rule.Input
          let baz = bar |> List.find (fun x -> match x with
                                               | Id (str,pos) -> root.FunctionDeclarations 
                                                                 |> List.tryFind (fun e->str=e.Name)
                                                                 |> Option.isSome
                                               | _ -> false)
          let name:Id = match baz with Id (str,_) -> str
          name,rule)
      listToMapOfLists foo
  }

type TypeId      = string
type NamespaceId = string

type TypeConstructors = Map<Id,Type>
type Type = Star       // type
          | Signature  // module
          | TypeId     of TypeId
          | BigArrow   of Type*Type
          | SmallArrow of Type*Type
          | Union      of Type*TypeConstructors

type SymbolTable = Map<Id,Type>
type Env = {
  Parents     : List<NamespaceId>
  SymbolTable : SymbolTable
} with static member Zero() = { Parents=[]; SymbolTable=Map.empty }

type TypedExpression =
  | Literal    of Literal * Type * Position
  | Identifier of Identifier
and Identifier = {
  Id        :Id
  Type      :Type
  LeftArgs  :List<TypedExpression>
  RightArgs :List<TypedExpression>
  Position  :Position
}

type TypedScope = TypedExpression * Env

let TypeCheck (scope:Map<Id,Scope>) = None

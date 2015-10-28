module TypeChecker
open Common
open ScopeBuilder // Scope
open LineSplitter // BasicExpression

type DeclaredScope = {
  Rules : List<Rule*SymbolDeclaration>
  Datas : List<SymbolDeclaration>
}
(*
let RankOperators (expr:BasicExpression) (scope:Scope) :List<SymbolDeclaration> =
  let rec foo expr = 
    match expr with
    | Id (str,_) -> str
  foo expr
*)
let DeclareScope (scope:ScopeBuilder.Scope) : DeclaredScope =
  let matchrules (rules:List<Rule>) (declaration : SymbolDeclaration) : Rule*SymbolDeclaration = 
    (rules.Head,declaration)
  {
    Rules = scope.FunctionDeclarations |> List.map (matchrules scope.Rules)
    Datas = scope.DataDeclarations
  }

type TypeId      = string
type NamespaceId = string

type TypeConstructors = Map<Id,Type>
type Type = Star 
          | Signature
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
  | Id          of Id * Position * Type
  | Literal     of Literal * Position * Type
  | Application of Id * Type * TypedExpression * TypedExpression

type TypedScope = TypedExpression * Env



let EnvBuilder (scope:ScopeBuilder.Scope) : Env = Env.Zero()

//let FindOperators (basic_expr:BasicExpression) (env:Env) :SymbolTable = Map.empty

// let TypeExpression (basic_expr:BasicExpression) (env:Env) : TypedExpression

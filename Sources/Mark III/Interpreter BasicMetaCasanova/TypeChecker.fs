module TypeChecker
open Common
open ScopeBuilder // Scope
open LineSplitter // BasicExpression
open DeclarationMatcher

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

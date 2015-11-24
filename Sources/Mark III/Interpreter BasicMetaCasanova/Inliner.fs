module Inliner
open Common
open ScopeBuilder
open TypeChecker

// eventually needs an evaluator, for now: special-case the Signatures and
// push everything else to run-time

type ConcreteType = Int8 | Int16 | Int32 | Int64 | Float | Double | String
                  | Struct    of List<ConcreteType>
                  | Reference of ConcreteType
                  | Function  of ConcreteType*List<ConcreteType>
                  | List      of ConcreteType
type TemplatedType = List<ConcreteType>

let FindSignatures (decls:Map<Id,SymbolDeclaration*Type>) =
  let rec hasSignature k (s,t) =
    match t with
    | Signature -> true
    | BigArrow (_,r) -> hasSignature k (s,r)
    | _ -> false
  decls |> Map.filter hasSignature 

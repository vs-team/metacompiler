module OutputAst
open Common
open TypeChecker

type VarId  = string
type TypeId = string

type OutputAstType = Int8 | Int16 | Int32 | Int64 | Float | Double | TypeId of TypeId

type OutputAstVar  = VarId      of VarId
                   | Temporary  of int
                   | Argument   of int

type OutputAstExpr = Call of string*seq<OutputAstExpr>
                   | Var       of OutputAstVar
                   | IntLit    of int
                   | StringLit of string
                   | FloatLit  of double

type OutputAstStatement = Assignment  of OutputAstVar*OutputAstExpr
                        | UnionSwitch of OutputAstVar*seq<seq<OutputAstStatement>>
                        | Return      of OutputAstExpr
                        | InlineCode  of string

type OutputAstBody = 
  | Struct       of VarId*seq<OutputAstBody>
  | TaggedUnion  of VarId*seq<OutputAstBody>
  | Data         of OutputAstType*VarId
  | Alias        of TypeId*TypeId
  | Func         of OutputAstFunc
and OutputAstFunc = {
  Static     : bool
  Name       : string
  ReturnType : OutputAstType
  Args       : seq<OutputAstType*VarId>
  Body       : seq<OutputAstStatement>
}

type OutputAst = Namespace    of VarId*seq<OutputAst>
               | NonNamespace of OutputAstBody

(*
let extractType (tree:TreeExpr) : Option<Type> =
  match tree with
  | TreeExpr.Abs (_,_,MaybeType.Known(r),_)   -> Some(r)
  | TreeExpr.App (_,_,MaybeType.Known(r),_) -> Some(r)
  | TreeExpr.Var (_,MaybeType.Known(r),_)   -> Some(r)
  | TreeExpr.Lit (_,r,_)                    -> Some(r)
  | _ -> None

let getGenerics (t:Type) :Set<int> = 
  let rec fn (t:Type) (m:Set<int>) :Set<int> =
    match t with
    | Type.Generic a        -> m |> Set.add a
    | Type.Arrow(l,r)       -> m |> fn l |> fn r
    | Type.Application(l,r) -> m |> fn l |> fn r
    | _                     -> m
  fn t Set.empty

let rec find_func_calls (expr:TreeExpr) : Option<Id*TreeExpr> =
  match expr with
  | TreeExpr.Abs (_,e,_,_) -> find_func_calls e
  | TreeExpr.App (TreeExpr.Var(id,_,_),r,_,_) -> Some(id,r)
  | None

let TemplateExpantion (scope :Eval.CodeGenScope) :Eval.CodeGenScope =
  // the only generics in a rule are present in input/output
  let templates :Map<Id,Type*Set<Id>> = 
    scope.FuncDecls
    |> Map.map (fun k v -> v,getGenerics(v)) 
    |> Map.filter (fun _ (_,m) -> not <| Set.isEmpty m)
    |> Map.map (fun templated_func_name (functype,generics) -> 
        scope.FuncRules |> Map.map (fun _ rules ->
          rules |> List.map (fun rule ->
            let i = rule.Input )))

              


let buildOutputAst (scope :Eval.CodeGenScope) =
  let rec realizeType k (t:Type) : OutputAst =
    match t with
    | Type.TypeId s when s="Int^primitives"   -> NonNamespace.Data(OutputAstType.Int32,k) 
    | Type.TypeId s when s="Float^primitives" -> NonNamespace.Data(OutputAstType.Float,k) 
    | Type.TypeId s -> NonNamespace.Alias (k,s)
    | Type.Arrow _ -> fail "can't (yet) deal with function pointers"
    | Type.Union (id,constr) -> 
      NonNamespace.TaggedUnion (id,constr|>Map.toSeq|>Seq.map(fun (k,v)->VarId k,realizeType k p))
    | Type.Generic _ -> fail "can't deal with generics"
    | Type.Application _ -> fail "can't deal with type-applications"
  let data = scope.DataDecls |> Map.toSeq |> Seq.map (fun (k,v) -> realizeType k v)
  nil
*)

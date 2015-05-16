module TypeInference

open BasicExpression
open ParserMonad
open ConcreteExpressionParserPrelude

type TypeVar = string

type Type =
    | TypeVariable of TypeVar // 'a
    | TypeConstant of string // int
    | TypeAbstraction of Type * Type // s -> 's
    | ConstructedType of Type * Type list  // List 'a
    | Unknown
    with
        override this.ToString() =
            match this with
            | TypeAbstraction(p, v) -> sprintf "%s -> %s" (p.ToString()) (v.ToString())
            | TypeConstant(t) -> sprintf "%s" (t.ToString())
            | ConstructedType(t, fs) -> sprintf "%A<%A>" (t) fs
            | TypeVariable(t) -> sprintf "%s" t
            | Unknown -> sprintf "Unkown"

type TypeEquivalence = 
  {
    Variables   : Set<TypeVar>
    Types       : List<Type>
  }
  with 
    static member Create(v : TypeVar, t : Type) =
      if t = Unknown then
        failwith "Cannot create binding to Unknown"
      {
        Variables = Set.singleton v
        Types     = [t]
      }
    static member (+) (equiv:TypeEquivalence, (v:TypeVar,t:Type)) =
      equiv + TypeEquivalence.Create(v,t)
    static member (+) (t1:TypeEquivalence, t2:TypeEquivalence) =
      let rec variables (t:Type) = 
          seq{
            match t with
            | TypeVariable v -> yield v
            | TypeConstant s -> ()
            | TypeAbstraction(a, b) -> 
              yield! variables a
              yield! variables b
            | ConstructedType(t,args) ->
              yield! variables t
              for a in args do
                yield! variables a
            | Unknown -> ()
          }
      let fv1 = t1.Types |> Seq.map variables |> Seq.concat |> Set.ofSeq
      let fv2 = t2.Types |> Seq.map variables |> Seq.concat |> Set.ofSeq
      let spurious = (fv1 |> Set.intersect t2.Variables) + (fv2 |> Set.intersect t1.Variables)
      if spurious |> Set.isEmpty |> not then
        {
          Variables = t1.Variables + t2.Variables
          Types     = t1.Types @ t2.Types
        }
      else
        failwithf "Error when merging %A with %A: variables %A would generate infinite types." t1 t2 spurious

and TypeContext =
  { BoundVariables  : Map<string, Type>
    VariableCount   : int
    Substitutions   : Map<TypeVar, TypeEquivalence> }
    with 
      static member Empty =
        { BoundVariables= Map.empty
          VariableCount = 0
          Substitutions = Map.empty }
      member this.FreshVariable =
        let freshVariable = "a" + this.VariableCount.ToString()
        let freshVariableType = TypeVariable(freshVariable)
        freshVariable, freshVariableType, { this with VariableCount = this.VariableCount + 1 }
      member this.introduceVariable (var:string) (varType:Type) : Type * TypeContext =
        if not (this.BoundVariables |> Map.containsKey var) then
          if varType = Unknown then 
            let freshVariable, freshVariableType, scheme1 = this.FreshVariable
            let newScheme = { scheme1 with
                                BoundVariables  = scheme1.BoundVariables.Add(var, freshVariableType)
                                Substitutions   = scheme1.Substitutions.Add(freshVariable, TypeEquivalence.Create(freshVariable, freshVariableType)) }
            freshVariableType, newScheme
          else
            let newScheme = { this with
                                BoundVariables  = this.BoundVariables.Add(var, varType) }
            varType, newScheme
        else
            failwithf "cannot introduce an already known variable %s." var
        member this.substitute (v:TypeVar) (t:Type)  =
          match this.Substitutions |> Map.tryFind v with
          | Some(equivalence) ->
            let mutable substitutions = this.Substitutions
            for v in equivalence.Variables do
              substitutions <- substitutions |> Map.add v (substitutions.[v] + (v,t))
            { this with Substitutions = substitutions }
          | None -> 
            { this with Substitutions = this.Substitutions |> Map.add v (TypeEquivalence.Create(v,t)) }

let isSuperType t1 t2 (ctxt:ConcreteExpressionContext) =
  match ctxt.InheritanceRelationships |> Map.tryFind t2 with
  | Some(inherritedClasses) ->
      inherritedClasses |> Set.contains t1
  | None -> false

let rec unify (expected:Type) (given:Type) (scheme:TypeContext) (ctxt:ConcreteExpressionContext) : TypeContext =
  match expected, given with
  | TypeConstant(e1), TypeConstant(g1) when e1 = g1 || (isSuperType e1 g1 ctxt) || (isSuperType g1 e1 ctxt) -> scheme
  | TypeAbstraction(e1, e2), TypeAbstraction(g1, g2) ->
    let firstModifiedScheme = unify e1 g1 scheme ctxt
    let secondModifiedScheme = unify e2 g2 firstModifiedScheme ctxt
    secondModifiedScheme
  | TypeVariable(v1), TypeVariable(v2) ->
      let t1 = scheme.Substitutions.[v1]
      let t2 = scheme.Substitutions.[v2]
      let t12 = t1 + t2
      { scheme with Substitutions = scheme.Substitutions |> Map.add v1 t1 |> Map.add v2 t2 }
  | _, Unknown 
  | Unknown, _ -> scheme
  | TypeVariable(v), t
  | t, TypeVariable(v) ->
      scheme.substitute v t
  | _ ->
    failwithf "cannot unify %A and %A\n" expected given
        
let rec annotateUnknown (expr:BasicExpression<Keyword, Var, Literal, Position, Unit>) = 
  match expr with
  | Application(br, args, di, ()) -> 
    Application(br, [ for a in args -> annotateUnknown a ], di, Unknown)
  | Keyword(k,pos,()) ->
    Keyword(k,pos,Unknown)
  | Imported(importedType, pos, ()) ->
    Imported(importedType, pos, Unknown)
  | Extension(e, pos, ()) ->
    Extension(e, pos, Unknown)

let rec traverse (ctxt:ConcreteExpressionContext) (expr:BasicExpression<Keyword, Var, Literal, Position, Unit>) 
                 (constraints:Type) (scheme:TypeContext) : BasicExpression<Keyword, Var, Literal, Position, Type> * TypeContext =
  match expr with
  | Application(Regular, expr::[], di, ()) -> 
    let expr',scheme' = traverse ctxt expr constraints scheme
    Application(Regular, expr'::[], di, expr'.TypeInformation), scheme'
  | Application(Angle, args, pos, ()) ->
      Application(Angle, args |> List.map annotateUnknown, pos, Unknown), scheme
  | Application(br, (Keyword(DefinedAs,_,()) as k)::left::right::[], pos, ())
  | Application(br, (Keyword(DoubleArrow,_,()) as k)::left::right::[], pos, ()) ->
    let k' = k |> annotateUnknown
    let left', scheme1 = traverse ctxt left Unknown scheme
    let right', scheme2 = traverse ctxt right Unknown scheme1
    Application(br, k'::left'::right'::[], pos, Unknown), unify left'.TypeInformation right'.TypeInformation scheme2 ctxt
  | Application(br, func::args, pos, ()) ->
      let func', scheme1 = traverse ctxt func Unknown scheme
      let rec traverseMap funcType args scheme =
        match funcType, args with
        | t,[] -> t,[],scheme
        | Type.TypeAbstraction(a,b), x::xs ->
          let x',scheme1 = traverse ctxt x a scheme
          let returnType,xs',scheme2 = traverseMap b xs scheme1
          returnType,x'::xs',scheme2
        | _ -> 
          failwithf "Unexpected function arguments %A; expected type was %A" args funcType
      let returnType,args',scheme2 = traverseMap func'.TypeInformation args scheme1
      let scheme3 = unify constraints returnType scheme2 ctxt
      Application(br, func'::args', pos, returnType), scheme3
  | Imported(imported, pos, ()) ->
    let importedType = TypeConstant(imported.typeString)
    Imported(imported, pos, importedType), unify constraints importedType scheme ctxt
  | Keyword(kw, pos, ()) ->
    let kwType,scheme1 = 
      match kw with
      | Custom(name) -> 
        match ctxt.CustomKeywordsMap |> Map.tryFind name with
        | Some(kwDescription) ->
          let mutable kwReturnType = 
            match kwDescription.Type |> Keyword.typeToString with
            | t::[] | _::t::[] -> t |> TypeConstant
            | _ -> failwithf "Unexpected keyword return type %A" kwDescription.Type
          for l in kwDescription.Arguments |> List.rev do
            let lList = [l] |> Keyword.typeToString
            kwReturnType <- TypeAbstraction(lList |> List.head |> TypeConstant, kwReturnType)
          kwReturnType, unify constraints kwReturnType scheme ctxt
        | _ ->
          failwithf "Unknown keyword %A" kw
      | GreaterThan | SmallerThan
      | Equals      | NotEquals
      | SmallerOrEqual ->
        let fV, fVT, scheme1 = scheme.FreshVariable
        TypeAbstraction(fVT, TypeAbstraction(fVT, TypeConstant("bool"))), scheme1
      | _ -> failwithf "Not implemented keyword sort %A" kw
    Keyword(kw, pos, kwType), scheme1
  | Extension({Name = var}, pos, ()) ->
    match scheme.BoundVariables |> Map.tryFind var with
    | Some(varType) ->
      Extension({Name = var}, pos, varType), scheme
    | _ ->
      let varType, scheme1 = scheme.introduceVariable var constraints
      Extension({Name = var}, pos, varType), scheme1
  | _ -> failwithf "Unexpected expression %A" expr

let inferTypes input output clauses ctxt =
  let rec traverseMap (ctxt:ConcreteExpressionContext) (exprs:List<BasicExpression<Keyword, Var, Literal, Position, Unit>>) 
                      (scheme:TypeContext) : List<BasicExpression<Keyword, Var, Literal, Position, Type>> * TypeContext =
    match exprs with
    | e::es ->
        let e', scheme1 = traverse ctxt e Unknown scheme
        let es', scheme2 = traverseMap ctxt es scheme1
        e'::es', scheme2
    | [] -> [], scheme

  let inputTyped, scheme1 = traverse ctxt input Unknown TypeContext.Empty
  let clausesTyped, scheme2 = traverseMap ctxt clauses scheme1
  let outputTyped, finalScheme = traverse ctxt output Unknown scheme2

  do printfn "%A" finalScheme
  do System.Console.ReadLine() |> ignore
    
  inputTyped, outputTyped, clausesTyped

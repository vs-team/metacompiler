module TypeInference

open BasicExpression
open ParserMonad
open TypeDefinition
open ConcreteExpressionParserPrelude

let isSuperType (ctxt:ConcreteExpressionContext) t1 t2 =
  match t1,t2 with
  | TypeDefinition.TypeConstant(t1,_), TypeDefinition.TypeConstant(t2,_) ->
    match ctxt.InheritanceRelationships |> Map.tryFind t2 with
    | Some(inherritedClasses) ->
        inherritedClasses |> Set.contains t1
    | None -> false
  | _ -> false

type TypeEquivalence = 
  {
    Variables   : Set<TypeVar>
    Types       : Set<Type>
  }
  with 
    static member Create(v : TypeVar, t : Type) =
      if t = Unknown then
        failwith "Cannot create binding to Unknown"
      {
        Variables = Set.singleton v
        Types     = Set.singleton t
      }
    static member add (equiv:TypeEquivalence, v:TypeVar,t:Type,ctxt) =
      TypeEquivalence.add(equiv, TypeEquivalence.Create(v,t),ctxt)
    static member add (t1:TypeEquivalence, t2:TypeEquivalence, ctxt) =
      for x in t1.Types do
        for y in t2.Types do
          let (==) a b =
            a = b || isSuperType ctxt x y || isSuperType ctxt y x
          if Type.compatible (==) x y |> not then
            failwithf "Cannot unify %A and %A because types %A and %A are incompatible" t1 t2 x y
      {
        Variables = t1.Variables + t2.Variables
        Types     = t1.Types + t2.Types
      }

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
        member this.substitute (v:TypeVar) (t:Type) ctxt =
          match this.Substitutions |> Map.tryFind v with
          | Some(equivalence) ->
            let mutable substitutions = this.Substitutions
            for v in equivalence.Variables do
              substitutions <- substitutions |> Map.add v (TypeEquivalence.add(substitutions.[v],v,t,ctxt))
            { this with Substitutions = substitutions }
          | None -> 
            { this with Substitutions = this.Substitutions |> Map.add v (TypeEquivalence.Create(v,t)) }

let rec unify (pos:Position) (expected:Type) (given:Type) (scheme:TypeContext) (ctxt:ConcreteExpressionContext) : TypeContext =
  match expected, given with
  | TypeConstant(e1,_), TypeConstant(g1,_) when e1 = g1 -> scheme
  | TypeAbstraction(e1, e2), TypeAbstraction(g1, g2) ->
    let firstModifiedScheme = unify pos e1 g1 scheme ctxt
    let secondModifiedScheme = unify pos e2 g2 firstModifiedScheme ctxt
    secondModifiedScheme
  | TypeVariable(v1), TypeVariable(v2) ->
      let t1 = scheme.Substitutions.[v1]
      let t2 = scheme.Substitutions.[v2]
      let t12 = TypeEquivalence.add(t1,t2,ctxt)
      { scheme with Substitutions = scheme.Substitutions |> Map.add v1 t1 |> Map.add v2 t2 }
  | _, Unknown 
  | Unknown, _ -> scheme
  | TypeVariable(v), t
  | t, TypeVariable(v) ->
      scheme.substitute v t ctxt
  | _, _ when (isSuperType ctxt expected given) -> scheme
  | _ ->
    failwithf "Error at %A: cannot unify %A and %A\n" pos expected given
        
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
  | Application(br, (Keyword(DefinedAs,_,()) as k)::left::right::[], pos, ()) ->
    let k' = k |> annotateUnknown
    let left', scheme1 = traverse ctxt left Unknown scheme
    let right', scheme2 = traverse ctxt right Unknown scheme1
    Application(br, k'::left'::right'::[], pos, Unknown), scheme2
  | Application(br, (Keyword(DoubleArrow,_,()) as k)::left::right::[], pos, ()) ->
    let k' = k |> annotateUnknown
    let left', scheme1 = traverse ctxt left Unknown scheme
    let right', scheme2 = traverse ctxt right Unknown scheme1
    Application(br, k'::left'::right'::[], pos, Unknown), unify pos left'.TypeInformation right'.TypeInformation scheme2 ctxt
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
      let scheme3 = unify pos constraints returnType scheme2 ctxt
      Application(br, func'::args', pos, returnType), scheme3
  | Imported(imported, pos, ()) ->
    let importedType = TypeConstant(imported.typeString, TypeConstantDescriptor.NativeValue)
    Imported(imported, pos, importedType), unify pos constraints importedType scheme ctxt
  | Keyword(kw, pos, ()) ->
    let kwType,scheme1 = 
      match kw with
      | Custom(name) -> 
        match ctxt.CustomKeywordsMap |> Map.tryFind name with
        | Some(kwDescription) ->
          let mutable kwReturnType = 
            match kwDescription.Type |> Keyword.typeToString with
            | t::[] | _::t::[] -> 
              TypeConstant(t, TypeConstantDescriptor.FromName t)
            | _ -> failwithf "Unexpected keyword return type %A" kwDescription.Type
          for arg in kwDescription.Arguments |> List.rev do
            kwReturnType <- TypeAbstraction(arg, kwReturnType)
          kwReturnType, unify pos constraints kwReturnType scheme ctxt
        | _ ->
          failwithf "Unknown keyword %A" kw
      | GreaterThan | SmallerThan
      | Equals      | NotEquals
      | SmallerOrEqual ->
        let fV, fVT, scheme1 = scheme.FreshVariable
        TypeAbstraction(fVT, TypeAbstraction(fVT, TypeConstant("bool", NativeValue))), scheme1
      | _ -> failwithf "Not implemented keyword sort %A" kw
    Keyword(kw, pos, kwType), scheme1
  | Extension({Name = var}, pos, ()) ->
    match scheme.BoundVariables |> Map.tryFind var with
    | Some(varType) ->
      let scheme1 = unify pos constraints varType scheme ctxt
      Extension({Name = var}, pos, varType), scheme1
    | _ ->
      let varType, scheme1 = scheme.introduceVariable var constraints
      Extension({Name = var}, pos, varType), scheme1
  | _ -> failwithf "Unexpected expression %A" expr

let normalize ctxt (substitutions:Map<TypeVar, TypeEquivalence>) =
  let rec visit (v:TypeVar) (closed:Set<TypeVar>) (opened:Set<TypeVar>) (substitutions:Map<TypeVar, TypeEquivalence>) =
    if opened |> Set.contains v then
      failwithf "Circular reference to variable %A while processing %A" v opened
    let opened = opened |> Set.add v
    let equivalence = 
      match substitutions |> Map.tryFind v with
      | Some x -> x
      | _ -> failwithf "Cannot find variable %A" v
    let otherVars = equivalence.Variables
    let mutable vTypeCandidates = equivalence.Types |> Set.toList

    let rec mergeTypes ctxt (t1:Type) (t2:Type) (closed:Set<TypeVar>) (opened:Set<TypeVar>) (substitutions:Map<TypeVar, TypeEquivalence>) =
      match t1,t2 with 
      | Unknown, Unknown -> failwith "Cannot merge Unknown and Unknown types."
      | Unknown, (TypeConstant _ as t)
      | (TypeConstant _ as t), Unknown
      | TypeVariable _, (TypeConstant _ as t)
      | (TypeConstant _ as t), TypeVariable _
      | TypeVariable _, (ConstructedType _ as t)
      | (ConstructedType _ as t), TypeVariable _ ->
        t, closed, opened, substitutions
      | _ ,_ when isSuperType ctxt t1 t2 ->
        let res = t1
        t1, closed, opened, substitutions
      | _ ,_ when isSuperType ctxt t2 t1 ->
        let res = t2
        t2, closed, opened, substitutions
      | _ -> failwithf "Not implemented type merging between %A and %A" t1 t2

    let rec collapseTypes ctxt (ts:List<Type>) (closed:Set<TypeVar>) (opened:Set<TypeVar>) (substitutions:Map<TypeVar, TypeEquivalence>) =
      match ts with
      | [] -> failwith "Cannot collapse empty list!"
      | t::[] -> t, closed, opened, substitutions
      | t1::t2::ts ->
        let t12, closed1, opened1, substitutions1 = mergeTypes ctxt t1 t2 closed opened substitutions
        collapseTypes ctxt (t12::ts) closed1 opened1 substitutions1
    
    let vType, closed1, opened1, substitutions1 = collapseTypes ctxt vTypeCandidates closed opened substitutions
    closed1 |> Set.add v, opened1 |> Set.remove v, substitutions1 |> Map.add v { equivalence with Types = Set.singleton vType }
  and normalize (closed:Set<TypeVar>) (opened:Set<TypeVar>) (unexplored:List<TypeVar>) (substitutions:Map<TypeVar, TypeEquivalence>) =
    match unexplored with
    | [] -> substitutions
    | v::vs ->
      if closed |> Set.contains v |> not then
        let closed1,opened1,substitutions1 = visit v closed opened substitutions
        normalize closed1 opened1 vs substitutions1
      else // just skip v, as it might have been visited within a previous visit to an equivalent variable
        normalize closed opened vs substitutions
  let allVariables = [ for kv in substitutions -> kv.Key]
  normalize Set.empty Set.empty allVariables substitutions
    

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
  let outputTyped, scheme3 = traverse ctxt output Unknown scheme2

  let finalSubstitutions = normalize ctxt scheme3.Substitutions

  let rec lookup ti =
    match ti with
    | Unknown -> Unknown
    | TypeVariable v ->
      match finalSubstitutions |> Map.tryFind v with
      | Some t when t.Types |> Set.isEmpty |> not -> 
        t.Types.MinimumElement
      | _ -> ti
    | TypeConstant _ -> ti
    | ConstructedType(t,args) ->
      ConstructedType(lookup t, [ for a in args -> lookup a])
    | _ -> failwith "Unsupported lookup on type %A" ti

  let rec annotate (expr:BasicExpression<Keyword, Var, Literal, Position, Type>) = 
    match expr with
    | Application(br, args, di, ti) -> 
      Application(br, [ for a in args -> annotate a ], di, ti)
    | Keyword(k,pos,ti) ->
      Keyword(k,pos,ti)
    | Imported(importedType, pos, ti) ->
      Imported(importedType, pos, ti)
    | Extension(e, pos, ti) ->
      Extension(e, pos, lookup ti)

  let inputTypedNormalized, outputTypedNormalized, clausesTypedNormalized = 
    inputTyped |> annotate, outputTyped |> annotate, [ for c in clausesTyped -> c |> annotate ]

//  do printfn "%A\n%A\n\n%A,%A,%A" scheme3 finalSubstitutions inputTypedNormalized outputTypedNormalized clausesTypedNormalized
//  do System.Console.ReadLine() |> ignore
    
  inputTypedNormalized, outputTypedNormalized, clausesTypedNormalized

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
    Variables   : Set<TypeVariableData>
    Types       : Set<Type>
  }
  with 
    static member Create(v : TypeVariableData, t : Type) =
      if t = Unknown then
        failwith "Cannot create binding to Unknown"
      {
        Variables = Set.singleton v
        Types     = Set.singleton t
      }
    static member add (pos:Position, equiv:TypeEquivalence, v:TypeVariableData,t:Type,ctxt) =
      TypeEquivalence.add(pos, equiv, TypeEquivalence.Create(v,t),ctxt)
    static member add (pos:Position, t1:TypeEquivalence, t2:TypeEquivalence, ctxt) =
      for x in t1.Types do
        for y in t2.Types do
          let (==) a b =
            a = b || isSuperType ctxt x y || isSuperType ctxt y x
          if Type.compatible (==) x y |> not then
            failwithf "Error at %A: cannot unify %A and %A because types %A and %A are incompatible" pos t1 t2 x y
      {
        Variables = t1.Variables + t2.Variables
        Types     = t1.Types + t2.Types
      }

and TypeContext =
  { BoundVariables  : Map<string, Type>
    VariableCount   : int
    Substitutions   : Map<TypeVariableData, TypeEquivalence> }
    with 
      static member Empty =
        { BoundVariables= Map.empty
          VariableCount = 0
          Substitutions = Map.empty }
      member this.ForceBind variables =
        let mutable scheme = this
        for v in variables do
          let vType = TypeVariable(v)
          scheme <- { scheme with
                        Substitutions   = scheme.Substitutions.Add(v, TypeEquivalence.Create(v,vType)) }
        scheme        
      member this.FreshVariable =
        let freshVariable = "a" + this.VariableCount.ToString(), TemporaryVariable
        let freshVariableType = TypeVariable(freshVariable)
        freshVariable, freshVariableType, { this with VariableCount = this.VariableCount + 1 }
      member this.introduceGenericVariable() : _ * Type * TypeContext =
        let freshVariable, freshVariableType, scheme1 = this.FreshVariable
        let newScheme = { scheme1 with
                            Substitutions   = scheme1.Substitutions.Add(freshVariable, TypeEquivalence.Create(freshVariable,freshVariableType)) }
        freshVariable, freshVariableType, newScheme
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
        member this.substitute (pos:Position) (v:TypeVariableData) (t:Type) ctxt =
          match this.Substitutions |> Map.tryFind v with
          | Some(equivalence) ->
            let mutable substitutions = this.Substitutions
            for v in equivalence.Variables do
              substitutions <- substitutions |> Map.add v (TypeEquivalence.add(pos,substitutions.[v],v,t,ctxt))
            { this with Substitutions = substitutions }
          | None -> 
            { this with Substitutions = this.Substitutions |> Map.add v (TypeEquivalence.Create(v,t)) }

let rec substituteVariables variableSubs t = 
  match t with
  | TypeVariable v ->
    match variableSubs |> Map.tryFind v with
    | Some fv -> TypeVariable(fv)
    | None -> t
  | TypeConstant(_) -> t
  | ConstructedType(t, args) -> ConstructedType(substituteVariables variableSubs t, [for a in args -> substituteVariables variableSubs a])
  | _ -> failwithf "Unsupported freshening of %A" t

let rec instantiateFresh (scheme:TypeContext) (boundVariables:List<TypeVariableData>) (t:Type) =
  let mutable scheme = scheme
  let mutable subs = Map.empty
  for v in boundVariables do
    let fvName,fv,s' = scheme.introduceGenericVariable()
    scheme <- s'
    subs <- subs |> Map.add v fvName
  let scheme,subs = scheme, subs
  let res = substituteVariables subs t
  res,subs,scheme

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
      let t12 = TypeEquivalence.add(pos,t1,t2,ctxt)
      { scheme with Substitutions = scheme.Substitutions |> Map.add v1 t12 |> Map.add v2 t12 }
  | _, Unknown 
  | Unknown, _ -> scheme
  | TypeVariable(v), (ConstructedType(h,args) as t)
  | (ConstructedType(h,args) as t), TypeVariable(v) ->
    let vSubs = scheme.Substitutions.[v]
    let mutable scheme = scheme
    for vSub in vSubs.Types do
      match vSub with
      | ConstructedType(h1,args1) -> 
        scheme <- unify pos h h1 scheme ctxt
        for a,a1 in Seq.zip args args1 do
          scheme <- unify pos a a1 scheme ctxt
      | _ -> ()
    scheme.substitute pos v t ctxt
  | TypeVariable(v), t
  | t, TypeVariable(v) ->
    scheme.substitute pos v t ctxt
  | _, _ when (isSuperType ctxt expected given) -> scheme
  | ConstructedType(h1,args1), ConstructedType(h2,args2) ->
    let scheme1 = unify pos h1 h2 scheme ctxt
    let mutable scheme2 = scheme1
    for a1,a2 in Seq.zip args1 args2 do
      scheme2 <- unify pos a1 a2 scheme2 ctxt
    scheme2
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

let rec extractLeadingKeyword e =
  match e with
  | Keyword(Custom k, _, _) -> k
  | Application(Implicit, (Keyword(Custom k, _, _)) :: _, pos, _) -> k
  | Application(Implicit, a :: _, pos, _) -> extractLeadingKeyword a
  | _ -> failwithf "Cannot extract leading keyword from %A" e.DebugInformation

let getBaseType depth (scheme:TypeContext) ctxt expr : Type * TypeContext =   
  let rec getBaseType recurseInApplication depth (scheme:TypeContext) ctxt expr : Type * TypeContext = 
    match expr with
    | Application(br, a::args, di, ti) -> 
      if recurseInApplication then
        getBaseType recurseInApplication depth scheme ctxt a
      else
        ti, scheme
    | Keyword((Custom kName),pos,ti) ->
      match ctxt.CustomKeywordsMap |> Map.tryFind kName with
      | Some kwDescription ->
        let t,variableSubs,scheme1 = 
          if depth = 0 then
            kwDescription.BaseType, Map.empty, scheme.ForceBind kwDescription.GenericArguments
          else
            kwDescription.BaseType |> instantiateFresh scheme kwDescription.GenericArguments
        if recurseInApplication then
          t, scheme1
        else
          let mutable tFun = t
          for a in kwDescription.Arguments do
            let a1 = substituteVariables variableSubs a
            tFun <- TypeAbstraction(a1, tFun)
          let scheme2 = unify pos ti tFun scheme1 ctxt
          t, scheme2
      | None -> failwithf "Invalid keyword %A" kName
    | Imported(importedType, pos, ti) ->
      ti, scheme
    | _ -> 
      failwithf "Cannot extract base type from expression %A" expr
  let kName = extractLeadingKeyword expr
  match ctxt.CustomKeywordsMap |> Map.tryFind kName with
  | Some kwDescription ->
    match kwDescription.Kind with
    | Data -> getBaseType false depth scheme ctxt expr
    | Func _ -> getBaseType true depth scheme ctxt expr
  | _ -> getBaseType true depth scheme ctxt expr

let rec traverse depth (ctxt:ConcreteExpressionContext) (expr:BasicExpression<Keyword, Var, Literal, Position, Unit>) 
                 (constraints:Type) (scheme:TypeContext) : BasicExpression<Keyword, Var, Literal, Position, Type> * TypeContext =
  match expr with
  | Application(Regular, expr::[], di, ()) -> 
    let expr',scheme' = traverse depth ctxt expr constraints scheme
    Application(Regular, expr'::[], di, expr'.TypeInformation), scheme'
  | Application(Angle, args, pos, ()) ->
      Application(Angle, args |> List.map annotateUnknown, pos, Unknown), scheme
  | Application(br, (Keyword(DefinedAs,_,()) as k)::left::right::[], pos, ()) ->
    let k' = k |> annotateUnknown
    let right', scheme1 = traverse (depth+1) ctxt right Unknown scheme
    let rightBaseType,scheme2 = getBaseType (depth+1) scheme1 ctxt right'
    let left', scheme3 = traverse (depth+1) ctxt left rightBaseType scheme2
    let scheme4 = unify pos left'.TypeInformation rightBaseType scheme3 ctxt
    Application(br, k'::left'::right'::[], pos, Unknown), scheme4
  | Application(br, (Keyword(DoubleArrow,_,()) as k)::left::right::[], pos, ()) ->
    let k' = k |> annotateUnknown
    let left', scheme1 = traverse (depth+1) ctxt left Unknown scheme
    let right', scheme2 = traverse (depth+1) ctxt right Unknown scheme1
    Application(br, k'::left'::right'::[], pos, Unknown), unify pos left'.TypeInformation right'.TypeInformation scheme2 ctxt
  | Application(br, func::args, pos, ()) ->
      let func', scheme1 = traverse depth ctxt func Unknown scheme
      let rec traverseMap depth funcType args scheme =
        match funcType, args with
        | t,[] -> t,[],scheme
        | Type.TypeAbstraction(a,b), x::xs ->
          let x',scheme1 = traverse depth ctxt x a scheme
          let returnType,xs',scheme2 = traverseMap (depth + 1) b xs scheme1
          returnType,x'::xs',scheme2
        | _ -> 
          failwithf "Unexpected function arguments %A; expected type was %A" args funcType
      let returnType,args',scheme2 = traverseMap (depth+1) func'.TypeInformation args scheme1
      let scheme3 = unify pos constraints returnType scheme2 ctxt
      Application(br, func'::args', pos, returnType), scheme3
  | Imported(imported, pos, ()) ->
    let importedType = imported.typeString
    let importedType = TypeConstant(importedType, TypeConstantDescriptor.FromName importedType)
    Imported(imported, pos, importedType), unify pos constraints importedType scheme ctxt
  | Keyword(kw, pos, ()) ->
    let kwType,scheme1 = 
      match kw with
      | Custom(name) -> 
        match ctxt.CustomKeywordsMap |> Map.tryFind name with
        | Some(kwDescription) ->
            let mutable kwReturnType, variableSubs, scheme2 = 
              match kwDescription.Kind with
              | KeywordKind.Data ->
                if depth = 0 then
                  kwDescription.BaseType, Map.empty, scheme.ForceBind kwDescription.GenericArguments
                else
                  kwDescription.BaseType |> instantiateFresh scheme kwDescription.GenericArguments
              | KeywordKind.Func kwReturnType -> 
                if depth = 0 then
                  kwReturnType, Map.empty, scheme.ForceBind kwDescription.GenericArguments
                else
                  kwReturnType |> instantiateFresh scheme kwDescription.GenericArguments
            for arg in kwDescription.Arguments |> List.rev do
              let arg' = substituteVariables variableSubs arg
              kwReturnType <- TypeAbstraction(arg', kwReturnType)
            kwReturnType, unify pos constraints kwReturnType scheme2 ctxt
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

let rec visit ctxt (v:TypeVariableData) (closed:Set<TypeVariableData>) (opened:Set<TypeVariableData>) (substitutions:Map<TypeVariableData, TypeEquivalence>) =
  if opened |> Set.contains v then
    opened,closed,substitutions
  else
    let opened = opened |> Set.add v
    let equivalence = 
      match substitutions |> Map.tryFind v with
      | Some x -> x
      | _ -> failwithf "Cannot find variable %A" v
    let otherVars = equivalence.Variables

    let rec mergeTypes ctxt (t1:Type) (t2:Type) (closed:Set<TypeVariableData>) (opened:Set<TypeVariableData>) (substitutions:Map<TypeVariableData, TypeEquivalence>) =
      match t1,t2 with 
      | Unknown, Unknown -> failwith "Cannot merge Unknown and Unknown types."
      | TypeConstant(a,_), TypeConstant(b,_) when a = b ->
        t1, closed, opened, substitutions
      | Unknown, (TypeConstant _ as t)
      | (TypeConstant _ as t), Unknown
      | TypeVariable _, (TypeConstant _ as t)
      | (TypeConstant _ as t), TypeVariable _
      | TypeVariable _, (ConstructedType _ as t)
      | (ConstructedType _ as t), TypeVariable _ ->
        t, closed, opened, substitutions
      | TypeVariable(v1,TemporaryVariable), (TypeVariable(v2,TemporaryVariable)) ->
        let closed1, opened1, substitutions1 = visit ctxt (v1,TemporaryVariable) closed opened substitutions
        let closed2, opened2, substitutions2 = visit ctxt (v2,TemporaryVariable) closed1 opened1 substitutions1
        t1, closed2, opened2, substitutions2
      | TypeVariable(_,TemporaryVariable), (TypeVariable(_,GenericParameter) as v)
      | (TypeVariable(_,GenericParameter) as v), TypeVariable(_,TemporaryVariable) ->
        v, closed, opened, substitutions
      | _ ,_ when isSuperType ctxt t1 t2 ->
        let res = t1
        t1, closed, opened, substitutions
      | _ ,_ when isSuperType ctxt t2 t1 ->
        let res = t2
        t2, closed, opened, substitutions
      | ConstructedType(h1,args1), ConstructedType(h2,args2) ->
        let mutable h, closed, opened, substitutions = mergeTypes ctxt h1 h2 closed opened substitutions
        let mutable args = []
        for a1,a2 in Seq.zip args1 args2 do
          let a, closed', opened', substitutions' = mergeTypes ctxt a1 a2 closed opened substitutions
          closed <- closed'
          opened <- opened'
          substitutions <- substitutions'
          args <- a :: args
        ConstructedType(h, args |> List.rev), closed, opened, substitutions
      | _ -> failwithf "Not implemented type merging between %A and %A" t1 t2

    let rec collapseTypes ctxt (ts:List<Type>) (closed:Set<TypeVariableData>) (opened:Set<TypeVariableData>) (substitutions:Map<TypeVariableData, TypeEquivalence>) =
      match ts with
      | [] -> failwith "Cannot collapse empty list!"
      | t::[] -> t, closed, opened, substitutions
      | t1::t2::ts ->
        let t12, closed1, opened1, substitutions1 = mergeTypes ctxt t1 t2 closed opened substitutions
        collapseTypes ctxt (t12::ts) closed1 opened1 substitutions1

    let rec collapseType (t:Type) (closed:Set<TypeVariableData>) (opened:Set<TypeVariableData>) (substitutions:Map<TypeVariableData, TypeEquivalence>) =
      match t with
      | Unknown
      | TypeConstant(_) -> t, closed, opened, substitutions
      | TypeAbstraction(a,b) ->
        let a1, closed1, opened1, substitutions1 = collapseType a closed opened substitutions
        let b1, closed2, opened2, substitutions2 = collapseType b closed1 opened1 substitutions1
        TypeAbstraction(a1,b1), closed2, opened2, substitutions2
      | ConstructedType(h,args) ->
        let mutable h1, closed1, opened1, substitutions1 = collapseType h closed opened substitutions
        let mutable args1 = []
        for a in args do
          let a1, closed', opened', substitutions' = collapseType a closed1 opened1 substitutions1
          closed1 <- closed'
          opened1 <- opened'
          substitutions1 <- substitutions'
          args1 <- a1 :: args1
        ConstructedType(h1, args1 |> List.rev), closed, opened, substitutions
      | TypeVariable((_,TemporaryVariable) as v) ->
        let closed1, opened1, substitutions1 = visit ctxt v closed opened substitutions
        let t1 = substitutions1.[v].Types.MinimumElement
        t1, closed1, opened1, substitutions1
      | TypeVariable((_,GenericParameter) as v) as t ->
        let closed1, opened1, substitutions1 = visit ctxt v closed opened substitutions
        t, closed1, opened1, substitutions1
    
    let mutable vTypeCandidates = equivalence.Types
    let mutable closed, opened, substitutions = closed, opened, substitutions
    for v' in equivalence.Variables do
      if v <> v' then
        let closed',opened',substitutions' = visit ctxt v' closed opened substitutions
        closed <- closed'
        opened <- opened'
        substitutions <- substitutions'
        vTypeCandidates <- vTypeCandidates + substitutions.[v'].Types

    let vTypeCandidates = vTypeCandidates |> Set.toList
    let vType, closed1, opened1, substitutions1 = 
      match vTypeCandidates with
      | [] -> TypeVariable v, closed, opened, substitutions
      | _ ->
        collapseTypes ctxt vTypeCandidates closed opened substitutions

    let vTypeCollapsed, closed2, opened2, substitutions2 = collapseType vType closed1 opened1 substitutions1
    closed2 |> Set.add v, opened2 |> Set.remove v, substitutions2 |> Map.add v { equivalence with Types = Set.singleton vTypeCollapsed }

and normalize ctxt (closed:Set<TypeVariableData>) (opened:Set<TypeVariableData>) (unexplored:List<TypeVariableData>) (substitutions:Map<TypeVariableData, TypeEquivalence>) =
  match unexplored with
  | [] -> substitutions
  | v::vs ->
    if closed |> Set.contains v |> not then
      let closed1,opened1,substitutions1 = visit ctxt v closed opened substitutions
      normalize ctxt closed1 opened1 vs substitutions1
    else // just skip v, as it might have been visited within a previous visit to an equivalent variable
      normalize ctxt closed opened vs substitutions

let normalizeSubstitutions ctxt (substitutions:Map<TypeVariableData, TypeEquivalence>) =
  let allVariables = [ for kv in substitutions -> kv.Key]
  normalize ctxt Set.empty Set.empty allVariables substitutions
    

let inferTypes input output clauses ctxt =
  let rec traverseMap (ctxt:ConcreteExpressionContext) (exprs:List<BasicExpression<Keyword, Var, Literal, Position, Unit>>) 
                      (scheme:TypeContext) : List<BasicExpression<Keyword, Var, Literal, Position, Type>> * TypeContext =
    match exprs with
    | e::es ->
        let e', scheme1 = traverse 0 ctxt e Unknown scheme
        let es', scheme2 = traverseMap ctxt es scheme1
        e'::es', scheme2
    | [] -> [], scheme

  let inputTyped, scheme1 = traverse 0 ctxt input Unknown TypeContext.Empty
  let clausesTyped, scheme2 = traverseMap ctxt clauses scheme1
  let outputTyped, scheme3 = traverse 1 ctxt output inputTyped.TypeInformation scheme2

  let finalSubstitutions = normalizeSubstitutions ctxt scheme3.Substitutions

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
    | TypeAbstraction(a,b) -> 
      TypeAbstraction(lookup a, lookup b)

  let rec annotate (expr:BasicExpression<Keyword, Var, Literal, Position, Type>) = 
    match expr with
    | Application(br, args, di, ti) -> 
      Application(br, [ for a in args -> annotate a ], di, lookup ti)
    | Keyword(k,pos,ti) ->
      Keyword(k,pos,lookup ti)
    | Imported(importedType, pos, ti) ->
      Imported(importedType, pos, lookup ti)
    | Extension(e, pos, ti) ->
      Extension(e, pos, lookup ti)

  let inputTypedNormalized, outputTypedNormalized, clausesTypedNormalized = 
    inputTyped |> annotate, outputTyped |> annotate, [ for c in clausesTyped -> c |> annotate ]

//  do printfn "%A\n\n%A,%A,%A" finalSubstitutions (inputTypedNormalized.ToString()) (outputTypedNormalized.ToString()) [for c in clausesTypedNormalized -> c.ToString()]
//  do System.Console.ReadLine() |> ignore
    
  inputTypedNormalized, outputTypedNormalized, clausesTypedNormalized

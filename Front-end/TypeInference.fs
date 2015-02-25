module TypeInference

open System
open Utilities
open ParserMonad
open BasicExpression
open ConcreteExpressionParser

let mutable cnt = 0

type [<CustomEquality; CustomComparison>] TypeVariable = 
  | Fresh of string
  | LocalGenericParameter of string
  | GenericParameter of string
  | Composite of TypeAnnotation
  with
    member t.CSharpString cleanup =
      match t with
      | Fresh s | LocalGenericParameter s | GenericParameter s -> cleanup s
      | Composite(t) -> t.CSharpString cleanup

    member v.Id =
      match v with
      | Fresh _ -> 0
      | LocalGenericParameter _  -> 1
      | GenericParameter _ -> 2
      | Composite _ -> 3

    override x.Equals(yobj) =
      match yobj with
      | :? TypeVariable as y -> 
          match x,y with
          | Fresh a,Fresh b -> a = b
          | LocalGenericParameter a, LocalGenericParameter b -> a = b
          | GenericParameter a, GenericParameter b -> a = b
          | Composite a, Composite b -> a = b
          | _ -> false
      | _ -> false
 
    override x.GetHashCode() = x.Id
    interface System.IComparable with
      member x.CompareTo yobj =
          match yobj with
          | :? TypeVariable as y -> 
            match x,y with
            | Fresh a,Fresh b -> compare a b
            | LocalGenericParameter a, LocalGenericParameter b -> compare a b
            | GenericParameter a, GenericParameter b -> compare a b
            | Composite a, Composite b -> compare a b
            | _ -> compare x.Id y.Id
          | _ -> failwithf "Cannot compare objects of different types"

and [<CustomEquality; CustomComparison>] TypeAnnotation = 
  | Builtin // ground, primitive, predefined keywords
  | Variable of TypeVariable // maybe ground
  | Native of string | Defined of string // ground types
  | Generic of TypeAnnotation * List<TypeAnnotation> // could be both ground and not ground type
  with 
    member t.CSharpString cleanup =
      match t with
      | Builtin -> ""
      | Variable(v) -> v.CSharpString cleanup
      | Native s | Defined s -> s |> cleanup
      | Generic(gt,args) -> 
        let args = args |> Seq.map (fun a -> a.CSharpString cleanup) |> Seq.reduce (fun s x -> sprintf "%s, %s" s x)
        let ht = gt.CSharpString cleanup
        let res = sprintf "%s<%s>" ht args
        res
      
    member v.Id =
      match v with
      | Builtin -> 0
      | Variable _  -> 1
      | Generic _ -> 2
      | Native _ -> 3
      | Defined _ -> 4

    override x.Equals(yobj) =
      match yobj with
      | :? TypeAnnotation as y -> 
          match x,y with
          | Builtin,Builtin -> true
          | Variable a, Variable b -> a = b
          | Native a, Native b -> a = b
          | Defined a, Defined b -> a = b
          | Generic(a1,a2), Generic(b1,b2) -> (a1,a2) = (b1,b2)
          | _ -> false
      | _ -> false
 
    override x.GetHashCode() = x.Id
    interface System.IComparable with
      member x.CompareTo yobj =
          match yobj with
          | :? TypeAnnotation as y -> 
            match x,y with
            | Builtin,Builtin -> 0
            | Variable a, Variable b -> compare a b
            | Native a, Native b -> compare a b
            | Defined a, Defined b -> compare a b
            | Generic(a1,a2), Generic(b1,b2) -> compare (a1,a2) (b1,b2)
            | _ -> compare x.Id y.Id
          | _ -> failwithf "Cannot compare objects of different types"

    override t.ToString() =
      match t with
      | Builtin -> ""
      | Variable(v) -> v.ToString()
      | Native s | Defined s -> s
      | Generic(gt,args) -> 
        let args = args |> Seq.map (fun a -> a.ToString()) |> Seq.reduce (fun s x -> sprintf "%s, %s" s x)
        sprintf "%s<%s>" (gt.ToString()) args

    static member FromKeywordArgument (ctxt:ConcreteExpressionContext) (typingContext:InferenceContext) (a:KeywordArgument) =
      match a with
      | KeywordArgument.Native k -> Native k
      | KeywordArgument.Defined k -> 
        match ctxt.CustomKeywordsMap |> Map.tryFind k with
        | Some _ -> Defined k
        | None ->
          if ctxt.CustomClasses |> Set.contains k || ctxt.CustomKeywordsMap |> Map.containsKey k then
            Defined k
          else
            match typingContext.GenericParameters |> Seq.tryFind (fun lp -> lp = GenericParameter(k)) with
            | Some lp -> lp |> Variable
            | None ->
              match typingContext.LocalGenericParameters |> Map.tryFind (LocalGenericParameter(k)) with
              | Some _ -> k |> LocalGenericParameter |> Variable
              | None -> failwithf "Unbound defined parameter %A" k
      | KeywordArgument.Generic(t,args) ->
        let gt =
          if ctxt.CustomClasses |> Set.contains t || ctxt.CustomKeywordsMap |> Map.containsKey t then
            Defined t
          else
            Native t
        Generic(gt, args |> List.map (TypeAnnotation.FromKeywordArgument ctxt typingContext))


and TypeEquivalence = { mutable GroundRepresentative : Option<TypeVariable>; mutable LocalGroundRepresentative : Option<TypeVariable>; mutable Members : Set<TypeVariable> }
  with 
    static member Singleton (v:TypeVariable) =
      match v with
      | Fresh s -> { GroundRepresentative = None; LocalGroundRepresentative = None; Members = Set.singleton v }
      | LocalGenericParameter l -> { GroundRepresentative = None; LocalGroundRepresentative = Some v; Members = Set.singleton v }
      | GenericParameter g -> { GroundRepresentative = Some v; LocalGroundRepresentative = None; Members = Set.singleton v }
      | Composite a -> { GroundRepresentative = Some v; LocalGroundRepresentative = None; Members = Set.singleton v }
    static member (+) (eq1:TypeEquivalence, eq2:TypeEquivalence) =
      let groundRepresentative = 
        match eq1.GroundRepresentative, eq2.GroundRepresentative with
        | None, None -> None
        | Some x, None | None, Some x -> Some x
        | Some x, Some y when x = y -> Some x
        | Some x, Some y -> failwithf "Cannot merge ground representatives %A and %A" x y
      let localGroundRepresentative = 
        match eq1.LocalGroundRepresentative, eq2.LocalGroundRepresentative with
        | None, None -> None
        | Some x, None | None, Some x -> Some x
        | Some x, Some y -> Some x
      { GroundRepresentative = groundRepresentative
        LocalGroundRepresentative = localGroundRepresentative
        Members = eq1.Members + eq2.Members }

and InferenceContext = { 
  mutable VarBindings : Map<string, TypeAnnotation>
  mutable LocalGenericParameters : Map<TypeVariable,int>
  mutable GenericParameters : Set<TypeVariable> 
  mutable TypeEquivalence : Map<TypeVariable, TypeEquivalence> 
  }
  with 
    member ctxt.CreateFreshVariable = 
      cnt <- cnt + 1
      let v = sprintf "T%d" cnt |> Fresh 
      match ctxt.TypeEquivalence |> Map.tryFind v with
      | Some eq -> 
        ctxt.TypeEquivalence <- ctxt.TypeEquivalence.Add(v, eq + (TypeEquivalence.Singleton v))
      | None -> ctxt.TypeEquivalence <- ctxt.TypeEquivalence.Add(v, TypeEquivalence.Singleton v)
      v |> Variable
    static member Empty = { VarBindings = Map.empty; LocalGenericParameters = Map.empty; GenericParameters = Set.empty; TypeEquivalence = Map.empty }
    member ctxt.AddLocalGenericParameter (l:TypeVariable) =
      match ctxt.LocalGenericParameters |> Map.tryFind l with
      | Some(pNestingDepth) -> 
        ctxt.LocalGenericParameters <- ctxt.LocalGenericParameters.Add(l, pNestingDepth+1)
      | _ ->
        ctxt.LocalGenericParameters <- ctxt.LocalGenericParameters.Add(l, 1)
    member ctxt.RemoveLocalGenericParameters (toRemove:List<TypeVariable>) =
      () // this is not correct, but in most cases it will work
//      for p in toRemove do
//        let pNestingDepth = ctxt.LocalGenericParameters.[p]
//        if pNestingDepth = 1 then
//          ctxt.LocalGenericParameters <- ctxt.LocalGenericParameters.Remove p
//        else
//          ctxt.LocalGenericParameters <- ctxt.LocalGenericParameters.Add(p, pNestingDepth-1)
//        ctxt.TypeEquivalence <- ctxt.TypeEquivalence.Remove p
    member ctxt.GetEquivalence (v:TypeVariable) =
      match ctxt.TypeEquivalence |> Map.tryFind v with
      | Some eq -> eq
      | None -> TypeEquivalence.Singleton v
    member ctxt.AddEquivalence (eq:TypeEquivalence) =
      for m in eq.Members do
        ctxt.TypeEquivalence <- ctxt.TypeEquivalence.Add(m, eq)
    member ctxt.AddTypeVariableEquivalence (v:TypeVariable) (t:TypeAnnotation) =
      let eq = 
        match ctxt.TypeEquivalence |> Map.tryFind v with
        | Some eq -> eq
        | None -> 
          let eq = TypeEquivalence.Singleton v
          ctxt.TypeEquivalence <- ctxt.TypeEquivalence.Add(v,eq)
          eq
      eq.Members <- eq.Members.Add(Composite t)
      

let unifyTypeVariables (typingContext:InferenceContext) (v1:TypeVariable) (v2:TypeVariable) = 
  let eq1 = typingContext.GetEquivalence v1
  let eq2 = typingContext.GetEquivalence v2
  let eq12 = eq1 + eq2
  typingContext.AddEquivalence eq12

let rec unify (ctxt:ConcreteExpressionContext) (typingContext:InferenceContext) (t1:TypeAnnotation) (t2:TypeAnnotation) =
  match t1, t2 with
  | Builtin, Builtin -> ()
  | Native s1, Native s2 when s1 = s2 -> ()
  | Defined s1, Defined s2 when s1 = s2 -> ()
  | Defined s1, Defined s2 
  | Generic(Defined s1, _), Defined s2
  | Defined s1, Generic(Defined s2, _) -> 
    if ctxt.Inherits s1 s2 ||
       ctxt.Inherits s2 s1 then
      ()
    else
      match ctxt.CustomKeywordsMap |> Map.tryFind s1 with
      | Some k1 when k1.Class.BaseName = s2 -> ()
      | Some k1 when ctxt.Inherits k1.Class.BaseName s2 -> ()
      | _ ->
        match ctxt.CustomKeywordsMap |> Map.tryFind s2 with
        | Some k2 when k2.Class.BaseName = s1 -> ()
        | Some k2 when ctxt.Inherits k2.Class.BaseName s1 -> ()
        | _ ->
          failwithf "Cannot unify types %A and %A" t1 t2
  | Generic(t1, t1_args), Generic(t2, t2_args) -> 
    unify ctxt typingContext t1 t2
    for a,b in Seq.zip t1_args t2_args do
      unify ctxt typingContext a b
  | Variable v1, Variable v2 -> 
    unifyTypeVariables typingContext v1 v2
  | Variable v, other 
  | other, Variable v -> 
    typingContext.AddTypeVariableEquivalence v other
  | _, Defined "CSharpExpr"
  | Defined "CSharpExpr", _ -> ()
  | _ -> failwithf "Cannot unify types %A and %A" t1 t2

and typeCheck (ctxt:ConcreteExpressionContext) (e:BasicExpression<Keyword, Var, Literal, Position, Unit>) (typingContext:InferenceContext) =
  match e with
  | Imported(i,pos,()) ->
    match i with
    | StringLiteral l -> Native "string"
    | IntLiteral i -> Native "int"
    | BoolLiteral b -> Native "bool"
    | SingleLiteral s -> Native "float"
    | DoubleLiteral d -> Native "double"
  | Application(Angle, _, _, ()) ->
    typingContext.CreateFreshVariable
  | Application(_,Extension({ Name = var }, _, ()) :: [], _, _) 
  | Extension({ Name = var }, _, ()) -> 
    match typingContext.VarBindings |> Map.tryFind var with
    | Some t -> t
    | None -> 
      let t = typingContext.CreateFreshVariable
      typingContext.VarBindings <- typingContext.VarBindings.Add(var, t)
      t
  | Keyword(Custom k, _, _) ->
    let kClass = ctxt.CustomKeywordsMap.[k]
    if kClass.IsGeneric then
      let addedLocalGenericParameters = 
        [ for gA in kClass.GenericArguments do
            let lGA = LocalGenericParameter(gA.Argument)
            typingContext.AddLocalGenericParameter lGA
            yield lGA ]
      let res = TypeAnnotation.Generic(TypeAnnotation.Defined kClass.Name, addedLocalGenericParameters |> List.map Variable)
      do typingContext.RemoveLocalGenericParameters addedLocalGenericParameters
      res
    else
      TypeAnnotation.FromKeywordArgument ctxt typingContext (KeywordArgument.Defined kClass.Name)
  | Application(_, Keyword(Custom k, _, _) :: args, pos, ()) ->
    let kClass = ctxt.CustomKeywordsMap.[k]
    if kClass.IsGeneric then
      let addedLocalGenericParameters = 
        [ for gA in kClass.GenericArguments do
            let lGA = LocalGenericParameter gA.Argument
            typingContext.AddLocalGenericParameter lGA
            yield lGA ]
      for a,aArgument in Seq.zip args kClass.Arguments do
        let aType = typeCheck ctxt a typingContext
        let expectedAType = TypeAnnotation.FromKeywordArgument ctxt typingContext aArgument
        unify ctxt typingContext aType expectedAType
      do typingContext.RemoveLocalGenericParameters addedLocalGenericParameters
      TypeAnnotation.Generic(TypeAnnotation.Defined kClass.Name, [ for a in addedLocalGenericParameters -> Variable a ])
    else
      for a,aArgument in Seq.zip args kClass.Arguments do
        let aType = typeCheck ctxt a typingContext
        let expectedAType = TypeAnnotation.FromKeywordArgument ctxt typingContext aArgument
        unify ctxt typingContext aType expectedAType
      TypeAnnotation.FromKeywordArgument ctxt typingContext (KeywordArgument.Defined kClass.Name)
  | _ -> failwithf "Unexpected expression %A" e
  
and addToContext (ctxt:ConcreteExpressionContext) (e:BasicExpression<Keyword, Var, Literal, Position, Unit>) (typingContext:InferenceContext) =
  match e with
  | Keyword(Custom k, _, _) -> 
    ()
  | Application(Angle, _, _, ()) ->
    ()
  | Application(_,Extension({ Name = var }, _, ()) :: [], _, _) 
  | Extension({ Name = var }, _, ()) -> 
    if typingContext.VarBindings.ContainsKey var |> not then
      typingContext.VarBindings <- typingContext.VarBindings |> Map.add var typingContext.CreateFreshVariable
  | Application(_, (Keyword(Custom k, _, _)) :: args, pos, _) ->
    let inputClass = ctxt.CustomKeywordsMap.[k]
    if inputClass.IsGeneric then
      for genericParameter in inputClass.GenericArguments do
        typingContext.GenericParameters <- typingContext.GenericParameters.Add (GenericParameter genericParameter.Argument)
    for a, aType in Seq.zip args inputClass.Arguments do
      addToContextConstrained ctxt a (TypeAnnotation.FromKeywordArgument ctxt typingContext aType) typingContext
  | Application(b, Keyword(Equals, _, ()) :: c_i :: c_o :: [], clausePos, ()) ->
    let clauseInputType = typeCheck ctxt c_i typingContext
    let clauseOutputType = typeCheck ctxt c_o typingContext
    unify ctxt typingContext clauseInputType clauseOutputType
  | _ -> failwithf "Cannot extract input keyword from %A" e

and addToContextConstrained (ctxt:ConcreteExpressionContext) (e:BasicExpression<Keyword, Var, Literal, Position, Unit>) 
                            (expectedType:TypeAnnotation) (typingContext:InferenceContext) =
  let foundType = typeCheck ctxt e typingContext
  do unify ctxt typingContext foundType expectedType

and createContext (ctxt:ConcreteExpressionContext) (e:BasicExpression<Keyword, Var, Literal, Position, Unit>) =
  let typingContext = InferenceContext.Empty
  do addToContext ctxt e typingContext
  typingContext


let rec rebuildTypedConstrained (ctxt:ConcreteExpressionContext) (e:BasicExpression<Keyword, Var, Literal, Position, Unit>) 
                            (expectedType:TypeAnnotation) (typingContext:InferenceContext) =
  let (rebuiltE:BasicExpression<_,_,_,_,TypeAnnotation>) = rebuildTypedLocal ctxt e typingContext
  let foundType = rebuiltE.TypeInformation
  do unify ctxt typingContext foundType expectedType
  rebuiltE

and rebuildTypedLocal (ctxt:ConcreteExpressionContext) (e:BasicExpression<Keyword, Var, Literal, Position, Unit>) (typingContext:InferenceContext) =
  match e with
  | Imported(i,pos,()) ->
    let iType = typeCheck ctxt e typingContext |> findMostSpecificMatch  ctxt typingContext
    Imported(i,pos,iType)
  | Extension({ Name = var }, pos, ()) -> 
    let t = typeCheck ctxt e typingContext |> findMostSpecificMatch  ctxt typingContext
    Extension({ Name = var }, pos, t)
  | Application(Angle, args, pos, ()) ->
    let typedArgs = 
      [ for a in args do
          yield rebuildTypedLocal ConcreteExpressionContext.CSharp a typingContext ]
    Application(Angle, typedArgs, pos, typingContext.CreateFreshVariable)
  | Application(b,Extension({ Name = var }, extPos, ()) :: [], appPos, ()) ->
    let t = typeCheck ctxt e typingContext |> findMostSpecificMatch  ctxt typingContext
    Application(b,Extension({ Name = var }, extPos, t) :: [], appPos, t)
  | Keyword(Custom k, kPos, ()) ->
    let kType = typeCheck ctxt e typingContext |> findMostSpecificMatch  ctxt typingContext
    Keyword(Custom k, kPos, kType)
  | Application(b, (Keyword(Custom k, kPos, ())) :: args, pos, ()) ->
    let kClass = ctxt.CustomKeywordsMap.[k]
    let kTyped = rebuildTypedLocal ctxt (Keyword(Custom k, kPos, ())) typingContext
    let addedLocalGenericParameters = 
      if kClass.IsGeneric then
        [ for genericParameter in kClass.GenericArguments do
            let lGA = genericParameter.Argument |> LocalGenericParameter
            typingContext.AddLocalGenericParameter lGA
            yield lGA ]
      else
        []
    let typedArgs = 
      [ for a, aType in Seq.zip args kClass.Arguments do
          yield rebuildTypedConstrained ctxt a (TypeAnnotation.FromKeywordArgument ctxt typingContext aType) typingContext ]
    typingContext.RemoveLocalGenericParameters addedLocalGenericParameters
    let eType =
      if kClass.IsGeneric then
        let argTypes = [ for typedArg in typedArgs -> typedArg.TypeInformation ]
        let actualArguments = [ for g in kClass.GenericArguments -> findInParameters g kClass.Arguments argTypes ]
        TypeAnnotation.Generic(TypeAnnotation.Defined kClass.Name, actualArguments)
      else
        TypeAnnotation.FromKeywordArgument ctxt typingContext (KeywordArgument.Defined kClass.Name)
    Application(b, kTyped :: typedArgs, pos, eType |> findMostSpecificMatch  ctxt typingContext)
  | _ -> failwithf "Cannot extract input keyword from %A" e

and findInParameters (genericArgument:KeywordArgument) (args:List<KeywordArgument>) (argTypes:List<TypeAnnotation>) =
  let argIndex =
    args |> List.findIndex (fun arg -> arg.Contains genericArgument)
  let arg,argType = args.[argIndex], argTypes.[argIndex]
  match arg with
  | KeywordArgument.Defined a -> argType
  | KeywordArgument.Generic(s,args) ->
    match argType with
    | Generic(sT, argsTypes) ->
      findInParameters genericArgument args argTypes
    | _ ->
      failwithf "Cannot extract generic argument instance %A in %A" genericArgument argTypes
  | _ -> failwithf "Cannot extract generic argument instance %A in %A" genericArgument argTypes

and findMostSpecificMatch (ctxt:ConcreteExpressionContext) (typingContext:InferenceContext) (t:TypeAnnotation) =
  let res = findMostSpecificMatch' ctxt typingContext t
  res

and findMostSpecificMatch' (ctxt:ConcreteExpressionContext) (typingContext:InferenceContext) (t:TypeAnnotation) =
  match t with
  | Builtin -> t
  | Defined _ -> t
  | Native _ -> t
  | Variable v ->
    match typingContext.TypeEquivalence |> Map.tryFind v with
    | Some vEq ->
      let bestRepresentative = vEq.Members |> Seq.max
      if bestRepresentative = v |> not then
        findMostSpecificVariable ctxt typingContext bestRepresentative
      else
        Variable bestRepresentative
    | None ->
      t
  | Generic(t, args) ->
    let specifiedArgs = [ for a in args -> a  |> findMostSpecificMatch' ctxt typingContext ]
    let res = Generic(t |> findMostSpecificMatch' ctxt typingContext, specifiedArgs)
    res

and findMostSpecificVariable (ctxt:ConcreteExpressionContext) (typingContext:InferenceContext) (v:TypeVariable) =
  match v with
  | Fresh _ -> Variable v |> findMostSpecificMatch' ctxt typingContext
  | LocalGenericParameter _ -> Variable v |> findMostSpecificMatch' ctxt typingContext
  | GenericParameter _ -> Variable v |> findMostSpecificMatch' ctxt typingContext
  | Composite t -> 
    findMostSpecificMatch' ctxt typingContext t

let rec rebuildTypedCSharp (ctxt:ConcreteExpressionContext) (e:BasicExpression<Keyword, Var, Literal, Position, Unit>) (typingContext:InferenceContext) =
  match e with
  | Imported(i,pos,()) ->
    let iType = typeCheck ctxt e typingContext |> findMostSpecificMatch  ctxt typingContext
    Imported(i,pos,iType)
  | Application(Angle, args, pos, ()) ->
    let typedArgs = 
      [ for a in args do
          yield rebuildTypedCSharp ConcreteExpressionContext.CSharp a typingContext ]
    Application(Angle, typedArgs, pos, Builtin)
  | Application(b,Extension({ Name = var }, extPos, ()) :: [], appPos, ()) ->
    Application(b,Extension({ Name = var }, extPos, Builtin) :: [], appPos, Builtin)
  | Extension({ Name = var }, pos, ()) -> 
    Extension({ Name = var }, pos, Builtin)
  | Keyword(Custom k, kPos, ()) ->
    Keyword(Custom k, kPos, Builtin)
  | Application(b, (Keyword(Custom k, kPos, ())) :: args, pos, ()) ->
    let kClass = ctxt.CustomKeywordsMap.[k]
    let kTyped = rebuildTypedCSharp ctxt (Keyword(Custom k, kPos, ())) typingContext
    let typedArgs = 
      [ for a, aType in Seq.zip args kClass.Arguments do
          yield rebuildTypedCSharp ctxt a typingContext ]
    let eType = Builtin
    Application(b, kTyped :: typedArgs, pos, eType |> findMostSpecificMatch  ctxt typingContext)
  | Application(Regular, Imported(i,iPos,())::[], pos, ()) ->
    Application(Regular, Imported(i,iPos,Builtin)::[], pos, Builtin)
  | _ -> failwithf "Cannot extract input keyword from %A" e

let rec rebuildTyped (ctxt:ConcreteExpressionContext) (e:BasicExpression<Keyword, Var, Literal, Position, Unit>) (typingContext:InferenceContext) =
  match e with
  | Imported(i,pos,()) ->
    let iType = typeCheck ctxt e typingContext |> findMostSpecificMatch  ctxt typingContext
    Imported(i,pos,iType)
  | Application(Angle, args, pos, ()) ->
    let typedArgs = 
      [ for a in args do
          yield rebuildTypedCSharp ConcreteExpressionContext.CSharp a typingContext ]
    Application(Angle, typedArgs, pos, typingContext.CreateFreshVariable)
  | Application(b,Extension({ Name = var }, extPos, ()) :: [], appPos, ()) ->
    let t = typeCheck ctxt e typingContext |> findMostSpecificMatch  ctxt typingContext
    Application(b,Extension({ Name = var }, extPos, t) :: [], appPos, t)
  | Extension({ Name = var }, pos, ()) -> 
    let t = typeCheck ctxt e typingContext |> findMostSpecificMatch  ctxt typingContext
    Extension({ Name = var }, pos, t)
  | Keyword(Custom k, kPos, ()) ->
    let kType = typeCheck ctxt e typingContext |> findMostSpecificMatch  ctxt typingContext
    Keyword(Custom k, kPos, kType)
  | Application(b, (Keyword(Custom k, kPos, ())) :: args, pos, ()) ->
    let kClass = ctxt.CustomKeywordsMap.[k]
    let kTyped = rebuildTyped ctxt (Keyword(Custom k, kPos, ())) typingContext
    if kClass.IsGeneric then
      for genericParameter in kClass.GenericArguments do
        typingContext.GenericParameters <- typingContext.GenericParameters.Add (GenericParameter genericParameter.Argument)
    let typedArgs = 
      [ for a, aType in Seq.zip args kClass.Arguments do
          yield rebuildTypedConstrained ctxt a (TypeAnnotation.FromKeywordArgument ctxt typingContext aType) typingContext ]
    let eType =
      if kClass.IsGeneric then
        let argTypes = [ for typedArg in typedArgs -> typedArg.TypeInformation ]
        let actualArguments = [ for g in kClass.GenericArguments -> findInParameters g kClass.Arguments argTypes ]
        TypeAnnotation.Generic(TypeAnnotation.Defined kClass.Name, actualArguments)
      else
        TypeAnnotation.FromKeywordArgument ctxt typingContext (KeywordArgument.Defined kClass.Name)
    Application(b, kTyped :: typedArgs, pos, eType |> findMostSpecificMatch  ctxt typingContext)
  | Application(Regular, Imported(i,iPos,())::[], pos, ()) ->
    Application(Regular, Imported(i,iPos,Builtin)::[], pos, Builtin)
  | _ -> failwithf "Cannot extract input keyword from %A" e


let inferTypeAnnotations (input:BasicExpression<Keyword, Var, Literal, Position, Unit>)
                         (output:BasicExpression<Keyword, Var, Literal, Position, Unit>)
                         (clauses:List<BasicExpression<Keyword, Var, Literal, Position, Unit>>)
                         (ctxt:ConcreteExpressionContext) =
 (*
   create initial context from input expression
   for each clause, expand the context with the clause input and then the clause output
   create final context from output expression
   use the context to rebuild input, output, and clauses with type annotations;
     if variables are still found, then fail with type error
  *)
  let typingContext = createContext ctxt input
  for clause in clauses do
    match clause with
    | Application(_, Keyword(Inlined, _, _) :: _, clausePos, _) -> ()
    | Application(_, Keyword(DoubleArrow, _, _) :: c_i :: c_o :: [], clausePos, _) -> 
      do addToContext ctxt c_i typingContext
      do addToContext ctxt c_o typingContext
    | Application(b, Keyword(Equals, _, _) :: c_i :: c_o :: [], clausePos, _) 
    | Application(b, Keyword(NotEquals, _, _) :: c_i :: c_o :: [], clausePos, _) 
    | Application(b, Keyword(DefinedAs, _, _) :: c_i :: c_o :: [], clausePos, _) 
    | Application(b, Keyword(GreaterThan, _, _) :: c_i :: c_o :: [], clausePos, _) 
    | Application(b, Keyword(GreaterOrEqual, _, _) :: c_i :: c_o :: [], clausePos, _) 
    | Application(b, Keyword(SmallerThan, _, _) :: c_i :: c_o :: [], clausePos, _) 
    | Application(b, Keyword(SmallerOrEqual, _, _) :: c_i :: c_o :: [], clausePos, _) -> 
      do addToContext ctxt (Application(b, Keyword(Equals, clausePos, ()) :: c_i :: c_o :: [], clausePos, ())) typingContext
    | _ -> failwithf "Cannot infer types of clause %A" clause
  do addToContext ctxt output typingContext
  let inputTyped = rebuildTyped ctxt input typingContext
  let clausesTyped = 
    [
      for clause in clauses do
        match clause with
        | Application(b, Keyword(Inlined, kPos, _) :: inlineCode, clausePos, _) ->
          let inlineCodeTyped = [ for inl in inlineCode -> rebuildTyped ctxt inl typingContext ]
          yield Application(b, Keyword(Inlined, kPos, Builtin) :: inlineCodeTyped, clausePos, Builtin)
        | Application(b, Keyword(DoubleArrow, kPos, _) :: c_i :: c_o :: [], clausePos, _) -> 
          let c_iTyped = rebuildTyped ctxt c_i typingContext
          let c_oTyped = rebuildTyped ctxt c_o typingContext
          yield Application(b, Keyword(DoubleArrow, kPos, Builtin) :: c_iTyped :: c_oTyped :: [], clausePos, Builtin)
        | Application(b, Keyword(Equals as k, kPos, _) :: c_i :: c_o :: [], clausePos, _) 
        | Application(b, Keyword(NotEquals as k, kPos, _) :: c_i :: c_o :: [], clausePos, _) 
        | Application(b, Keyword(DefinedAs as k, kPos, _) :: c_i :: c_o :: [], clausePos, _) 
        | Application(b, Keyword(GreaterThan as k, kPos, _) :: c_i :: c_o :: [], clausePos, _) 
        | Application(b, Keyword(GreaterOrEqual as k, kPos, _) :: c_i :: c_o :: [], clausePos, _) 
        | Application(b, Keyword(SmallerThan as k, kPos, _) :: c_i :: c_o :: [], clausePos, _) 
        | Application(b, Keyword(SmallerOrEqual as k, kPos, _) :: c_i :: c_o :: [], clausePos, _) -> 
          let c_iTyped = rebuildTyped ctxt c_i typingContext
          let c_oTyped = rebuildTyped ctxt c_o typingContext
          yield Application(b, Keyword(k, kPos, Builtin) :: c_iTyped :: c_oTyped :: [], clausePos, Builtin)
        | _ -> ()
    ]
  let outputTyped = rebuildTyped ctxt output typingContext
//  let _ = debug_log (inputTyped.ToString(), outputTyped.ToString(), [ for c in clausesTyped -> c.ToString() ]) //, typingContext)
//  let _ = Console.ReadLine()
//  do printfn "\n\n\n\n\n\n\n\n\n"
  inputTyped, outputTyped, clausesTyped


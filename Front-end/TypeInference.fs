module TypeInference

open BasicExpression
open ParserMonad
open ConcreteExpressionParserPrelude

let (|ExtractedKeyword|) kw =
    (Keyword.Arguments kw), (Keyword.Type kw)

let (|Native|Defined|Generic|) kw =
    match kw with
    | Application(Generic, _, _, _) -> Generic(Keyword.Name kw)
    | _ ->
        match Keyword.IsNative kw with
        | true -> Native(Keyword.Name kw)
        | false -> Defined(Keyword.Name kw)


type Type =
    | TypeVariable of string // 'a
    | TypeConstant of string // int
    | TypeApplication of Type * Type // s -> 's
    | ConstructedType of Type * Type list  // List 'a
    | Unknown

    with

        override this.ToString() =
            match this with
            | TypeApplication(p, v) -> sprintf "%s -> %s" (p.ToString()) (v.ToString())
            | TypeConstant(t) -> sprintf "%s" (t.ToString())
            | ConstructedType(t, fs) -> sprintf "%A<%A>" (t) fs
            | TypeVariable(t) -> sprintf "%s" t
            | Unknown -> sprintf "Unkown"


type TypeScheme = { BoundVariables : Map<string, Type>
                    VariableCount : int
                    Substitutions : Map<Type, Type> }

    with 
        
        static member Empty =
            { BoundVariables= Map.empty;
              VariableCount = 0 ;
              Substitutions = Map.empty }

        member this.introduceVariable (var:string) (varType:Type) : Type * TypeScheme =
            if not (this.BoundVariables |> Map.containsKey var) then
                let constructedType = (TypeVariable("a" + this.VariableCount.ToString()))
                let varType = if varType = Unknown then constructedType else varType
                let newScheme = { BoundVariables= this.BoundVariables.Add(var, constructedType);
                                    VariableCount = this.VariableCount + 1 ;
                                    Substitutions = this.Substitutions.Add(constructedType, varType) }
                constructedType, newScheme
            else
                failwithf "cannot introduce an already known variable %s." var

        member this.lookupSubstituion (variable:Type) : Type * TypeScheme =
            match this.Substitutions |> Map.tryFind variable with
            | Some(substition) ->
                substition, this
            | None -> failwithf "unknown substituion for type %A" variable

        member this.substitute (variable:Type) (substitution:Type)  =
            match this.Substitutions |> Map.tryFind variable with
            | Some(variableType) ->
                let newScheme =
                    {   BoundVariables= this.BoundVariables ;
                        VariableCount = this.VariableCount ;
                        Substitutions = this.Substitutions.Add(variable, substitution) }
                substitution, newScheme
            | None -> failwithf "Undefined substituion %A" variable 

        member this.lookup (variable:string) : Type =
            match this.BoundVariables |> Map.tryFind variable with
            | Some(variableType) ->
                let rec lookup var =
                    match this.Substitutions |> Map.tryFind var, var with
                    | Some(TypeVariable(sub)), TypeVariable(var) when sub = var -> Unknown
                    | Some(substituion), _ when this.Substitutions |> Map.containsKey substituion -> 
                        lookup substituion
                    | Some(substituion), _ -> substituion
                    | _, _ -> failwithf "Undefined substituion for %s" variable
                lookup variableType
            | None -> failwithf "Undefined variable %s" variable

let isSuperType t1 t2 (ctxt:ConcreteExpressionContext) =
    match ctxt.InheritanceRelationships |> Map.tryFind t2 with
    | Some(inherritedClasses) ->
        inherritedClasses |> Set.contains t1
    | None -> false

let rec unify (expected:Type) (given:Type) (scheme:TypeScheme) (ctxt:ConcreteExpressionContext) =
    match expected, given with
    | TypeConstant(e1), TypeConstant(g1) when e1 = g1 || (isSuperType e1 g1 ctxt) || (isSuperType g1 e1 ctxt) -> given, scheme
    | TypeApplication(e1, e2), TypeApplication(g1, g2) ->
        let firstUnifiedParameter, firstModifiedScheme = (unify e1 g1 scheme ctxt)
        let secondUnifiedParameter, secondModifiedScheme = (unify e2 g2 firstModifiedScheme ctxt)
        TypeApplication(firstUnifiedParameter, secondUnifiedParameter), secondModifiedScheme
    | TypeVariable(v1), TypeVariable(v2) ->
        let ignoredResult, firstModifiedScheme = scheme.substitute expected given
        let ignoredResult, secondModifiedScheme = firstModifiedScheme.substitute given expected
        expected, secondModifiedScheme
    | TypeVariable(_), _ ->
        scheme.substitute expected given
    | _, TypeVariable(_) ->
        scheme.substitute given expected
    | expected, Unknown -> expected, scheme
    | Unknown, given -> given, scheme
    | _ ->
        failwithf "cannot unify %s = %s\n" (expected.ToString()) (given.ToString())
        given, scheme

let keywordType kw ctxt =
    match kw with
    | Custom(name) ->
        match ctxt.CustomKeywordsMap |> Map.tryFind name with
        | Some(ExtractedKeyword(arguments, definedType)) ->
            definedType
        | None -> failwithf "Undefined keyword %s\n" name
    | _ -> failwithf "keyword %A" kw
        
let expandKeyword kw ctxt =
    match kw with
    | Custom(name) ->
        match ctxt.CustomKeywordsMap |> Map.tryFind name with
        | Some(ExtractedKeyword(arguments, definedType)) ->
            let extract arg = 
                match arg with
                | Generic(name) -> failwith "Unsupported"
                | Defined(name) | Native(name) -> TypeConstant(name)
            let rec foldArguments args =
                match args with
                | [] -> failwithf "undefined keyword %A" kw
                | arg::[] -> extract arg
                | arg::args ->
                    TypeApplication(extract arg, foldArguments args)
            foldArguments arguments
        | None -> failwithf "Undefined keyword %s\n" name
    | _ -> failwithf "keyword %A" kw

let rec traverse (ctxt:ConcreteExpressionContext) (expr:BasicExpression<Keyword, Var, Literal, Position, Unit>) (constraints:Type) (scheme:TypeScheme) : Type * TypeScheme =
    match expr with
    | Application(Regular, expr::[], _, _) -> traverse ctxt expr constraints scheme
    | Application(Angle, _, pos, _) ->
        Unknown, scheme
    | Application(_, Keyword(GreaterThan, _, _)::left::right::[], pos, _)      
    | Application(_, Keyword(SmallerThan, _, _)::left::right::[], pos, _)      
    | Application(_, Keyword(Equals, _, _)::left::right::[], pos, _)        
    | Application(_, Keyword(NotEquals, _, _)::left::right::[], pos, _)        
    | Application(_, Keyword(DoubleArrow,_,_)::left::right::[], pos, _)
    | Application(_, Keyword(DefinedAs,_,_)::left::right::[], pos, _) ->
        let leftConstraints, firstModifiedScheme = traverse ctxt left Unknown scheme
        let rightConstraints, secondModifiedScheme = traverse ctxt right Unknown firstModifiedScheme
        unify leftConstraints rightConstraints secondModifiedScheme ctxt
    | Application(_, func::keywordArguments, _, _) ->
        match func with
        | Keyword(kw, _, _) ->
            let keywordParameters = expandKeyword kw ctxt
            let rec iterateOverArguments parameter argument scheme =
                match parameter, argument with
                | TypeApplication(t1, t2), argument::arguments ->
                    let localConstraints, newScheme = traverse ctxt argument t1 scheme
                    iterateOverArguments t2 arguments newScheme
                | parameter, argument::[] ->
                    traverse ctxt argument parameter scheme
                | parameter, argument::arguments ->
                    let localConstraints, newScheme = traverse ctxt argument Unknown scheme
                    unify localConstraints parameter newScheme ctxt
                | _, _ -> failwithf "undefined behaviour"
            let ignoredConstraints, populatedScheme = iterateOverArguments keywordParameters keywordArguments scheme
            (TypeConstant(keywordType kw ctxt)), populatedScheme
        | _ -> failwithf "Malformed application of %A" expr
    | Keyword(kw, _, _) ->
        unify constraints (TypeConstant(keywordType kw ctxt)) scheme ctxt
    | Imported(importedType, _, _) ->
        unify constraints (TypeConstant(importedType.typeString)) scheme ctxt
    | Extension({Name = var}, _, _) ->
        match scheme.BoundVariables |> Map.tryFind var, constraints with
        | Some(variableType), Unknown ->
            scheme.lookupSubstituion variableType
        | Some(variableType), _ ->
            let substitution, ignoredScheme = scheme.lookupSubstituion variableType
            unify constraints substitution scheme ctxt
        | _, _ ->
            scheme.introduceVariable var constraints
    | _ -> failwithf "Unexpected expression %A" expr

let rec traverseFully (ctxt:ConcreteExpressionContext) (expr:BasicExpression<Keyword, Var, Literal, Position, Unit> list) : TypeScheme =
    let rec traverseList expr scheme =
        match expr with
        | expr::exprs ->
            let e, scheme = traverse ctxt expr Unknown scheme
            traverseList exprs scheme
        | [] ->
            scheme
    traverseList expr TypeScheme.Empty

let rec annotate (scheme:TypeScheme) (ctxt:ConcreteExpressionContext) (expr:BasicExpression<Keyword, Var, Literal, Position, Unit>) =
    match expr with
    | Application(bracket, exprs, pos, _) ->
        Application(bracket, exprs |> List.map (annotate scheme ctxt), pos, Unknown)
    | Extension({Name = name}, pos, _) ->
        let extensionType = scheme.lookup name
        Extension({Name = name}, pos, extensionType)
    | Keyword(kw, pos, _) ->
        Keyword(kw, pos, Unknown)
    | Imported(t, pos, _) ->
        Imported(t, pos, TypeConstant(t.typeString))
    | _ -> failwithf "Undefined state, given %A" expr
        

let rec annotateAll (scheme:TypeScheme) (ctxt:ConcreteExpressionContext) (expr:BasicExpression<Keyword, Var, Literal, Position, Unit> list) =
    let rec annotateOne expr =
        match expr with
        | expr::exprs ->
            (annotate scheme ctxt expr) :: (annotateOne exprs)
        | [] -> []
    annotateOne expr


let inferTypes i o c ctxt =
    let scheme1 = traverseFully ctxt c
    let inType, scheme2 = traverse ctxt i Unknown scheme1
    let outType, populatedScheme = traverse ctxt o Unknown scheme2
    
    let annotatedOut = annotate populatedScheme ctxt o
    let annotatedIn = annotate populatedScheme ctxt i
    let annotatedClauses = annotateAll populatedScheme ctxt c
    
    annotatedIn, annotatedOut, annotatedClauses

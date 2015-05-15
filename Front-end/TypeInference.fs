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

let (|ExtractedKeyword|) (parsedKeyword) : Type * Type =
        let rec fold args =
            match args with
            | arg::[] ->
                TypeConstant(arg)
            | arg::args ->
                TypeAbstraction(TypeConstant(arg), fold args)
            | [] ->
                Unknown
        fold ((parsedKeyword.LeftArguments @ parsedKeyword.RightArguments) |> Keyword.typeToString), fold (Keyword.typeToString parsedKeyword.Type)

let (|DefinedKeyword|) ctxt kw : Type * Type =
    match kw with
    | Custom(name) ->
        match ctxt.CustomKeywordsMap |> Map.tryFind name with
        | Some(ExtractedKeyword(arguments, definedType)) ->
            arguments, definedType
        | None -> failwithf "Undefined keyword %s\n" name
    | _ -> failwithf "keyword %A" kw

type TypeContext ={ BoundVariables : Map<string, TypeVar>
                    VariableCount : int
                    Substitutions : Map<TypeVar, Type> }

    with 
        
        static member Empty =
            { BoundVariables= Map.empty;
              VariableCount = 0 ;
              Substitutions = Map.empty }

        member this.introduceVariable (var:string) (varType:Type) : Type * TypeContext =
            if not (this.BoundVariables |> Map.containsKey var) then
                let constructedType = "a" + this.VariableCount.ToString()
                let varType = if varType = Unknown then TypeVariable(constructedType) else varType
                let newScheme = { BoundVariables= this.BoundVariables.Add(var, constructedType);
                                    VariableCount = this.VariableCount + 1 ;
                                    Substitutions = this.Substitutions.Add(constructedType, varType) }
                TypeVariable(constructedType), newScheme
            else
                failwithf "cannot introduce an already known variable %s." var

        member this.lookupSubstitution (variable:TypeVar) : Type * TypeContext =
            match this.Substitutions |> Map.tryFind variable with
            | Some(substition) ->
                substition, this
            | None -> failwithf "unknown substituion for type %A" variable

        member this.substitute (variable:TypeVar) (substitution:Type)  =
            match this.Substitutions |> Map.tryFind variable with
            | Some(variableType) ->
                let newScheme =
                    {   this with Substitutions = this.Substitutions.Add(variable, substitution) }
                substitution, newScheme
            | None -> failwithf "Undefined substituion %A" variable 

        member this.lookup (variable:string) : Type =
            match this.BoundVariables |> Map.tryFind variable with
            | Some(variableType) ->
                let rec lookup var =
                    match this.Substitutions |> Map.tryFind var with
                    | Some(TypeVariable(sub)) when sub = var -> Unknown
                    | Some(TypeVariable(substitution)) when this.Substitutions |> Map.containsKey substitution -> 
                        lookup substitution
                    | Some(substituion) -> substituion
                    | _ -> failwithf "Undefined substituion for %s" variable
                lookup variableType
            | None -> failwithf "Undefined variable %s" variable

let isSuperType t1 t2 (ctxt:ConcreteExpressionContext) =
    match ctxt.InheritanceRelationships |> Map.tryFind t2 with
    | Some(inherritedClasses) ->
        inherritedClasses |> Set.contains t1
    | None -> false

let rec unify (expected:Type) (given:Type) (scheme:TypeContext) (ctxt:ConcreteExpressionContext) =
    match expected, given with
    | TypeConstant(e1), TypeConstant(g1) when e1 = g1 || (isSuperType e1 g1 ctxt) || (isSuperType g1 e1 ctxt) -> given, scheme
    | TypeAbstraction(e1, e2), TypeAbstraction(g1, g2) ->
        let firstUnifiedParameter, firstModifiedScheme = (unify e1 g1 scheme ctxt)
        let secondUnifiedParameter, secondModifiedScheme = (unify e2 g2 firstModifiedScheme ctxt)
        TypeAbstraction(firstUnifiedParameter, secondUnifiedParameter), secondModifiedScheme
    | TypeVariable(v1), TypeVariable(v2) ->
        let ignoredResult, firstModifiedScheme = scheme.substitute v1 given
        let ignoredResult, secondModifiedScheme = firstModifiedScheme.substitute v2 expected
        expected, secondModifiedScheme
    | TypeVariable(v), _ ->
        scheme.substitute v given
    | _, TypeVariable(v) ->
        scheme.substitute v expected
    | expected, Unknown -> expected, scheme
    | Unknown, given -> given, scheme
    | _ ->
        failwithf "cannot unify %s = %s\n" (expected.ToString()) (given.ToString())
        given, scheme
        
let rec traverse (ctxt:ConcreteExpressionContext) (expr:BasicExpression<Keyword, Var, Literal, Position, Unit>) (constraints:Type) (scheme:TypeContext) : Type * TypeContext =
    match expr with
    | Application(Regular, expr::[], _, _) -> traverse ctxt expr constraints scheme
    | Application(Angle, _, pos, _) ->
        Unknown, scheme
    | Application(_, Keyword(GreaterThan, _, _)::left::right::[], pos, _)      
    | Application(_, Keyword(SmallerThan, _, _)::left::right::[], pos, _)      
    | Application(_, Keyword(Equals, _, _)::left::right::[], pos, _)        
    | Application(_, Keyword(NotEquals, _, _)::left::right::[], pos, _)        
    | Application(_, Keyword(SmallerOrEqual, _, _)::left::right::[], pos, _)
    | Application(_, Keyword(DefinedAs,_,_)::left::right::[], pos, _)
    | Application(_, Keyword(DoubleArrow,_,_)::left::right::[], pos, _) ->
        let leftConstraints, firstModifiedScheme = traverse ctxt left Unknown scheme
        let rightConstraints, secondModifiedScheme = traverse ctxt right Unknown firstModifiedScheme
        unify leftConstraints rightConstraints secondModifiedScheme ctxt
    | Application(_, func::keywordArguments, _, _) ->
        match func with
        | Keyword(DefinedKeyword ctxt (keywordParameters, keywordType), _, _) ->
            let rec iterateOverArguments parameter argument scheme =
                match parameter, argument with
                | TypeAbstraction(t1, t2), argument::arguments ->
                    let localConstraints, newScheme = traverse ctxt argument t1 scheme
                    iterateOverArguments t2 arguments newScheme
                | parameter, argument::[] ->
                    traverse ctxt argument parameter scheme
                | parameter, argument::arguments ->
                    let localConstraints, newScheme = traverse ctxt argument Unknown scheme
                    unify localConstraints parameter newScheme ctxt
                | _, _ -> failwithf "undefined behaviour"
            let ignoredConstraints, populatedScheme = iterateOverArguments keywordParameters keywordArguments scheme
            let rec returnType t =
                match t with
                | TypeAbstraction(t, z) -> returnType z
                | t -> t
            returnType keywordType, populatedScheme
        | _ -> failwithf "Malformed application of %A" expr
    | Keyword(DefinedKeyword ctxt (_, keywordType), _, _) ->
        unify constraints keywordType scheme ctxt
    | Imported(importedType, _, _) ->
        unify constraints (TypeConstant(importedType.typeString)) scheme ctxt
    | Extension({Name = var}, _, _) ->
        match scheme.BoundVariables |> Map.tryFind var, constraints with
        | Some(variableType), Unknown ->
            scheme.lookupSubstitution variableType
        | Some(variableType), _ ->
            let substitution, ignoredScheme = scheme.lookupSubstitution variableType
            unify constraints substitution scheme ctxt
        | _, _ ->
            scheme.introduceVariable var constraints
    | _ -> failwithf "Unexpected expression %A" expr

let rec traverseFully (ctxt:ConcreteExpressionContext) (expr:BasicExpression<Keyword, Var, Literal, Position, Unit> list) : TypeContext =
    let rec traverseList expr scheme =
        match expr with
        | expr::exprs ->
            let e, scheme = traverse ctxt expr Unknown scheme
            traverseList exprs scheme
        | [] ->
            scheme
    traverseList expr TypeContext.Empty

let rec annotate (scheme:TypeContext) (ctxt:ConcreteExpressionContext) (expr:BasicExpression<Keyword, Var, Literal, Position, Unit>) =
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
        

let rec annotateAll (scheme:TypeContext) (ctxt:ConcreteExpressionContext) (expr:BasicExpression<Keyword, Var, Literal, Position, Unit> list) =
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
    
    //printf "%A\n" populatedScheme

    let annotatedOut = annotate populatedScheme ctxt o
    let annotatedIn = annotate populatedScheme ctxt i
    let annotatedClauses = annotateAll populatedScheme ctxt c
        
    annotatedIn, annotatedOut, annotatedClauses

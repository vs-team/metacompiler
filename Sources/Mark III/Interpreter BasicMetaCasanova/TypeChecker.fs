module TypeChecker

open Common
open ParserAST

exception TypeError of string

type LocalContext =
  {
    Variables : Map<Id,TypeDecl * Position>
  }
  with
    static member Empty =
      {
        Variables = Map.empty
      }

type TypedProgramDefinition =
  {
    Declarations      : List<Declaration>
    TypedRules        : List<TypedRuleDefinition>
    SymbolTable       : SymbolContext
  }

and TypedRuleDefinition =
| TypedRule of TypedRule
| TypedTypeRule of TypedRule

and TypedRule = 
  {
    Premises        : List<Premise>
    Conclusion      : Conclusion
    Locals          : LocalContext
    ReturnType      : TypeDecl
  }


//built-in types. Maybe they are not necessary in the end because they are defined in the prelude. Leave them for debugging.
let builtInTypes =
  [
    "int64"
    "int"
    "unint64"
    "uint32"
    "string"
    "double"
    "float"
    "string"
    "bool"
    "void"
  ]

let emptyPos = { File = "empty"; Line = 0; Col = 0}
let (!!) s = Arg(Id({ Namespace = []; Name = s },emptyPos))
let (~~) s = Id({Namespace = []; Name = s},emptyPos)
let (!!!) s = {Namespace = []; Name = s}
let (-->) t1 t2 = Arrow(t1,t2)
let (.|) ps c = Rule(ps,c)

//extract function name from a CallArg and rearrange the term in the form: functioName arg1 arg2 ... argn. The same form data constructors
let rec normalizeDataOrFunctionCall (_symbolTable : SymbolContext) (args : List<ParserAST.CallArg>) : List<ParserAST.CallArg> =
  let normCall =
    args |> 
    List.fold(fun (fArg,args) arg ->
                match arg with
                | Literal _ ->
                    (fArg,arg :: args)
                | Id(s,_) ->
                    match fArg with
                    | [] ->
                        let funcOpt = _symbolTable.FuncTable |> Map.tryFindKey(fun name sym -> name.Name = s.Name)
                        let dataOpt = _symbolTable.DataTable |> Map.tryFindKey(fun name sym -> name.Name = s.Name)
                        match funcOpt with
                        | None ->
                            match dataOpt with
                            | Some _ ->
                                (arg :: fArg,args)
                            | None ->
                                (fArg,arg :: args)
                        | Some _ ->
                            (arg :: fArg,args)
                    | _ ->
                        (fArg,arg :: args)
                | Lambda(_) -> failwith "Anonymous functions not supported yet"
                | NestedExpression (nestedArgs) ->
                    (fArg,(NestedExpression(normalizeDataOrFunctionCall _symbolTable nestedArgs)) :: args)) ([],[])
  let argList = snd normCall |> List.rev
  let fArg = fst normCall
  match fArg with
  | [] -> raise(TypeError("Undefined function or data constructor"))
  | _ ->
    if fArg.Length > 1 then
      failwith "Something went wrong when normalizing data or function call: more than a function name found"
    else
      (fArg.Head) :: argList



let rec checkType (_type : TypeDecl) (symbolTable : SymbolContext) : TypeDecl =
  match _type with
  | Zero -> _type
  | Arrow(left,right) ->
      let leftType = checkType left symbolTable
      let rightType = checkType right symbolTable
      Arrow(leftType,rightType)
  | Arg(arg) ->
      match arg with
      | Id(arg,pos) ->
          let typeOpt = symbolTable.DataTable |> Map.tryFindKey(fun (k : Id) (s : SymbolDeclaration) -> 
                                                    match s.Return with
                                                    | Arg(sarg) ->
                                                        match sarg with
                                                        | Id(arg1,_) -> arg = arg1
                                                        | _ -> false
                                                    | _ -> false)
          match typeOpt with
          | Some id -> _type
          | None ->
              let builtInTypeOpt = builtInTypes |> List.tryFind(fun t -> arg.Name = t)
              match builtInTypeOpt with
              | Some _ -> _type
              | None -> raise(TypeError(sprintf "Type Error: Undefined type %s at %A" (_type.ToString()) (pos.Line,pos.Col)))
      | _ -> raise(TypeError(sprintf "Type Error: You cannot use Data constructors or literals in function declarations"))
  | _ -> failwithf "The type %A is not supported in declarations yet... Back to work Germoney T_T" _type

let buildSymbols (declarations : List<Declaration>) (symbols : Map<Id,SymbolDeclaration>) =
//  let check (symDecl : SymbolDeclaration) =
    
  declarations |> List.fold(fun sym decl ->
                              match decl with
                              | Data(data) ->
                                  //do checkType data.Args sym |> ignore
                                  match data.Return with
                                  | Arg(Id(arg,_)) ->
                                    {sym with DataTable = sym.DataTable.Add(data.Name,data)}
                                  | _ -> raise(TypeError(sprintf "Type Error: invalid type %s for the data %s" (data.Return.ToString()) data.Name.Name))
                              | Func(func) ->
                                  //do checkType func.Args sym |> ignore
                                  {sym with FuncTable = sym.FuncTable.Add(func.Name,func)}
                              | TypeFunc(tf) ->
                                  {sym with TypeFuncTable = sym.TypeFuncTable.Add(tf.Name,tf)}
                              | TypeAlias(ta) ->
                                  {sym with TypeAliasTable = sym.TypeAliasTable.Add(ta.Name,ta)}) SymbolContext.Empty

let checkSymbols (declarations : List<Declaration>) (symbolTable : SymbolContext) =
  for decl in declarations do
    match decl with
    | Data(data) ->
        do checkType data.Args symbolTable |> ignore
    | Func(func) ->
        do checkType func.Args symbolTable |> ignore
    | TypeFunc(tf) ->
        failwith "TypeFunctions not implemented yet..."
    | TypeAlias(ta) ->
        failwith "TypeAliases not implemented yet..."


let checkTypeEquivalence (t1: TypeDecl) (t2 : TypeDecl) (p : Position) (ctxt : SymbolContext)  =
  if t1 = t2 || (TypeDecl.SubtypeOf t1 t2 ctxt.Subtyping) then
    ()
  else
    raise(TypeError(sprintf "Type Error: given %s but expected %s at %s" (t1.ToString()) (t2.ToString()) (p.ToString())))
    
let checkLiteral (l : Literal) (typeDecl : TypeDecl) (p : Position) (ctxt : SymbolContext) : TypeDecl =
  match l with
  | I64(_) ->
    do checkTypeEquivalence !!"int64" typeDecl p ctxt
    !!"int64"
  | I32(_) ->
    do checkTypeEquivalence !!"int" typeDecl p ctxt
    !!"int"
  | U64(_) ->
    do checkTypeEquivalence  !!"uint64" typeDecl p ctxt
    !!"uint64"
  | U32(_) ->
    do checkTypeEquivalence  !!"uint32" typeDecl p ctxt
    !!"uint32"
  | F64(_) ->
    do checkTypeEquivalence  !!"double" typeDecl p ctxt
    !!"double"
  | F32(_) ->
    do checkTypeEquivalence  !!"float" typeDecl p ctxt
    !!"float"
  | String(_) ->
    do checkTypeEquivalence  !!"string" typeDecl p ctxt
    !!"string"
  | Bool(_) ->
    do checkTypeEquivalence  !!"bool" typeDecl p ctxt
    !!"bool"
  | Void ->
    do checkTypeEquivalence  !!"void" typeDecl p ctxt
    !!"void"


let rec checkSingleArg
  (arg : ParserAST.CallArg)
  (symbolTable : SymbolContext)
  (typeDecl : TypeDecl)
  (ctxt : LocalContext)
  (buildLocals : bool) : TypeDecl * LocalContext =

  match arg with
  | Literal(l,p) ->
      match typeDecl with
      | Arg(Id(id,_)) ->        
          checkLiteral l typeDecl p symbolTable, ctxt
      | Generic(_) ->
          failwith "Generics not supported yet..."
      | _ ->
          failwith "Something went wrong: the type definition has an invalid structure"
  | Id(id,p) ->
      if buildLocals then
        Arg(Id(id,p)),{ctxt with Variables = ctxt.Variables |> Map.add id (typeDecl,p)}
      else
        let idOpt = ctxt.Variables |> Map.tryFind id
        match idOpt with
        | Some (t,_) ->
            do checkTypeEquivalence t typeDecl p symbolTable
            t,ctxt             
        | None ->
            raise(TypeError(sprintf "Type Error: undefined variable %s at %s" id.Name (p.ToString())))
  | Lambda(_) -> failwith "Anonymous functions not supported yet"   
  | NestedExpression(call) ->
      let nestedType,nestedCtxt = checkNormalizedCall call symbolTable ctxt buildLocals
      match call.Head with
      | Id(id,p) ->
          let dataOpt = symbolTable.DataTable |> Map.tryFind(id)
          match dataOpt with
          | Some decl ->
              checkTypeEquivalence nestedType typeDecl p symbolTable
              nestedType,nestedCtxt
          | None -> 
              let funcOpt = symbolTable.FuncTable |> Map.tryFind(id)
              match funcOpt with
              | Some decl ->
                checkTypeEquivalence nestedType typeDecl p symbolTable
                nestedType,nestedCtxt
              | None ->
                  failwith "Something went wrong: apparently the term is neither a data constructor nor a function call"
      | _ ->
          failwith "Something went wrong when checking the nested expression"

and checkNormalizedArgs 
  (args : List<ParserAST.CallArg>)
  (symbolTable : SymbolContext)
  (typeDecl : TypeDecl)
  (ctxt : LocalContext)
  (currentType : TypeDecl)
  (returnType: TypeDecl)
  (buildLocals : bool) : TypeDecl * LocalContext =

  let rec appendToArrow (a : TypeDecl) (t : TypeDecl) : TypeDecl =
    match a with
    | Arrow(left,right) -> Arrow(left,appendToArrow right t)
    | _ -> Arrow(a,t)

  match args with
  | [] ->
    match currentType with
    | Arrow(_) -> (appendToArrow currentType returnType),ctxt
    | _ -> currentType,ctxt
  | x :: xs ->
      match typeDecl with
      | Arrow(left,right) ->
          let t,newCtxt = checkSingleArg x symbolTable left ctxt buildLocals
          checkNormalizedArgs xs symbolTable right newCtxt right returnType buildLocals
      | Zero ->
          raise(TypeError("Type Error: the function expects no arguments"))
      | _ -> 
          let t,newCtxt = checkSingleArg x symbolTable typeDecl ctxt buildLocals
          returnType, newCtxt

and checkNormalizedCall 
  (call : List<ParserAST.CallArg>) 
  (symbolTable : SymbolContext) 
  (ctxt : LocalContext)
  (buildLocals : bool) : TypeDecl * LocalContext =

  match call with
  | arg :: args ->
      match arg with
      | Id(id,pos) ->
        let funcOpt = symbolTable.FuncTable |> Map.tryFind(id)
        let dataOpt = symbolTable.DataTable |> Map.tryFind(id)            
        match funcOpt with
        | None ->
            match dataOpt with
            | None ->
                failwith "You are checking arguments that are not data constructors or functions with checkNormalizedCall"
            | Some dSym ->
              checkNormalizedArgs args symbolTable dSym.Args ctxt (dSym.Return) (dSym.Return) buildLocals
        | Some fSym ->
            checkNormalizedArgs args symbolTable fSym.Args ctxt (fSym.Return) (fSym.Return) buildLocals
      | _ ->
        failwith "Something went wrong when normalizing the function call in the typechecker. The first argument is not a function name"
  | [] -> failwith "Something went wrong with the call normalization: there are no arguments in the call"

//We need to add the position to premise calls and conclusion calls
and checkPremise (premise : Premise) (symbolTable : SymbolContext) (locals : LocalContext) =
  match premise with
  | FunctionCall(func,result) ->
      let normFunc = normalizeDataOrFunctionCall symbolTable func
      let funcType,_ = checkNormalizedCall normFunc symbolTable locals false
      match result with
      | [r] -> 
          let localVarOpt = locals.Variables |> Map.tryFind(r)
          match localVarOpt with
          | Some (_,p) ->
              raise(TypeError(sprintf "Type Error: the variable %s is already defined at %s" r.Name (p.ToString())))
          | None ->
              { locals with Variables = locals.Variables |> Map.add r (funcType,Position.Zero) }
      | id :: ids ->
          let normalizedData = normalizeDataOrFunctionCall symbolTable (result |> List.map(fun x -> Id(id,Position.Zero)))
          let normIds = normalizedData |> List.map(fun x -> match x with
                                                            | Id(id,_) -> id
                                                            | _ -> failwith "Invalid premise result format")
          let funcOpt = symbolTable.FuncTable |> Map.tryFind (normIds.Head)
          match funcOpt with
          | Some _ -> raise(TypeError(sprintf "Type Error: It is not allowed to call a function in the return part of a premise: %s" id.Name))
          | None ->
              let dataType,newLocals = checkNormalizedCall normalizedData symbolTable locals true
              do checkTypeEquivalence dataType funcType Position.Zero symbolTable
              newLocals
      | _ -> failwith "Something went wrong: the return argument of a premise is empty"
  | Conditional(conditional) -> failwith "Conditionals not implemented yet..."

and checkRule (rule : RuleDefinition) (symbolTable : SymbolContext) =
  match rule with
  | Rule(premises,conclusion) ->
    match conclusion with
    | ValueOutput(call,result) ->
        let normalizedCall = normalizeDataOrFunctionCall symbolTable call
        let callType,locals = checkNormalizedCall normalizedCall symbolTable LocalContext.Empty true
        let localsAfterPremises =
          premises |> List.fold(fun l p -> checkPremise p symbolTable l) locals
        match result with
        | [arg] ->
            checkSingleArg arg symbolTable callType localsAfterPremises false
        | x :: xs ->
            let normalizedRes = normalizeDataOrFunctionCall symbolTable result
            checkNormalizedCall normalizedRes symbolTable localsAfterPremises false 
        | _ -> failwith "Why is the result of a conclusion empty?"          
    | ModuleOutput(_) ->
        raise(TypeError("You can only output modules in a type rule"))
  | TypeRule(premises,conclusion) -> failwith "type rules not supported yet..."


and buildSubTypes (subTypesDef : List<TypeDecl * TypeDecl>) : Map<TypeDecl,List<TypeDecl>> =
  subTypesDef |> List.fold(fun sts (t,alias) ->
                              let subTypeOpt = sts |> Map.tryFind t
                              match subTypeOpt with
                              | Some _ -> 
                                  sts |> Map.add t (alias :: (sts.[t]))
                              | None ->
                                  sts |> Map.add t [alias]) Map.empty

and checkProgramDefinition ((decls,rules,subtypes) : ProgramDefinition) : TypedProgramDefinition = 
  let symbolTable = buildSymbols decls Map.empty
  do checkSymbols decls symbolTable
  let symbolTable = { symbolTable with Subtyping = buildSubTypes subtypes }
  let typedRules =
    [for r in rules do
        match r with
        | Rule(r1) ->
            let _type,locals = checkRule r symbolTable
            let typedRule = { Premises = fst r1; Conclusion = snd r1; Locals = locals; ReturnType = _type }
            yield TypedRule(typedRule)
        | TypeRule(r) -> failwith "Type rule not supported yet..."]
  {
    Declarations = decls
    TypedRules = typedRules
    SymbolTable = symbolTable
  }

and checkProgram ((imports,def) : Program) : TypedProgramDefinition =
  //missing support for imports
  checkProgramDefinition def



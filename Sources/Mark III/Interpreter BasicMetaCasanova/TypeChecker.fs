module TypeChecker

open Common
open ParserAST

exception TypeError of string

//Type checker AST: move it to a separate file when it is complete

type TypedCallArg = List<ParserAST.CallArg * TypeDecl>

type TypedConclusion = 
| ValueOutput of List<TypedCallArg> * List<TypedCallArg>

type LocalContext =
  {
    Variables : Map<Id,TypeDecl * Position>
  }
  with
    static member Empty =
      {
        Variables = Map.empty
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
    
let checkLiteral (l : Literal) (typeDecl : TypeDecl) (p : Position) (ctxt : SymbolContext) =
  match l with
  | I64(_) ->
    checkTypeEquivalence !!"int64" typeDecl p ctxt
  | I32(_) ->
    checkTypeEquivalence !!"int" typeDecl p ctxt
  | U64(_) ->
    checkTypeEquivalence  !!"uint64" typeDecl p ctxt
  | U32(_) ->
    checkTypeEquivalence  !!"uint32" typeDecl p ctxt
  | F64(_) ->
    checkTypeEquivalence  !!"double" typeDecl p ctxt
  | F32(_) ->
    checkTypeEquivalence  !!"float" typeDecl p ctxt
  | String(_) ->
    checkTypeEquivalence  !!"string" typeDecl p ctxt
  | Bool(_) ->
    checkTypeEquivalence  !!"bool" typeDecl p ctxt
  | Void ->
    checkTypeEquivalence  !!"void" typeDecl p ctxt


let rec checkSingleArg
  (arg : ParserAST.CallArg)
  (symbolTable : SymbolContext)
  (typeDecl : TypeDecl)
  (ctxt : LocalContext)
  (buildLocals : bool) : LocalContext =

  match arg with
  | Literal(l,p) ->
      match typeDecl with
      | Arg(Id(id,_)) ->
          checkLiteral l typeDecl p symbolTable
          ctxt
      | Generic(_) ->
          failwith "Generics not supported yet..."
      | _ ->
          failwith "Something went wrong: the type definition has an invalid structure"
  | Id(id,p) ->
      if buildLocals then
        {ctxt with Variables = ctxt.Variables |> Map.add id (typeDecl,p)}
      else
        let idOpt = ctxt.Variables |> Map.tryFind id
        match idOpt with
        | Some (t,_) ->
            checkTypeEquivalence t typeDecl p symbolTable
            ctxt             
        | None ->
            raise(TypeError(sprintf "Type Error: undefined variable %s at %s" id.Name (p.ToString())))    
  | NestedExpression(call) ->
      let nestedCtxt = checkNormalizedCall call symbolTable ctxt buildLocals
      match call.Head with
      | Id(id,p) ->
          let dataOpt = symbolTable.DataTable |> Map.tryFind(id)
          match dataOpt with
          | Some decl ->
              checkTypeEquivalence decl.Return typeDecl p symbolTable
              nestedCtxt
          | None -> 
              let funcOpt = symbolTable.FuncTable |> Map.tryFind(id)
              match funcOpt with
              | Some decl ->
                checkTypeEquivalence decl.Return typeDecl p symbolTable
                nestedCtxt
              | None ->
                  failwith "Something went wrong: apparently the term is neither a data constructor nor a function call"
      | _ ->
          failwith "Something went wrong when checking the nested expression"

and checkNormalizedArgs 
  (args : List<ParserAST.CallArg>)
  (symbolTable : SymbolContext)
  (typeDecl : TypeDecl)
  (ctxt : LocalContext)
  (buildLocals : bool) : LocalContext =
  match args with
  | [] -> ctxt
  | x :: xs ->
      match typeDecl with
      | Arrow(left,right) ->
          let newCtxt = checkSingleArg x symbolTable left ctxt buildLocals
          checkNormalizedArgs xs symbolTable right newCtxt buildLocals
      | Zero ->
          raise(TypeError("Type Error: the function expects no arguments"))
      | _ -> checkSingleArg x symbolTable typeDecl ctxt buildLocals

and checkNormalizedCall 
  (call : List<ParserAST.CallArg>) 
  (symbolTable : SymbolContext) 
  (ctxt : LocalContext)
  (buildLocals : bool) : LocalContext =

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
              checkNormalizedArgs args symbolTable dSym.Args ctxt buildLocals
        | Some fSym ->
            checkNormalizedArgs args symbolTable fSym.Args ctxt buildLocals
      | _ ->
        failwith "Something went wrong when normalizing the function call in the typechecker. The first argument is not a function name"
  | [] -> failwith "Something went wrong with the call normalization: there are no arguments in the call"

//let checkConclusion (conclusion : ParserAST.Conclusion) (symbols : Map<Id,SymbolDeclaration>) (ctxt : LocalContext) =
//  match conclusion with
//  | ValueOutput(call,result) ->
//      let normalizedCall = normalizeDataOrFunctionCall symbols call
//      let normalizedResult = normalizeDataOrFunctionCall symbols result

let conclusionTest = [~~"eval";NestedExpression [NestedExpression [~~"a1";~~"-";~~"b1"];~~"+";~~"b"]]
let subtypingTest =
  [
    !!"int",[!!"expr"]
    !!"float",[!!"expr"]
  ] |> Map.ofList
let testLocals =
  {
    Variables =
      [
        !!!"a1",(!!"float",Position.Zero)
        !!!"b1",(!!"int",Position.Zero)
        !!!"b",(!!"int",Position.Zero)
      ] |> Map.ofList
  }

let (tcTest : Program) =
  let plus =
    {
      Name = { Namespace = []; Name = "+" }
      Args = !!"expr" --> !!"expr"
      Return = !!"expr"
      Order = Infix
      Priority = 0
      Position = emptyPos
      Associativity = Left
      Premises = []
    }
  let neg =
    {
      Name = { Namespace = []; Name = "-" }
      Args = !!"expr" --> !!"expr"
      Return = !!"expr"
      Order = Prefix
      Priority = 0
      Position = emptyPos
      Associativity = Left
      Premises = []
    }
  let eval =
    {
      Name = { Namespace = []; Name = "eval" }
      Args = !!"expr"
      Return = !!"expr"
      Order = Prefix
      Priority = 0
      Position = emptyPos
      Associativity = Left
      Premises = []
    }
  let evalPlus =
    let premises =
      [
        FunctionCall([~~"eval";~~"a"],[!!!"x1"])
        FunctionCall([~~"eval";~~"b"],[!!!"x2"])
      ]
    let (conclusion : Conclusion) =
      ParserAST.ValueOutput (conclusionTest,[~~"x2"])
    premises .| conclusion
  [],([Data plus; Data neg; Func eval],[evalPlus])



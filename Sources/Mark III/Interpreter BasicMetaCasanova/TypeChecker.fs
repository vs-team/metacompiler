module TypeChecker

open Common
open ParserAST

exception TypeError of string

//Type checker AST: move it to a separate file when it is complete

type TypedCallArg = ParserAST.CallArg * TypeDecl

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
    "int"
    "string"
    "float"
    "float32"
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
                                  do checkType data.Args sym |> ignore
                                  match data.Return with
                                  | Arg(Id(arg,_)) ->
                                    {sym with DataTable = sym.DataTable.Add(data.Name,data)}
                                  | _ -> raise(TypeError(sprintf "Type Error: invalid type %s for the data %s" (data.Return.ToString()) data.Name.Name))
                              | Func(func) ->
                                  do checkType func.Args sym |> ignore
                                  {sym with FuncTable = sym.FuncTable.Add(func.Name,func)}
                              | TypeFunc(tf) ->
                                  {sym with TypeFuncTable = sym.TypeFuncTable.Add(tf.Name,tf)}
                              | TypeAlias(ta) ->
                                  {sym with TypeAliasTable = sym.TypeAliasTable.Add(ta.Name,ta)}) SymbolContext.Empty

//let checkProgram (program : ProgramDefinition) (symbols : Map<Id,SymbolDeclaration>) =
//  (fst program) |> List.map(fun decl -> checkType decl symbols) |> ignore


//remember to reverse the arguments at the end
let rec checkNormalizedArgs 
  (args : List<ParserAST.CallArg>)
  (symbolTable : SymbolContext)
  (typeDecl : TypeDecl)
  (typedArgs : List<TypedCallArg>)
  (ctxt : LocalContext)
  (buildLocals : bool) : List<TypedCallArg> * LocalContext =

  match args with
  | [] -> typedArgs,ctxt
  | arg :: otherargs ->
      match typeDecl with
      | Arrow(left,right) ->
          match left with
          | Arg(Id(id,pos)) ->
              let checkLiteral (typeName : string) (expectedType : Id) (arg : CallArg) (pos : Position) =
                if id.Name = typeName then
                  (arg,left) :: typedArgs
                else
                  raise(TypeError(sprintf "Type error: given int64, expected %s at %s" (left.ToString()) (pos.ToString())))
              match arg with
              | Literal(l,p) ->
                  match l with
                  | I64 _ ->
                      checkNormalizedArgs  otherargs symbolTable right (checkLiteral "int64" id arg p) ctxt buildLocals
                  | U64 _ ->
                      checkNormalizedArgs  otherargs symbolTable right (checkLiteral "uint64" id arg p) ctxt buildLocals
                  | I32 _ ->
                      checkNormalizedArgs  otherargs symbolTable right (checkLiteral "int" id arg p) ctxt buildLocals
                  | U32 _ ->
                      checkNormalizedArgs  otherargs symbolTable right (checkLiteral "uint" id arg p) ctxt buildLocals
                  | F64 _ ->
                      checkNormalizedArgs  otherargs symbolTable right (checkLiteral "float" id arg p) ctxt buildLocals
                  | F32 _ ->
                      checkNormalizedArgs  otherargs symbolTable right (checkLiteral "float32" id arg p) ctxt buildLocals
                  | String _ ->
                      checkNormalizedArgs  otherargs symbolTable right (checkLiteral "string" id arg p) ctxt buildLocals
                  | Bool _ ->
                      checkNormalizedArgs  otherargs symbolTable right (checkLiteral "bool" id arg p) ctxt buildLocals
                  | Void _ ->
                      checkNormalizedArgs  otherargs symbolTable right (checkLiteral "void" id arg p) ctxt buildLocals
              | Id(name,p) ->
                  if buildLocals then
                    checkNormalizedArgs otherargs symbolTable right typedArgs {ctxt with Variables = ctxt.Variables.Add(id,(left,p))} buildLocals
                  else
                    match Map.tryFind id ctxt.Variables with
                    | None -> raise(TypeError(sprintf "Type Error: Undefined variable %s at %s" id.Name (p.ToString())))
                    | Some (t,p) ->
                        match t with
                        | Arg(Id(id1,pos1)) ->
                            if id1 = id then
                              raise(TypeError(sprintf "Type Error: expected %s but given %s at %s" (left.ToString()) (t.ToString()) (pos.ToString())))
                            else
                              checkNormalizedArgs otherargs symbolTable right ((arg,left) :: typedArgs) ctxt buildLocals
              | NestedExpression(expr) ->
                  let callType,typedArgs,ctxt = checkNormalizedCall expr symbolTable ctxt buildLocals
                  if callType = left then
                    checkNormalizedArgs otherargs symbolTable right ((arg,left) :: typedArgs) ctxt buildLocals
                  else
                    raise(TypeError(sprintf "Type Error: expected %s but given %s" (left.ToString()) (callType.ToString())))

and checkNormalizedCall 
  (call : List<ParserAST.CallArg>) 
  (symbolTable : SymbolContext) 
  (ctxt : LocalContext)
  (buildLocals : bool) : TypeDecl * List<TypedCallArg> * LocalContext =
  match call with
  | arg :: args ->
      match arg with
      | Id(id,pos) ->
          let funcOpt = symbolTable.FuncTable |> Map.tryFind(id)
          match funcOpt with
          | None -> 
              raise(TypeError(sprintf "Type Error: the argument at %s is not a function" (pos.ToString())))
          | Some fSym ->
              let typedArgs,locals = checkNormalizedArgs args symbolTable fSym.Args [] ctxt buildLocals
              fSym.Return,typedArgs,locals
      | _ ->
        failwith "Something went wrong when normalizing the function call in the typechecker. The first argument is not a function name"
  | [] -> failwith "Something went wrong with the call normalization: there are no arguments in the call"

//let checkConclusion (conclusion : ParserAST.Conclusion) (symbols : Map<Id,SymbolDeclaration>) (ctxt : LocalContext) =
//  match conclusion with
//  | ValueOutput(call,result) ->
//      let normalizedCall = normalizeDataOrFunctionCall symbols call
//      let normalizedResult = normalizeDataOrFunctionCall symbols result

let conclusionTest = [~~"eval";NestedExpression [NestedExpression [~~"a1";~~"-";~~"b1"];~~"+";~~"b"]]

let (tcTest : Program) =
  let plus =
    {
      Name = { Namespace = []; Name = "+" }
      Args = !!"int" --> !!"int"
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
      Args = !!"int" --> !!"int"
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
      Return = !!"int"
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



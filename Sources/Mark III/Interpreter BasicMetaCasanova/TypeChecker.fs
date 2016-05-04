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
    Variables : Map<Id,Position * TypeDecl>
  }
  with
    member this.Empty =
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

type NormalizedCall =
  {
    Name : ParserAST.CallArg option
    Args : List<NormalizedCall>
  }
  with
    static member Empty = { Name = None; Args = [] }

//extract function name from a CallArg and rearrange the term in the form: functioName arg1 arg2 ... argn. The same form data constructors
let rec normalizeDataOrFunctionCall (_symbolTable : SymbolContext) (args : List<ParserAST.CallArg>) : NormalizedCall =
  let normCall =
    args |> List.fold(fun res arg ->
                        let addArg (call : NormalizedCall) (arg : ParserAST.CallArg) : NormalizedCall =
                          { call with Args = call.Args @ [{ Name = Some arg; Args = []}] }
                        match arg with
                        | Literal _ ->
                            addArg res arg 
                        | Id(s,_) ->
                            match res.Name with
                            | None ->
                                let funcOpt = _symbolTable.FuncTable |> Map.tryFindKey(fun name sym -> name.Name = s.Name)
                                let dataOpt = _symbolTable.DataTable |> Map.tryFindKey(fun name sym -> name.Name = s.Name)
                                match funcOpt with
                                | None ->
                                    match dataOpt with
                                    | Some _ ->
                                        { res with Name = Some arg }
                                    | None ->
                                        addArg res arg
                                | Some _ ->
                                    { res with Name = Some arg }
                            | Some _ ->
                                addArg res arg
                        | NestedExpression (nestedArgs) ->
                            { res with Args = res.Args @ [(normalizeDataOrFunctionCall _symbolTable nestedArgs)] }) NormalizedCall.Empty
  match normCall.Name with
  | None -> raise(TypeError("Undefined function or data constructor"))
  | Some _ -> normCall



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

//let rec checkNormalizedArgs (args : List<NormalizedCall>) (symbol : SymbolDeclaration) (ctxt : LocalContext) =
//  args |> List.fold(fun (typedArgs,c) arg ->
//                      match arg.Name with
//                      | None -> failwith "After call normalization every normalized call should have a name")
//                      | Some name ->
//                          match name with
//                          | Literal(l,p) -> 
//                              match l with
//                              | I64(x) -> )
//                              
//                          
//                   ([],ctxt)
//
//let checkNormalizedCall (call : NormalizedCall) (symbols : Map<Id,SymbolDeclaration>) (ctxt : LocalContext) =
//  match call.Name with
//  | None -> failwith "Bug in typechecker call normalization: please fix it!"
//  | Some name ->
//      match name with
//      | Id (s,p) ->
//          let symDecl = symbols.[s]
//          checkNormalizedArgs call.Args symDecl ctxt
//      | _ -> failwith "The first argument of a normalized call should be an Id"

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



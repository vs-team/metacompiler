module TypeChecker

open Common
open ParserAST


let buildSymbols (declarations : List<Declaration>) (symbols : Map<Id,SymbolDeclaration>) =
  declarations |> List.fold(fun sym decl ->
                              match decl with
                              | Data(data) ->
                                  
                                  {sym with DataTable = sym.DataTable.Add(data.Name,data)}
                              | Func(func) ->
                                  {sym with FuncTable = sym.FuncTable.Add(func.Name,func)}
                              | TypeFunc(tf) ->
                                  {sym with TypeFuncTable = sym.TypeFuncTable.Add(tf.Name,tf)}
                              | TypeAlias(ta) ->
                                  {sym with TypeAliasTable = sym.TypeAliasTable.Add(ta.Name,ta)}) SymbolContext.Empty

//and checkType (_type : TypeDecl) (ret : TypeDecl) =
//  
//let checkProgram (program : ProgramDefinition) (symbols : Map<Id,SymbolDeclaration>) =
//  let   

let emptyPos = { File = "empty"; Line = 0; Col = 0}
let (!!) s = Arg(Id({ Namespace = []; Name = s },emptyPos))
let (~~) s = Id({Namespace = []; Name = s},emptyPos)
let (!!!) s = {Namespace = []; Name = s}
let (-->) t1 t2 = Arrow(t1,t2)
let (.|) ps c = Rule(ps,c) 

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
      Args = !!"int"
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
    let conclusion =
      ValueOutput([~~"eval";NestedExpression [~~"a";~~"+";~~"b"]],[~~"x2"])
    premises .| conclusion
  [],([Data plus; Data neg; Func eval],[evalPlus])



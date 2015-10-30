module ScopeBuilder

open Common
open ParserMonad

type BasicExpression =
  | Id of Id * Position
  | Literal of Literal * Position
  | Application of Bracket * List<BasicExpression>

and SymbolDeclaration = 
  {
    Name              : string
    GenericParameters : List<Id>
    LeftArgs          : List<Type>
    RightArgs         : List<Type>
    Return            : Type
    Priority          : int
    Associativity     : Associativity
    Position          : Position
  }
  
and Type = List<BasicExpression>

and Associativity = Left | Right

and Rule = 
  {
    Input     : List<BasicExpression>
    Output    : List<BasicExpression>
    Premises  : List<Premise>
  }

and Premise = Conditional of List<BasicExpression> | Implication of List<BasicExpression> * List<BasicExpression>

and TypeFuncDefinition = 
  {
    NameId      : Id
    SymDec      : SymbolDeclaration
    content     : Scope
  }

and Scope = 
  {
    ImportDeclaration       : List<Id>
    FunctionDeclarations    : List<SymbolDeclaration>
    TypeFunctionDeclarations: List<SymbolDeclaration>
    TypeFunctionDefinitions : List<TypeFuncDefinition>
    DataDeclarations        : List<SymbolDeclaration>
    Rules                   : List<Rule>
  } 
  with 
    static member Zero = 
      {
        ImportDeclaration       = []
        FunctionDeclarations    = []
        TypeFunctionDeclarations= []
        TypeFunctionDefinitions = []
        DataDeclarations        = []
        Rules                   = []
      }

let getPosition : Parser<LineSplitter.BasicExpression, _, _> = 
  fun (exprs,ctxt) ->
    Done(exprs |> LineSplitter.BasicExpression.tryGetNextPosition, exprs, ctxt)
  
let id = 
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.Id(i,pos)::es -> Done(i, es, ctxt)
    | _ -> Error (sprintf "Error: expected id at %A." (exprs |> LineSplitter.BasicExpression.tryGetNextPosition))

let string_literal = 
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.Literal(String s,pos)::es -> Done(s, es, ctxt)
    | _ -> Error (sprintf "Error: expected string literal at %A." (exprs |> LineSplitter.BasicExpression.tryGetNextPosition))

let arrow = 
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.BasicExpression.Keyword(LineSplitter.SingleArrow,pos)::es -> Done((), es, ctxt)
    | _ -> Error (sprintf "Error: expected -> at %A." (exprs |> LineSplitter.BasicExpression.tryGetNextPosition))

let doublearrow = 
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.BasicExpression.Keyword(LineSplitter.DoubleArrow,pos)::es -> Done((), es, ctxt)
    | _ -> Error (sprintf "Error: expected => at %A." (exprs |> LineSplitter.BasicExpression.tryGetNextPosition))

let import = 
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.BasicExpression.Keyword(LineSplitter.Import,pos)::es -> Done((), es, ctxt)
    | _ -> Error (sprintf "Error: expected import at %A." (exprs |> LineSplitter.BasicExpression.tryGetNextPosition))

let func = 
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.BasicExpression.Keyword(LineSplitter.Func,pos)::es -> Done((), es, ctxt)
    | _ -> Error (sprintf "Error: expected func at %A." (exprs |> LineSplitter.BasicExpression.tryGetNextPosition))

let typefunc = 
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.BasicExpression.Keyword(LineSplitter.TypeFunc,pos)::es -> Done((), es, ctxt)
    | _ -> Error (sprintf "Error: expected typefunc at %A." (exprs |> LineSplitter.BasicExpression.tryGetNextPosition))

let data = 
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.BasicExpression.Keyword(LineSplitter.Data,pos)::es -> Done((), es, ctxt)
    | _ -> Error (sprintf "Error: expected data at %A." (exprs |> LineSplitter.BasicExpression.tryGetNextPosition))

let rec simple_expression : Parser<LineSplitter.BasicExpression, Scope, BasicExpression> =
  fun (exprs,ctxt) ->
  match exprs with
  | LineSplitter.Id(i,pos) :: es -> Done(Id(i,pos),es,ctxt)
  | LineSplitter.Literal(l,pos) :: es -> Done(Literal(l,pos),es,ctxt)
  | LineSplitter.Application(b,inner) :: es -> 
    match (simple_expression |> repeat) (inner,ctxt) with
    | Done(inner',[],ctxt) -> Done(Application(b,inner'),es,ctxt)
    | _ -> Error(sprintf "Error: expected simple expression at %A" (exprs |> LineSplitter.BasicExpression.tryGetNextPosition))
  | _ -> Error(sprintf "Error: expected simple expression at %A" (exprs |> LineSplitter.BasicExpression.tryGetNextPosition))

let rec nested_id_application : Parser<LineSplitter.BasicExpression, Scope, BasicExpression> =
  fun (exprs,ctxt) ->
  match exprs with
  | LineSplitter.Id(i,pos) :: es -> Done(Id(i,pos),es,ctxt)
  | LineSplitter.Keyword(k,pos) :: es -> Done(Id("Keyword__",pos),es,ctxt)
  | LineSplitter.Application(b,inner) :: es -> 
    match (nested_id_application |> repeat) (inner,ctxt) with
    | Done(inner',[],ctxt) -> Done(Application(b,inner'),es,ctxt)
    | _ -> Error(sprintf "Error: expected id (also nested) inside brackets at %A" (exprs |> LineSplitter.BasicExpression.tryGetNextPosition))
  | _ -> Error(sprintf "Error: expected id (also nested) but could not find at %A" (exprs |> LineSplitter.BasicExpression.tryGetNextPosition))

let nested_id : Parser<LineSplitter.BasicExpression, Scope, BasicExpression> =
  fun (exprs,ctxt) ->
  match exprs with
  | LineSplitter.Id(i,pos) :: es -> Done(Id(i,pos),es,ctxt)
  //| LineSplitter.Application(Curly,inner) :: es -> Done(Application(Curly,[]),es,ctxt)
  | LineSplitter.Application(b,inner) :: es -> 
    match (nested_id_application |> repeat) (inner,ctxt) with
    | Done(inner',[],ctxt) -> Done(Application(b,inner'),es,ctxt)
    | _ -> Error(sprintf "Error: expected id (also nested) inside brackets at %A" (exprs |> LineSplitter.BasicExpression.tryGetNextPosition))
  | _ -> Error(sprintf "Error: expected id (also nested) but could not find at %A" (exprs |> LineSplitter.BasicExpression.tryGetNextPosition))

let type_expression : Parser<_, _, Type> = 
  prs{
    let! pos = getPosition
    let! i = nested_id
    let! is = nested_id |> repeat // or recursive application of id's, converted
    return i :: is
  }

let rec priority : Parser<_, _, int> =
  fail "Not implemented"

let rec associativity : Parser<_, _, Associativity> =
  fail "Not implemented"

let rec arguments() : Parser<_, _, List<Type>> =
  prs{
    let! arg = type_expression
    do! arrow
    let! rest = arguments()
    return arg::rest
  } .|| (prs {return []})

let rec typefunc_arguments() : Parser<_, _, List<Type>> =
  prs{
    let! arg = type_expression
    do! doublearrow
    let! rest = typefunc_arguments()
    return arg::rest
  } .|| (prs{return []})


let symbol_declaration_body : Parser<_, _, SymbolDeclaration> =
  prs{
    let! position = getPosition
    let! left_arguments = arguments()
    let! name = string_literal
    let! generic_parameters = id |> repeat
    do! arrow
    let! right_arguments = arguments()
    let! return_type = type_expression
    let! priority = priority .|| (prs{ return 0 })
    let! associativity = associativity .|| (prs{ return Left })
    return  {
              Name              = name
              GenericParameters = generic_parameters
              LeftArgs          = left_arguments
              RightArgs         = right_arguments
              Return            = return_type
              Priority          = priority
              Associativity     = associativity
              Position          = position
            }
  }
let typefunc_declaration_body : Parser<_, _, SymbolDeclaration> =
  prs{
    let! position = getPosition
    let! left_arguments = typefunc_arguments()
    let! name = string_literal
    let! generic_parameters = id |> repeat
    do! doublearrow
    let! right_arguments = typefunc_arguments()
    let! return_type = type_expression
    let! priority = priority .|| (prs{ return 0 })
    let! associativity = associativity .|| (prs{ return Left })
    return  {
              Name              = name
              GenericParameters = generic_parameters
              LeftArgs          = left_arguments
              RightArgs         = right_arguments
              Return            = return_type
              Priority          = priority
              Associativity     = associativity
              Position          = position
            }
  }

let typefunc_declaration : Parser<LineSplitter.BasicExpression,Scope,Unit> = 
  prs{
    do! typefunc
    let! sym_decl = typefunc_declaration_body
    let! ctxt = getContext
    do! setContext { ctxt with TypeFunctionDeclarations = sym_decl :: ctxt.TypeFunctionDeclarations }
  }

let import_declaration : Parser<LineSplitter.BasicExpression,Scope,Unit> = 
  prs{
    do! import
    let! parameter = id
    let! ctxt = getContext
    do! setContext { ctxt with ImportDeclaration = parameter :: ctxt.ImportDeclaration}
  }

let func_declaration : Parser<LineSplitter.BasicExpression,Scope,Unit> = 
  prs{
    do! func
    let! sym_decl = symbol_declaration_body
    let! ctxt = getContext
    do! setContext { ctxt with FunctionDeclarations = sym_decl :: ctxt.FunctionDeclarations }
  }

let data_declaration : Parser<LineSplitter.BasicExpression,Scope,Unit> = 
  prs{
    do! data
    let! sym_decl = symbol_declaration_body
    let! ctxt = getContext
    do! setContext { ctxt with DataDeclarations = sym_decl :: ctxt.DataDeclarations }
  }

let parse_first_line (p:Parser<LineSplitter.BasicExpression,_,'a>) : Parser<LineSplitter.Line,_,'a> = 
  fun (lines,ctxt) ->
    match lines with
    | line::lines -> 
      match p (line,ctxt) with
      | Done(res,_,ctxt') ->
        Done(res,lines,ctxt')
      | Error e -> Error e
    | [] -> Error(sprintf "Error: cannot extract leading line at %A" (LineSplitter.BasicExpression.tryGetNextPosition lines))

let line : Parser<LineSplitter.Line,_,_> = 
  fun (lines,ctxt) ->
    match lines with
    | l::ls -> Done(l,ls,ctxt)
    | _ -> Error(sprintf "Error: cannot extract line at %A" (LineSplitter.BasicExpression.tryGetNextPosition lines))

let empty_line : Parser<LineSplitter.Line,_,_> = 
  prs{
    let! line = line
    match line with
    | [] -> return ()
    | _ -> return! fail "Error: line is not empty!"
  }

let rec skip_empty_lines() =
  prs{
    do! empty_line
    do! skip_empty_lines()
  } .|| (prs{ return () })

let premise : Parser<LineSplitter.BasicExpression, Scope, Premise> =
  prs{
    let! i = simple_expression |> repeat
    return! (prs{
              do! arrow
              let! o = simple_expression |> repeat
              return Implication(i,o)
            }) .||
            (prs{
              do! eof
              return Conditional i
            })  
  }

let horizontal_bar : Parser<LineSplitter.BasicExpression,_,_> = 
  fun (exprs,ctxt) ->
    match exprs with
    | [LineSplitter.BasicExpression.Keyword(LineSplitter.HorizontalBar,pos)] -> Done((), [], ctxt)
    | _ -> Error (sprintf "Error: expected ---- at %A." (exprs |> LineSplitter.BasicExpression.tryGetNextPosition))

let rule : Parser<LineSplitter.Line, Scope, Unit> =
  prs{
    do! skip_empty_lines()
    let! premises = premise |> parse_first_line |> repeat
    do! horizontal_bar |> parse_first_line
    let! i,o = 
      prs{
        let! i = simple_expression |> repeat              
        do! arrow
        let! o = simple_expression |> repeat
        return i,o
      } |> parse_first_line
    let! ctxt = getContext
    do! setContext { ctxt with Rules = { Premises = premises; Input = i; Output = o } :: ctxt.Rules }
  } .|| 
  prs{
    let! i,o = 
      prs{
        let! i = simple_expression |> repeat              
        do! arrow
        let! o = simple_expression |> repeat
        return i,o
      } |> parse_first_line
    let! ctxt = getContext
    do! setContext { ctxt with Rules = { Premises = []; Input = i; Output = o } :: ctxt.Rules }
  }

let rec module_lines =
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.Application(Curly,[LineSplitter.Application(Indent,[LineSplitter.Block(b)])]) :: rest -> Done(b,exprs,ctxt)
    | _ -> Error (sprintf "Error: expected module_lines at %A." (exprs |> LineSplitter.BasicExpression.tryGetNextPosition))

let rec module_scope() : Parser<LineSplitter.Line, Scope, Scope> =
  prs{
    do! skip_empty_lines()
    do! (parse_first_line (func_declaration))
    return! module_scope()
  } .|| 
  (eof >> getContext)


let rec scope() : Parser<LineSplitter.Line, Scope, Scope> =
  prs{
    do! skip_empty_lines()
    do! (parse_first_line (import_declaration .|| func_declaration .|| typefunc_declaration .|| 
                           typefunc_definition .|| data_declaration )) .|| rule
    return! scope()
  } .|| 
  (eof >> getContext)

and typefunc_definition_body : Parser<_, _, TypeFuncDefinition> =
  prs{
    let! position = getPosition
    let! nameid  = id
    let! right_arguments = type_expression |> repeat
    do! doublearrow
    let! return_type = type_expression

    let! lines = module_lines .|| (prs{ return []})
    let inside_scope = (module_scope() (lines,Scope.Zero))
    let! priority = priority .|| (prs{ return 0 })
    let! associativity = associativity .|| (prs{ return Left })
    return{ 
            NameId = nameid 
            SymDec = {
                        Name              = ""
                        GenericParameters = []
                        LeftArgs          = []
                        RightArgs         = right_arguments
                        Return            = return_type
                        Priority          = priority
                        Associativity     = associativity
                        Position          = position
                    }
            content = match inside_scope with 
                      | Done (res,_,_) -> res
                      | _ -> Scope.Zero
         }
  }

and typefunc_definition : Parser<LineSplitter.BasicExpression,Scope,Unit> =
  prs{
    let! typefunc_decl = typefunc_definition_body
    let! ctxt = getContext
    do! setContext { ctxt with TypeFunctionDefinitions = typefunc_decl :: ctxt.TypeFunctionDefinitions }
  }

let block : Parser<LineSplitter.BasicExpression, Unit, List<_>> = 
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.Block(b) :: es -> Done(b,es,ctxt)
    | _ -> Error (sprintf "Error: expected block at %A" (LineSplitter.BasicExpression.tryGetNextPosition exprs))

let rec traverse() : Parser<LineSplitter.BasicExpression, Unit, List<Scope>> = 
  prs {
    let! inner_lines = block
    match scope () (inner_lines,Scope.Zero) with
    | Done(scope,_,_) ->
      let! rest = traverse()
      return scope :: rest
    | Error e -> 
      return! fail e
  } .||
    (eof >>
      prs{ return [] })

let build_scopes (lines:List<LineSplitter.Line>) : Option<Scope> =
  match scope() (lines,Scope.Zero) with
  | Done (res,_,_) -> Some res
  | Error e ->
    do printfn "%s" e
    None

module ScopeBuilder

open Common
open ParserMonad
open ScopeBuilderId

type Arrow = SingleArrow | DoubleArrow 

type BasicExpression =
  | Id of Id * Position
  | Literal of Literal * Position
  | Arrow of Arrow * Position
  | Application of Bracket * List<BasicExpression>
  | Lambda of Rule
  | Module of Id

and SymbolDeclaration = 
  {
    Name              : string
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

and Premise =   Conditional of List<BasicExpression> 
              | Implication of List<BasicExpression> * List<BasicExpression>

and Scope = 
  {
    CurrentNamespace         : Namespace
    ImportDeclaration        : List<Id>
    InheritDeclaration       : List<Id>
    FunctionDeclarations     : List<SymbolDeclaration>
    TypeFunctionDeclarations : List<SymbolDeclaration> 
    ArrowFunctionDeclarations: List<SymbolDeclaration> 
    TypeAliasDeclarations    : List<SymbolDeclaration> 
    DataDeclarations         : List<SymbolDeclaration>
    TypeFunctionRules        : List<Rule>
    Rules                    : List<Rule>
    Modules                  : List<Id*Scope>
  } 
  with 
    static member Zero = 
      {
        CurrentNamespace          = ""
        ImportDeclaration         = []
        InheritDeclaration        = []
        FunctionDeclarations      = []
        TypeFunctionDeclarations  = []
        ArrowFunctionDeclarations = []
        TypeAliasDeclarations     = []
        DataDeclarations          = []
        TypeFunctionRules         = []
        Rules                     = []
        Modules                   = []
      }

let getPosition : Parser<LineSplitter.BasicExpression, _, _> = 
  fun (exprs,ctxt) ->
    Done(exprs |> LineSplitter.BasicExpression.tryGetNextPosition, exprs, ctxt)
  
let id = 
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.Id(i,pos)::es -> Done(i, es, ctxt)
    | _ -> Error (ScopeError ("Error: expected id at.", (exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))

let int_literal = 
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.Literal(Int s,pos)::es -> Done(s, es, ctxt)
    | _ -> Error (ScopeError ("Error: expected int literal at.",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))

let string_literal = 
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.Literal(String s,pos)::es -> Done(s, es, ctxt)
    | _ -> Error (ScopeError ("Error: expected string literal at.",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))

let arrow = 
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.BasicExpression.Keyword(LineSplitter.SingleArrow,pos)::es -> Done((), es, ctxt)
    | _ -> Error (ScopeError (",->",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))

let doublearrow = 
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.BasicExpression.Keyword(LineSplitter.DoubleArrow,pos)::es -> Done((), es, ctxt)
    | _ -> Error (ScopeError (",=>",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))

let priorityarrow = 
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.BasicExpression.Keyword(LineSplitter.PriorityArrow,pos)::es -> Done((), es, ctxt)
    | _ -> Error (ScopeError (",#>",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))

let import = 
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.BasicExpression.Keyword(LineSplitter.Import,pos)::es -> Done((), es, ctxt)
    | _ -> Error (ScopeError ("expected import",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))

let inherit__ = 
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.BasicExpression.Keyword(LineSplitter.Inherit,pos)::es -> Done((), es, ctxt)
    | _ -> Error (ScopeError (",inherit",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))

let func = 
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.BasicExpression.Keyword(LineSplitter.Func,pos)::es -> Done((), es, ctxt)
    | _ -> Error (ScopeError (",func",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))

let typefunc = 
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.BasicExpression.Keyword(LineSplitter.TypeFunc,pos)::es -> Done((), es, ctxt)
    | _ -> Error (ScopeError (",typefunc",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))

let arrowfunc = 
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.BasicExpression.Keyword(LineSplitter.ArrowFunc,pos)::es -> Done((), es, ctxt)
    | _ -> Error (ScopeError (",arrowfunc",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))

let typealias = 
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.BasicExpression.Keyword(LineSplitter.TypeAlias,pos)::es -> Done((), es, ctxt)
    | _ -> Error (ScopeError (",typealias",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))

let data = 
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.BasicExpression.Keyword(LineSplitter.Data,pos)::es -> Done((), es, ctxt)
    | _ -> Error (ScopeError (",data",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))

let comment =
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.Application(Comment,inner) :: es -> Done((), es, ctxt)
    | LineSplitter.BasicExpression.Keyword(LineSplitter.CommentLine,pos)::es -> Done((), es, ctxt)
    | _ -> Error (ScopeError (",comment",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))

let not_empty : Parser<LineSplitter.BasicExpression, Scope, _> =
  fun (exprs,ctxt) ->
  match exprs with
  | x::xs -> Done ((),exprs,ctxt)
  | [] -> Error(ScopeError ("Error: cannot extract line at %A",(LineSplitter.BasicExpression.tryGetNextPosition exprs)))

let parse_first_line (p:Parser<LineSplitter.BasicExpression,_,'a>) : Parser<LineSplitter.Line,_,'a> = 
  fun (lines,ctxt) ->
    match lines with
    | line::lines -> 
      match p (line,ctxt) with
      | Done(res,_,ctxt') ->
        Done(res,lines,ctxt')
      | Error(p) -> Error(p)
    | [] -> Error(ScopeError ("Error: cannot extract leading line at",(LineSplitter.BasicExpression.tryGetNextPosition lines)))

let rec simple_expression : Parser<LineSplitter.BasicExpression, Scope, BasicExpression> =
  fun (exprs,ctxt) ->
  match exprs with
  | LineSplitter.Id(i,pos) :: es -> Done(Id(i,pos),es,ctxt)
  | LineSplitter.Literal(l,pos) :: es -> Done(Literal(l,pos),es,ctxt)
  | LineSplitter.Application(b,inner) :: es -> 
    match (simple_expression |> repeat) (inner,ctxt) with
    | Done(inner',[],ctxt) -> Done(Application(b,inner'),es,ctxt)
    | _ -> Error(ScopeError ("Error: expected simple expression at",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))
  | _ -> Error(ScopeError ("Error: expected simple expression at",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))

let rec flatten_nested_id (exprs:List<BasicExpression>) :List<BasicExpression> =
  match exprs with
  | Application(Indent,inner) :: es -> (flatten_nested_id inner @ flatten_nested_id es)
  | Application(b,inner) :: es -> Application(b,flatten_nested_id inner) :: flatten_nested_id es
  | x::xs -> x :: flatten_nested_id xs
  | [] -> []

let rec nested_id_application : Parser<LineSplitter.BasicExpression, Scope, BasicExpression> =
  fun (exprs,ctxt) ->
  match exprs with
  | LineSplitter.Id(i,pos) :: es -> Done(Id(i,pos),es,ctxt)
  | LineSplitter.Literal(l,pos) :: es -> Done(Literal(l,pos),es,ctxt)
  | LineSplitter.Keyword(k,pos) :: es -> 
    match k with
    | LineSplitter.SingleArrow -> Done(Arrow(SingleArrow,pos),es,ctxt)
    | LineSplitter.DoubleArrow -> Done(Arrow(DoubleArrow,pos),es,ctxt)
    | _ -> Error(ScopeError ("arrow",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))
  | LineSplitter.Block(b) :: es -> 
    match (line_to_id_basicexpression |> repeat)(b,ctxt) with
    | Done(inner',[],ctxt) -> Done(Application(Indent,inner'),es,ctxt)
    | _ -> Error(ScopeError ("Error: expected indent at",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))
  | LineSplitter.Application(Common.Lambda,inner) :: es -> 
    match (lambda_id_basicexpression) (inner,ctxt) with
    | Done(inner',_,ctxt) -> Done(inner',es,ctxt)
    | _ -> Error(ScopeError (",lambda",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))
  | LineSplitter.Application(b,inner) :: es -> 
    match (nested_id_application |> repeat) (inner,ctxt) with
    | Done(inner',[],ctxt) -> Done(Application(b,inner'),es,ctxt)
    | _ -> Error(ScopeError ("Error: expected id (also nested) inside brackets at",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))
  | _ -> Error(ScopeError ("Error: expected id (also nested) but could not find at",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))

and lambda_id_basicexpression =
  let open_block block = 
    match block with
    | LineSplitter.Block(b) -> b
  let getlambdainput lines =
    match lines with
    | x :: xs ->
      match (left_nested_id |> repeat) (x,Scope.Zero) with
      |Done(inner,_,ctxt) -> inner,xs
  let getlambdaoutput lines =
    match lines with
    | x :: xs ->
      match x with
      | LineSplitter.Application(Indent,[LineSplitter.Block(x)]) :: xs -> x
  
  let getpremise lines = 
    match (premise |> parse_first_line |> repeat) (lines,Scope.Zero) with
    | Done(p,_,ctxt) -> p
    | Error (p) -> []

  let return_lambda =
    fun (exprs,ctxt) ->
      match exprs with
      | LineSplitter.Block(x) :: es -> 
        let input,xs = getlambdainput x
        let output   = getlambdaoutput xs 
        let prem = getpremise output
        let out::prem = List.rev prem
        let prem = List.rev prem
        match out with
        | Conditional(x) ->
          Done(Lambda({Input = input; Output = x ; Premises = prem}),es,ctxt)
        | _ -> Error (ScopeError ("no return in lambda", Position.Zero))
  prs{
    let! argleft = left_nested_id |> repeat
    do! arrow
    let! dump = right_nested_id |> repeat
    return Lambda({Input = argleft; Output = dump; Premises = []})
  } .|| return_lambda
  //prs{ return Lambda({Input = []; Output = []; Premises = []})}

and line_to_id_basicexpression =
  fun (exprs,ctxt) ->
    match exprs with
    | x :: es -> 
      match (nested_id_application |> repeat) (x,ctxt) with
      | Done(inner',[],ctxt) -> Done(Application(Indent,inner'),es,ctxt)
      | _ -> Error(ScopeError ("Error: expected indent at",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))
    | _ -> Error(ScopeError ("Error: expected another line in scope at",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))

and left_nested_id : Parser<LineSplitter.BasicExpression, Scope, BasicExpression> =
  fun (exprs,ctxt) ->
  match exprs with
  | LineSplitter.Id(i,pos) :: es -> Done(Id(i,pos),es,ctxt)
  | LineSplitter.Application(Round,inner) :: es -> 
    match (nested_id_application |> repeat) (inner,ctxt) with
    | Done(inner',[],ctxt) -> Done(Application(Round,flatten_nested_id inner'),es,ctxt)
    | _ -> Error(ScopeError ("Error: expected id (also nested) inside brackets at %A",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))
  | _ -> Error(ScopeError ("Error: expected id (also nested) but could not find at %A",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))

and right_nested_id : Parser<LineSplitter.BasicExpression, Scope, BasicExpression> =
  fun (exprs,ctxt) ->
  match exprs with
  | LineSplitter.Id(i,pos) :: es -> Done(Id(i,pos),es,ctxt)
  | LineSplitter.Literal(l,pos) :: es -> Done(Literal(l,pos),es,ctxt)
  | LineSplitter.Application(Round,inner) :: es -> 
    match (nested_id_application |> repeat) (inner,ctxt) with
    | Done(inner',[],ctxt) -> Done(Application(Round,flatten_nested_id inner'),es,ctxt)
    | _ -> Error(ScopeError ("Error: expected id (also nested) inside brackets at %A",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))
  | _ -> Error(ScopeError ("Error: expected id (also nested) but could not find at %A",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))

and premise : Parser<LineSplitter.BasicExpression, Scope, Premise> =
  prs{
    do! not_empty
    let! i = left_nested_id |> repeat
    return! (prs{
              do! arrow
              let! o = right_nested_id |> repeat
              return Implication(i,o)
            }) .||
            (prs{
              do! eof
              return Conditional i
            })  
  }


let string_to_associativivaty (s:string) : Parser<LineSplitter.BasicExpression, Scope, Associativity> =
  fun (exprs,ctxt) ->
    if   s = "L" then Done(Left ,exprs,ctxt)
    elif s = "R" then Done(Right,exprs,ctxt)
    else Error(ScopeError (",L or R",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))

let type_expression : Parser<_, _, Type> = 
  prs{
    let! pos = getPosition
    let! i = left_nested_id
    let! is = left_nested_id |> repeat // or recursive application of id's, converted
    return i :: is
  }

let rec priority : Parser<_, _, int*Associativity> =
  prs{
    do! priorityarrow
    let! pri = int_literal 
    let! str = id .|| (prs{return "L"})
    let! ass = string_to_associativivaty str
    return pri,ass
  }

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
    do! arrow
    let! right_arguments = arguments()
    let! return_type = type_expression
    let! priority,associativity = priority .|| (prs{ return 0,Left })
    return  {
              Name              = name
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
    do! doublearrow
    let! right_arguments = typefunc_arguments()
    let! return_type = type_expression
    let! priority,associativity = priority .|| (prs{ return 0,Left })
    return  {
              Name              = name
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

let Arrowfunc_declaration : Parser<LineSplitter.BasicExpression,Scope,Unit> = 
  prs{
    do! arrowfunc
    let! sym_decl = symbol_declaration_body
    let! ctxt = getContext
    do! setContext { ctxt with ArrowFunctionDeclarations = sym_decl :: ctxt.ArrowFunctionDeclarations }
  }

let TypeAlias_declaration : Parser<LineSplitter.BasicExpression,Scope,Unit> = 
  prs{
    do! typealias
    let! sym_decl = typefunc_declaration_body
    let! ctxt = getContext
    do! setContext { ctxt with TypeAliasDeclarations = sym_decl :: ctxt.TypeAliasDeclarations }
  }

let import_declaration : Parser<LineSplitter.BasicExpression,Scope,Unit> = 
  prs{
    do! import
    let! parameter = id
    let! ctxt = getContext
    do! setContext { ctxt with ImportDeclaration = parameter :: ctxt.ImportDeclaration}
  }

let inherit_declaration : Parser<LineSplitter.BasicExpression,Scope,Unit> = 
  prs{
    do! inherit__
    let! parameter = id
    let! ctxt = getContext
    do! setContext { ctxt with InheritDeclaration = parameter :: ctxt.InheritDeclaration}
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

let skip_comment : Parser<LineSplitter.BasicExpression,Scope,Unit> = 
  prs{
    do! comment
  }

let line : Parser<LineSplitter.Line,_,_> = 
  fun (lines,ctxt) ->
    match lines with
    | l::ls -> Done(l,ls,ctxt)
    | _ -> Error(ScopeError ("Error: cannot extract line at",(LineSplitter.BasicExpression.tryGetNextPosition lines)))

let empty_line : Parser<LineSplitter.Line,_,_> = 
  prs{
    let! line = line
    match line with
    | [] -> return ()
    | _ -> return! fail (ScopeError ("Error: line is not empty!",Position.Zero))
  }

let rec skip_empty_lines() =
  prs{
    do! empty_line
    do! skip_empty_lines()
  } .|| (prs{ return () })


let typefunc_premise : Parser<LineSplitter.BasicExpression, Scope, Premise> =
  prs{
    do! not_empty
    let! i = left_nested_id |> repeat
    return! (prs{
              do! doublearrow
              let! o = right_nested_id |> repeat
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
    | _ -> Error (ScopeError (",----",(exprs |> LineSplitter.BasicExpression.tryGetNextPosition)))

let rule_io :Parser<LineSplitter.BasicExpression,Scope,_> = 
  prs{
    let! i = left_nested_id |> repeat              
    do! arrow
    let! o = right_nested_id |> repeat
    return i,o
  }

let rule : Parser<LineSplitter.Line, Scope, Unit> =
  prs{
    let! premises = premise |> parse_first_line |> repeat
    do! horizontal_bar |> parse_first_line
    let! i,o = rule_io |> parse_first_line
    let! ctxt = getContext
    do! setContext { ctxt with Rules = { Premises = premises; Input = i; Output = o } :: ctxt.Rules }
  } .|| prs{
    let! i,o = rule_io |> parse_first_line
    let! ctxt = getContext
    do! setContext { ctxt with Rules = { Premises = []; Input = i; Output = o } :: ctxt.Rules }
  }

let rec scope() : Parser<LineSplitter.Line, Scope, Scope> =
  prs{
    do! skip_empty_lines()
    do! (parse_first_line (Arrowfunc_declaration .|| skip_comment .|| import_declaration .|| inherit_declaration .|| func_declaration .|| 
                           typefunc_declaration .|| TypeAlias_declaration .|| data_declaration )) .|| 
                           typefunc_rule .|| rule
    do! skip_empty_lines()
    return! scope()
  } .|| 
  (eof >> getContext)

and scope_lines (pos:Position) :Parser<LineSplitter.BasicExpression,Scope,List<BasicExpression>> = 
  let rec extract app :(LineSplitter.Line List) = 
    match app with
    | LineSplitter.Block([[LineSplitter.Application(br,inner)]]) :: es -> (extract inner)
    | LineSplitter.Block([]::xs) :: es -> extract [LineSplitter.Block(xs)]
    | LineSplitter.Block([LineSplitter.Application(br,inner)]::xs) :: es -> (extract inner) @ extract [LineSplitter.Block(xs)]
    | LineSplitter.Application(br,inner) :: es -> extract inner
    | LineSplitter.Block(lines) :: es -> lines
  fun (exprs,ctxt) ->
    match exprs with
    | LineSplitter.Application(Curly,b) :: rest -> 
      let lines = extract b
      let modulename = (sprintf "mod-%s-r:%dc:%d" ctxt.CurrentNamespace pos.Line pos.Col)
      let newscp = {Scope.Zero with CurrentNamespace = modulename ; ImportDeclaration = [ctxt.CurrentNamespace]}
      match (scope() (lines,newscp)) with 
      | Done (res,_,_) -> 
        let ctxt' = {ctxt with Modules = (modulename,res)::ctxt.Modules }
        Done([(Module modulename)],exprs,ctxt')
      | Error (p) -> 
        printfn "%A" p
        Error (p) 
    | _ -> Done(([]),exprs,ctxt)
and typefunc_rule : Parser<LineSplitter.Line, Scope, Unit> =
  prs{
    let! premises = typefunc_premise |> parse_first_line |> repeat
    do! horizontal_bar |> parse_first_line
    let! i,o = typefunc_io |> parse_first_line
    let! ctxt = getContext
    do! setContext { ctxt with TypeFunctionRules = { Premises = premises; Input = i; Output = o } :: ctxt.TypeFunctionRules }
  } .|| prs{
    let! i,o = typefunc_io |> parse_first_line
    let! ctxt = getContext
    do! setContext { ctxt with TypeFunctionRules = { Premises = []; Input = i; Output = o } :: ctxt.TypeFunctionRules }
  }
and typefunc_io :Parser<LineSplitter.BasicExpression,Scope,_> = 
  prs{
    let! pos = getPosition
    let! i = left_nested_id |> repeat              
    do! doublearrow
    let! o = right_nested_id |> repeat
    let! inside_scope = scope_lines pos   
    return i,(o@inside_scope)
    //return i,o
  }


let build_scopes (lines:List<LineSplitter.Line>) (scp:Scope) : Option<Scope> =
  match scope() (lines,scp) with
  | Done (res,_,_) -> Some res
  | Error (p) ->
    //let test = ErrorType.expand e 
    do printfn "%A" (p)
    None

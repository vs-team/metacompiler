module DeclParser2

open Common
open ParserMonad
open Lexer2
open ExtractFromToken2
open GlobalSyntaxCheck2

type Associativity = Left | Right

type DeclType =
  | Id          of Id * Position
  | IdVar       of Id * Position
  | IdKind      of Id * Position
  | Arrow       of DeclType*DeclType
  | TypeArrow of DeclType*DeclType
  | Application of Id*DeclType*DeclType


type ArgStructure = LeftArg of DeclType*DeclType
                  | RightArgs of List<DeclType>

type ArgId = Id*Position

type FunctionBranch = 
  {
    Name              : Id
    Args              : List<ArgId>
    Pos               : Position
  }

type IdBranch =
  {
    Name              : Id
    Pos               : Position
  }

type PremisFunctionTree = Literal         of Literal*Position
                        | TypeRuleBranch  of FunctionBranch 
                        | TypeAliasBranch of FunctionBranch 
                        | IdBranch        of IdBranch

type Condition = Less | LessEqual | Greater | GreaterEqual | Equal

type Premises = Conditional of Condition*PremisFunctionTree*PremisFunctionTree
              | Implication of PremisFunctionTree*PremisFunctionTree

type SymbolDeclaration =
  {
    Name              : Id
    Args              : ArgStructure
    Return            : DeclType
    Priority          : int
    Premises          : List<Premises>
    Associativity     : Associativity
    Pos               : Position
  }

type DeclParseScope = 
  {
    Name             : Id
    DataDecl         : List<SymbolDeclaration>
    FuncDecl         : List<SymbolDeclaration>
    //ArrowDecl        : List<SymbolDeclaration>
    TypeDecl         : List<SymbolDeclaration>
    AliasDecl        : List<SymbolDeclaration>
  } with 
    static member Zero =
      {
        Name              = {Namespace = []; Name = ""}
        DataDecl          = []
        FuncDecl          = []
        //ArrowDecl         = []
        TypeDecl          = []
        AliasDecl         = []
      }
    static member add pc1 pc2=
      {
        Name              = pc2.Name
        DataDecl          = pc1.DataDecl  @ pc2.DataDecl
        FuncDecl          = pc1.FuncDecl  @ pc2.FuncDecl
        TypeDecl          = pc1.TypeDecl  @ pc2.TypeDecl
        AliasDecl         = pc1.AliasDecl @ pc2.AliasDecl
        //ArrowDecl         = pc1.ArrowDecl @ pc2.ArrowDecl
      }



let skip_newline :Parser<Token,_,_> =
  prs{do! (check_keyword() NewLine) |> repeat |> ignore}

let parse_single_arg :Parser<Token,DeclParseScope,DeclType> =
  prs{
    let! id,pos = extract_id()
    let! ctxt = getContext
    let id = {Namespace = ctxt.Name.Namespace ; Name = id}
    return Id(id,pos)
  } .|| prs{
    let! id,pos = extract_varid()
    let! ctxt = getContext
    let id = {Namespace = ctxt.Name.Namespace; Name = id}
    return IdVar(id,pos)
  } .|| prs{
    let! id,pos = extract_kindid()
    let! ctxt = getContext
    let id = {Namespace = ctxt.Name.Namespace; Name = id}
    return IdKind(id,pos)
  }

let rec parse_round :Parser<Token,DeclParseScope,DeclType> =
  prs{
    do! check_keyword() (Open Round)
    let! res = parse_arg
    do! check_keyword() (Close Round)
    return res
  }
and parse_lambda_signature :Parser<Token,DeclParseScope,DeclType> =
  prs{
    do! check_keyword() (Open Round)
    let! before_arrow = parse_round .|| parse_single_arg .|| parse_arg
    do! check_keyword() SingleArrow
    let! after_arrow = parse_arg
    do! check_keyword() (Close Round)
    return Arrow(before_arrow,after_arrow)
  }
and parse_type_lambda_signature :Parser<Token,DeclParseScope,DeclType> =
  prs{
    do! check_keyword() (Open Round)
    let! before_arrow = parse_round .|| parse_single_arg .|| parse_arg
    do! check_keyword() DoubleArrow
    let! after_arrow = parse_arg
    do! check_keyword() (Close Round)
    return TypeArrow(before_arrow,after_arrow)
  }
and parse_arg :Parser<Token,DeclParseScope,DeclType> =
  parse_lambda_signature .|| prs{
    let! before_symbol = parse_round .|| parse_single_arg
    let! symbol,pos = extract_id()
    let! after_symbol = parse_arg
    let! ctxt = getContext
    let symbol = {Namespace = ctxt.Name.Namespace;Name = symbol}
    return Application(symbol,before_symbol,after_symbol)
  } .|| parse_round .|| parse_single_arg

let parse_small_args (arrow:Keyword) :Parser<Token,DeclParseScope,DeclType> =
  prs{
      let! res = parse_arg
      do! check_keyword() arrow
      return res
   } 
  
let parse_arg_structure (arrow:Keyword) :Parser<Token,DeclParseScope,Id*ArgStructure*Position> =
  prs{
    let! left_arg = parse_small_args arrow
    let! name,pos = extract_string_literal()
    do! check_keyword() arrow
    let! right_arg = parse_small_args arrow 
    let! ctxt = getContext
    let name = {Namespace=ctxt.Name.Namespace;Name=name}
    return (name,LeftArg(left_arg,right_arg),pos)
  } .|| prs{
    let! name,pos = extract_string_literal()
    do! check_keyword() arrow
    let! right_args = (parse_small_args arrow) |> repeat
    let! ctxt = getContext
    let name = {Namespace=ctxt.Name.Namespace;Name=name}
    return (name,RightArgs(right_args),pos)
  }

let parse_assosiotivity :Parser<Token,DeclParseScope,Associativity> =
  prs{
    let! st,pos = extract_id()
    if st = "L" then return Left
    elif st = "R" then return Right
    else return! fail (ParserError pos)
  }

let lift_parse_decl (setctxt) (arrow:Keyword):Parser<Token,DeclParseScope,_> =
  prs{
    let! name,args,pos = parse_arg_structure arrow
    let! result = parse_arg
    let! (pri,_),ass = (check_keyword() PriorityArrow >>.
                        (extract_int_literal() .>>. parse_assosiotivity)) .||
                         prs{return((0,Position.Zero),Left)}
    let! ctxt = getContext
    let res = {Name = name ; Args = args; Return = result ; Priority = pri ; 
               Premises = [] ; Associativity = ass; Pos = pos}
    do! setContext (setctxt ctxt res)
  }

let parse_datadecl :Parser<Token,DeclParseScope,_> =
  prs{do! check_keyword() Data} >>. 
  lift_parse_decl (fun ctxt res -> {ctxt with DataDecl = res :: ctxt.DataDecl}) SingleArrow

let parse_funcdecl :Parser<Token,DeclParseScope,_> =
  prs{do! check_keyword() Func} >>. 
  lift_parse_decl (fun ctxt res -> {ctxt with FuncDecl = res :: ctxt.FuncDecl}) SingleArrow

let parse_typefuncdecl :Parser<Token,DeclParseScope,_> =
  prs{do! check_keyword() TypeFunc} >>. 
  lift_parse_decl (fun ctxt res -> {ctxt with TypeDecl = res :: ctxt.TypeDecl}) DoubleArrow

let parse_aliasdecl :Parser<Token,DeclParseScope,_> =
  prs{do! check_keyword() TypeAlias} >>. 
  lift_parse_decl (fun ctxt res -> {ctxt with AliasDecl = res :: ctxt.AliasDecl}) DoubleArrow


let parse_lines :Parser<Token,DeclParseScope,_> =
  prs{
    do! skip_newline
    do! parse_datadecl .|| parse_funcdecl .|| parse_typefuncdecl .|| parse_aliasdecl .|| 
         (UseDifferentCtxt check_rule Position.Zero)
    do! skip_newline
    return ()
  }

let parse_scope :Parser<Token,DeclParseScope,_> =
  prs{
    do! parse_lines |> itterate |> ignore
    return! getContext
  }


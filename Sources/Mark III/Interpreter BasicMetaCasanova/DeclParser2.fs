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
  | Arrow       of DeclType*DeclType
  | Application of Id*DeclType*DeclType


type ArgStructure = LeftArg of DeclType*DeclType
                  | RightArgs of List<DeclType>

type SymbolDeclaration =
  {
    Name              : string
    CurrentNamespace  : Namespace
    Args              : ArgStructure
    Return            : DeclType
    Priority          : int
    Associativity     : Associativity
    Pos               : Position
  }

type DeclParseScope = 
  {
    CurrentNamespace : Namespace
    DataDecl         : List<SymbolDeclaration>
    FuncDecl         : List<SymbolDeclaration>
    //ArrowDecl        : List<SymbolDeclaration>
    //TypeDecl         : List<SymbolDeclaration>
    //AliasDecl        : List<SymbolDeclaration>
  } with 
    static member Zero =
      {
        CurrentNamespace  = []
        DataDecl          = []
        FuncDecl          = []
        //ArrowDecl         = []
        //TypeDecl          = []
        //AliasDecl         = []
      }
    static member add pc1 pc2=
      {
        CurrentNamespace  = pc2.CurrentNamespace
        DataDecl          = pc1.DataDecl  @ pc2.DataDecl
        FuncDecl          = pc1.FuncDecl  @ pc2.FuncDecl
        //TypeDecl          = pc1.TypeDecl  @ pc2.TypeDecl
        //AliasDecl         = pc1.AliasDecl @ pc2.AliasDecl
        //ArrowDecl         = pc1.ArrowDecl @ pc2.ArrowDecl
      }



let skip_newline :Parser<Token,_,_> =
  prs{do! (check_keyword() NewLine) |> repeat |> ignore}

let parse_single_arg :Parser<Token,_,DeclType> =
  prs{
    let! id,pos = extract_id()
    return Id(id,pos)
  } .|| prs{
    let! id,pos = extract_varid()
    return IdVar(id,pos)
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
and parse_arg :Parser<Token,DeclParseScope,DeclType> =
  parse_lambda_signature .|| prs{
    let! before_symbol = parse_round .|| parse_single_arg
    let! symbol,pos = extract_id()
    let! after_symbol = parse_arg
    return Application(symbol,before_symbol,after_symbol)
  } .|| parse_round .|| parse_single_arg

let parse_small_args :Parser<Token,DeclParseScope,DeclType> =
  prs{
      let! res = parse_arg
      do! check_keyword() SingleArrow
      return res
   } 
  
let parse_arg_structure :Parser<Token,DeclParseScope,Id*ArgStructure*Position> =
  prs{
    let! left_arg = parse_small_args
    let! name,pos = extract_string_literal()
    do! check_keyword() SingleArrow
    let! right_arg = parse_small_args
    return (name,LeftArg(left_arg,right_arg),pos)
  } .|| prs{
    let! name,pos = extract_string_literal()
    do! check_keyword() SingleArrow
    let! right_args = parse_small_args |> repeat
    return (name,RightArgs(right_args),pos)
  }

let parse_assosiotivity :Parser<Token,DeclParseScope,Associativity> =
  prs{
    let! st,pos = extract_id()
    if st = "L" then return Left
    elif st = "R" then return Right
    else return! fail (ParserError pos)
  }

let lift_parse_decl (setctxt) :Parser<Token,DeclParseScope,_> =
  prs{
    let! name,args,pos = parse_arg_structure
    let! result = parse_arg
    let! (pri,_),ass = (check_keyword() PriorityArrow >>.
                        (extract_int_literal() .>>. parse_assosiotivity)) .||
                         prs{return((0,Position.Zero),Left)}
    let! ctxt = getContext
    let res = {Name = name ; CurrentNamespace = ctxt.CurrentNamespace ; Args = args;
                Return = result ; Priority = pri ; Associativity = ass;
                Pos = pos}
    do! setContext (setctxt ctxt res)
  }

let parse_datadecl :Parser<Token,DeclParseScope,_> =
  prs{do! check_keyword() Data} >>. 
  lift_parse_decl (fun ctxt res -> {ctxt with DataDecl = res :: ctxt.DataDecl})

let parse_funcdecl :Parser<Token,DeclParseScope,_> =
  prs{do! check_keyword() Func} >>. 
  lift_parse_decl (fun ctxt res -> {ctxt with FuncDecl = res :: ctxt.FuncDecl})

let parse_lines :Parser<Token,DeclParseScope,_> =
  prs{
    do! skip_newline
    do! parse_datadecl .|| parse_funcdecl .|| 
         (UseDifferentCtxt check_rule Position.Zero)
    do! skip_newline
    return ()
  }

let parse_scope :Parser<Token,DeclParseScope,_> =
  prs{
    do! parse_lines |> itterate |> ignore
    return! getContext
  }


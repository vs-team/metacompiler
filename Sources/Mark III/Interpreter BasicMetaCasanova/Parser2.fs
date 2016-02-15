module Parser2

open Common
open ParserMonad
open Lexer2

type Associativity = Left | Right

type DeclType =
  | Id          of Id * Position
  | IdVar       of Id * Position
  | Arrow       of DeclType*DeclType
  | Application of DeclType*DeclType
  | Tuple       of DeclType*DeclType
  | Bar         of DeclType*DeclType

type SymbolDeclaration =
  {
    Name            :string
    LeftArgs        :List<DeclType>
    RightArgs       :List<DeclType>
    Return          :DeclType
    Priority        :int
    Associativity   :Associativity
    Pos             :Position
  }

type Premise = Conditional of List<DeclType> * List<DeclType>
             | Implication of List<DeclType> * List<DeclType>
             | ArrowStruct of List<DeclType> * List<DeclType>

type Rule =
  {
    Input       : List<DeclType>
    Output      : List<DeclType>
    Premises    : List<Premise>
  }

type ParseScope = 
  {
    Import      : List<Id>
    DataDecl    : List<SymbolDeclaration>
    FuncDecl    : List<SymbolDeclaration>
    ArrowDecl   : List<SymbolDeclaration>
    TypeDecl    : List<SymbolDeclaration>
    AliasDecl   : List<SymbolDeclaration>
    FuncRule    : List<Id*Rule>
    TypeRule    : List<Id*Rule>
    AliasRule   : List<Id*Rule>
    Modules     : List<Id>
  } with 
    static member Zero =
      {
        Import      = []
        DataDecl    = []
        FuncDecl    = []
        ArrowDecl   = []
        TypeDecl    = []
        AliasDecl   = []
        FuncRule    = []
        TypeRule    = []
        Modules     = []
        AliasRule   = []
      }

let extract_keyword :Parser<Token,ParseScope,Keyword*Position> =
  prs{
    let! next = step
    match next with
    | Keyword(k,p) -> return k,p
    | _ -> return! (fail MatchError)
  }

let extract_id :Parser<Token,ParseScope,Id*Position> =
  prs{
    let! next = step
    match next with
    | Lexer2.Id(i,p) -> return i,p
    | _ -> return! (fail MatchError)
  }

let extract_varid :Parser<Token,ParseScope,Id*Position> =
  prs{
    let! next = step
    match next with
    | Lexer2.VarId(i,p) -> return i,p
    | _ -> return! (fail MatchError)
  }

let extract_string_literal :Parser<Token,ParseScope,string*Position> =
  prs{
    let! next = step
    match next with
    | Lexer2.Literal(String(str),pos) -> return str,pos
    | _ -> return! (fail MatchError)
  }

let extract_int_literal :Parser<Token,ParseScope,int*Position> =
  prs{
    let! next = step
    match next with
    | Lexer2.Literal(Int(i),pos) -> return i,pos
    | _ -> return! (fail MatchError)
  }

let check_keyword (expected:Keyword) :Parser<Token,ParseScope,_> =
  prs{
    let! k,p = extract_keyword
    if k = expected then return () else return! fail (ParserError p)
  }

let skip_newline :Parser<Token,ParseScope,_> =
  prs{do! (check_keyword NewLine) |> repeat |> ignore}

let parse_single_arg :Parser<Token,ParseScope,DeclType> =
  prs{
    let! id,pos = extract_id 
    return Id(id,pos)
  } .|| prs{
    let! id,pos = extract_varid 
    return IdVar(id,pos)
  }

let rec parse_arg :Parser<Token,ParseScope,DeclType> =
  prs{
    do! check_keyword (Open Round)
    let! res = parse_arg
    do! check_keyword (Close Round)
    return res
  } .|| prs{
    do! check_keyword (Open Round)
    let! before_arrow = parse_single_arg
    do! check_keyword SingleArrow
    let! after_arrow = parse_arg
    do! check_keyword (Close Round)
    return Arrow(before_arrow,after_arrow)
  } .|| prs{
    let! before_symbol = parse_single_arg
    let! symbol,pos = extract_id 
    let! after_symbol = parse_arg
    if symbol = "*" then return Tuple(before_symbol,after_symbol)
    elif symbol = "|" then return Bar(before_symbol,after_symbol)
    else return! fail (ParserError pos)
  } .|| prs{
    let! first_arg = parse_single_arg
    let! n_arg     = parse_arg
    return Application(first_arg,n_arg)
  } .|| prs{return! parse_single_arg}

let parse_small_args :Parser<Token,ParseScope,List<DeclType>> =
  prs{
    return! prs{
      let! res = parse_arg
      do! check_keyword SingleArrow
      return res
    } |> repeat
  }

let parse_assosiotivity :Parser<Token,ParseScope,Associativity> =
  prs{
    let! st,pos = extract_id
    if st = "L" then return Left
    elif st = "R" then return Right
    else return! fail (ParserError pos)
  }

let parse_data :Parser<Token,ParseScope,_> =
  prs{
    do! check_keyword Data
    let! left_args = parse_small_args
    let! name = extract_string_literal
    let! right_args = parse_small_args
    do! check_keyword SingleArrow
    let! result = parse_single_arg
    let! pri,ass = (check_keyword PriorityArrow >>.
                    (extract_int_literal .>>. parse_assosiotivity)) .||
                    prs{return((0,Position.Zero),Left)}
    let! ctxt = getContext
    do! setContext {ctxt with DataDecl = []}
  }

let parse_lines :Parser<Token,ParseScope,_> =
  prs{
    do! parse_data
    return ()
  }

let parse_scope :Parser<Token,ParseScope,_> =
  prs{
    do! parse_lines |> itterate |> ignore
    return! getContext
  }

let check_presens :Parser<Id*ParseScope,Id,Id*ParseScope> =
  prs{
    let! ctxt = getContext
    return! getFirstInstanceOf (fun (id,scp) -> id = ctxt)
  }

let parse_import :Parser<Token,List<Id*ParseScope>,List<Id*ParseScope>> =
  prs{
    let! id,pos = UseDifferentCtxt ((check_keyword Import) >>. extract_id) ParseScope.Zero
    let! ctxt = getContext
    return! UseDifferentSrcAndCtxt (check_presens |> repeat) ctxt id
  }

let parse2 :Parser<Token,List<Id*ParseScope>,ParseScope> =
  prs{
    let! res = UseDifferentCtxt parse_scope ParseScope.Zero
    return res
  }
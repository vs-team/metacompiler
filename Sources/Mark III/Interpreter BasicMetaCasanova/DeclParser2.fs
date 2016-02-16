module DeclParser2

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

type ParseScope = 
  {
    CurrentNamespace : Id
    DataDecl         : List<SymbolDeclaration>
    FuncDecl         : List<SymbolDeclaration>
    ArrowDecl        : List<SymbolDeclaration>
    TypeDecl         : List<SymbolDeclaration>
    AliasDecl        : List<SymbolDeclaration>
  } with 
    static member Zero =
      {
        CurrentNamespace  = ""
        DataDecl          = []
        FuncDecl          = []
        ArrowDecl         = []
        TypeDecl          = []
        AliasDecl         = []
      }
    static member add pc1 pc2=
      {
        CurrentNamespace  = pc2.CurrentNamespace
        DataDecl          = pc1.DataDecl  @ pc2.DataDecl
        TypeDecl          = pc1.TypeDecl  @ pc2.TypeDecl
        FuncDecl          = pc1.FuncDecl  @ pc2.FuncDecl
        AliasDecl         = pc1.AliasDecl @ pc2.AliasDecl
        ArrowDecl         = pc1.ArrowDecl @ pc2.ArrowDecl
      }


let extract_keyword() :Parser<Token,_,Keyword*Position> =
  prs{
    let! next = step
    match next with
    | Keyword(k,p) -> return k,p
    | _ -> return! (fail MatchError)
  }

let extract_id :Parser<Token,_,Id*Position> =
  prs{
    let! next = step
    match next with
    | Lexer2.Id(i,p) -> return i,p
    | _ -> return! (fail MatchError)
  }

let extract_varid :Parser<Token,_,Id*Position> =
  prs{
    let! next = step
    match next with
    | Lexer2.VarId(i,p) -> return i,p
    | _ -> return! (fail MatchError)
  }

let extract_string_literal :Parser<Token,_,string*Position> =
  prs{
    let! next = step
    match next with
    | Lexer2.Literal(String(str),pos) -> return str,pos
    | _ -> return! (fail MatchError)
  }

let extract_int_literal :Parser<Token,_,int*Position> =
  prs{
    let! next = step
    match next with
    | Lexer2.Literal(Int(i),pos) -> return i,pos
    | _ -> return! (fail MatchError)
  }

let check_keyword() (expected:Keyword) :Parser<Token,_,_> =
  prs{
    let! k,p = extract_keyword()
    if k = expected then return () else return! fail (ParserError p)
  }

let skip_newline :Parser<Token,_,_> =
  prs{do! (check_keyword() NewLine) |> repeat |> ignore}

let parse_single_arg :Parser<Token,_,DeclType> =
  prs{
    let! id,pos = extract_id
    return Id(id,pos)
  } .|| prs{
    let! id,pos = extract_varid 
    return IdVar(id,pos)
  }

let rec parse_arg :Parser<Token,ParseScope,DeclType> =
  prs{
    do! check_keyword() (Open Round)
    let! res = parse_arg
    do! check_keyword() (Close Round)
    return res
  } .|| prs{
    do! check_keyword() (Open Round)
    let! before_arrow = parse_single_arg
    do! check_keyword() SingleArrow
    let! after_arrow = parse_arg
    do! check_keyword() (Close Round)
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
      do! check_keyword() SingleArrow
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
    do! check_keyword() Data
    let! left_args = parse_small_args
    let! name,pos = extract_string_literal
    let! right_args = parse_small_args
    do! check_keyword() SingleArrow
    let! result = parse_single_arg
    let! (pri,_),ass = (check_keyword() PriorityArrow >>.
                        (extract_int_literal .>>. parse_assosiotivity)) .||
                         prs{return((0,Position.Zero),Left)}
    let res = {Name = name ; LeftArgs = left_args; RightArgs = right_args;
                Return = result ; Priority = pri ; Associativity = ass;
                Pos = pos}
    let! ctxt = getContext
    do! setContext {ctxt with DataDecl = res :: ctxt.DataDecl}
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

let check_presens :Parser<Id,List<Id*ParseScope>,Id*ParseScope> =
  prs{
    let! next = step
    let! ctxt = getContext
    if List.exists (fun (id,_) -> next = id) ctxt then
     return List.find (fun (id,_) -> next = id) ctxt
    else return! fail ImportError
  }

let parse_import :Parser<Token,List<Id*ParseScope>,Id> =
  prs{
    do! skip_newline
    let! id,pos = UseDifferentCtxt ((check_keyword() Import) >>. extract_id) ParseScope.Zero
    return id
  }

let import_to_parsescope :Parser<Token,List<Id*ParseScope>,ParseScope> =
  prs{
    let! imp = parse_import |> repeat
    let! ctxt = getContext
    let! scp = UseDifferentSrcAndCtxt (check_presens |> itterate) imp ctxt
    return List.fold (fun state (_,x) -> ParseScope.add state x) ParseScope.Zero scp
  }

let decl_parse (curnamespace:Id) :Parser<Token,List<Id*ParseScope>,ParseScope> =
  prs{
    let! scp = import_to_parsescope
    let  scp = {scp with CurrentNamespace = curnamespace}
    return! UseDifferentCtxt parse_scope scp
  }

let start_decl_parse :Parser<Id*List<Token>,List<Id*ParseScope>,ParseScope> =
  prs{
    let! id,token = step
    return ParseScope.Zero
  }
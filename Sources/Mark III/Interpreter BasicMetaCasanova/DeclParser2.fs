module DeclParser2

open Common
open ParserMonad
open Lexer2
open ExtractFromToken2
open GlobalSyntaxCheck2
open ParserTypes

let parse_single_arg :Parser<Token,ParseScope,DeclType> =
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

let rec parse_round :Parser<Token,ParseScope,DeclType> =
  prs{
    do! check_keyword() (Open Round)
    let! res = parse_arg
    do! check_keyword() (Close Round)
    return res
  }
and parse_lambda_signature :Parser<Token,ParseScope,DeclType> =
  prs{
    do! check_keyword() (Open Round)
    let! before_arrow = parse_round .|| parse_single_arg .|| parse_arg
    do! check_keyword() SingleArrow
    let! after_arrow = parse_arg
    do! check_keyword() (Close Round)
    return Arrow(before_arrow,after_arrow)
  }
and parse_type_lambda_signature :Parser<Token,ParseScope,DeclType> =
  prs{
    do! check_keyword() (Open Round)
    let! before_arrow = parse_round .|| parse_single_arg .|| parse_arg
    do! check_keyword() DoubleArrow
    let! after_arrow = parse_arg
    do! check_keyword() (Close Round)
    return TypeArrow(before_arrow,after_arrow)
  }
and parse_arg :Parser<Token,ParseScope,DeclType> =
  parse_lambda_signature .|| prs{
    let! before_symbol = parse_round .|| parse_single_arg
    let! symbol,pos = extract_id()
    let! after_symbol = parse_arg
    let! ctxt = getContext
    let symbol = {Namespace = ctxt.Name.Namespace;Name = symbol}
    return Application(symbol,[before_symbol;after_symbol])
  } .|| parse_round .|| parse_single_arg

let parse_small_args (arrow:Keyword) :Parser<Token,ParseScope,DeclType> =
  prs{
      let! res = parse_arg
      do! check_keyword() arrow
      return res
   } 
  
let parse_arg_structure (arrow:Keyword) 
  :Parser<Token,ParseScope,Id*ArgStructure*Position> =
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

let parse_assosiotivity :Parser<Token,ParseScope,Associativity> =
  prs{
    let! st,pos = extract_id()
    if st = "L" then return Left
    elif st = "R" then return Right
    else 
      let err = sprintf "expected l or r for assosiotivity but got: %s. %A" st pos
      return! fail (ParserError err)
  }

let lift_parse_decl (setctxt) (arrow:Keyword) (prem:Option<List<Premises>>)
  :Parser<Token,ParseScope,_> =
  prs{
    let! name,args,pos = parse_arg_structure arrow
    let! result = parse_arg
    let! (pri,_),ass = (check_keyword() PriorityArrow >>.
                        (extract_int_literal() .>>. parse_assosiotivity)) .||
                         prs{return((0,Position.Zero),Left)}
    let! ctxt = getContext 
    match prem with
    | Some(x) ->
      let res = {Name = name ; Args = args; Return = result ; Priority = pri ; 
             Premises = x; Associativity = ass; Pos = pos}
      do! setContext (setctxt ctxt res)
    | None -> 
      let res = {Name = name ; Args = args; Return = result ; Priority = pri ; 
             Premises = [] ; Associativity = ass; Pos = pos}
      do! setContext (setctxt ctxt res)
  }

let Lift_decl_premises decl_pars :Parser<Token,ParseScope,_> =
  prs{
    let! ctxt = getContext
    match ctxt.DeclFunctions.PremisFunctions with
    | Some(x) ->
      return! prs{
        let! premises = RepeatUntil x (check_keyword() HorizontalBar)
        do! (check_keyword() HorizontalBar) >>. (check_keyword() NewLine)
        do! decl_pars (Some(premises))
        return ()
      } .|| prs{
        do! decl_pars None
        return ()
      }
    | None -> return! fail (ParserError "there is no parser to parse the declaration premises.")
  }

let parse_datadecl (prem:Option<List<Premises>>) :Parser<Token,ParseScope,_> =
  prs{do! check_keyword() Data} >>. 
  lift_parse_decl (fun ctxt res -> {ctxt with DataDecl = res :: ctxt.DataDecl}) SingleArrow prem

let parse_funcdecl (prem:Option<List<Premises>>) :Parser<Token,ParseScope,_> =
  prs{do! check_keyword() Func} >>. 
  lift_parse_decl (fun ctxt res -> {ctxt with FuncDecl = res :: ctxt.FuncDecl}) SingleArrow prem

let parse_typefuncdecl (prem:Option<List<Premises>>) :Parser<Token,ParseScope,_> =
  prs{do! check_keyword() TypeFunc} >>. 
  lift_parse_decl (fun ctxt res -> {ctxt with TypeDecl = res :: ctxt.TypeDecl}) DoubleArrow prem

let parse_aliasdecl (prem:Option<List<Premises>>) :Parser<Token,ParseScope,_> =
  prs{do! check_keyword() TypeAlias} >>. 
  lift_parse_decl (fun ctxt res -> {ctxt with AliasDecl = res :: ctxt.AliasDecl}) DoubleArrow prem


let parse_decl_line :Parser<Token,ParseScope,ParseScope> =
  prs{
    // to later add premises to the declarations add :
    // Lift_decl_premises 
    // before the decl parsers and remove the none. 
    
    do! (parse_datadecl None) .|| 
        (parse_funcdecl None) .|| 
        (parse_typefuncdecl None) .|| 
        (parse_aliasdecl None)
    let! ctxt = getContext
    return ctxt
  }


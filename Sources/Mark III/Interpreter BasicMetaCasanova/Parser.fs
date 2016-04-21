module Parser

open Common 
open ParserMonad
open Lexer2
open ParserAST
open ExtractFromToken2

let skip_newlines :Parser<Token,List<string>*Program,_> =
  prs{
    do! check_keyword() NewLine |> repeat |> ignore
  }

let parse_imports :Parser<Token,List<string>*Program,_> =
  prs{
    do! skip_newlines
    do! check_keyword() Import
    let! str,_ = extract_id()
    let! ns,(ls,ctxt) = getContext
    do! setContext (ns,((str::ls),ctxt))
  }

let rec arg_list_to_typedecl (args:List<CallArg>) : TypeDecl =
  match args with
  | [x] -> Arg(x)
  | x::xs -> 
    let next = arg_list_to_typedecl xs
    Arrow(Arg(x),next)
  | [] -> Zero

let rec parse_arg :Parser<Token,List<string>*Program,CallArg> =
  prs{
    let! id,pos = extract_id()
    let! ns,_ = getContext
    return Id({Namespace = ns ; Name = id},pos)
  } 

and parse_arg_and_arrow (arrow:Keyword):Parser<Token,List<string>*Program,CallArg> =
  prs{
    let! arg = parse_arg
    do! check_keyword() arrow
    return arg
  }

let insert_decl_into_ctxt (key:Keyword) (sym:SymbolDeclaration)
  :Parser<Token,List<string>*Program,_> =
  prs{
    let! ns,(imp,(decl,def)) = getContext
    match key with
    | Lexer2.Data -> do! setContext (ns,(imp,((Data(sym)::decl),def))) 
    | Lexer2.Func -> do! setContext (ns,(imp,((Func(sym)::decl),def))) 
    | Lexer2.TypeFunc -> do! setContext (ns,(imp,((TypeFunc(sym)::decl),def))) 
    | Lexer2.TypeAlias -> do! setContext (ns,(imp,((TypeAlias(sym)::decl),def)))
    | err -> return! fail (ParserError (sprintf "expected a decl keyword but got: %A" err))
  }

let parse_call (arrow:Keyword) :Parser<Token,List<string>*Program,Premise> =
  prs{
    let! l_id = parse_arg |> repeat
    do! check_keyword() arrow
    let! r_id = parse_arg |> repeat
    do! check_keyword() NewLine
    return FunctionCall(l_id,[])
  }

let parse_premises (arrow:Keyword) :Parser<Token,List<string>*Program,List<Premise>> =
  prs{
    let! prem = parse_call arrow |> repeat
    return prem
  }

let parse_conclusion (arrow:Keyword) :Parser<Token,List<string>*Program,_> =
  prs{
    let! l_id = parse_arg |> repeat
    do! check_keyword() SingleArrow
    let! r_id = parse_arg |> repeat
    do! check_keyword() NewLine
    return ValueOutput(l_id,r_id)
  }

let parse_symbol (arrow:Keyword) :Parser<Token,List<string>*Program,SymbolDeclaration> =
  prs{
    let! arg_left = parse_arg_and_arrow arrow |> repeat
    let! name,pos = extract_string_literal()
    do! check_keyword() SingleArrow 
    let! arg_right = parse_arg_and_arrow arrow |> repeat
    let! ret = parse_arg 
    do! check_keyword() NewLine
    let! ns,_ = getContext
    let name = {Namespace = ns ; Name = name}
    let args = arg_list_to_typedecl (arg_left@arg_right)
    let ret = Arg(ret) 
    let res = {Name = name ; Args = args ; Return = ret ;
               Order = Infix; Priority = 0 ; Position = pos;
               Associativity = Left ;Premises = []}
    return res
  }

let parse_lift_decl (key:Keyword) (arrow:Keyword) :Parser<Token,List<string>*Program,_> =
  prs{
    do! skip_newlines
    do! check_keyword() key
    let! res = parse_symbol arrow
    do! insert_decl_into_ctxt key res
  }

let parse_decl :Parser<Token,List<string>*Program,_> =
  parse_lift_decl Lexer2.Data SingleArrow .|| 
  parse_lift_decl Lexer2.Func SingleArrow .||
  parse_lift_decl Lexer2.TypeFunc DoubleArrow .|| 
  parse_lift_decl Lexer2.TypeAlias DoubleArrow

let parse_rule (arrow:Keyword) :Parser<Token,List<string>*Program,_> =
  prs{
    do! skip_newlines
    let! prem = parse_premises arrow
    do! check_keyword() HorizontalBar
    do! check_keyword() NewLine
    let! concl = parse_conclusion arrow
    let! (ns,(im,(decl,def))) = getContext
    let res = Rule(prem,concl)
    do! setContext (ns,(im,(decl,(res::def))))
  }

let parse_tokens :Parser<Token,List<string>*Program,Program> =
  prs{
    let pars = parse_imports .|| parse_decl .|| 
               parse_rule SingleArrow .|| parse_rule DoubleArrow
    do! pars |> itterate |> ignore
    do! check_keyword() NewLine |> repeat |> ignore
    let! _,ctxt = getContext
    return ctxt
  }
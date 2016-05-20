module Parser

open Common 
open ParserMonad
open Lexer2
open ParserAST
open ExtractFromToken2

type Priass = Pri of int
            | Ass of Associativity

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

let rec arg_list_to_typedecl (args:List<TypeDecl>) : TypeDecl =
  match args with
  | [x] -> x
  | x::xs -> 
    let next = arg_list_to_typedecl xs
    Arrow(x,next)
  | [] -> Zero

let parse_arg :Parser<Token,List<string>*Program,CallArg> =
  prs{
    let! id,pos = extract_id()
    let! ns,_ = getContext
    return Id({Namespace = ns ; Name = id},pos)
  } 

let parse_decl_arg :Parser<Token,List<string>*Program,TypeDecl> =
  prs{
    let! arg = parse_arg
    return Arg(arg)
  }

let parse_id :Parser<Token,List<string>*Program,Id> =
  prs{
    let! id,_ = extract_id()
    let! ns,_ = getContext
    return {Namespace = ns ; Name = id}
  }

let rec parse_typearg :Parser<Token,List<string>*Program,TypeDecl> =
  prs{
    do! check_keyword() (Open Round)
    let! args = (parse_arg_and_arrow SingleArrow) |> repeat
    let! arg = parse_typearg
    let args = args @ [arg]
    do! check_keyword() (Close Round)
    return (arg_list_to_typedecl args)
  } .|| prs{
    let! id = parse_id
    do! check_keyword() (Open Angle)
    let! gen = parse_decl_arg |> repeat
    do! check_keyword() (Close Angle)
    return Generic(id,gen)
  } .|| parse_decl_arg

and parse_arg_and_arrow (arrow:Keyword):Parser<Token,List<string>*Program,TypeDecl> =
  prs{
    let! arg = parse_typearg
    do! check_keyword() arrow
    return arg
  }

let insert_decl_into_ctxt (key:Keyword) (sym:SymbolDeclaration)
  :Parser<Token,List<string>*Program,_> =
  prs{
    let! ns,(imp,(decl,def,is)) = getContext
    match key with
    | Lexer2.Data -> do! setContext (ns,(imp,((Data(sym)::decl),def,is))) 
    | Lexer2.Func -> do! setContext (ns,(imp,((Func(sym)::decl),def,is))) 
    | Lexer2.TypeFunc -> do! setContext (ns,(imp,((TypeFunc(sym)::decl),def,is))) 
    | Lexer2.TypeAlias -> do! setContext (ns,(imp,((TypeAlias(sym)::decl),def,is)))
    | err -> return! fail (ParserError (sprintf "expected a decl keyword but got: %A" err))
  }

let rec parse_lambda_and_arg :Parser<Token,List<string>*Program,CallArg> =
  parse_arg .|| prs{
    do! check_keyword() (Open Common.Lambda)
    let! l_concl = parse_arg |> repeat
    do! check_keyword() SingleArrow
    let! r_concl = parse_arg |> repeat
    do! check_keyword() (Close Round)
    return Lambda(ValueOutput(l_concl,r_concl),[])
  } .|| prs{
    do! check_keyword() (Open Common.Lambda)
    let! l_concl = parse_arg |> repeat
    do! check_keyword() SingleArrow
    do! check_keyword() NewLine
    let! prem = (parse_call SingleArrow .|| parse_condition) |> repeat
    let! r_concl = parse_arg |> repeat
    do! check_keyword() (Close Round) 
      .|| (check_keyword() NewLine .>> check_keyword() (Close Round))
    return Lambda(ValueOutput(l_concl,r_concl),prem)
  }

and parse_call (arrow:Keyword) :Parser<Token,List<string>*Program,Premise> =
  prs{
    let! l_id = parse_lambda_and_arg |> repeat
    do! check_keyword() arrow
    let! r_id = parse_id |> repeat
    do! check_keyword() NewLine
    return FunctionCall(l_id,r_id)
  }

and parse_condition :Parser<Token,List<string>*Program,Premise> =
  prs{
    let! l_id = parse_lambda_and_arg |> repeat
    let! con = check_condition() 
    let! r_id = parse_lambda_and_arg |> repeat
    do! check_keyword() NewLine
    return Conditional(l_id,con,r_id)
  }

and parse_premises (arrow:Keyword) :Parser<Token,List<string>*Program,List<Premise>> =
  prs{
    let! prem = RepeatUntil (parse_call arrow .|| parse_condition) (check_keyword() HorizontalBar)
    return prem
  }

and parse_conclusion (arrow:Keyword) :Parser<Token,List<string>*Program,Conclusion> =
  prs{
    let! l_id = parse_arg |> repeat
    do! check_keyword() arrow
    let! r_id = parse_arg |> repeat
    do! check_keyword() NewLine
    return ValueOutput(l_id,r_id)
  }

let rec priass_list_to_priass ((pri,ass):int*Associativity) (pa:List<Priass>) :int*Associativity =
  match pa with
  | (Pri x)::xs -> priass_list_to_priass (x,ass) xs
  | (Ass x)::xs -> priass_list_to_priass (pri,x) xs
  | [] -> (pri,ass)

let parse_priority :Parser<Token,List<string>*Program,int*Associativity> =
  prs{
    return! pifelse (check_keyword() PriorityArrow)(
      prs{
        let parse_pri_and_ass = (
          prs{
            let! pri,_ = extract_int_literal() 
            return Pri pri
          } .|| prs{
            let! ass,_ = extract_id()
            if ass = "L" then return Ass Left 
            elif ass = "R" then return Ass Right
            else return! fail (ParserError "Not L or R as associotivity.")
          })
        let! priass_list =  RepeatUntil parse_pri_and_ass (check_keyword() NewLine)
        return priass_list_to_priass (0,Left) priass_list
      }) (prs{return 0,Left}
    )
  }

let parse_symbol (arrow:Keyword) (prem:List<Premise>) :Parser<Token,List<string>*Program,SymbolDeclaration> =
  prs{
    let! arg_left = parse_arg_and_arrow arrow |> repeat
    let! name,pos = extract_string_literal()
    do! check_keyword() arrow
    let! arg_right = parse_arg_and_arrow arrow |> repeat
    let! ret = parse_typearg 
    let! pri,ass = parse_priority
    do! check_keyword() NewLine
    let! ns,_ = getContext
    let order = if (List.length arg_left) < 1 then Infix else Prefix
    let name = {Namespace = ns ; Name = name}
    let args = arg_list_to_typedecl (arg_left@arg_right)
    let res = {Name = name ; Args = args ; Return = ret ;
               Order = order; Priority = pri ; Position = pos;
               Associativity = ass ;Premises = prem}
    return res
  }

let parse_lift_decl (key:Keyword) (arrow:Keyword) :Parser<Token,List<string>*Program,_> =
  prs{
    do! skip_newlines
    let! prem = 
      parse_premises arrow .>> (check_keyword() HorizontalBar .>> check_keyword() NewLine)
    if (List.length prem) < 1 
    then return! fail (ParserError "premises do not match declaration.") else
    do! check_keyword() key
    let! res = parse_symbol arrow prem
    do! insert_decl_into_ctxt key res
  } .|| prs{
    do! skip_newlines
    do! check_keyword() key
    let! res = parse_symbol arrow []
    do! insert_decl_into_ctxt key res
  }

let parse_decl :Parser<Token,List<string>*Program,_> =
  parse_lift_decl Lexer2.Data SingleArrow .| 
  parse_lift_decl Lexer2.Func SingleArrow .|
  parse_lift_decl Lexer2.TypeFunc DoubleArrow .|
  parse_lift_decl Lexer2.TypeAlias DoubleArrow

let lift_parse_rule (arrow:Keyword) (prem:List<Premise>) :Parser<Token,List<string>*Program,_> =
  prs{
    let! concl = parse_conclusion arrow
    let! (ns,(im,(decl,def,is))) = getContext
    match arrow with
    | SingleArrow -> do! setContext (ns,(im,(decl,((Rule(prem,concl))::def),is)))
    | DoubleArrow -> do! setContext (ns,(im,(decl,((TypeRule(prem,concl))::def),is)))
    | _ -> return! fail (ParserError (sprintf "you can't use %A in the ruleparser as a arrow." arrow))
  }

let parse_rule (arrow:Keyword) :Parser<Token,List<string>*Program,_> =
  prs{
    do! skip_newlines
    let! prem = parse_premises arrow
    do! check_keyword() HorizontalBar
    do! check_keyword() NewLine
    return! lift_parse_rule arrow prem
  } .|| (skip_newlines >>. lift_parse_rule arrow [])

let parse_is :Parser<Token,List<string>*Program,_> =
  prs{
    do! skip_newlines
    let! left = parse_typearg
    do! check_keyword() Is
    let! right = parse_typearg
    do! check_keyword() NewLine
    let! (ns,(im,(decl,def,is))) = getContext
    do! setContext (ns,(im,(decl,def,((left,right)::is))))
  }

let parse_tokens :Parser<Token,List<string>*Program,Program> =
  prs{
    let pars = parse_imports .|| parse_decl .|| parse_is .||
               parse_rule SingleArrow .|| parse_rule DoubleArrow
    do! pars |> itterate |> ignore
    do! check_keyword() NewLine |> repeat |> ignore
    let! _,ctxt = getContext
    return ctxt
  }
module Parser

open Common 
open ParserMonad
open Lexer2
open ParserAST
open ExtractFromToken2

type Priass = Pri of int
            | Ass of Associativity

type ParserCtxt = 
  {
    ns : List<string>
    prog : Program
    prog_parser : Parser<Token,ParserCtxt,Program>
  } 

let skip_newlines :Parser<Token,ParserCtxt,_> =
  prs{
    do! check_keyword() NewLine |> repeat |> ignore
  }

let parse_imports :Parser<Token,ParserCtxt,_> =
  prs{
    do! skip_newlines
    do! check_keyword() Import
    let! str,_ = extract_id()
    let! ctxt = getContext
    let imp,pro = ctxt.prog
    do! setContext {ctxt with prog = (str::imp),pro}
  }

let rec arg_list_to_typedecl (args:List<TypeDecl>) : TypeDecl =
  match args with
  | [x] -> x
  | x::xs -> 
    let next = arg_list_to_typedecl xs
    Arrow(x,next)
  | [] -> Zero

let parse_arg :Parser<Token,ParserCtxt,CallArg> =
  prs{
    let! id,pos = extract_id()
    let! ctxt = getContext
    return Id({Namespace = ctxt.ns ; Name = id},pos)
  } 

let parse_decl_arg :Parser<Token,ParserCtxt,TypeDecl> =
  prs{
    let! arg = parse_arg
    return Arg(arg)
  }

let parse_id :Parser<Token,ParserCtxt,Id> =
  prs{
    let! id,_ = extract_id()
    let! ctxt = getContext
    return {Namespace = ctxt.ns ; Name = id}
  }

let rec parse_typearg :Parser<Token,ParserCtxt,TypeDecl> =
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

and parse_arg_and_arrow (arrow:Keyword):Parser<Token,ParserCtxt,TypeDecl> =
  prs{
    let! arg = parse_typearg
    do! check_keyword() arrow
    return arg
  }

let insert_decl_into_ctxt (key:Keyword) (sym:SymbolDeclaration)
  :Parser<Token,ParserCtxt,_> =
  prs{
    let! ctxt = getContext
    let (imp,(decl,def,is)) = ctxt.prog
    match key with
    | Lexer2.Data ->      do! setContext {ctxt with prog = (imp,(     (Data(sym)::decl),def,is))} 
    | Lexer2.Func ->      do! setContext {ctxt with prog = (imp,(     (Func(sym)::decl),def,is))}
    | Lexer2.TypeFunc ->  do! setContext {ctxt with prog = (imp,( (TypeFunc(sym)::decl),def,is))}
    | Lexer2.TypeAlias -> do! setContext {ctxt with prog = (imp,((TypeAlias(sym)::decl),def,is))}
    | err -> return! fail (ParserError (sprintf "expected a decl keyword but got: %A" err))
  }

let parse_left_lambda_conclusion_arg :Parser<Token,ParserCtxt,CallArg*TypeDecl> =
  prs{
    do! check_keyword() (Open Round)
    let! concl = parse_arg
    do! check_keyword() (Close Round)
    do! check_id() ":"
    let! concl_type = parse_typearg
    return (concl, concl_type)
  }

let rec parse_lambda_and_arg :Parser<Token,ParserCtxt,CallArg> =
  parse_arg .|| prs{
    do! check_keyword() (Open Common.Lambda)
    let! l_concl = parse_left_lambda_conclusion_arg |> repeat
    do! check_keyword() SingleArrow
    let! r_concl = parse_arg |> repeat
    do! check_keyword() (Close Round)
    return Lambda((l_concl,r_concl),[])
  } .|| prs{
    do! check_keyword() (Open Common.Lambda)
    let! l_concl = parse_left_lambda_conclusion_arg |> repeat
    do! check_keyword() SingleArrow
    do! check_keyword() NewLine
    let! prem = (parse_call SingleArrow .|| parse_condition) |> repeat
    let! r_concl = parse_arg |> repeat
    do! check_keyword() (Close Round) 
      .|| (check_keyword() NewLine .>> check_keyword() (Close Round))
    return Lambda((l_concl,r_concl),prem)
  }

and parse_call (arrow:Keyword) :Parser<Token,ParserCtxt,Premise> =
  prs{
    let! l_id = parse_lambda_and_arg |> repeat
    do! check_keyword() arrow
    let! r_id = parse_id |> repeat
    do! check_keyword() NewLine
    return FunctionCall(l_id,r_id)
  }

and parse_condition :Parser<Token,ParserCtxt,Premise> =
  prs{
    let! l_id = parse_lambda_and_arg |> repeat
    let! con = check_condition() 
    let! r_id = parse_lambda_and_arg |> repeat
    do! check_keyword() NewLine
    return Conditional(l_id,con,r_id)
  }

let parse_premises (arrow:Keyword) :Parser<Token,ParserCtxt,List<Premise>> =
  prs{
    let! prem = RepeatUntil (parse_call arrow .| parse_condition) (check_keyword() HorizontalBar)
    return prem
  }

let parse_conclusion (arrow:Keyword) :Parser<Token,ParserCtxt,Conclusion> =
  prs{
    let! l_id = parse_arg |> repeat
    do! check_keyword() arrow
    let! r_id = parse_arg |> repeat
    return! pifelse (check_keyword() (Open Curly))( 
      prs{
        let! ctxt = getContext
        do! check_keyword() NewLine
        let! prog = ctxt.prog_parser
        do! skip_newlines .>> (check_keyword() (Close Curly))
        return ModuleOutput(l_id,r_id,prog)
      }) (prs{
        do! check_keyword() NewLine
        return ValueOutput(l_id,r_id)
      })
  }

let rec priass_list_to_priass ((pri,ass):int*Associativity) (pa:List<Priass>) :int*Associativity =
  match pa with
  | (Pri x)::xs -> priass_list_to_priass (x,ass) xs
  | (Ass x)::xs -> priass_list_to_priass (pri,x) xs
  | [] -> (pri,ass)

let parse_priority :Parser<Token,ParserCtxt,int*Associativity> =
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

let parse_symbol (arrow:Keyword) (prem:List<Premise>) :Parser<Token,ParserCtxt,SymbolDeclaration> =
  prs{
    let! arg_left = parse_arg_and_arrow arrow |> repeat
    let! name,pos = extract_string_literal()
    do! check_keyword() arrow
    let! arg_right = parse_arg_and_arrow arrow |> repeat
    let! ret = parse_typearg 
    let! pri,ass = parse_priority
    do! check_keyword() NewLine
    let! ctxt = getContext
    let order = if (List.length arg_left) = 0 then Infix else Prefix
    let name = {Namespace = ctxt.ns ; Name = name}
    let args = arg_list_to_typedecl (arg_left@arg_right)
    let res = {Name = name ; Args = args ; Return = ret ;
               Order = order; Priority = pri ; Position = pos;
               Associativity = ass ;Premises = prem}
    return res
  }

let parse_lift_decl (key:Keyword) (arrow:Keyword) :Parser<Token,ParserCtxt,_> =
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

let parse_decl :Parser<Token,ParserCtxt,_> =
  parse_lift_decl Lexer2.Data SingleArrow .| 
  parse_lift_decl Lexer2.Func SingleArrow .|
  parse_lift_decl Lexer2.TypeFunc DoubleArrow .|
  parse_lift_decl Lexer2.TypeAlias DoubleArrow

let lift_parse_rule (arrow:Keyword) (prem:List<Premise>) :Parser<Token,ParserCtxt,_> =
  prs{
    let! concl = parse_conclusion arrow
    let! ctxt = getContext
    let (im,(decl,def,is)) = ctxt.prog
    match arrow with
    | SingleArrow -> do! setContext {ctxt with prog = (im,(decl,(    (Rule(prem,concl))::def),is))}
    | DoubleArrow -> do! setContext {ctxt with prog = (im,(decl,((TypeRule(prem,concl))::def),is))}
    | _ -> return! fail (ParserError (sprintf "you can't use %A in the ruleparser as a arrow." arrow))
  }

let parse_rule (arrow:Keyword) :Parser<Token,ParserCtxt,_> =
  prs{
    do! skip_newlines
    let! prem = parse_premises arrow
    do! check_keyword() HorizontalBar
    do! check_keyword() NewLine
    return! lift_parse_rule arrow prem
  } .|| (skip_newlines >>. lift_parse_rule arrow [])

let parse_is :Parser<Token,ParserCtxt,_> =
  prs{
    do! skip_newlines
    let! left = parse_typearg
    do! check_keyword() Is
    let! right = parse_typearg
    do! check_keyword() NewLine
    let! ctxt = getContext
    let (im,(decl,def,is)) = ctxt.prog
    do! setContext {ctxt with prog = (im,(decl,def,((left,right)::is)))}
  }

let parse_lift (itterator) :Parser<Token,ParserCtxt,Program> =
  prs{
    let pars = parse_imports .| parse_decl .| parse_is .|
               parse_rule SingleArrow .| parse_rule DoubleArrow .|
               (check_keyword() NewLine .>> skip_newlines)
    do! (itterator pars) |> ignore
    let! ctxt = getContext
    return ctxt.prog
  }

let parse_module :Parser<Token,ParserCtxt,Program> =
  prs{
    let modpars = PrintPosibleError (AddPriority 100 (parse_lift (fun it -> RepeatUntil it (skip_newlines .>> check_keyword() (Close Curly)))))
    let! ctxt = getContext
    let! prog = UseDifferentCtxt modpars {ctxt with prog = ([],([],[],[]))}
    return prog
  }

let parse_tokens :Parser<Token,ParserCtxt,Program> =
  parse_lift itterate
module RuleParser2

open ParserMonad
open Lexer2
open Common
open ParserTypes
open DeclParser2
open GlobalSyntaxCheck2
open ExtractFromToken2

let argstructure_parser (argstruct:ArgStructure) 
  :Parser<Token,ParseScope,string*List<ArgId>*Position> =
  prs{
    match argstruct with
    | LeftArg(_) -> 
      let! left_id,lpos = extract_id()
      let! name,pos    = extract_id()
      let! right_id,rpos = extract_id()
      let! ctxt = getContext
      let left_id = {Namespace = ctxt.Name.Namespace; Name = left_id}
      let right_id = {Namespace = ctxt.Name.Namespace; Name = right_id}
      return (name,((left_id,pos)::(right_id,pos)::[]),pos)
    | RightArgs (ls) -> 
      let! name,pos = extract_id()
      let! rightargs = extract_id() |> repeat
      let! ctxt = getContext
      let rightargs:List<ArgId> = 
        List.map (fun (id,pos) -> 
          {Namespace = ctxt.Name.Namespace; Name = id},pos) rightargs
      return (name,rightargs,pos)
  }

let decl_to_parser (decl:SymbolDeclaration):Parser<Token,ParseScope,FunctionBranch> =
  prs{
    let! name,args,pos = argstructure_parser decl.Args
    if name = decl.Name.Name then 
      return {Name = {Namespace = decl.Name.Namespace; Name = name} ; 
              Args = args ; Pos = pos }
    else
      return! fail (ParserError (sprintf "%A" (name,pos)))
  }

let id_to_parser :Parser<Token,ParseScope,PremisFunctionTree> =
  prs{
    let! id,pos = extract_id()
    let! ctxt = getContext
    return IdBranch({Name = {Namespace=ctxt.Name.Namespace;Name=id} ; Pos = pos})
  }

let datas_to_parser (datas:List<SymbolDeclaration>) 
  :Parser<Token,ParseScope,PremisFunctionTree> =
  (prs{
    let! res = FirstSuccesfullInList datas decl_to_parser
    return DataBranch res
  }) .|| id_to_parser

let literal_to_parser :Parser<Token,ParseScope,PremisFunctionTree> =
  prs{
    let! lit,pos = extract_literal()
    return Literal(lit,pos)
  }
let type_alias_to_parser (aliases:List<SymbolDeclaration>) 
  :Parser<Token,ParseScope,PremisFunctionTree> =
  (prs{
    let! res = FirstSuccesfullInList aliases decl_to_parser
    return TypeAliasBranch res
  }) .|| id_to_parser

let type_func_to_parser (ctxt:ParseScope) 
  :Parser<Token,ParseScope,PremisFunctionTree> =
  (prs{
    let! res = FirstSuccesfullInList ctxt.TypeDecl decl_to_parser
    return TypeRuleBranch res
    
  }) .|| type_alias_to_parser ctxt.AliasDecl .|| literal_to_parser

let decls_to_parser (ctxt:ParseScope) 
  :Parser<Token,ParseScope,PremisFunctionTree> =
  (prs{
    let! res = FirstSuccesfullInList ctxt.FuncDecl decl_to_parser
    return RuleBranch res
  }) .|| datas_to_parser ctxt.DataDecl .|| literal_to_parser

let implication_premis (arrow:Keyword) l r :Parser<Token,ParseScope,Premises> =
  prs{
    let! ctxt = getContext
    let! left = l ctxt
    do! check_keyword() arrow
    let! right = r ctxt.DataDecl .>> (check_keyword() NewLine)
    //let! right,_ = extract_id() .>> (check_keyword() NewLine)
    return Implication (left,right)
  }

let parse_premis (arrow:Keyword) 
  (l:(ParseScope -> Parser<Token,ParseScope,PremisFunctionTree>)) 
  r :Parser<Token,ParseScope,Premises> =
  implication_premis arrow l r .|| prs{
    let! ctxt = getContext
    let! left = (l ctxt)
    let! condition,pos = extract_predicate()
    let! right = r ctxt.DataDecl
    do! check_keyword() NewLine
    return Conditional (condition,left,right)
  }

let parser_module :Parser<Token,ParseScope,ModuleScope*Position> =
  prs{
    let! ctxt = getContext
    match ctxt.DeclFunctions.ModuleParser with
    | Some(p) ->
      do! check_keyword() (Open(Curly))
      let! mo = p
      return {Inherit = []; Scope = mo},Position.Zero

  }

let parse_rule (pf:ParserRuleFunctions) 
  :Parser<Token,ParseScope,RuleDef> =
  prs{
    let! premises = RepeatUntil (parse_premis pf.PremiseArrow pf.LeftPremiseParser pf.RightPremiseParser) (check_keyword() HorizontalBar)
    do! (check_keyword() HorizontalBar) >>. (check_keyword() NewLine)
    let! ctxt = getContext
    let! input = FirstSuccesfullInList (pf.InputDeclList ctxt) pf.InputParser
    do! check_keyword() pf.PremiseArrow
    let! output = pf.OutputDeclList ctxt
    return {Name = input.Name ;
            Input = input.Args ; Output = output ;
            Modules = None ;Premises = premises ; Pos = input.Pos}
  }

let normal_function_parser =
  {
    PremiseArrow        = SingleArrow
    LeftPremiseParser   = decls_to_parser
    RightPremiseParser  = datas_to_parser
    InputDeclList       = (fun ctxt -> ctxt.FuncDecl)
    InputParser         = decl_to_parser
    OutputDeclList      = (fun ctxt -> datas_to_parser ctxt.DataDecl)
    ContextBuilder      = (fun ctxt rule -> {ctxt with Rules = rule::ctxt.Rules})
  
  }
  
let type_function_parser =
  {
    PremiseArrow        = DoubleArrow
    LeftPremiseParser   = type_func_to_parser
    RightPremiseParser  = type_alias_to_parser
    InputDeclList       = (fun ctxt -> ctxt.TypeDecl)
    InputParser         = decl_to_parser
    OutputDeclList      = (fun ctxt -> type_alias_to_parser ctxt.AliasDecl)
    ContextBuilder      = (fun ctxt rule -> {ctxt with TypeRules = rule::ctxt.TypeRules})
  } 

let parse_rules (decl:ParseScope) (parser_functions:ParserRuleFunctions) 
  :Parser<Token,ParseScope,_> =
  prs{
    let! ctxt = getContext
    let! res = UseDifferentCtxt (parse_rule parser_functions) decl 
    do! setContext (parser_functions.ContextBuilder ctxt res)
  }

let parse_rule_line (decl:ParseScope) :Parser<Token,ParseScope,ParseScope> =
  prs{
    do! (parse_rules decl normal_function_parser) .||
        (parse_rules decl type_function_parser) 
    let! ctxt = getContext
    return ctxt
  }

let parse_full_line :Parser<Token,ParseScope,_> =
  prs{
    do! (check_keyword() NewLine) |> repeat |> ignore
    let! ctxt = getContext
    let! line = (UseDifferentCtxt (parse_decl_line) ctxt) .||
                (UseDifferentCtxt (parse_rule_line (ctxt)) ctxt) 
    
    do! setContext line
    do! (check_keyword() NewLine) |> repeat |> ignore
    return ()
  }

let parse_full_lines :Parser<Token,ParseScope,ParseScope> =
  prs{
    do! parse_full_line |> itterate |> ignore
    return! getContext
  }


module RuleParser2

open ParserMonad
open Common
open Lexer2
open DeclParser2
open GlobalSyntaxCheck2
open ExtractFromToken2

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

type PremisFunctionTree = Literal    of Literal*Position
                        | RuleBranch of FunctionBranch 
                        | DataBranch of FunctionBranch 
                        | IdBranch   of IdBranch

type Condition = Less | LessEqual | Greater | GreaterEqual | Equal

type Premises = Conditional of Condition*PremisFunctionTree*PremisFunctionTree
              | Implication of PremisFunctionTree*PremisFunctionTree

type RuleDef =
  {
    Name              : Id
    Input             : List<ArgId>
    Output            : PremisFunctionTree
    Premises          : List<Premises>
    Pos               : Position
  }

let token_condition_to_condition (key:Keyword)(pos:Position) 
  :Parser<Token,DeclParseScope,Condition> =
  prs{
    match key with
    | Lexer2.Less         -> return Less
    | Lexer2.Greater      -> return Greater
    | Lexer2.LessEqual    -> return LessEqual
    | Lexer2.GreaterEqual -> return GreaterEqual
    | Lexer2.Equal        -> return Equal
    | _                   -> return! fail (RuleError ("conditional expected, got keyword.",pos))
  }

let argstructure_parser (argstruct:ArgStructure) 
  :Parser<Token,DeclParseScope,string*List<Id*Position>*Position> =
  prs{
    match argstruct with
    | LeftArg(_) -> 
      let! left_id,lpos = extract_id()
      let! name,pos    = extract_id()
      let! right_id,rpos = extract_id()
      let! ctxt = getContext
      let left_id =  {Namespace = ctxt.Name.Namespace; Name = left_id}
      let right_id = {Namespace = ctxt.Name.Namespace; Name = right_id}
      return (name,((left_id,lpos)::(right_id,rpos)::[]),pos)
    | RightArgs (ls) -> 
      let! name,pos = extract_id()
      let! rightargs = extract_id() |> repeat
      let! ctxt = getContext
      let rightargs = List.map (fun (id,pos) -> 
        ({Namespace = ctxt.Name.Namespace; Name = id}),pos) rightargs
      return (name,rightargs,pos)
  }

let decl_to_parser (decl:SymbolDeclaration):Parser<Token,DeclParseScope,FunctionBranch> =
  prs{
    let! name,args,pos = argstructure_parser decl.Args
    if name = decl.Name.Name then 
      return {Name = {Namespace = decl.Name.Namespace; Name = name} ; 
              Args = args ; Pos = pos }
    else
      return! fail (RuleError (name,pos))
  }

let decls_to_parser (funcs:List<SymbolDeclaration>) 
  (datas:List<SymbolDeclaration>)
  :Parser<Token,DeclParseScope,PremisFunctionTree> =
  (prs{
    let! res = FirstSuccesfullInList funcs decl_to_parser
    return RuleBranch res
  }) .|| (prs{
    let! res = FirstSuccesfullInList datas decl_to_parser
    return DataBranch res
  }) .|| prs{
    let! id,pos = extract_id()
    let! ctxt = getContext
    return IdBranch({Name = {Namespace=ctxt.Name.Namespace;Name=id} ; Pos = pos})
  } .|| prs{
    let! lit,pos = extract_literal()
    return Literal(lit,pos)
  }

let implication_premis :Parser<Token,DeclParseScope,Premises> =
  prs{
    let! ctxt = getContext
    let! left = decls_to_parser ctxt.FuncDecl ctxt.DataDecl
    do! check_keyword() SingleArrow
    let! right = decls_to_parser [] (ctxt.DataDecl) .>> (check_keyword() NewLine)
    //let! right,_ = extract_id() .>> (check_keyword() NewLine)
    return Implication (left,right)
  }

let parse_premis :Parser<Token,DeclParseScope,Premises> =
   implication_premis .|| prs{
    let! ctxt = getContext
    let! left = decls_to_parser ctxt.FuncDecl ctxt.DataDecl
    let! condition,pos = extract_keyword()
    let! right = decls_to_parser ctxt.FuncDecl ctxt.DataDecl 
    do! check_keyword() NewLine
    let! cond = token_condition_to_condition condition pos
    return Conditional (cond,left,right)
  }

let parse_rule :Parser<Token,DeclParseScope,RuleDef> =
  prs{
    let! premises =  RepeatUntil parse_premis (check_keyword() HorizontalBar)
    do! (check_keyword() HorizontalBar) >>. (check_keyword() NewLine)
    let! ctxt = getContext
    let! input = FirstSuccesfullInList ctxt.FuncDecl decl_to_parser
    do! check_keyword() SingleArrow
    let! output = decls_to_parser [] ctxt.DataDecl
    return {Name = input.Name ;
            Input = input.Args ; Output = output ;
            Premises = premises ; Pos = input.Pos}
  }

let parse_rules (decl:DeclParseScope) :Parser<Token,List<RuleDef>,_> =
  prs{
    let! ctxt = getContext
    let! res = UseDifferentCtxt parse_rule decl
    do! setContext (res::ctxt)
  }

let parse_line (decl:DeclParseScope) :Parser<Token,List<RuleDef>,_> =
  prs{
    do! (check_keyword() NewLine) |> repeat |> ignore
    do! (UseDifferentCtxt (check_decl_keyword >>. check_parse_decl) Position.Zero) .|| 
          (parse_rules decl)
    do! (check_keyword() NewLine) |> repeat |> ignore
    return ()
  }

let parse_lines (decl:DeclParseScope) :Parser<Token,List<RuleDef>,List<RuleDef>> =
  prs{
    do! parse_line decl |> itterate |> ignore
    return! getContext
  }

let parse_rule_scope :Parser<string*DeclParseScope*List<Token>,List<Id>,string*List<RuleDef>*List<SymbolDeclaration>> =
  prs{
    let! id,decl,tok = step
    let! res = UseDifferentSrcAndCtxt (parse_lines decl) tok []
    return id,res,decl.FuncDecl
  }
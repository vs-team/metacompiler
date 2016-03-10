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
    CurrentNamespace  : Namespace
    Args              : List<ArgId*DeclType>
    Return            : DeclType
    Pos               : Position
  }

type IdBranch =
  {
    Name              : Id
    CurrentNamespace  : Namespace
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
    CurrentNamespace  : Namespace
    Input             : List<ArgId*DeclType>
    Output            : PremisFunctionTree
    Premises          : List<Premises>
    Pos               : Position
  }

let token_condition_to_condition (key:Keyword)(pos:Position) :Parser<Token,DeclParseScope,Condition> =
  prs{
    match key with
    | Lexer2.Less         -> return Less
    | Lexer2.Greater      -> return Greater
    | Lexer2.LessEqual    -> return LessEqual
    | Lexer2.GreaterEqual -> return GreaterEqual
    | Lexer2.Equal        -> return Equal
    | _                   -> return! fail (RuleError ("conditional expected, got keyword.",pos))
  }

let arg_to_parser (rightarg:DeclType) :Parser<Token,DeclParseScope,ArgId*DeclType> =
  prs{ 
    let! arg,pos = extract_id()
    return (arg,pos),rightarg
  }

let argstructure_parser (argstruct:ArgStructure) :Parser<Token,DeclParseScope,_> =
  prs{
    match argstruct with
    | LeftArg(l,r) -> 
      let! left_id,lpos = extract_id()
      let! name,pos    = extract_id()
      let! right_id,rpos = extract_id()
      return (name,((left_id,lpos),l)::((right_id,pos),r)::[],pos)
    | RightArgs (ls) -> 
      let! name,pos    = extract_id()
      let! rightargs = IterateTroughGivenList ls arg_to_parser 
      return (name,rightargs,pos)
  }

let decl_to_parser (decl:SymbolDeclaration):Parser<Token,DeclParseScope,FunctionBranch> =
  prs{
    let! name,args,pos = argstructure_parser decl.Args
    if name = decl.Name then 
      return {Name = name ; CurrentNamespace = decl.CurrentNamespace ; 
              Args = args ; Return = decl.Return ; Pos = pos }
    else
      return! fail (RuleError (name,pos))
  }

let decls_to_parser (decls:List<SymbolDeclaration>):Parser<Token,DeclParseScope,PremisFunctionTree> =
  (prs{
    let! res = FirstSuccesfullInList decls decl_to_parser
    return RuleBranch res
  }) .|| prs{
    let! id,pos = extract_id()
    let! ctxt = getContext
    return IdBranch({Name = id ; CurrentNamespace = ctxt.CurrentNamespace ; Pos = pos})
  } .|| prs{
    let! lit,pos = extract_literal()
    return Literal(lit,pos)
  }

let implication_premis :Parser<Token,DeclParseScope,Premises> =
  prs{
    let! ctxt = getContext
    let! left = decls_to_parser (ctxt.DataDecl @ ctxt.FuncDecl)
    do! check_keyword() SingleArrow
    let! right = decls_to_parser (ctxt.DataDecl) .>> (check_keyword() NewLine)
    //let! right,_ = extract_id() .>> (check_keyword() NewLine)
    return Implication (left,right)
  }

let parse_premis :Parser<Token,DeclParseScope,Premises> =
   implication_premis .|| prs{
    let! ctxt = getContext
    let! left = decls_to_parser (ctxt.DataDecl @ ctxt.FuncDecl)
    let! condition,pos = extract_keyword()
    let! right = decls_to_parser (ctxt.DataDecl @ ctxt.FuncDecl)
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
    let! output = decls_to_parser ctxt.DataDecl
    return {Name = input.Name ; CurrentNamespace = ctxt.CurrentNamespace ;
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

let parse_rule_scope :Parser<Id*DeclParseScope*List<Token>,List<Id>,Id*List<RuleDef>*List<SymbolDeclaration>> =
  prs{
    let! id,decl,tok = step
    let! res = UseDifferentSrcAndCtxt (parse_lines decl) tok []
    return id,res,decl.DataDecl
  }
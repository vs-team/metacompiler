module RuleParser2

open ParserMonad
open Common
open Lexer2
open DeclParser2
open GlobalSyntaxCheck2
open ExtractFromToken2

type Type = GenericType     of Id
          | McType          of Id
          | TypeApplication of Id*List<Type>

type FunctionBranch = 
  {
    Name              : Id
    CurrentNamespace  : Namespace
    Args              : List<Id*DeclType>
    Return            : DeclType
    Pos               : Position
  }

type IdBranch =
  {
    Name              : Id
    CurrentNamespace  : Namespace
    Pos               : Position
  }

type PremisFunctionTree = RuleBranch of FunctionBranch 
                        | DataBranch of FunctionBranch 
                        | IdBranch   of IdBranch

type Condition = Less | LessEqual | Greater | GreaterEqual | Equal

type Premises = Conditional of Condition*PremisFunctionTree*PremisFunctionTree
              | Implication of PremisFunctionTree*PremisFunctionTree

type Rule =
  {
    Name              : Id
    CurrentNamespace  : Namespace
    Input             : List<Id*DeclType>
    Output            : Id*DeclType
    Premises          : List<Premises>
    TypeTable         : List<Id*Type>
  
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

let arg_to_parser (rightarg:DeclType) :Parser<Token,DeclParseScope,Id*DeclType> =
  prs{ 
    let! arg,pos = extract_id()
    return arg,rightarg
  }

let argstructure_parser (argstruct:ArgStructure) :Parser<Token,DeclParseScope,_> =
  prs{
    match argstruct with
    | LeftArg(l,r) -> 
      let! left_id,_ = extract_id()
      let! name,pos    = extract_id()
      let! right_id,_ = extract_id()
      return (name,(left_id,l)::(right_id,r)::[],pos)
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

let parse_rule :Parser<Token,DeclParseScope,Rule> =
  prs{
    let! premises =  RepeatUntil parse_premis (check_keyword() HorizontalBar)
    do! (check_keyword() HorizontalBar) >>. (check_keyword() NewLine)
    let! ctxt = getContext
    let! input = FirstSuccesfullInList ctxt.FuncDecl decl_to_parser
    do! check_keyword() SingleArrow
    let! output,_ = extract_id() .>> (check_keyword() NewLine)
    return {Name = input.Name ; CurrentNamespace = ctxt.CurrentNamespace ;
            Input = input.Args ; Output = (output,input.Return) ;
            Premises = premises ; TypeTable = []}
  }

let parse_rules (decl:DeclParseScope) :Parser<Token,List<Rule>,_> =
  prs{
    let! ctxt = getContext
    let! res = UseDifferentCtxt parse_rule decl
    do! setContext (res::ctxt)
  }

let parse_line (decl:DeclParseScope) :Parser<Token,List<Rule>,_> =
  prs{
    do! (check_keyword() NewLine) |> repeat |> ignore
    do! (UseDifferentCtxt (check_decl_keyword >>. check_parse_decl) Position.Zero) .|| 
          (parse_rules decl)
    do! (check_keyword() NewLine) |> repeat |> ignore
    return ()
  }

let parse_lines (decl:DeclParseScope) :Parser<Token,List<Rule>,List<Rule>> =
  prs{
    do! parse_line decl |> itterate |> ignore
    return! getContext
  }

let parse_rule_scope :Parser<Id*DeclParseScope*List<Token>,List<Id>,Id*List<Rule>> =
  prs{
    let! id,decl,tok = step
    let! res = UseDifferentSrcAndCtxt (parse_lines decl) tok []
    return id,res
  }
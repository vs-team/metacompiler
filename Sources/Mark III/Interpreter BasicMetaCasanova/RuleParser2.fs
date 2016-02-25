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

type Premises = Conditional
              | Implication

type Rule =
  {
    Name              : Id
    CurrentNamespace  : Namespace
    Input             : List<Id>
    Output            : Id
    Premises          : List<Premises>
    TypeTable         : List<Id*Type>
  
  }

type LeftOfPremis = 
  {
    Name              : Id
    CurrentNamespace  : Namespace
    Args              : List<Id*DeclType>
    Return            : DeclType
    Pos               : Position
  }

type premise = LeftOfPremis*Id

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

let decl_to_parser (decl:SymbolDeclaration):Parser<Token,DeclParseScope,_> =
  prs{
    let! name,args,pos = argstructure_parser decl.Args
    if name = decl.Name then 
      return {Name = name ; CurrentNamespace = decl.CurrentNamespace ; 
              Args = args ; Return = decl.Return ; Pos = pos }
    else
      return! fail (RuleError (name,pos))
  }

let decls_to_parser (decls:List<SymbolDeclaration>):Parser<Token,DeclParseScope,_> =
  prs{return! FirstSuccesfullInList decls decl_to_parser}


let parse_premis :Parser<Token,DeclParseScope,_> =
  prs{
    let! ctxt = getContext
    let! left = decls_to_parser (ctxt.DataDecl @ ctxt.FuncDecl)
    do! check_keyword() SingleArrow
    //let! right = decls_to_parser (ctxt.DataDecl @ ctxt.FuncDecl)
    let! right,_ = extract_id() .>> (check_keyword() NewLine)
    return left,right
  }

let parse_rule :Parser<Token,DeclParseScope,_> =
  prs{
    let! premises = parse_premis |> repeat1
    do! (check_keyword() HorizontalBar) .|| (check_keyword() NewLine)
    let! ctxt = getContext
    let! input = decls_to_parser ctxt.FuncDecl
    do! check_keyword() SingleArrow
    let! output,_ = extract_id() .>> (check_keyword() NewLine)
    return ()
  }

let parse_rule_scope :Parser<Id*DeclParseScope*List<Token>,Position,_> =
  prs{
    let! next = step
    return ()
  }
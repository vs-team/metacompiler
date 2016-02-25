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
    Output            : string
    Premises          : List<Premises>
    TypeTable         : List<Id*Type>
  
  }

type LeftOfPremis =
  {
    Name              : Id
    CurrentNamespace  : Namespace
  }

let rec leftarg_to_parser (leftarg:DeclType) :Parser<Token,DeclParseScope,Id*DeclType> =
  prs{ 
    let! arg,pos = extract_id()
    return arg,leftarg
  }

let decl_to_parser (decl:SymbolDeclaration):Parser<Token,DeclParseScope,_> =
  prs{
    let! leftargs = IterateTroughGivenList decl.LeftArgs leftarg_to_parser
    let! name,pos = extract_id()
    if name = decl.Name then 
      let! rightargs = IterateTroughGivenList decl.RightArgs leftarg_to_parser
      return ()
    else
      return! fail (RuleError (name,pos))
  }



let parse_premis :Parser<Token,DeclParseScope,_> =
  prs{
    return ()
  }

let parse_rule :Parser<Token,DeclParseScope,_> =
  prs{
    return ()
  }

let parse_rule_scope :Parser<Id*DeclParseScope*List<Token>,Position,_> =
  prs{
    let! next = step
    return ()
  }
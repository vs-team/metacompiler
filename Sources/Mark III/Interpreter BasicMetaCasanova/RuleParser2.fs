module RuleParser2

open ParserMonad
open Common
open Lexer2
open DeclParser2
open GlobalSyntaxCheck2

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

let parse_rule :Parser<Id*DeclParseScope*List<Token>,Position,_> =
  prs{
    return ()
  }

let parse_rule_scope :Parser<Id*DeclParseScope*List<Token>,Position,_> =
  prs{
    return ()
  }
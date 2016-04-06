module TypeRuleParser2

open Common
open Lexer2
open DeclParser2
open GlobalSyntaxCheck2
open ParserMonad
open ExtractFromToken2
open RuleParser2


let parse_type_rule :Parser<Token,DeclParseScope,RuleDef> =
  prs{
    let! premises = RepeatUntil (parse_premis DoubleArrow type_func_to_parser type_alias_to_parser) (check_keyword() HorizontalBar)
    do! (check_keyword() HorizontalBar) >>. (check_keyword() NewLine)
    let! ctxt = getContext
    let! input = FirstSuccesfullInList ctxt.FuncDecl decl_to_parser
    do! check_keyword() SingleArrow
    let! output = datas_to_parser ctxt.DataDecl
    return {Name = input.Name ;
            Input = input.Args ; Output = output ;
            Premises = premises ; Pos = input.Pos}
  }

module ParserAST

open Common

type Program = List<string> * ProgramDefinition
and ProgramDefinition = List<Declaration> * List<RuleDefinition>

and OpOrder =
| Suffix
| Infix

and Associativity =
| Left
| Right

and TypeDecl =
| Arrow of TypeDecl * TypeDecl
| Generic of Id * List<TypeDecl>
| Arg of CallArg

and Declaration =
| Data of SymbolDeclaration
| Func of SymbolDeclaration
| TypeFunc of SymbolDeclaration
| TypeAlias of SymbolDeclaration

and SymbolDeclaration =
  {
    Name      : Id
    Args      : TypeDecl
    Return    : TypeDecl
    Order     : OpOrder
    Priority  : int
    Position  : Position
    Associativity : Associativity
    Premises : List<Premise>
  }

and RuleDefinition =
| Rule of Rule
| TypeRule of Rule

and Premise =
| FunctionCall of Call
| Conditional of Conditional

and CallArg =
| Literal of Literal * Position
| Id of Id * Position
| NestedExpression of List<CallArg>


and Call = List<CallArg> * List<Id>
and Conditional = List<CallArg> *  Predicate * List<CallArg>
and Conclusion = 
| ValueOutput of List<CallArg> * List<CallArg>
| ModuleOutput of List<CallArg> * List<CallArg> * Program

and Rule = List<Premise> * Conclusion


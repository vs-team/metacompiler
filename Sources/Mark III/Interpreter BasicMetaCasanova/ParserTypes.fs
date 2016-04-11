module ParserTypes

open Common
open Lexer2
open ParserMonad
type Associativity = Left | Right

type DeclType =
  | Id          of Id * Position
  | IdVar       of Id * Position
  | IdKind      of Id * Position
  | Arrow       of DeclType*DeclType
  | TypeArrow   of DeclType*DeclType
  | Application of Id*DeclType*DeclType


type ArgStructure = LeftArg of DeclType*DeclType
                  | RightArgs of List<DeclType>


type SymbolDeclaration =
  {
    Name              : Id
    Args              : ArgStructure
    Return            : DeclType
    Priority          : int
    Associativity     : Associativity
    Pos               : Position
  }

type DeclParseScope = 
  {
    Name             : Id
    DataDecl         : List<SymbolDeclaration>
    FuncDecl         : List<SymbolDeclaration>
    //ArrowDecl        : List<SymbolDeclaration>
    TypeDecl         : List<SymbolDeclaration>
    AliasDecl        : List<SymbolDeclaration>
  } with 
    static member Zero =
      {
        Name              = {Namespace = []; Name = ""}
        DataDecl          = []
        FuncDecl          = []
        //ArrowDecl         = []
        TypeDecl          = []
        AliasDecl         = []
      }
    static member add pc1 pc2=
      {
        Name              = pc2.Name
        DataDecl          = pc1.DataDecl  @ pc2.DataDecl
        FuncDecl          = pc1.FuncDecl  @ pc2.FuncDecl
        TypeDecl          = pc1.TypeDecl  @ pc2.TypeDecl
        AliasDecl         = pc1.AliasDecl @ pc2.AliasDecl
        //ArrowDecl         = pc1.ArrowDecl @ pc2.ArrowDecl
      }

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
                        | TypeRuleBranch  of FunctionBranch 
                        | TypeAliasBranch of FunctionBranch 
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

type RuleContext = 
  {
    Rules       : List<RuleDef>
    TypeRules   : List<RuleDef>
    Decls       : DeclParseScope
  }
  with 
    static member Zero = 
      {
        Rules       = []
        TypeRules   = []
        Decls       = DeclParseScope.Zero
      }

type ParserRuleFunctions = 
  {
     PremiseArrow : Keyword   
     LeftPremiseParser : DeclParseScope -> Parser<Token,DeclParseScope,PremisFunctionTree>
     RightPremiseParser : List<SymbolDeclaration> -> Parser<Token,DeclParseScope,PremisFunctionTree>
     InputDeclList  : DeclParseScope -> List<SymbolDeclaration>
     InputParser : SymbolDeclaration -> Parser<Token,DeclParseScope,FunctionBranch>
     OutputDeclList : DeclParseScope -> Parser<Token,DeclParseScope,PremisFunctionTree>
     ContextBuilder : RuleContext -> RuleDef -> RuleContext
  } 

type ParserContexts = RuleCtxt of RuleContext
                    | DeclCtxt of DeclParseScope

type ParserScope =
  {
    DeclsScp : DeclParseScope
    RuleScp  : RuleContext
  }
  with
    static member Zero =
      {
        DeclsScp = DeclParseScope.Zero
        RuleScp = RuleContext.Zero
      }
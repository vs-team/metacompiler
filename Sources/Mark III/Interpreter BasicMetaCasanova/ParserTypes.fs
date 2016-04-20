module ParserTypes
//
//open Common
//open Lexer2
//open ParserMonad
//
//type Associativity = Left | Right
//
//type DeclType =
//  | Id          of Id * Position
//  | IdVar       of Id * Position
//  | IdKind      of Id * Position
//  | Arrow       of DeclType*DeclType
//  | TypeArrow   of DeclType*DeclType
//  | Application of Id*List<DeclType>
//
//type ArgStructure = LeftArg of DeclType*DeclType
//                  | RightArgs of List<DeclType>
//
//type ArgId = Id*Position
//
//type FunctionBranch = 
//  {
//    Name              : Id
//    Args              : List<PremisFunctionTree>
//    Pos               : Position
//  }
//
//and IdBranch =
//  {
//    Name              : Id
//    Pos               : Position
//  }
//
//and PremisFunctionTree = Literal    of Literal*Position
//                        | RuleBranch of FunctionBranch 
//                        | DataBranch of FunctionBranch 
//                        | DotNetBranch of FunctionBranch 
//                        | TypeRuleBranch  of FunctionBranch 
//                        | TypeAliasBranch of FunctionBranch 
//                        | IdBranch   of IdBranch
//
//type Premises = Conditional of Predicate*PremisFunctionTree*PremisFunctionTree
//              | Implication of PremisFunctionTree*PremisFunctionTree
//
//type SymbolDeclaration =
//  {
//    Name              : Id
//    Args              : ArgStructure
//    Return            : DeclType
//    Premises          : List<Premises>
//    Priority          : int
//    Associativity     : Associativity
//    Pos               : Position
//  }
//
//type RuleDef =
//  {
//    Name              : Id
//    Input             : List<PremisFunctionTree>
//    Output            : PremisFunctionTree
//    Premises          : List<Premises>
//    Modules           : Option<Id*Position>
//    Pos               : Position
//  }
//
//type ParsingFunctions =
//  {
//    PremisFunctions : option<Parser<Token,ParseScope,Premises>>
//    ModuleParser    : option<Parser<Token,ParseScope,ParseScope>>
//    LeftPremiseParser : option<ParseScope -> Parser<Token,ParseScope,PremisFunctionTree>>
//  }
//
//and ModuleScope = 
//  {
//    Inherit : List<Id>
//    Scope  : ParseScope
//  }
//
//and ParseScope = 
//  {
//    Name             : Id
//    Imports          : List<string>
//    DataDecl         : List<SymbolDeclaration>
//    FuncDecl         : List<SymbolDeclaration>
//    TypeDecl         : List<SymbolDeclaration>
//    AliasDecl        : List<SymbolDeclaration>
//    Rules            : List<RuleDef>
//    TypeRules        : List<RuleDef>
//    TypeAlias        : List<RuleDef>
//    Modules          : Map<(Id*Position),ModuleScope>
//    DeclFunctions    : ParsingFunctions
//  } with 
//    static member Zero =
//      {
//        Name              = {Namespace = []; Name = ""}
//        Imports           = []
//        DataDecl          = []
//        FuncDecl          = []
//        TypeDecl          = []
//        AliasDecl         = []
//        Rules             = []
//        TypeRules         = []
//        TypeAlias         = []
//        Modules           = Map.empty
//        DeclFunctions     = {PremisFunctions = None ; ModuleParser = None ; LeftPremiseParser = None}
//      }
//
//type ParserRuleFunctions = 
//  {
//     PremiseArrow : Keyword   
//     LeftPremiseParser : ParseScope -> Parser<Token,ParseScope,PremisFunctionTree>
//     RightPremiseParser : List<SymbolDeclaration> -> Parser<Token,ParseScope,PremisFunctionTree>
//     InputDeclList  : ParseScope -> List<SymbolDeclaration>
//     InputParser : SymbolDeclaration -> Parser<Token,ParseScope,FunctionBranch>
//     OutputDeclList : ParseScope -> Parser<Token,ParseScope,PremisFunctionTree>
//     ContextBuilder : ParseScope -> RuleDef -> ParseScope
//  }
//
//
//
//type Scope = 
//  {
//    
//    DataDecl         : List<SymbolDeclaration>
//    FuncDecl         : List<SymbolDeclaration>
//    TypeDecl         : List<SymbolDeclaration>
//    AliasDecl        : List<SymbolDeclaration>
//  
//  }
//

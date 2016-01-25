module Parser2

open Common
open ParserMonad
open Lexer2

type Associativity = Left | Right

type DeclType =
  | Id          of Id * Position
  | IdVar       of Id * Position
  | Module      of Id * Namespace * Position
  | Arrow       of DeclType*DeclType
  | Application of DeclType*DeclType
  | Tuple       of DeclType*DeclType
  | Bar         of DeclType*DeclType

type SymbolDeclaration =
  {
    Name            :string
    LeftArgs        :List<DeclType>
    RightArgs       :List<DeclType>
    Return          :DeclType
    Priority        :int
    Associativity   :Associativity
    Position        :Position
  }

type Premise = Conditional of List<DeclType> * List<DeclType>
             | Implication of List<DeclType> * List<DeclType>
             | ArrowStruct of List<DeclType> * List<DeclType>

type Rule =
  {
    Input       : List<DeclType>
    Output      : List<DeclType>
    Premises    : List<Premise>
  }

type ParseScope = 
  {
    Import      : List<Id>
    DataDecl    : List<SymbolDeclaration>
    FuncDecl    : List<SymbolDeclaration>
    ArrowDecl   : List<SymbolDeclaration>
    TypeDecl    : List<SymbolDeclaration>
    AliasDecl   : List<SymbolDeclaration>
    FuncRule    : List<Id*Rule>
    TypeRule    : List<Id*Rule>
    AliasRule   : List<Id*Rule>
    Modules     : List<Id>
  }

let parse_import :Parser<Token,ParseScope,_> =
  prs{
    let! x = step
    return ()
  }

let parse_scope :Parser<Token,ParseScope,_> =
  prs{
    return ()
  }

let parse2 :Parser<Token,ParseScope,_> =
  prs{
    return ()
  }
module RuleNormalizer2

open Common
open DeclParser2
open RuleParser2

type StandardId = Id*Namespace

type GlobalId = FuncId of Id*Namespace
              | LambdaId  of int*Namespace

type NormalId = FuncId of Id
              | TempId of int

type Premisse = Literal            of Literal*NormalId
              | Conditional        of NormalId*Condition*NormalId
              | Destructor         of NormalId*StandardId*List<NormalId>
              | McClosure          of GlobalId*NormalId
              | DotNetClosure      of StandardId*NormalId
              | BuiltinClosure     of NormalId*NormalId
              | ConstructorClosure of StandardId*NormalId
              | Apply              of NormalId*NormalId*NormalId

type NormalizedRule =
  {
    CurentNamespace : Namespace
    Input : List<NormalId>
    Output : (int)
    Premis : List<Premisse*List<DeclType>>
    Signature : List<DeclType>
  }

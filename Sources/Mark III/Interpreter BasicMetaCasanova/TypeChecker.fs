module TypeChecker
open Common
open ScopeBuilder // Scope

type Id       = List<string>*string
type LambdaId = List<string>*int

type lit = I64 of System.Int64
         | U64 of System.UInt64
         | I32 of System.Int32
         | U32 of System.UInt32
         | I16 of System.Int16
         | U16 of System.UInt16
         | I8  of System.SByte
         | U8  of System.Byte
         | F32 of System.Single
         | F64 of System.Double
         | String of System.String

type var = Lambda of int
         | Named  of Id

type tree = Lit of lit
          | Var of var
          | Lambda     of LambdaId*List<tree>
          | RuleCall   of Id*List<tree>
          | DotNetCall of Id*List<tree>

type conditional = Less | LessEqual | Equal | GreaterEqual | Greater | NotEqual

type premisse = Assignment  of var*tree
              | Conditional of conditional*var*var

type dataElem = DotNetType      of Id
              | McType          of Id
              | TypeLambda      of List<dataElem>
              | TypeApplication of Id*List<dataElem>

type rule = {
  input  :tree
  output :tree
  premis :List<premisse>
  typemap:Map<Id,dataElem>
}

type data = {
  id     :Id
  args   :List<Id>
  output :Id
  typemap:Map<Id,dataElem>
}


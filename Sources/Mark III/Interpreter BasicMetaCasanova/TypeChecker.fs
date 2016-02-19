module TypeChecker
open Common
open ScopeBuilder // Scope

type Type = DotNetType      of Id
          | McType          of Id
          | TypeApplication of Id*List<Type>

type Id       = List<string>*string*Type
type DataId   = List<string>*string*Type
type LambdaId = List<string>*int*Type

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

type lexpr = Lit of lit
           | Var of var
           | RuleCall        of var*list<var>
           | DotNetCall      of var*list<var>
           | ConstructorCall of DataId*list<var>

type rexpr = Id of Id
           | DestructorCall of DataId*List<Id>

type conditional = Less | LessEqual | Equal | GreaterEqual | Greater | NotEqual

type premisse = Assignment  of lexpr*rexpr
              | Conditional of conditional*lexpr*lexpr

type rule = {
  input  :rexpr
  output :lexpr
  premis :List<premisse>
  typemap:Map<Id,Type>
}

type data = {
  id     :Id
  args   :List<Id>
  output :Id
  typemap:Map<Id,Type>
}

type fromTypecheckerWithLove = {
  rules  : Map<Id,List<rule>>
  lamdas : Map<LambdaId,rule>
  datas  : Map<Id,data>
}
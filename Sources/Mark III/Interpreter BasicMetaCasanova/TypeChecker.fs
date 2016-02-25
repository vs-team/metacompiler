module TypeChecker
open Common
open ScopeBuilder // Scope

type Type = DotNetType      of List<string>*string
          | McType          of List<string>*string
          | TypeApplication of Type*List<Type>

type Id       = {Namespace:List<string>;Name:string;Type:Type}
type LambdaId = {Namespace:List<string>;Name:int;   Type:Type}

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
         | Bool of System.Boolean

type var = Lambda of LambdaId
         | Named  of Id
         | Tmp    of int

type lexpr = Lit of lit
           | Var of var
           | RuleCall        of var*list<var>
           | DotNetCall      of var*list<var>
           | ConstructorCall of Id*list<var>

type rexpr = Id of Id
           | DestructorCall  of Id*List<var>

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
  args   :List<string*Type>
  output :Type
}

type fromTypecheckerWithLove = {
  rules   : Map<Id,List<rule>>
  lambdas : Map<LambdaId,rule>
  datas   : Map<Id,data>
}
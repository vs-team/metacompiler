module TypeChecker
open Common
open ScopeBuilder // Scope

type Type = DotNetType      of List<string>*string
          | McType          of List<string>*string
          | TypeApplication of Type*List<Type>
          | Arrow           of Type*Type

type genericId<'a>= {Namespace:List<string>;Name:'a;Type:Type}
type Id       = genericId<string>
type LambdaId = genericId<int>

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

type global_id = Lambda of LambdaId
               | Named  of Id

type local_id = Named of string
              | Tmp   of int

type lexpr = Lit of lit
           | Var of global_id
           | RuleCall        of global_id*list<local_id>
           | DotNetCall      of global_id*list<local_id>
           | ConstructorCall of Id*list<local_id>

type rexpr = Id of local_id
           | DestructorCall  of Id*List<local_id>

type conditional = Less | LessEqual | Equal | GreaterEqual | Greater | NotEqual

type premisse = Assignment  of lexpr*rexpr
              | Conditional of conditional*lexpr*lexpr

type rule = {
  input  :List<rexpr>
  output :lexpr
  premis :List<premisse>
  typemap:Map<local_id,Type>
}

type data = {
  args   :List<local_id*Type>
  output :Type
}

type fromTypecheckerWithLove = {
  rules   : Map<Id,List<rule>>
  lambdas : Map<LambdaId,rule>
  datas   : Map<Id,data>
}
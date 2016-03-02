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

type builtin     = ADD | FADD | SUB | FSUB | MUL | FMUL | DIV | FDIV | UDIV
type conditional = Less | LessEqual | Equal | GreaterEqual | Greater | NotEqual

type premisse = Literal         of local_id*lit
              | Call            of local_id*global_id*list<local_id>
              | DotNetCall      of local_id*Id*list<local_id>
              | BuiltinCall     of local_id*builtin*list<local_id>
              | ConstructorCall of local_id*Id*list<local_id>
              | DestructorCall  of local_id*Id*list<local_id>
              | Conditional     of local_id*conditional*local_id

type rule = {
  input  :List<local_id>
  output :local_id
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
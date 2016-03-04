module TypeChecker
open Common

type Type = DotNetType      of List<string>*string
          | McType          of List<string>*string
          | TypeApplication of Type*List<Type>
          | Arrow           of Type*Type

type genericId<'a>= {Namespace:List<string>;Name:'a;Type:Type}
type Id       = genericId<string>
type LambdaId = genericId<int>

type lit = I64 of System.Int64
         | U64 of System.UInt64
         | F64 of System.Double
         | String of System.String
         | Bool of System.Boolean

type global_id = Lambda of LambdaId
               | Func   of Id

type local_id = Named of string
              | Tmp   of int

type builtin = ADD | FADD 
             | SUB | FSUB
             | MUL | FMUL
             | DIV | FDIV | UDIV
             | REM | FREM | UREM
type conditional = Less | LessEqual | Equal | GreaterEqual | Greater | NotEqual

type premisse = Literal            of lit*local_id
              | Conditional        of local_id*conditional*local_id
              | Destructor         of local_id*Id*List<local_id>
              | McClosure          of global_id*local_id
              | DotNetClosure      of Id*local_id
              | BuiltinClosure     of builtin*local_id
              | ConstructorClosure of Id*local_id
              | Apply              of local_id*local_id*local_id

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
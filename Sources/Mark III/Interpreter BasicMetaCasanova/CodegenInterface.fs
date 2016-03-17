module CodegenInterface
open Common

type genericId<'a>= {Namespace:List<string>;Name:'a;}
type Id       = genericId<string>
type LambdaId = genericId<int>
type TypeId   = genericId<string>

type Type = DotNetType      of TypeId
          | McType          of TypeId
          | TypeApplication of Type*List<Type>
          | Arrow           of Type*Type

type lit = I64 of System.Int64
         | U64 of System.UInt64
         | F64 of System.Double
         | String of System.String
         | Bool of System.Boolean

type local_id = Named of string
              | Tmp   of int

type predicate = Less | LessEqual | Equal | GreaterEqual | Greater | NotEqual

type premisse = Literal               of Literal
              | Conditional           of Conditional
              | Destructor            of Destructor
              | FuncClosure           of closure<Id>
              | LambdaClosure         of closure<LambdaId>
              | DotNetClosure         of closure<Id>
              | ConstructorClosure    of closure<Id>
              | Application           of Application
              | ApplicationCall       of Application
              | ImpureApplicationCall of Application
and Literal     = {value:lit; dest:local_id}
and Conditional = {left:local_id; predicate:predicate; right:local_id}
and Destructor  = {source:local_id; destructor:Id; args:List<local_id>}
and closure<'a> = {func:'a;dest:local_id}
and Application = {closure:local_id; argument:local_id; dest:local_id}

type rule = {
  side_effect :bool
  input  :List<local_id>
  output :local_id
  premis :List<premisse>
  typemap:Map<local_id,Type>
}

type data = {
  args       :List<Type>
  outputType :Type
}

type fromTypecheckerWithLove = {
  rules   : Map<Id,List<rule>>
  lambdas : Map<LambdaId,rule>
  datas   : List<Id*data>
  main    : rule
}
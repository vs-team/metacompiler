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
         | F32 of System.Single
         | String of System.String
         | Bool of System.Boolean
         | Void

type local_id = Named of string
              | Tmp   of int

type predicate = Less | LessEqual | Equal | GreaterEqual | Greater | NotEqual

type premisse = Literal               of Literal
              | Conditional           of Conditional
              | Destructor            of Destructor
              | ConstructorClosure    of closure<Id>
              | FuncClosure           of closure<Id>
              | LambdaClosure         of closure<LambdaId>
              | Application           of Application
              | ApplicationCall       of Application
              | ImpureApplicationCall of Application
              | DotNetCall            of DotNetCall
              | DotNetConstructor     of DotNetCall
              | DotNetProperty        of DotNetProperty
and Literal     = {value:lit; dest:local_id}
and Conditional = {left:local_id; predicate:predicate; right:local_id}
and Destructor  = {source:local_id; destructor:Id; args:List<local_id>}
and closure<'a> = {func:'a;dest:local_id}
and Application = {closure:local_id; argument:local_id; dest:local_id}
and DotNetCall  = {func: Id;args:List<local_id>; dest:local_id}
and DotNetProperty  = {instance : local_id; property: local_id; dest:local_id}

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
  rules   : Map<Id,List<rule>> // this is actually funcs
  lambdas : Map<LambdaId,rule>
  datas   : List<Id*data>
  main    : rule
}

let (-->) t1 t2 =
  Arrow(t1,t2)
   
let mutable tmp_index = 0

let next_tmp () = 
  let current = tmp_index
  tmp_index <- tmp_index+1
  Tmp(current)

let current_tmp () =
  Tmp(tmp_index-1)

let reset_tmp () =
 do tmp_index <- 1
 Tmp(0)

let reset () =
 do tmp_index <- 0

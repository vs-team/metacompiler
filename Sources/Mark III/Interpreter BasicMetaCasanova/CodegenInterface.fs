module CodegenInterface
open Common

type LambdaId = genericId<int>
type TypeId   = genericId<string>

type Type = DotNetType      of TypeId
          | McType          of TypeId
          | TypeApplication of Type*List<Type>
          | Arrow           of Type*Type

type local_id = Named of string
              | Tmp   of int

type premisse = Literal            of LiteralAssignment // assign literal to local
              | Conditional        of Conditional       // stops evaluation if condition is false
              | Destructor         of Destructor        // destructs Mc data into its constructor arguments
              | ConstructorClosure of closure<Id>       // assigns mc data constructor closure to local
              | FuncClosure        of closure<Id>       // assigns mc func closure to local
              | LambdaClosure      of closure<LambdaId> // assigns lambda closure to local
              | Application        of Application       // applies an argument to a local closure
              | ApplicationCall    of ApplicationCall   // applies an argument to a local closure and calls it
              | DotNetCall         of DotNetCall        // calls .Net method and assigns result to local
              | DotNetStaticCall   of DotNetStaticCall  // calls .Net static method and assigns result to local
              | DotNetConstructor  of DotNetStaticCall  // calls .Net constructor
              | DotNetGet          of DotNetGet         // gets field and assigns it to local
              | DotNetSet          of DotNetSet         // sets field from local
and LiteralAssignment = {value:Literal; dest:local_id}
and Conditional = {left:local_id; predicate:Predicate; right:local_id}
and Destructor  = {source:local_id; destructor:Id; args:List<local_id>}
and closure<'a> = {func:'a;dest:local_id}
and Application = {closure:local_id; argument:local_id; dest:local_id}
and ApplicationCall = {closure:local_id; argument:local_id; dest:local_id; side_effect:bool}
and DotNetStaticCall = {func: Id; args:List<local_id>; dest:local_id; side_effect:bool}
and DotNetCall       = {instance: local_id; func: string; args:List<local_id>; dest:local_id; side_effect:bool; mutates_instance:bool}
and DotNetGet   = {instance: local_id; field: string; dest:local_id}
and DotNetSet   = {instance: local_id; field: string; src:local_id}

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
  assemblies : List<string> 
  funcs   : Map<Id,List<rule>>
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

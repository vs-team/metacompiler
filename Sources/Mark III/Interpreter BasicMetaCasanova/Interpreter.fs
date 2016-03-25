module Interpreter
open CodegenInterface
open ParserMonad

let rec eval_step (p:premisse)
              (global_context:fromTypecheckerWithLove)
              (type_map:Map<local_id,Type>)
              (symbol_table:Map<local_id,obj>)
              :List<Map<local_id,obj>> =
  match p with
  | Literal x ->
    let value = match x.value with 
                | I64 x->box x | U64 x->box x | F64 x->box x 
                | F32 x->box x | String x->box x | Bool x->box x 
                | Void->box()
    [symbol_table.Add(x.dest,value)]
  | Conditional x ->
    let l = symbol_table.[x.left]  :?> System.IComparable
    let r = symbol_table.[x.right] :?> System.IComparable
    let f = match x.predicate with Less -> (<) | LessEqual->(<=) | Equal -> (=) | GreaterEqual -> (>=) | Greater -> (>) | NotEqual -> (<>)
    if f l r then [symbol_table] else []
  | Destructor x ->
    let id,args = symbol_table.[x.source] :?> Id*List<obj>
    if x.destructor = id then 
      let additions = args |> List.zip x.args |> Map.ofList
      [symbol_table |> Map.fold (fun a k v->a|>Map.add k v) additions]
    else
      []
  | ConstructorClosure x ->
    let ret = x.func,List.empty
    [symbol_table.Add(x.dest,(box ret))]
  | Application x -> // todo: lambdas (use :?)
    let id,args = symbol_table.[x.closure] :?> Id*List<obj>
    let res = id,(symbol_table.[x.argument]::args)
    [symbol_table.Add(x.dest,(box res))]
  | ApplicationCall x -> // todo: lambdas (use :?)
    let id,args = symbol_table.[x.closure] :?> Id*List<obj>
    let filled_args = symbol_table.[x.argument]::args
    match global_context.datas |> List.tryFind (fun(x,_)->x=id) with
    | Some(id,data) ->
      [symbol_table.Add(x.dest,box filled_args)]
    | None ->
      global_context.rules.[id] |> List.map (fun rule -> // for each rule
        ([],rule.premis) ||> List.fold (fun (stabs:List<Map<local_id,obj>>) p -> // for each instruction
            stabs |> List.map (fun stab -> // for each fork
              eval_step p global_context rule.typemap stab
            ) |> List.concat
          )
        ) |> List.concat 
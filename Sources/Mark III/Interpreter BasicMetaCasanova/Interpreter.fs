module Interpreter
open Common
open CodegenInterface
open ParserMonad

let print_loc  (x:local_id) = match x with Named x -> x | Tmp x -> sprintf "_%d" x
let print_id   (x:Id) = x.Namespace @ [x.Name] |> String.concat "." 

let print_premisse (p:premisse,i:int) :string =
  let print_lamb (x:LambdaId) = x.Namespace @ [sprintf "lambda%d" x.Name] |> String.concat "." 
  match p with
  | Literal            x -> sprintf "%03d: LITR %A -> %s" i x.value (print_loc x.dest)
  | Conditional        x -> sprintf "%03d: COND %s %A %s" i (print_loc x.left) x.predicate (print_loc x.right)
  | Destructor         x -> sprintf "%03d: DTOR %s %s -> %s" i (print_id x.destructor) (print_loc x.source) (x.args|>List.map print_loc|>String.concat " ")
  | ConstructorClosure x -> sprintf "%03d: CTOR %s -> %s" i (print_id x.func) (print_loc x.dest)
  | FuncClosure        x -> sprintf "%03d: FUNC %s -> %s" i (print_id x.func) (print_loc x.dest)
  | LambdaClosure      x -> sprintf "%03d: LAMB %s -> %s" i (print_lamb x.func) (print_loc x.dest)
  | Application        x -> sprintf "%03d: APPL %s %s -> %s" i (print_loc x.closure) (print_loc x.argument) (print_loc x.dest)
  | ApplicationCall    x -> sprintf "%03d: CALL %s %s -> %s %s" i (print_loc x.closure) (print_loc x.argument) (print_loc x.dest) (if x.side_effect then "{SIDE-EFFECT}" else "")
  | DotNetConstructor  x -> sprintf "%03d: NCON %s(%s) -> %s %s" i (print_id x.func) (x.args|>List.map print_loc|>String.concat " ") (print_loc x.dest)(if x.side_effect then "{SIDE-EFFECT}" else "")
  | DotNetStaticCall   x -> sprintf "%03d: NSCA %s(%s) -> %s %s" i (print_id x.func) (x.args|>List.map print_loc|>String.concat " ") (print_loc x.dest) (if x.side_effect then "{SIDE-EFFECT}" else "")
  | DotNetCall         x -> sprintf "%03d: NDCA %s.%s(%s) -> %s %s %s" i (print_loc x.instance) x.func (x.args|>List.map print_loc|>String.concat " ") (print_loc x.dest) (if x.side_effect then "{SIDE-EFFECT}" else "") (if x.mutates_instance then "{MUTATES-INSTANCE}" else "")
  | DotNetGet          x -> sprintf "%03d: NGET %s.%s -> %s" i (print_loc x.instance) x.field (print_loc x.dest)
  | DotNetSet          x -> sprintf "%03d: NSET %s.%s <- %s" i (print_loc x.instance) x.field (print_loc x.src)

type global_context = {assemblies:List<System.Reflection.Assembly>;funcs:Map<Id,List<rule>*Position>;lambdas:Map<LambdaId,rule>;datas:List<Id*data>;main:rule;}

let getClass (assemblies) (name:string) :System.Type =
  let ts = 
    assemblies |> List.fold (fun (a:List<System.Type>) (v:System.Reflection.Assembly)->
      let t:System.Type = v.GetType(name,false)
      if t=null then a else t::a
    ) []
  match ts with
  | [t] -> t
  | []  -> failwith ("EVAL ERROR: "+name+" not found in assembly")
  | _   -> failwith ("EVAL ERROR: "+name+" found in multiple assemblies")

let staticCallNonBuiltin (x:DotNetStaticCall) (symbol_table:Map<local_id,obj>) (assemblies:list<System.Reflection.Assembly>) :obj =
  let t = getClass assemblies (x.func.Namespace|>String.concat ".")
  let args = x.args |> List.map (fun a->symbol_table.[a]) |> List.toArray
  let argtypes = System.Type.GetTypeArray(args)
  let f = t.GetMethod(x.func.Name,argtypes)
  f.Invoke(null,args)

let DotNetConstruct (x:DotNetStaticCall) (symbol_table:Map<local_id,obj>) (assemblies:list<System.Reflection.Assembly>) :obj =
  let t = getClass assemblies (x.func.Namespace@[x.func.Name]|>String.concat ".")
  let args = x.args |> List.map (fun a->symbol_table.[a]) |> List.toArray
  let argtypes = System.Type.GetTypeArray(args)
  let c = t.GetConstructor(argtypes)
  c.Invoke(args)

let dynamicCallNonBuiltin (x:DotNetCall) (parent_type:Id) (symbol_table:Map<local_id,obj>) (assemblies:list<System.Reflection.Assembly>) :obj =
  let t = getClass assemblies (parent_type.Namespace@[parent_type.Name]|>String.concat ".")
  let args = x.args |> List.map (fun a->symbol_table.[a]) |> List.toArray
  let argtypes = System.Type.GetTypeArray(args)
  let f = t.GetMethod(x.func,argtypes)
  f.Invoke(symbol_table.[x.instance],args)

let dotNetGet (x:DotNetGet) (parent_type:Id) (symbol_table:Map<local_id,obj>) (assemblies:list<System.Reflection.Assembly>) :obj =
  let t = getClass assemblies (parent_type.Namespace@[parent_type.Name]|>String.concat ".")
  let field = t.GetField(x.field)
  field.GetValue(symbol_table.[x.instance])

let dotNetSet (x:DotNetSet) (parent_type:Id) (symbol_table:Map<local_id,obj>) (assemblies:list<System.Reflection.Assembly>) =
  let t = getClass assemblies (parent_type.Namespace@[parent_type.Name]|>String.concat ".")
  let field = t.GetField(x.field)
  field.SetValue(symbol_table.[x.instance],symbol_table.[x.src])

let rec eval_step (p:premisse,i:int)
                  (global_context:global_context)
                  (type_map:Map<local_id,Type>)
                  (symbol_table:Map<local_id,obj>)
                  :List<Map<local_id,obj>> =
  (*
  do symbol_table |> Map.toSeq |> Seq.iter (fun(id,o)-> 
      do System.Console.ForegroundColor <- System.ConsoleColor.Magenta
      do printf "%s\n" (print_loc id)
      do System.Console.ResetColor()
      do printf "%A\n" o
      ()
    )
  *)
  do System.Console.ForegroundColor <- System.ConsoleColor.Yellow
  do printf "%s\n" (print_premisse (p,i))
  do System.Console.ResetColor()
  match p with
  | Literal x ->
    let value = match x.value with 
                | I64 x->box x | U64 x->box x | F64 x->box x 
                | I32 x->box x | U32 x->box x | F32 x->box x
                | String x->box x | Bool x->box x | Void->box()
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
  | FuncClosure x 
  | ConstructorClosure x ->
    let ret = x.func,List.empty
    [symbol_table.Add(x.dest,(box ret))]
  | LambdaClosure x ->
    let ret = x.func,List.empty
    [symbol_table.Add(x.dest,(box ret))]
  | Application x ->
    match symbol_table.[x.closure] with
    | :? (Id*List<obj>) as foo ->
      let id,args = foo
      let res = id,(args@[symbol_table.[x.argument]])
      [symbol_table.Add(x.dest,(box res))]
    | :? (LambdaId*List<obj>) as foo ->
      let id,args = foo
      let res = id,(args@[symbol_table.[x.argument]])
      [symbol_table.Add(x.dest,(box res))]
  | ApplicationCall x -> 
    match symbol_table.[x.closure] with
    | :? (Id*List<obj>) as foo ->
      let id,args = foo
      let filled_args = args@[symbol_table.[x.argument]]
      match global_context.datas |> List.tryFind (fun(x,_)->x=id) with
      | Some(id,data) ->
        [symbol_table.Add(x.dest,box filled_args)]
      | None ->
        fst global_context.funcs.[id] |> List.map (fun rule -> // for each rule
            let results = eval_rule rule global_context filled_args 
            results |> List.map (fun v->symbol_table.Add(x.dest,v))
          )|>List.concat
    | :? (LambdaId*List<obj>) as foo ->
      let id,args = foo
      let filled_args = args@[symbol_table.[x.argument]]
      let rule = global_context.lambdas.[id]
      let results = eval_rule rule global_context filled_args
      results |> List.map (fun v->symbol_table.Add(x.dest,v))
  | DotNetStaticCall x ->
    let ret:obj = 
      match x.func.Namespace with
      | ["System";"Int32"] when x.args.Length=2 ->
        let l = symbol_table.[x.args.[0]] :?> System.Int32
        let r = symbol_table.[x.args.[1]] :?> System.Int32
        match x.func.Name with "+"->box(l+r) | "/"->box(l/r) | "*"->box(l*r) | "%"->box(l%r) | "-"->box(l-r) | _ ->staticCallNonBuiltin x symbol_table global_context.assemblies
      | ["System";"Int64"] when x.args.Length=2 ->
        let l = symbol_table.[x.args.[0]] :?> System.Int64
        let r = symbol_table.[x.args.[1]] :?> System.Int64
        match x.func.Name with "+"->box(l+r) | "/"->box(l/r) | "*"->box(l*r) | "%"->box(l%r) | "-"->box(l-r) | _ ->staticCallNonBuiltin x symbol_table global_context.assemblies
      | ["System";"Single"] when x.args.Length=2 ->
        let l = symbol_table.[x.args.[0]] :?> System.Single
        let r = symbol_table.[x.args.[1]] :?> System.Single
        match x.func.Name with "+"->box(l+r) | "/"->box(l/r) | "*"->box(l*r) | "%"->box(l%r) | "-"->box(l-r) | _ ->staticCallNonBuiltin x symbol_table global_context.assemblies
      | ["System";"Double"] when x.args.Length=2 ->
        let l = symbol_table.[x.args.[0]] :?> System.Double
        let r = symbol_table.[x.args.[1]] :?> System.Double
        match x.func.Name with "+"->box(l+r) | "/"->box(l/r) | "*"->box(l*r) | "%"->box(l%r) | "-"->box(l-r) | _ ->staticCallNonBuiltin x symbol_table global_context.assemblies
      | _ -> staticCallNonBuiltin x symbol_table global_context.assemblies
    [symbol_table.Add(x.dest,ret)]
  | DotNetConstructor x ->
    let ret = DotNetConstruct x symbol_table global_context.assemblies
    [symbol_table.Add(x.dest,ret)]
  | DotNetCall x ->
    match type_map.[x.instance] with 
    | DotNetType t -> 
      let ret = dynamicCallNonBuiltin x t symbol_table global_context.assemblies
      [symbol_table.Add(x.dest,ret)]
  | DotNetGet x ->
    match type_map.[x.instance] with 
    | DotNetType t -> 
      let ret = dotNetGet x t symbol_table global_context.assemblies
      [symbol_table.Add(x.dest,ret)]
  | DotNetSet x ->
    match type_map.[x.instance] with 
    | DotNetType t -> 
      do dotNetSet x t symbol_table global_context.assemblies
      [symbol_table]
   
and eval_rule (rule:rule)
              (ctxt:global_context)
              (args:List<obj>) :List<obj> =
    let input = Seq.zip rule.input args |> Map.ofSeq
    let called_stabs = ([input],rule.premis) ||> List.fold (fun (stabs:List<Map<local_id,obj>>) p -> // for each instruction
        stabs |> List.map (fun stab -> // for each fork
          eval_step p ctxt rule.typemap stab
        ) |> List.concat
      )
    let output = called_stabs |> List.map (fun stab-> stab.[rule.output])
    output

let load_assemblies (src:List<string>) :List<System.Reflection.Assembly> =
  src |> List.map (fun str-> System.Reflection.Assembly.LoadFrom(str) )

let eval_main (src:fromTypecheckerWithLove) =
  let ctxt:global_context={assemblies=load_assemblies src.assemblies;funcs=src.funcs;datas=src.datas;lambdas=src.lambdas;main=src.main}
  do printf "starting interpreting\n"
  let res = eval_rule ctxt.main ctxt []
  do printf "results:\n%s" (res|>List.map (sprintf "  %A\n")|>String.concat "")
  res

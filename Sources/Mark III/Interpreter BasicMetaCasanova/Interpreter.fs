module Interpreter
open CodegenInterface
open ParserMonad

let print_loc  (x:local_id) = match x with Named x -> x | Tmp x -> sprintf "_%d" x

let print_premisse (p:premisse) :string =
  let print_id   (x:Id) = x.Namespace @ [x.Name] |> String.concat "." 
  let print_lamb (x:LambdaId) = x.Namespace @ [sprintf "lambda%d" x.Name] |> String.concat "." 
  match p with
  | Literal            x -> sprintf "LITR %A -> %s" x.value (print_loc x.dest)
  | Conditional        x -> sprintf "COND %s %A %s" (print_loc x.left) x.predicate (print_loc x.right)
  | Destructor         x -> sprintf "DTOR %s %s -> %s" (print_id x.destructor) (print_loc x.source) (x.args|>List.map print_loc|>String.concat " ")
  | ConstructorClosure x -> sprintf "CTOR %s -> %s" (print_id x.func) (print_loc x.dest)
  | FuncClosure        x -> sprintf "FUNC %s -> %s" (print_id x.func) (print_loc x.dest)
  | LambdaClosure      x -> sprintf "LAMB %s -> %s" (print_lamb x.func) (print_loc x.dest)
  | Application        x -> sprintf "APPL %s %s -> %s" (print_loc x.closure) (print_loc x.argument) (print_loc x.dest)
  | ApplicationCall    x -> sprintf "CALL %s %s -> %s" (print_loc x.closure) (print_loc x.argument) (print_loc x.dest)
  | DotNetCall         x -> sprintf "NCAL %s(%s) -> %s" (print_id x.func) (x.args|>List.map print_loc|>String.concat " ") (print_loc x.dest)
  | DotNetConstructor  x -> sprintf "NCON %s(%s) -> %s" (print_id x.func) (x.args|>List.map print_loc|>String.concat " ") (print_loc x.dest)
  | DotNetProperty     x -> sprintf "NPRO %s.%s -> %s" (print_loc x.instance) (print_loc x.property) (print_loc x.dest)

let callNonBuiltin (x:DotNetCall):obj =
    let t = System.Type.GetType(x.func.Namespace|>String.concat ".")
    //let res = t.InvokeMember(x.func.Name,System.Reflection.BindingFlags.Default,System.Type.DefaultBinder,
    box 1

let rec eval_step (p:premisse)
                  (global_context:fromTypecheckerWithLove)
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
  do System.Console.ForegroundColor <- System.ConsoleColor.Yellow
  do printf "%s\n" (print_premisse p)
  do System.Console.ResetColor()
  *)
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
  | Application x -> // todo: lambdas (use :?)
    let id,args = symbol_table.[x.closure] :?> Id*List<obj>
    let res = id,(args@[symbol_table.[x.argument]])
    [symbol_table.Add(x.dest,(box res))]
  | ApplicationCall x -> // todo: lambdas (use :?)
    let id,args = symbol_table.[x.closure] :?> Id*List<obj>
    let filled_args = args@[symbol_table.[x.argument]]
    match global_context.datas |> List.tryFind (fun(x,_)->x=id) with
    | Some(id,data) ->
      [symbol_table.Add(x.dest,box filled_args)]
    | None ->
      global_context.rules.[id] |> List.map (fun rule -> // for each rule
          let results = eval_rule rule global_context filled_args 
          results |> List.map (fun v->symbol_table.Add(x.dest,v))
        )|>List.concat
  | DotNetCall x ->
    let o = 
      match x.func.Namespace with
      | ["System";"Int32"] when x.args.Length=2 ->
        let l = symbol_table.[x.args.[0]] :?> System.Int32
        let r = symbol_table.[x.args.[1]] :?> System.Int32
        match x.func.Name with "+"->box(l+r) | "/"->box(l/r) | "*"->box(l*r) | "%"->box(l%r) | "-"->box(l-r) | _ ->callNonBuiltin x
      | ["System";"Int64"] when x.args.Length=2 ->
        let l = symbol_table.[x.args.[0]] :?> System.Int64
        let r = symbol_table.[x.args.[1]] :?> System.Int64
        match x.func.Name with "+"->box(l+r) | "/"->box(l/r) | "*"->box(l*r) | "%"->box(l%r) | "-"->box(l-r) | _ ->callNonBuiltin x
      | ["System";"Single"] when x.args.Length=2 ->
        let l = symbol_table.[x.args.[0]] :?> System.Single
        let r = symbol_table.[x.args.[1]] :?> System.Single
        match x.func.Name with "+"->box(l+r) | "/"->box(l/r) | "*"->box(l*r) | "%"->box(l%r) | "-"->box(l-r) | _ ->callNonBuiltin x
      | ["System";"Double"] when x.args.Length=2 ->
        let l = symbol_table.[x.args.[0]] :?> System.Double
        let r = symbol_table.[x.args.[1]] :?> System.Double
        match x.func.Name with "+"->box(l+r) | "/"->box(l/r) | "*"->box(l*r) | "%"->box(l%r) | "-"->box(l-r) | _ ->callNonBuiltin x
      | _ -> callNonBuiltin x
    [symbol_table.Add(x.dest,o)]

and eval_rule (rule:rule)
              (global_context:fromTypecheckerWithLove)
              (args:List<obj>) :List<obj> =
    let input = Seq.zip rule.input args |> Map.ofSeq
    let called_stabs = ([input],rule.premis) ||> List.fold (fun (stabs:List<Map<local_id,obj>>) p -> // for each instruction
        stabs |> List.map (fun stab -> // for each fork
          eval_step p global_context rule.typemap stab
        ) |> List.concat
      )
    let output = called_stabs |> List.map (fun stab-> stab.[rule.output])
    output

let eval_main (woo:fromTypecheckerWithLove) =
  do printf "starting interpreting\n"
  let res = eval_rule woo.main woo []
  do printf "results:\n%s" (res|>List.map (sprintf "  %A\n")|>String.concat "")
  res

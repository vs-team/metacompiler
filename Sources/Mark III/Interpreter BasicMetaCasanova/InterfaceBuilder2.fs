module InterfaceBuilder2

open Common
open CodegenInterface

let build_interface (rules:List<string*List<Id*rule>>)
  (datas:List<string*List<Id*data>>) =
  let add_to_rule_map = List.fold (fun (state:Map<Id,List<rule>>) (id,(ru:rule)) -> 
    match state.TryFind(id) with
    | Some x -> state.Add(id,(ru::x))
    | None -> state.Add(id,[ru]))
  let rule_res = List.fold (fun (state:Map<Id,List<rule>>) (st,ls) -> add_to_rule_map state ls) Map.empty rules
  
  let data_res = List.collect (fun (_,ls) -> ls) datas

  let build_interface main :fromTypecheckerWithLove = 
    {
      assemblies = []
      funcs = rule_res
      lambdas = Map.empty
      datas = data_res
      main = main
    }
  let rule_list = Map.toList rule_res
  match List.tryFind (fun (id,r) -> id.Name = "main") rule_list with
  | Some (_,[x]) ->
    Some (build_interface x)
  | Some (_,_) -> 
    printfn "could not find main or there is more than one main."
    None
  | None -> 
    printfn "could not find main."
    None
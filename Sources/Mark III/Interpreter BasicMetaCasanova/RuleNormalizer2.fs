module RuleNormalizer2

open Common
open ParserMonad
open RuleParser2

type StandardId = Id*Namespace

type GlobalId = FuncId of Id*Namespace
              | LambdaId  of int*Namespace

type NormalId = VarId of Id*Position*int
              | TempId of int*Position*int

type Premisse = Alias              of NormalId*NormalId
              | Literal            of Literal*NormalId
              | Conditional        of NormalId*Condition*NormalId
              | Destructor         of NormalId*StandardId*List<NormalId>
              | McClosure          of GlobalId*NormalId
              | DotNetClosure      of StandardId*NormalId
              | ConstructorClosure of StandardId*NormalId
              | Apply              of NormalId*NormalId*NormalId
              | ApplyCall          of NormalId*NormalId*NormalId

type NormalizedRule =
  {
    Name : Id
    CurentNamespace : Namespace
    Input : List<NormalId>
    Output : NormalId
    Premis : List<Premisse>
    Pos : Position
  }

type NormalizerContext =
  {
    LocalIdCounter : int
  } with 
  static member Zero =
    {
      LocalIdCounter = 0
    }

let get_local_id_number :Parser<Premises,NormalizerContext,int> =
  prs{
    let! ctxt = getContext
    let new_local_id = ctxt.LocalIdCounter
    do! setContext {ctxt with LocalIdCounter = ctxt.LocalIdCounter + 1}
    return new_local_id
  }

let normalize_right_premistree (sourceid:NormalId) (arg:ArgId) 
  (output:PremisFunctionTree):Parser<Premises,NormalizerContext,_> =
  prs{
    match output with
    | RuleParser2.Literal(lit,pos) -> 
      return! fail (NormalizeError (sprintf "right of premis may not have literals%A" pos))
    | RuleParser2.RuleBranch(x) ->
      return! fail (NormalizeError (sprintf "right of premis may not have a rule%A" x.Pos))
    | RuleParser2.DataBranch(x) ->
      let id,pos,i = arg 
      let arg = VarId(arg)
      let! int_id = get_local_id_number
      let apply_dest = TempId(int_id,pos,i)
      let apply_call = ApplyCall(sourceid,arg,apply_dest)
      let list_normal_id = List.collect (fun (arg) -> [VarId(arg)]) x.Args
      let data_standardid = x.Name,x.CurrentNamespace
      let destructor = Destructor(apply_dest,data_standardid,list_normal_id)
      return apply_call::[destructor]
    | RuleParser2.IdBranch(x) -> 
      let arg = VarId(arg)
      let apply_dest = VarId(x.Name,x.Pos,0)
      let apply_call = ApplyCall(sourceid,arg,apply_dest)
      return [apply_call]
  }

let rec Normalize_arguments (sourceid:NormalId) (args:List<ArgId>) 
  (output:PremisFunctionTree) :Parser<Premises,NormalizerContext,List<Premisse>> =
  prs{
    match args with
    | arg::[] -> 
      return! normalize_right_premistree sourceid arg output
    | (id,pos,i)::xs -> 
      let arg = VarId(id,pos,i)
      let! int_id = get_local_id_number
      let dest = TempId(int_id,pos,i)
      let! res = Normalize_arguments dest xs output
      return (Apply(sourceid,arg,dest)::res)
    | [] ->
      return! fail (NormalizeError "Normalizer failded at")

  }

let normalize_Left_literal (input:PremisFunctionTree) (output:PremisFunctionTree)
  :Parser<Premises,NormalizerContext,List<Premisse>> =
  prs{
    return []
  }

let normalize_Left_premistree (input:PremisFunctionTree) (output:PremisFunctionTree)
  :Parser<Premises,NormalizerContext,List<Premisse>> =
  prs{
    match input with
    | RuleParser2.Literal(lit,pos) -> 
      let! local_id = get_local_id_number
      let normalid = TempId(local_id,pos,0)
      let res = Literal(lit,normalid)
      return [res]
    | RuleParser2.RuleBranch(x) -> 
      let! cons_id = get_local_id_number
      let construct_id = TempId(cons_id,x.Pos,0)
      let construct = McClosure (FuncId(x.Name,x.CurrentNamespace), construct_id)
      let! applypremisses = 
        Normalize_arguments construct_id x.Args output
      return (construct::applypremisses)
    | RuleParser2.DataBranch(x) -> 
      let! cons_id = get_local_id_number
      let construct_id = TempId(cons_id,x.Pos,0)
      let construct = ConstructorClosure ((x.Name,x.CurrentNamespace), construct_id)
      let! applypremisses = 
        Normalize_arguments construct_id x.Args output
      return (construct::applypremisses)
    | RuleParser2.IdBranch(x) -> 
      let source = VarId(x.Name,x.Pos,0)
      match output with
      | DataBranch(y) ->
        let data_id = y.Name,y.CurrentNamespace
        let list_normal_id = List.collect (fun (arg) -> [VarId(arg)]) y.Args
        return [Destructor(source,data_id,list_normal_id)]
      | _ -> return! fail (NormalizeError "id can only go to data.")
  }

let normalize_premis :Parser<Premises,NormalizerContext,List<Premisse>> =
  prs{
    let! nextprem = step
    match nextprem with
    | RuleParser2.Implication(left,right) ->
      return! normalize_Left_premistree left right
    | RuleParser2.Conditional(cond,left,right) -> return []
  }

let normalize_rules :Parser<RuleDef,NormalizerContext,_> =
  prs{
    let! nextrule = step
    let name = nextrule.Name
    let currentnamespace = nextrule.CurrentNamespace
    let! ctxt = getContext
    let! premises = 
      UseDifferentSrcAndCtxt normalize_premis nextrule.Premises ctxt
    let inputs = List.collect (fun x -> [VarId(x)]) nextrule.Input
    //let output = nextrule.
    return ()
  }

let normalize_rules_and_data :Parser<Id*List<RuleDef>,Position,_> =
  prs{
    let! id,rules = step
    return ()
  }
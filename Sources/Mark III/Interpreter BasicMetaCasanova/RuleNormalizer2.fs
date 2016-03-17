module RuleNormalizer2

open Common
open ParserMonad
open RuleParser2

type StandardId = Id*Namespace

type GlobalId = FuncId of Id*Namespace
              | LambdaId  of int*Namespace

type NormalId = VarId of Id*Position
              | TempId of int*Position

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

let normalize_right_premistree (right_prem:PremisFunctionTree)
  :Parser<Premises,NormalizerContext,List<Premisse>*NormalId> =
  prs{
    match right_prem with
    | RuleParser2.Literal(lit,pos) -> 
      return! fail (NormalizeError (sprintf "right of premis may not have literals%A" pos))
    | RuleParser2.RuleBranch(x) ->
      return! fail (NormalizeError (sprintf "right of premis may not have a rule%A" x.Pos))
    | RuleParser2.DataBranch(x) ->
      let destruct = x.Name,x.CurrentNamespace
      let! tempid = get_local_id_number
      let sourceid = TempId(tempid,x.Pos)
      let idlist = List.collect (fun arg -> [VarId(arg)]) x.Args
      let destruct = Destructor(sourceid,destruct,idlist)
      return [destruct],sourceid
    | RuleParser2.IdBranch(x) -> 
      let sourceid = VarId(x.Name,x.Pos)
      return [],sourceid
  }

let rec Normalize_arguments (sourceid:NormalId) (args:List<ArgId>) 
  (output:NormalId) :Parser<Premises,NormalizerContext,List<Premisse>> =
  prs{
    match args with
    | arg::[] -> 
      let apply_arg = VarId(arg)
      let apply_call = ApplyCall(sourceid,apply_arg,output)
      return [apply_call]
    | (id,pos)::xs -> 
      let apply_arg = VarId(id,pos)
      let! int_id = get_local_id_number
      let dest = TempId(int_id,pos)
      let! res = Normalize_arguments dest xs output
      return (Apply(sourceid,apply_arg,dest)::res)
    | [] ->
      return! fail (NormalizeError "Normalizer failded at")

  }

let normalize_Left_premistree (input:PremisFunctionTree) (output:NormalId)
  :Parser<Premises,NormalizerContext,List<Premisse>> =
  prs{
    match input with
    | RuleParser2.Literal(lit,pos) -> 
      let! local_id = get_local_id_number
      let normalid = TempId(local_id,pos)
      let res = Literal(lit,normalid)
      return [res]
    | RuleParser2.RuleBranch(x) -> 
      let! cons_id = get_local_id_number
      let construct_id = TempId(cons_id,x.Pos)
      let construct = McClosure (FuncId(x.Name,x.CurrentNamespace), construct_id)
      let! applypremisses = 
        Normalize_arguments construct_id x.Args output
      return (construct::applypremisses)
    | RuleParser2.DataBranch(x) -> 
      let! cons_id = get_local_id_number
      let construct_id = TempId(cons_id,x.Pos)
      let construct = ConstructorClosure ((x.Name,x.CurrentNamespace), construct_id)
      let! applypremisses = 
        Normalize_arguments construct_id x.Args output
      return (construct::applypremisses)
    | RuleParser2.IdBranch(x) -> 
      let normalid = VarId(x.Name,x.Pos)
      return [Alias(normalid,output)]
  }

let normalize_premis :Parser<Premises,NormalizerContext,List<Premisse>> =
  prs{
    let! nextprem = step
    match nextprem with
    | RuleParser2.Implication(left,right) ->
      let! premeses,normalid = normalize_right_premistree right
      return! normalize_Left_premistree left normalid
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
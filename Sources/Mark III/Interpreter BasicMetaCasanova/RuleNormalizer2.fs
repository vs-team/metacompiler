module RuleNormalizer2

open Common
open ParserMonad
open DeclParser2
open RuleParser2

type NormalId = VarId of Id*Position
              | TempId of int*Position

type AliasType = NormalId*NormalId

type Premisse = Alias              of AliasType
              | Literal            of Literal*NormalId
              | Conditional        of NormalId*Condition*NormalId
              | Destructor         of NormalId*Id*List<NormalId>
              | McClosure          of Id*NormalId
              | DotNetClosure      of Id*NormalId
              | ConstructorClosure of Id*NormalId
              | Apply              of NormalId*NormalId*NormalId
              | ApplyCall          of NormalId*NormalId*NormalId

type NormalizedRule =
  {
    Name : Id
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
      let (destruct:Id) = x.Name
      let! tempid = get_local_id_number
      let sourceid = TempId(tempid,x.Pos)
      let idlist = List.collect (fun arg -> [VarId(arg)]) x.Args
      let destruct = Destructor(sourceid,destruct,idlist)
      return [destruct],sourceid
    | RuleParser2.IdBranch(x) -> 
      let sourceid = VarId(x.Name,x.Pos)
      return [],sourceid
    | _ -> return! fail (NormalizeError "no typefunc premises alowed in normal rules.")
  }

let rec normalize_arguments (sourceid:NormalId) (args:List<ArgId>) 
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
      let! res = normalize_arguments dest xs output
      return (Apply(sourceid,apply_arg,dest)::res)
    | [] -> return [Alias(sourceid,output)]
  }

let rec normalize_data_arguments (sourceid:NormalId) (args:List<ArgId>) 
  (output:NormalId) :Parser<Premises,NormalizerContext,List<Premisse>> =
  prs{
    match args with
    | (id,pos)::xs -> 
      let apply_arg = VarId(id,pos)
      let! int_id = get_local_id_number
      let dest = TempId(int_id,pos)
      let! res = normalize_data_arguments dest xs output
      return (Apply(sourceid,apply_arg,dest)::res)
    | [] -> return [Alias(sourceid,output)]
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
      let construct = McClosure (x.Name, construct_id)
      let! applypremisses = 
        normalize_arguments construct_id x.Args output
      return (construct::applypremisses)
    | RuleParser2.DataBranch(x) -> 
      let! cons_id = get_local_id_number
      let construct_id = TempId(cons_id,x.Pos)
      let construct = ConstructorClosure (x.Name, construct_id)
      let! applypremisses = 
        normalize_data_arguments construct_id x.Args output
      return (construct::applypremisses)
    | RuleParser2.IdBranch(x) -> 
      let normalid = VarId(x.Name,x.Pos)
      return [Alias(normalid,output)]
    | _ -> return! fail (NormalizeError "no typefunc premises alowed in normal rules.")
  }

let normalize_premis :Parser<Premises,NormalizerContext,List<Premisse>> =
  prs{
    let! nextprem = step
    match nextprem with
    | RuleParser2.Implication(left,right) ->
      let! prem_right,normalid = normalize_right_premistree right
      let! prem_left = normalize_Left_premistree left normalid
      return prem_left@prem_right
    | RuleParser2.Conditional(cond,left,right) -> return []
  }

let normalize_rule :Parser<RuleDef,NormalizerContext,NormalizedRule> =
  prs{
    let! next = step
    let! premises_list = UseDifferentSrc (normalize_premis |> itterate) next.Premises 
    let premises = List.concat premises_list
    let inputs = List.collect (fun x -> [VarId(x)]) next.Input
    let! outputid = UseDifferentSrc get_local_id_number []
    let outputnormalid = TempId(outputid,next.Pos)
    let outputmonad = normalize_Left_premistree next.Output outputnormalid
    let! output = UseDifferentSrc outputmonad []
    let premises = premises @ output
    let result = {Name = next.Name ;
                  Input = inputs ; Output = outputnormalid ; Premis = premises ;
                  Pos = next.Pos}
    return result
  }

let rec find_alias (prem:List<Premisse>): List<Premisse>*List<AliasType> =
  match prem with
  | x::xs -> 
    match x with
    | Alias(ali) -> 
      let pr,al = (find_alias xs)
      pr,(ali::al)
    | ls -> 
      let pr,al = find_alias xs
      (x::pr,al)
  | [] -> [],[]

let (|=) (a:NormalId) (b:NormalId) =
  match a with
  | VarId(ida,posa) ->
    match b with
    | VarId(idb,posb) -> ida = idb 
    | _ -> false
  | TempId(ida,pos) -> 
    match b with
    | TempId(idb,posb) -> ida = idb
    | _ -> false

let change_alias_premis (prem:Premisse) ((lalias,ralias):AliasType) =
  match prem with
  | Alias(_) -> failwith "Alias sould not be in this list."
  | Literal(l,r) ->
    if r |= ralias then Literal(l,lalias) else Literal(l,r)
  | Conditional(l,c,r) ->
    if l |= ralias then Conditional(lalias,c,r)
    elif r |= ralias then Conditional(l,c,lalias)
    else Conditional(l,c,r)
  | Destructor(l,s,r) ->
    let test = List.exists (fun x -> x |= ralias) r
    if l |= ralias then Destructor(lalias,s,r)
    elif test then 
      let right = List.collect (fun x -> if x |= ralias then [lalias] else [x]) r
      Destructor(l,s,right)
    else Destructor(l,s,r)
  | McClosure(g,r) -> McClosure(g,r)
  | DotNetClosure(s,r) -> DotNetClosure(s,r)  
  | ConstructorClosure(s,r) -> ConstructorClosure(s,r)  
  | Apply(l,a,r) -> 
    if a |= ralias then Apply(l,lalias,r) 
    else Apply(l,a,r)
  | ApplyCall (l,a,r) -> 
    if a |= ralias then ApplyCall(l,lalias,r)
    elif r |= ralias then ApplyCall(l,a,lalias)
    else ApplyCall(l,a,r)

let de_alias_output (output:NormalId) (alias:List<AliasType>) =
  List.fold (fun ni (l,r) -> if ni |= r then l else ni) output (List.rev alias)

let de_alias_rule :Parser<NormalizedRule,NormalizerContext,_> =
  prs{
    let! next = step
    let prem,alias = find_alias next.Premis
    let revalias = List.rev alias
    let res = 
      List.collect(fun p -> 
        [List.fold(fun pr a -> change_alias_premis pr a) p revalias]) prem
    let output = de_alias_output next.Output alias
    return {next with Output = output ; Premis = res}
  }

let normalize_rules 
  :Parser<string*RuleContext*List<SymbolDeclaration>,string
    ,string*List<NormalizedRule>*List<SymbolDeclaration>> =
  prs{
    let! id,rules,decl = step
    let rule_normalizer = normalize_rule |> itterate
    let! normalized_rules = 
      UseDifferentSrcAndCtxt rule_normalizer rules.Rules NormalizerContext.Zero
    let de_alias = de_alias_rule |> itterate
    let! de_aliased_rules = 
      UseDifferentSrcAndCtxt de_alias normalized_rules NormalizerContext.Zero
    return id,de_aliased_rules,decl
  }


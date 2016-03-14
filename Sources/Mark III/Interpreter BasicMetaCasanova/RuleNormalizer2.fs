module RuleNormalizer2

open Common
open ParserMonad
open DeclParser2
open RuleParser2

type StandardId = Id*Namespace

type GlobalId = FuncId of Id*Namespace
              | LambdaId  of int*Namespace

type NormalId = VarId of Id*List<DeclType>*Position
              | TempId of int*List<DeclType>*Position

type Premisse = Literal            of Literal*NormalId
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
    Signature : List<DeclType>
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

let data_args_to_normalid (data:FunctionBranch) (decl:List<DeclType>) 
  :Parser<Premises,NormalizerContext,List<NormalId>> =
  prs{
    match decl with
    | x::[] -> 
      if data.Return = x 
      then return List.collect (fun ((id,pos),dec) -> [VarId(id,[dec],pos)]) data.Args
      else return! fail (NormalizeError "wrong decl.")
    | _ -> return! fail (NormalizeError "signature for destructor is to long.")
  }

let normalize_right_premistree (sourceid:NormalId) (arg:ArgId) (decl:List<DeclType>) 
  (output:PremisFunctionTree):Parser<Premises,NormalizerContext,_> =
  prs{
    match output with
    | RuleParser2.Literal(lit,pos) -> 
      return! fail (NormalizeError (sprintf "right of premis may not have literals%A" pos))
    | RuleParser2.RuleBranch(x) ->
      return! fail (NormalizeError (sprintf "right of premis may not have a rule%A" x.Pos))
    | RuleParser2.DataBranch(x) ->
      let id,pos = arg 
      let arg = VarId(id,decl,pos)
      let! int_id = get_local_id_number
      let apply_dest = TempId(int_id,decl,pos)
      let apply_call = ApplyCall(sourceid,arg,apply_dest)
      let! list_normal_id = data_args_to_normalid x decl
      let data_standardid = x.Name,x.CurrentNamespace
      let destructor = Destructor(apply_dest,data_standardid,list_normal_id)
      return apply_call::[destructor]
  }

let rec Normalize_arguments (sourceid:NormalId) (args:List<ArgId>) (decl:List<DeclType>) 
  (output:PremisFunctionTree) :Parser<Premises,NormalizerContext,List<Premisse>> =
  prs{
    match args with
    | arg::[] -> 
      return! normalize_right_premistree sourceid arg decl output
    | (id,pos)::xs -> 
      match decl with
      | a::al ->
        let arg = VarId(id,[a],pos)
        let! int_id = get_local_id_number
        let dest = TempId(int_id,al,pos)
        let! res = Normalize_arguments dest xs al output
        return (Apply(sourceid,arg,dest)::res)
      | [] -> 
        return! fail (NormalizeError "Type aplication shorter then given arguments")
    | [] ->
      return! fail (NormalizeError "Normalizer failded at")

  }

let normalize_Left_premistree (input:PremisFunctionTree) (output:PremisFunctionTree)
  :Parser<Premises,NormalizerContext,List<Premisse>> =
  prs{
    match input with
    | RuleParser2.Literal(lit,pos) -> 
      let! local_id = get_local_id_number
      let normalid = TempId(local_id,[DeclType.Id("int",pos)],pos)
      let res = Literal(lit,normalid)
      return [res]
    | RuleParser2.RuleBranch(x) -> 
      let! cons_id = get_local_id_number
      let cons_args,cons_types = 
        List.fold (fun (xs,ys) (x,y) -> ((x::xs),(y::ys))) ([],[]) x.Args
      let construct_id = TempId(cons_id,cons_types@[x.Return],x.Pos)
      let construct = McClosure (FuncId(x.Name,x.CurrentNamespace), construct_id)
      let! applypremisses = 
        Normalize_arguments construct_id cons_args (cons_types@[x.Return]) output
      return (construct::applypremisses)
    | RuleParser2.DataBranch(x) -> 
      let! cons_id = get_local_id_number
      let cons_args,cons_types = 
        List.fold (fun (xs,ys) (x,y) -> ((x::xs),(y::ys))) ([],[]) x.Args
      let construct_id = TempId(cons_id,cons_types@[x.Return],x.Pos)
      let construct = ConstructorClosure ((x.Name,x.CurrentNamespace), construct_id)
      let! applypremisses = 
        Normalize_arguments construct_id cons_args (cons_types@[x.Return]) output
      return (construct::applypremisses)
    //| RuleParser2.IdBranch(x) ->
      
  }

let normalize_premis :Parser<Premises,NormalizerContext,List<Premisse>> =
  prs{
    let! nextprem = step
    match nextprem with
    | RuleParser2.Implication(left,right) ->
      return! normalize_Left_premistree left right
    //| RuleParser2.Conditional(cond,left,right) ->
  }

let normalize_rules :Parser<RuleDef,NormalizerContext,_> =
  prs{
    let! nextrule = step

    let name = nextrule.Name
    let currentnamespace = nextrule.CurrentNamespace
    return ()
  }

let normalize_rules_and_data :Parser<Id*List<RuleDef>*List<SymbolDeclaration>,Position,_> =
  prs{
    let! id,rules,datadecl = step
    return ()
  }
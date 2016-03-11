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

let rec Normalize_arguments (norid:NormalId) (args:List<ArgId>) (decl:List<DeclType>) 
  :Parser<Premises,NormalizerContext,_> =
  prs{
    match args with
    | (id,pos)::[] -> 
      let arg = VarId(id,decl,pos)
      let! int_id = get_local_id_number
      let dest = TempId(int_id,decl,pos)
      return ([ApplyCall(norid,arg,dest)]),dest
    | (id,pos)::xs -> 
      match decl with
      | a::al ->
        let arg = VarId(id,[a],pos)
        let! int_id = get_local_id_number
        let dest = TempId(int_id,al,pos)
        let! res,end_dest = Normalize_arguments dest xs al
        return (Apply(norid,arg,dest)::res),end_dest
      | [] -> 
        return! fail (NormalizeError "Type aplication shorter then given arguments")
    | [] ->
      return! fail (NormalizeError "Normalizer failded at")

  }

let normalize_Left_premistree (input:PremisFunctionTree) 
  :Parser<Premises,NormalizerContext,List<Premisse>*NormalId> =
  prs{
    match input with
    | RuleParser2.Literal(lit,pos) -> 
      let! local_id = get_local_id_number
      let normalid = TempId(local_id,[DeclType.Id("int",pos)],pos)
      let res = Literal(lit,normalid)
      return [res],normalid
    | RuleParser2.RuleBranch(x) -> 
      let! cons_id = get_local_id_number
      let cons_args,cons_types = 
        List.fold (fun (xs,ys) (x,y) -> ((x::xs),(y::ys))) ([],[]) x.Args
      let construct_id = TempId(cons_id,cons_types@[x.Return],x.Pos)
      let construct = McClosure (FuncId(x.Name,x.CurrentNamespace), construct_id)
      let! applypremisses,applyres = 
        Normalize_arguments construct_id cons_args (cons_types@[x.Return])
      return (construct::applypremisses),applyres
    | RuleParser2.DataBranch(x) -> 
      let! cons_id = get_local_id_number
      let cons_args,cons_types = 
        List.fold (fun (xs,ys) (x,y) -> ((x::xs),(y::ys))) ([],[]) x.Args
      let construct_id = TempId(cons_id,cons_types@[x.Return],x.Pos)
      let construct = ConstructorClosure ((x.Name,x.CurrentNamespace), construct_id)
      let! applypremisses,applyres = 
        Normalize_arguments construct_id cons_args (cons_types@[x.Return])
      return (construct::applypremisses),applyres
    //| RuleParser2.IdBranch(x) ->
      
  }

let normalize_premis :Parser<Premises,NormalizerContext,_> =
  prs{
    let! nextprem = step
    match nextprem with
    | RuleParser2.Conditional(cond,left,right) ->
      return ()
    | RuleParser2.Implication(left,right) ->
      return ()
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
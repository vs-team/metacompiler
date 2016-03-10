module RuleNormalizer2

open Common
open ParserMonad
open DeclParser2
open RuleParser2

type StandardId = Id*Namespace

type GlobalId = FuncId of Id*Namespace
              | LambdaId  of int*Namespace

type NormalId = FuncId of Id*List<DeclType>*Position
              | TempId of int*List<DeclType>*Position

type Premisse = Literal            of Literal*NormalId
              | Conditional        of NormalId*Condition*NormalId
              | Destructor         of NormalId*StandardId*List<NormalId>
              | McClosure          of GlobalId*NormalId
              | DotNetClosure      of StandardId*NormalId
              | ConstructorClosure of StandardId*NormalId
              | Apply              of NormalId*NormalId*NormalId
              | Call               of NormalId*NormalId*NormalId

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

let get_local_id :Parser<Premises,NormalizerContext,int> =
  prs{
    let! ctxt = getContext
    let new_local_id = ctxt.LocalIdCounter
    do! setContext {ctxt with LocalIdCounter = ctxt.LocalIdCounter + 1}
    return new_local_id
  }

let normalize_Premis_functiontree (input:PremisFunctionTree) :Parser<Premises,NormalizerContext,_> =
  prs{
    match input with
    | RuleParser2.Literal(lit,pos) -> 
      let! local_id = get_local_id
      let normalid = (local_id,[DeclType.Id("int",pos)],pos)
      let res = Literal(lit,TempId(normalid))
      return [res]
    | RuleBranch(x) -> 
      
      return []
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
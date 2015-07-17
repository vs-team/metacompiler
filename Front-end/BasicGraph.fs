module BasicGraph

open TypeInference
open System
open Utilities
open ParserMonad
open BasicExpression
open TypeDefinition
open ConcreteExpressionParserPrelude
open ConcreteExpressionParser

type TypedExpression   = BasicExpression<Keyword, Var, Literal, Position, Type>
type UntypedExpression = BasicExpression<Keyword, Var, Literal, Position, unit>

type Scope = Scope of List<int> // which order?
  with
    override this.ToString() = 
      match this with Scope(p) -> p |> List.rev |> Seq.map (fun i -> string i + "_") |> Seq.fold (+) ""
    member this.Tail =
      match this with 
      | Scope(p::ps) -> Scope ps
      | _ -> failwith "Cannot reduce empty path"

type Rule = {
  position : Position
  input  : TypedExpression
  output : TypedExpression
  scope : Scope
  hasScope : bool
}

type Clause = {
  position : Position
  input : TypedExpression
  output: TypedExpression
  keyword: Keyword
}

type FunctionDef = {
  Identifier : string
  
}

type RuleID     = RuleID of Position
type ClauseID   = ClauseID of Position
type FunctionDefinitionID = FunctionDefinitionID of string

type BasicGraph = {
  Rules   : Map<RuleID,Rule>
  Clauses : Map<ClauseID,Clause> 
  ClausesPerRule : Map<RuleID, List<ClauseID>>
  CustomKeywordsMap : Map<FunctionDefinitionID, ParsedKeyword<Keyword, Var, Literal, Position, unit>>
}

let emptyBasicGraph = {Rules=Map.empty; Clauses=Map.empty; ClausesPerRule=Map.empty; CustomKeywordsMap = Map.empty}
let join (p:Map<'a,'b>) (q:Map<'a,'b>) = 
    [ (Map.toSeq p) ; (Map.toSeq q) ] |> Seq.concat |> Map.ofSeq

let add_rule (rule:BasicExpression<_,_,Literal, Position, Unit>) (rule_path:Scope) (hasScope:bool) ctxt : Rule*List<Clause> =
  let method_path = rule_path.Tail
  match rule with
  | Application(Implicit, Keyword(FractionLine, _, _) :: (Application(Implicit, Keyword(DoubleArrow, _, _) :: input :: output :: [], innerPos, _)) :: clauses, pos, _) ->
    let input, output, clauses = TypeInference.inferTypes input output clauses ctxt
    let rule = { 
        position = pos
        input    = input
        output   = output
        scope    = rule_path
        hasScope = hasScope }
    let clauses:List<Clause> =  
          [ for c in clauses do
              match c with
              | Application(_, Keyword(Inlined, _, _) :: _, clausePos, _)                       -> yield { keyword=Inlined;        input = c  ; output = c  ; position = clausePos }
              | Application(_, Keyword(DoubleArrow, _, _) :: (Application(Angle, _, _, _)
                as c_i) :: (Extension(_,_,_) as c_o) :: [], clausePos, _)                       -> yield { keyword=DefinedAs;      input = c_i; output = c_o; position = clausePos } 
              | Application(_, Keyword(DoubleArrow, _, _) :: c_i :: c_o :: [], clausePos, _)    -> yield { keyword=DoubleArrow;    input = c_i; output = c_o; position = clausePos } 
              | Application(_, Keyword(Equals, _, _) :: c_i :: c_o :: [], clausePos, _)         -> yield { keyword=Equals;         input = c_i; output = c_o; position = clausePos } 
              | Application(_, Keyword(NotEquals, _, _) :: c_i :: c_o :: [], clausePos, _)      -> yield { keyword=NotEquals;      input = c_i; output = c_o; position = clausePos } 
              | Application(_, Keyword(DefinedAs, _, _) :: c_i :: c_o :: [], clausePos, _)      -> yield { keyword=DefinedAs;      input = c_i; output = c_o; position = clausePos } 
              | Application(_, Keyword(GreaterThan, _, _) :: c_i :: c_o :: [], clausePos, _)    -> yield { keyword=GreaterThan;    input = c_i; output = c_o; position = clausePos } 
              | Application(_, Keyword(GreaterOrEqual, _, _) :: c_i :: c_o :: [], clausePos, _) -> yield { keyword=GreaterOrEqual; input = c_i; output = c_o; position = clausePos } 
              | Application(_, Keyword(SmallerThan, _, _) :: c_i :: c_o :: [], clausePos, _)    -> yield { keyword=SmallerThan;    input = c_i; output = c_o; position = clausePos } 
              | Application(_, Keyword(SmallerOrEqual, _, _) :: c_i :: c_o :: [], clausePos, _) -> yield { keyword=SmallerOrEqual; input = c_i; output = c_o; position = clausePos } 
              | Application(_, Keyword(Divisible, _, _) :: c_i :: c_o :: [], clausePos, _)      -> yield { keyword=Divisible;      input = c_i; output = c_o; position = clausePos } 
              | Application(_, Keyword(NotDivisible, _, _) :: c_i :: c_o :: [], clausePos, _)   -> yield { keyword=NotDivisible;   input = c_i; output = c_o; position = clausePos } 
              | _ -> failwithf "Unexpected clause @ %A" c.DebugInformation
          ]
    rule,clauses
  | _ ->
    failwithf "Cannot extract rule shape @ %A" rule.DebugInformation

let process_rules (path:List<int>) (rules:List<BasicExpression<_,_,Literal,Position, Unit>>) ctxt :List<Rule*List<Clause>> = 
  let mutable ret : ref<List<Rule*List<Clause>>> = ref []
  let rec process_rules (path:List<int>) (rules:List<BasicExpression<_,_,Literal,Position, Unit>>) ctxt (lst : ref<List<Rule*List<Clause>>>):unit= 
    for rule,i in rules |> Seq.mapi (fun i r -> r,i) do
      let path' = i :: path
      let self,hasScope = 
        match rule with
        | Application(_, Keyword(Nesting, _, _) :: self :: children, pos, _) -> 
          do process_rules path' children ctxt lst
          self,true
        | self -> self,false
      match self with
      | Application(Implicit, Keyword(FractionLine, _, _) :: (Application(Implicit, Keyword(DoubleArrow, _, _) :: input :: output, clausesPos, _)) :: clauses, pos, _) ->
        let inputKeyword = extractLeadingKeyword input
        let new_rule = (add_rule self (Scope path') hasScope ctxt)
        lst := new_rule  :: !lst
      | _ -> failwithf "Malformed rule @ %A" self.DebugInformation
      ()
  do process_rules path rules ctxt ret
  !ret

let rec generateBasicGraph (rules:List<Rule*List<Clause>>) (graph:BasicGraph) : BasicGraph =
  match rules with
  | []    -> graph
  | x::xs ->
      let rule,clauses = x
      let new_rule_map = graph.Rules.Add(RuleID(rule.position),rule)
      let rec foreach_clause (clauses:List<Clause>) (clausemap:Map<ClauseID,Clause>) : Map<ClauseID,Clause> =
        match clauses with
        | []    -> clausemap
        | x::xs -> foreach_clause xs (clausemap.Add(ClauseID(x.position),x))
      let new_clause_map = join graph.Clauses (foreach_clause clauses Map.empty)
      let new_clauses_per_rule_map = graph.ClausesPerRule.Add(RuleID(rule.position), (new_clause_map |> Map.toList |> List.map (fun (x,y) -> x)))
      let newgraph:BasicGraph = { 
        Rules = new_rule_map
        Clauses = new_clause_map
        ClausesPerRule = new_clauses_per_rule_map
        CustomKeywordsMap = Map.empty}
      generateBasicGraph xs newgraph

let generate (rules:List<BasicExpression<_,_,Literal,Position, Unit>>) (ctxt:ConcreteExpressionContext) = 
    let graph = generateBasicGraph (process_rules [] rules ctxt) emptyBasicGraph
    let keywords = ctxt.CustomKeywordsMap |> Map.toList |> List.map (fun (k,v) -> FunctionDefinitionID(k),v) |> Map.ofList
    { graph with CustomKeywordsMap = keywords }

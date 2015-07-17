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
  Position : Position
  Input    : TypedExpression
  Output   : TypedExpression
  Scope    : Scope
  HasScope : bool
}

type Clause = {
  Position : Position
  Input    : TypedExpression
  Output   : TypedExpression
  Keyword  : Keyword
}

type FunctionDefinition   = ParsedKeyword<Keyword, Var, Literal, Position, unit>

type RuleID     = RuleID of Position
type ClauseID   = ClauseID of Position

type BasicGraph = {
  Rules   : Map<RuleID,Rule>
  Clauses : Map<ClauseID,Clause> 
  ClausesPerRule : Map<RuleID, List<ClauseID>>
  CustomKeywordsMap : Map<string, FunctionDefinition>
}

let emptyBasicGraph = { Rules=Map.empty; Clauses=Map.empty; ClausesPerRule=Map.empty; CustomKeywordsMap = Map.empty }
let join (p:Map<'a,'b>) (q:Map<'a,'b>) = 
    [ (Map.toSeq p) ; (Map.toSeq q) ] |> Seq.concat |> Map.ofSeq

let add_rule (rule:BasicExpression<_,_,Literal, Position, Unit>) (rule_path:Scope) (HasScope:bool) ctxt : Rule*List<Clause> =
  let method_path = rule_path.Tail
  match rule with
  | Application(Implicit, Keyword(FractionLine, _, _) :: (Application(Implicit, Keyword(DoubleArrow, _, _) :: Input :: Output :: [], innerPos, _)) :: clauses, pos, _) ->
    let Input, Output, clauses = TypeInference.inferTypes Input Output clauses ctxt
    let rule = { 
        Position = pos
        Input    = Input
        Output   = Output
        Scope    = rule_path
        HasScope = HasScope }
    let clauses:List<Clause> =  
          [ for c in clauses do
              match c with
              | Application(_, Keyword(Inlined, _, _) :: _, clausePos, _)                       -> yield { Keyword=Inlined;        Input = c  ; Output = c  ; Position = clausePos }
              | Application(_, Keyword(DoubleArrow, _, _) :: (Application(Angle, _, _, _)
                as c_i) :: (Extension(_,_,_) as c_o) :: [], clausePos, _)                       -> yield { Keyword=DefinedAs;      Input = c_i; Output = c_o; Position = clausePos } 
              | Application(_, Keyword(DoubleArrow, _, _) :: c_i :: c_o :: [], clausePos, _)    -> yield { Keyword=DoubleArrow;    Input = c_i; Output = c_o; Position = clausePos } 
              | Application(_, Keyword(Equals, _, _) :: c_i :: c_o :: [], clausePos, _)         -> yield { Keyword=Equals;         Input = c_i; Output = c_o; Position = clausePos } 
              | Application(_, Keyword(NotEquals, _, _) :: c_i :: c_o :: [], clausePos, _)      -> yield { Keyword=NotEquals;      Input = c_i; Output = c_o; Position = clausePos } 
              | Application(_, Keyword(DefinedAs, _, _) :: c_i :: c_o :: [], clausePos, _)      -> yield { Keyword=DefinedAs;      Input = c_i; Output = c_o; Position = clausePos } 
              | Application(_, Keyword(GreaterThan, _, _) :: c_i :: c_o :: [], clausePos, _)    -> yield { Keyword=GreaterThan;    Input = c_i; Output = c_o; Position = clausePos } 
              | Application(_, Keyword(GreaterOrEqual, _, _) :: c_i :: c_o :: [], clausePos, _) -> yield { Keyword=GreaterOrEqual; Input = c_i; Output = c_o; Position = clausePos } 
              | Application(_, Keyword(SmallerThan, _, _) :: c_i :: c_o :: [], clausePos, _)    -> yield { Keyword=SmallerThan;    Input = c_i; Output = c_o; Position = clausePos } 
              | Application(_, Keyword(SmallerOrEqual, _, _) :: c_i :: c_o :: [], clausePos, _) -> yield { Keyword=SmallerOrEqual; Input = c_i; Output = c_o; Position = clausePos } 
              | Application(_, Keyword(Divisible, _, _) :: c_i :: c_o :: [], clausePos, _)      -> yield { Keyword=Divisible;      Input = c_i; Output = c_o; Position = clausePos } 
              | Application(_, Keyword(NotDivisible, _, _) :: c_i :: c_o :: [], clausePos, _)   -> yield { Keyword=NotDivisible;   Input = c_i; Output = c_o; Position = clausePos } 
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
      let self,HasScope = 
        match rule with
        | Application(_, Keyword(Nesting, _, _) :: self :: children, pos, _) -> 
          do process_rules path' children ctxt lst
          self,true
        | self -> self,false
      match self with
      | Application(Implicit, Keyword(FractionLine, _, _) :: (Application(Implicit, Keyword(DoubleArrow, _, _) :: Input :: Output, clausesPos, _)) :: clauses, pos, _) ->
        let InputKeyword = extractLeadingKeyword Input
        let new_rule = (add_rule self (Scope path') HasScope ctxt)
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
      let new_rule_map = graph.Rules.Add(RuleID(rule.Position),rule)
      let rec foreach_clause (clauses:List<Clause>) (clausemap:Map<ClauseID,Clause>) : Map<ClauseID,Clause> =
        match clauses with
        | []    -> clausemap
        | x::xs -> foreach_clause xs (clausemap.Add(ClauseID(x.Position),x))
      let new_clause_map = join graph.Clauses (foreach_clause clauses Map.empty)
      let new_clauses_per_rule_map = graph.ClausesPerRule.Add(RuleID(rule.Position), (new_clause_map |> Map.toList |> List.map (fun (x,y) -> x)))
      let newgraph:BasicGraph = { 
        Rules = new_rule_map
        Clauses = new_clause_map
        ClausesPerRule = new_clauses_per_rule_map
        CustomKeywordsMap = Map.empty
      }
      generateBasicGraph xs newgraph

let generate (rules:List<BasicExpression<_,_,Literal,Position, Unit>>) (ctxt:ConcreteExpressionContext) = 
    let graph = generateBasicGraph (process_rules [] rules ctxt) emptyBasicGraph
    { graph with CustomKeywordsMap = ctxt.CustomKeywordsMap }
    
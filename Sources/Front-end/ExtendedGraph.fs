module ExtendedGraph

open TypeInference
open System
open Utilities
open ParserMonad
open BasicExpression
open TypeDefinition
open ConcreteExpressionParserPrelude
open ConcreteExpressionParser
open BasicGraph

type MethodID   = MethodID of Position

type Method = {
  Position : Position
  Keyword  : string
}

type ExtendedGraph = {
  Rules   : Map<RuleID,Rule>
  Clauses : Map<ClauseID,Clause> 
  ClausesPerRule : Map<RuleID, List<ClauseID>>
  Methods : Map<MethodID,Method>
  RulesPerMethod : Map<MethodID,List<RuleID>>
}


let BuildExtendedGraph (basic:BasicGraph) (ctxt:ConcreteExpressionContext) :ExtendedGraph = 
   let NewGraph:ExtendedGraph = {
     Rules = basic.Rules
     Clauses = basic.Clauses
     ClausesPerRule = basic.ClausesPerRule
     Methods = Map.empty
     RulesPerMethod = Map.empty
   }
   NewGraph

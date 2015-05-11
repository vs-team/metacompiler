module AnalyserAST

open BasicExpression
open ConcreteExpressionParserPrelude
open ParserMonad

let convertTypeInfo ti = 
  Keyword(Custom "_null",Position.Zero,Position.Zero)

let convertDebugInfo (di : ParserMonad.Position) =
  let debugOp = Keyword(Custom "di",Position.Zero,Position.Zero)
  let arg1 = Imported(IntLiteral di.Line,Position.Zero,Position.Zero)
  let arg2 = Imported(IntLiteral di.Col,Position.Zero,Position.Zero)
  let arg3 = Imported(StringLiteral di.File,Position.Zero,Position.Zero)
  Application(Regular,[debugOp;arg1;arg2;arg3],Position.Zero,Position.Zero)


let convertKeyword k di ti =
  let keywordOp = Keyword(Custom "keyword",Position.Zero,Position.Zero)
  let debugInfo = convertDebugInfo di
  let typeInfo = convertTypeInfo ti
  match k with
  | Sequence ->
      let keywordName = Keyword(Custom "sequence",Position.Zero,Position.Zero)
      Application(Implicit,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | SmallerThan ->
      let keywordName = Keyword(Custom "smallerThan",Position.Zero,Position.Zero)
      Application(Implicit,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | SmallerOrEqual -> 
      let keywordName = Keyword(Custom "smallerOrEqual",Position.Zero,Position.Zero)
      Application(Implicit,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | GreaterThan ->
      let keywordName = Keyword(Custom "greaterThan",Position.Zero,Position.Zero)
      Application(Implicit,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)    
  | NotDivisible -> 
      let keywordName = Keyword(Custom "notDivisible",Position.Zero,Position.Zero)
      Application(Implicit,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | Divisible -> 
      let keywordName = Keyword(Custom "divisible",Position.Zero,Position.Zero)
      Application(Implicit,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | GreaterOrEqual ->
      let keywordName = Keyword(Custom "greaterOrEqual",Position.Zero,Position.Zero)
      Application(Implicit,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero) 
  | Equals -> 
      let keywordName = Keyword(Custom "equals",Position.Zero,Position.Zero)
      Application(Implicit,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | NotEquals -> 
      let keywordName = Keyword(Custom "notEquals",Position.Zero,Position.Zero)
      Application(Implicit,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | DoubleArrow -> 
      let keywordName = Keyword(Custom "doubleArrow",Position.Zero,Position.Zero)
      Application(Implicit,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | FractionLine -> 
      let keywordName = Keyword(Custom "fractionLine",Position.Zero,Position.Zero)
      Application(Implicit,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | Nesting -> 
      let keywordName = Keyword(Custom "nesting",Position.Zero,Position.Zero)
      Application(Implicit,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | DefinedAs -> 
      let keywordName = Keyword(Custom "definedAs",Position.Zero,Position.Zero)
      Application(Implicit,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | Inlined ->
      let keywordName = Keyword(Custom "inlined",Position.Zero,Position.Zero)
      Application(Implicit,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | Custom s -> 
    let customOp = Keyword(Custom "custom",Position.Zero,Position.Zero)
    let customOpArg = Imported(StringLiteral s,Position.Zero,Position.Zero)
    let keywordApp = Application(Regular,[customOp;customOpArg],Position.Zero,Position.Zero)
    Application(Implicit,[keywordOp;keywordApp;debugInfo;typeInfo],Position.Zero,Position.Zero)

let convert (ast : BasicExpression<'k, 'e, 'i, 'di, 'ti>) : BasicExpression<'k, 'e, 'i, 'di, 'ti> =
  match ast with
  | Keyword(k,di,ti) -> convertKeyword k di ti
  | _ -> failwith "Unsupported basic expression"
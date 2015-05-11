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
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | SmallerThan ->
      let keywordName = Keyword(Custom "smallerThan",Position.Zero,Position.Zero)
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | SmallerOrEqual -> 
      let keywordName = Keyword(Custom "smallerOrEqual",Position.Zero,Position.Zero)
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | GreaterThan ->
      let keywordName = Keyword(Custom "greaterThan",Position.Zero,Position.Zero)
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)    
  | NotDivisible -> 
      let keywordName = Keyword(Custom "notDivisible",Position.Zero,Position.Zero)
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | Divisible -> 
      let keywordName = Keyword(Custom "divisible",Position.Zero,Position.Zero)
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | GreaterOrEqual ->
      let keywordName = Keyword(Custom "greaterOrEqual",Position.Zero,Position.Zero)
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero) 
  | Equals -> 
      let keywordName = Keyword(Custom "equals",Position.Zero,Position.Zero)
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | NotEquals -> 
      let keywordName = Keyword(Custom "notEquals",Position.Zero,Position.Zero)
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | DoubleArrow -> 
      let keywordName = Keyword(Custom "doubleArrow",Position.Zero,Position.Zero)
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | FractionLine -> 
      let keywordName = Keyword(Custom "fractionLine",Position.Zero,Position.Zero)
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | Nesting -> 
      let keywordName = Keyword(Custom "nesting",Position.Zero,Position.Zero)
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | DefinedAs -> 
      let keywordName = Keyword(Custom "definedAs",Position.Zero,Position.Zero)
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | Inlined ->
      let keywordName = Keyword(Custom "inlined",Position.Zero,Position.Zero)
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,Position.Zero)
  | Custom s -> 
    let customOp = Keyword(Custom "custom",Position.Zero,Position.Zero)
    let customOpArg = Imported(StringLiteral s,Position.Zero,Position.Zero)
    let keywordApp = Application(Regular,[customOp;customOpArg],Position.Zero,Position.Zero)
    Application(Regular,[keywordOp;keywordApp;debugInfo;typeInfo],Position.Zero,Position.Zero)

let convertImported i di ti =
  let importedOp = Keyword(Custom "imported",Position.Zero,Position.Zero)
  let debugInfo = convertDebugInfo di
  let typeInfo = convertTypeInfo ti

  let createLiteralApp name content (createContent : 'content -> Literal) =
    let literalOp = Keyword(Custom name,Position.Zero,Position.Zero)
    let literalOpArg = Imported(createContent content,Position.Zero,Position.Zero)
    let literalApp = Application(Regular,[literalOp;literalOpArg],Position.Zero,Position.Zero)
    Application(Regular,[importedOp;literalApp;debugInfo;typeInfo],Position.Zero,Position.Zero)

  match i with
  | StringLiteral s -> createLiteralApp "stringLiteral" s (fun content -> StringLiteral content)
  | IntLiteral n -> createLiteralApp "intLiteral" n (fun content -> IntLiteral content)
  | BoolLiteral b -> createLiteralApp "boolLiteral" b (fun content -> BoolLiteral content)
  | SingleLiteral f -> createLiteralApp "singleLiteral" f (fun content -> SingleLiteral content)
  | DoubleLiteral d -> createLiteralApp "doubleLiteral" d (fun content -> DoubleLiteral content)


let convertExtension e di ti =
  let extensionOp = Keyword(Custom "extension",Position.Zero,Position.Zero)
  let debugInfo = convertDebugInfo di
  let typeInfo = convertTypeInfo ti
  let varOp = Keyword(Custom "var",Position.Zero,Position.Zero)
  let varOpArg = Imported(StringLiteral e.Name,Position.Zero,Position.Zero)
  let varApp = Application(Regular,[varOp;varOpArg],Position.Zero,Position.Zero)
  Application(Regular,[extensionOp;varApp;debugInfo;typeInfo],Position.Zero,Position.Zero)



let rec convert (expr : BasicExpression<'k, 'e, 'i, 'di, 'ti>) : BasicExpression<'k, 'e, 'i, 'di, 'ti> =
  match expr with
  | Keyword(k,di,ti) -> convertKeyword k di ti
  | Imported(i,di,ti) -> convertImported i di ti
  | Extension(e,di,ti) -> convertExtension e di ti
  | _ -> failwith "Unsupported basic expression"
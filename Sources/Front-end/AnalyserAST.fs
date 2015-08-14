module AnalyserAST

open BasicExpression
open ConcreteExpressionParserPrelude
open ParserMonad

let private convertTypeInfo ti = 
  Keyword(Custom "none",Position.Zero,())

let private convertDebugInfo (di : ParserMonad.Position) =
  let debugOp = Keyword(Custom "di",Position.Zero,())
  let arg1 = Imported(IntLiteral di.Line,Position.Zero,())
  let arg2 = Imported(IntLiteral di.Col,Position.Zero,())
  let arg3 = Imported(StringLiteral di.File,Position.Zero,())
  Application(Regular,[debugOp;arg1;arg2;arg3],Position.Zero,())


let private convertKeyword k di ti =
  let keywordOp = Keyword(Custom "keyword",Position.Zero,())
  let debugInfo = convertDebugInfo di
  let typeInfo = convertTypeInfo ti
  match k with
  | Sequence ->
      let keywordName = Keyword(Custom "sequence",Position.Zero,())
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,())
  | SmallerThan ->
      let keywordName = Keyword(Custom "smallerThan",Position.Zero,())
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,())
  | SmallerOrEqual -> 
      let keywordName = Keyword(Custom "smallerOrEqual",Position.Zero,())
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,())
  | GreaterThan ->
      let keywordName = Keyword(Custom "greaterThan",Position.Zero,())
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,())    
  | NotDivisible -> 
      let keywordName = Keyword(Custom "notDivisible",Position.Zero,())
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,())
  | Divisible -> 
      let keywordName = Keyword(Custom "divisible",Position.Zero,())
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,())
  | GreaterOrEqual ->
      let keywordName = Keyword(Custom "greaterOrEqual",Position.Zero,())
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,()) 
  | Equals -> 
      let keywordName = Keyword(Custom "equals",Position.Zero,())
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,())
  | NotEquals -> 
      let keywordName = Keyword(Custom "notEquals",Position.Zero,())
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,())
  | DoubleArrow -> 
      let keywordName = Keyword(Custom "doubleArrow",Position.Zero,())
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,())
  | FractionLine -> 
      let keywordName = Keyword(Custom "fractionLine",Position.Zero,())
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,())
  | Nesting -> 
      let keywordName = Keyword(Custom "nesting",Position.Zero,())
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,())
  | DefinedAs -> 
      let keywordName = Keyword(Custom "definedAs",Position.Zero,())
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,())
  | Inlined ->
      let keywordName = Keyword(Custom "inlined",Position.Zero,())
      Application(Regular,[keywordOp;keywordName;debugInfo;typeInfo],Position.Zero,())
  | Custom s -> 
    let customOp = Keyword(Custom "custom",Position.Zero,())
    let customOpArg = Imported(StringLiteral s,Position.Zero,())
    let keywordApp = Application(Regular,[customOp;customOpArg],Position.Zero,())
    Application(Regular,[keywordOp;keywordApp;debugInfo;typeInfo],Position.Zero,())

let private convertImported i di ti =
  let importedOp = Keyword(Custom "imported",Position.Zero,())
  let debugInfo = convertDebugInfo di
  let typeInfo = convertTypeInfo ti

  let createLiteralApp name content (createContent : 'content -> Literal) =
    let literalOp = Keyword(Custom name,Position.Zero,())
    let literalOpArg = Imported(createContent content,Position.Zero,())
    let literalApp = Application(Regular,[literalOp;literalOpArg],Position.Zero,())
    Application(Regular,[importedOp;literalApp;debugInfo;typeInfo],Position.Zero,())

  match i with
  | StringLiteral s -> createLiteralApp "stringLiteral" s (fun content -> StringLiteral content)
  | IntLiteral n -> createLiteralApp "intLiteral" n (fun content -> IntLiteral content)
  | BoolLiteral b -> createLiteralApp "boolLiteral" b (fun content -> BoolLiteral content)
  | SingleLiteral f -> createLiteralApp "singleLiteral" f (fun content -> SingleLiteral content)
  | DoubleLiteral d -> createLiteralApp "doubleLiteral" d (fun content -> DoubleLiteral content)


let private convertExtension e di ti =
  let extensionOp = Keyword(Custom "extension",Position.Zero,())
  let debugInfo = convertDebugInfo di
  let typeInfo = convertTypeInfo ti
  let varOp = Keyword(Custom "_var",Position.Zero,())
  let varOpArg = Imported(StringLiteral e.Name,Position.Zero,())
  let varApp = Application(Regular,[varOp;varOpArg],Position.Zero,())
  Application(Regular,[extensionOp;varApp;debugInfo;typeInfo],Position.Zero,())

let rec private convertExpressions exprs =
  match exprs with
  | [] -> Keyword(Custom "nilExpr",Position.Zero,())
  | expr :: exprs ->
      let listOp = Keyword(Custom "nextExpr",Position.Zero,())
      let basicExpr = convert expr
      let otherExprs = convertExpressions exprs
      Application(Regular,[listOp;basicExpr;otherExprs],Position.Zero,())

and private convertApplication b exprs di ti =
  let applicationOp = Keyword(Custom "application",Position.Zero,())
  let debugInfo = convertDebugInfo di
  let typeInfo = convertTypeInfo ti
  let bracket =
    match b with
    | Implicit -> Keyword(Custom "_implicit",Position.Zero,())
    | Square -> Keyword(Custom "square",Position.Zero,())
    | Curly -> Keyword(Custom "curly",Position.Zero,())
    | Angle -> Keyword(Custom "angle",Position.Zero,())
    | Regular -> Keyword(Custom "regular",Position.Zero,())
  let arguments = convertExpressions exprs
  Application(Regular,[applicationOp;bracket;arguments;debugInfo;typeInfo],Position.Zero,())

and convert (expr : BasicExpression<_,_,_,_,_>) : BasicExpression<_,_,_,_,_> =
  match expr with
  | Keyword(k,di,ti) -> convertKeyword k di ti
  | Application(b,exprs,di,ti) -> convertApplication b exprs di ti
  | Imported(i,di,ti) -> convertImported i di ti
  | Extension(e,di,ti) -> convertExtension e di ti

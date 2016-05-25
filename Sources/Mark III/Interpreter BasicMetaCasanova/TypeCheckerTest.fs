module TypeCheckerTest

open ParserAST
open TypeChecker

let (test1 : Program) =
  let f =
    {
      Name = { Namespace = []; Name = "f" }
      Args = (!!"int" --> !!"int") --> !!"string"
      Return = !!"string"
      Order = Prefix
      Priority = 0
      Position = emptyPos
      Associativity = Left
      Premises = []
    }
  let g =
    {
      Name = { Namespace = []; Name = "g" }
      Args = !!"int"
      Return = !!"int"
      Order = Prefix
      Priority = 0
      Position = emptyPos
      Associativity = Left
      Premises = []
    }
  let test1 =
    {
      Name = { Namespace = []; Name = "test1" }
      Args = (!!"int" --> !!"int")
      Return = !!"string"
      Order = Prefix
      Priority = 0
      Position = emptyPos
      Associativity = Left
      Premises = []
    }
  let rule1 =
    let conclusion =
      ParserAST.ValueOutput([~~"g"; ~~"x"],[~~"x"])
    [] .| conclusion
  let rule2 =
    let conclusion =
      ParserAST.ValueOutput([~~"f"; ~~"g"; ~~"s"],[~~"s"])
    [] .| conclusion
  let rule3 =
    let premises =
      [
        FunctionCall([~~"f";~~"g";Literal(Common.Literal.String("Hello world!"),emptyPos)],[!!!"x"])
      ]
    let conclusion =
      ParserAST.ValueOutput([~~"test1";~~"g"],[~~"x"])
    premises .| conclusion
  [],([Func f;Func g;Func test1],[rule1;rule2;rule3],[])
let (tcTest : Program) =
  let subtypingTest =
    [
      !!"int",!!"expr"
      !!"float",!!"expr"
    ]
  let plus =
    {
      Name = { Namespace = []; Name = "+" }
      Args = !!"expr" --> !!"expr"
      Return = !!"expr"
      Order = Infix
      Priority = 0
      Position = emptyPos
      Associativity = Left
      Premises = []
    }
  let neg =
    {
      Name = { Namespace = []; Name = "-" }
      Args = !!"expr" --> !!"expr"
      Return = !!"expr"
      Order = Prefix
      Priority = 0
      Position = emptyPos
      Associativity = Left
      Premises = []
    }
  let eval =
    {
      Name = { Namespace = []; Name = "eval" }
      Args = !!"expr"
      Return = !!"expr"
      Order = Prefix
      Priority = 0
      Position = emptyPos
      Associativity = Left
      Premises = []
    }
  let evalPlus =
    let premises =
      [
        FunctionCall([~~"eval";~~"a"],[!!!"x1"])
        FunctionCall([~~"eval";~~"b"],[!!!"x2"])
      ]
    let (conclusion : Conclusion) =
      ParserAST.ValueOutput ([~~"eval";NestedExpression [~~"a";~~"+";~~"b"]],[~~"x2"])
    premises .| conclusion
  [],([Data plus; Data neg; Func eval],[evalPlus],subtypingTest)

module TypeChecker
open Common
open ScopeBuilder // Scope

type TreeExpr = Fun of Id*Namespace*TreeExpr*TreeExpr //points to library function
              | Rul of Id*Namespace*TreeExpr*TreeExpr //points to rule
              | DataLR of Id*Namespace*TreeExpr*TreeExpr //points to data
              | DataRL of Id*Namespace*TreeExpr*TreeExpr //points to data
              | Lambda of Id*Namespace*TreeExpr*TreeExpr 
              | App of TreeExpr*TreeExpr     
              | Var of Id*Type
              | Lit of Literal

and Rule = {
  Input    :TreeExpr
  Output   :TreeExpr
  Premises :List<Premise>
}
and Premise = Assignment      of TreeExpr*TreeExpr
            | Conditional     of TreeExpr*TreeExpr
            | ArrowAssignment of TreeExpr*TreeExpr

and Data = {
  Input    :TreeExpr
  Output   :TreeExpr
}

and Type = Id           of Id
         | Arrow        of Type*Type
         | Union        of Type*Type
         | Tuple        of Type*Type

type Scope = {
    Lambdas             :List<Id*Namespace*TreeExpr*TreeExpr>
    LibraryFunctions    :List<Id*Namespace*TreeExpr*TreeExpr>
    Rules               :List<Id*Namespace*List<Rule>>
    Datas               :List<Id*Namespace*List<Data>>
}

(*
let rec selectDecls (exprs:List<BasicExpression>) (decls:List<SymbolDeclaration>) : List<SymbolDeclaration> =
  // recursively find names of operators and add them to the set
  let rec used_names exprs decls =
    exprs |> List.fold (fun (acc:Set<string>) (next:BasicExpression) -> 
      match next with 
      | Id(str,_) -> acc.Add(str)
      | BasicExpression.Application(_,e) -> acc + (used_names e decls)
      | _ -> acc) Set.empty
  let set = used_names exprs decls
  // sort them on priority
  decls |> List.filter   (fun x-> set.Contains(x.Name))
        |> List.sortWith (fun x y-> x.Priority - y.Priority)

let capture (exprs:List<Expr>) (decls:List<SymbolDeclaration>) :List<Expr> = 
  let foo = decls |> List.map (fun decl-> 
    let idx = match decl.Associativity with 
              | Left  -> exprs |> List.tryFindIndex     (fun x->"todo"=decl.Name)
              | Right -> exprs |> List.tryFindIndexBack (fun x->"todo"=decl.Name)
    idx)
  []

let rec prioritize (exprs:List<BasicExpression>) (decls:List<SymbolDeclaration>) :Option<TreeExpr> =
  // first apply this function recursively to parens
  let decls = selectDecls exprs decls
  let exprs = exprs |> List.map (fun expr ->
    match expr with
    | BasicExpression.Application(b,e) -> prioritize exprs decls |> Option.map (Tree)
    | Literal(l,p)     -> match l:Literal with
                          | Int     _ -> Some(Tree(Lit(l,TypeId("int"),p)))
                          | String  _ -> Some(Tree(Lit(l,TypeId("string"),p)))
                          | Float32 _ -> Some(Tree(Lit(l,TypeId("float"),p)))
    | Id(s,p) when (decls|>List.exists(fun x->x.Name=s)|>not) -> Some(Tree(Var(s,Unknown,p)))
    | Id(s,p)          -> Some(Basic(Id(s,p)))
    | Lambda(_)        -> do printfn "can't deal with lambdas."
                          None // TODO
    | Module(_)        -> do printfn "can't deal with scopes."
                          None
    | BasicExpression.Arrow(_) -> do printfn "can't deal with arrows."
                                  None )// what even are arrows?
  if exprs |> List.contains None then None else
  let exprs = exprs |> List.map Option.get
  // then capture the parameters of the functions as declared in decls
  match capture exprs decls with
  | [Tree x] -> Some(x)
  | _ -> do printfn "capturing failed."
         None

let listToMapOfLists (lst:List<'k*'v>) :Map<'k,List<'v>> =
  lst |> List.fold 
    (fun acc (k,v)-> 
      match acc |> Map.tryFind k with
      | None   -> acc |> Map.add k [v]
      | Some x -> acc |> Map.add k (v::x))
    Map.empty
  |> Map.map (fun _ v -> List.rev v)

let test_decls = 
  let pos = { File="not_a_real_file.mc";Line=1;Col=1; }
  [
    {Name="+";LeftArgs=[[Id("int",pos)]];RightArgs=[[Id("int",pos)]];Return=[Id("int",pos)];Priority=30;Associativity=Left;Position=pos}
    {Name="*";LeftArgs=[[Id("int",pos)]];RightArgs=[[Id("int",pos)]];Return=[Id("int",pos)];Priority=40;Associativity=Left;Position=pos}
    {Name="^";LeftArgs=[[Id("int",pos)]];RightArgs=[[Id("int",pos)]];Return=[Id("int",pos)];Priority=50;Associativity=Right;Position=pos}
    {Name="sqrt";LeftArgs=[];RightArgs=[[Id("int",pos)]];Return=[Id("int",pos)];Priority=0;Associativity=Right;Position=pos}
    {Name="mod";LeftArgs=[];RightArgs=[[Id("int",pos)];[Id("int",pos)]];Return=[Id("int",pos)];Priority=0;Associativity=Right;Position=pos}
  ]

let test_exprs = 
  let pos = { File="not_a_real_file.mc";Line=1;Col=1; }
  [ [
      Literal(Int(2),{pos with Col=1})
      Id("^",        {pos with Col=2})
      Id("x",        {pos with Col=3})
      Id("+",        {pos with Col=4})
      Literal(Int(3),{pos with Col=5})
      Id("*",        {pos with Col=6})
      Literal(Int(5),{pos with Col=7})
      Id("^",        {pos with Col=8})
      Literal(Int(7),{pos with Col=9})
    ];[
      Id("sqrt",     {pos with Col=1})
      Id("mod",      {pos with Col=2})
      Literal(Int(2),{pos with Col=3})
      Id("+",        {pos with Col=4})
      Id("x",        {pos with Col=5})
      Literal(Int(3),{pos with Col=6})
      Id("*",        {pos with Col=7})
      Id("y",        {pos with Col=8})
    ];[
      Id("a",{pos with Col=1})
      Id("+",{pos with Col=2})
      Id("*",{pos with Col=3})
      Id("b",{pos with Col=4})
    ];[
      BasicExpression.Application(Round,[
        Literal(Int(2),{pos with Col=1})
        Id("^",        {pos with Col=2})
      ])
      Literal(Int(3),  {pos with Col=3})
    ];[
      BasicExpression.Application(Round,[
        Id("^",        {pos with Col=1})
        Literal(Int(2),{pos with Col=2})
      ])
      Literal(Int(3),  {pos with Col=3})
    ]
  ]

let TypeCheck (root:Scope) (scopes:Map<Id,Scope>) =
  let test = test_exprs |> List.map (fun x-> prioritize x test_decls)
  None
*)
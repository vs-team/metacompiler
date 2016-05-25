module ParserAST

open Common

type Program = List<string> * ProgramDefinition
and ProgramDefinition = List<Declaration> * List<RuleDefinition> * List<TypeDecl*TypeDecl>

and OpOrder =
| Prefix
| Infix

and Associativity =
| Left
| Right

and TypeDecl =
| Arrow of TypeDecl * TypeDecl
| Generic of Id * List<TypeDecl>
| Arg of CallArg
| Zero
  static member op_Equality(t1 : TypeDecl,t2 : TypeDecl) =
    match t1,t2 with
    | Arg(Id(id1,_)),Arg(Id(id2,_)) ->
        id1 = id2
    | Arrow(l1,r1),Arrow(l2,r2) ->
        if l1 <> l2 then
          false
        else
          r1 = r2
    | Zero, Zero -> true
    | _ -> failwith "You are using type equality improperly"
  static member op_Inequality (t1 : TypeDecl,t2 : TypeDecl) =
    not (t1 = t2)
  static member SubtypeOf (t1 : TypeDecl) (t2 : TypeDecl) (subtypeDefinitions : Map<TypeDecl,List<TypeDecl>>) =
    match t1,t2 with
    | Arg(Id(id1,p1)),Arg(Id(id2,p2)) ->
        let subtypesOpt = subtypeDefinitions.TryFind t1
        match subtypesOpt with
        | Some subtypes ->
            subtypes |> List.exists(fun t -> 
                                      match t with
                                      | Arg(Id(tid,_)) ->
                                          tid = id2
                                      | _ -> failwith "Error in subtyping list format")
        | None -> false
    | Arrow(l1,r1),Arrow(l2,r2) ->
        if (l1 <> l2 && TypeDecl.SubtypeOf l1 l2 subtypeDefinitions) |> not then
          false
        else
          TypeDecl.SubtypeOf r1 r2 subtypeDefinitions
    | _ -> failwith "You are using type equality improperly"
  override this.ToString() =
    match this with
    | Arrow(t1,t2) ->
        "(" + t1.ToString() + "->" + t2.ToString() + ")"
    | Generic(id,innerGeneric) ->
        "<" + id.Name + (innerGeneric |> List.fold(fun s x -> s + x.ToString()) "") + ">"
    | Arg(arg) -> arg.ToString()
    | Zero -> ""

and Declaration =
| Data of SymbolDeclaration
| Func of SymbolDeclaration
| TypeFunc of SymbolDeclaration
| TypeAlias of SymbolDeclaration

and SymbolDeclaration =
  {
    Name      : Id
    Args      : TypeDecl
    Return    : TypeDecl
    Order     : OpOrder
    Priority  : int
    Position  : Position
    Associativity : Associativity
    Premises : List<Premise>
  }
  with
    static member Create(name,args,ret,order,priority,pos,ass,prem) =
      {
        Name = name
        Args = args
        Return = ret
        Order = order
        Priority = priority
        Position = pos
        Associativity = ass
        Premises = prem
      }

and RuleDefinition =
| Rule of Rule
| TypeRule of Rule

and Premise =
| FunctionCall of Call
| Conditional of Conditional

and CallArg =
| Literal of Literal * Position
| Id of Id * Position
| NestedExpression of List<CallArg>
| Lambda of LambdaConclusion * List<Premise>
with
  override this.ToString() =
    match this with
    | Literal(l,_) -> l.ToString()
    | Id(id,_) -> id.Name
    | NestedExpression(args) ->
        "(" + (args |> List.fold(fun s x -> s + x.ToString()) "") + ")"
    | Lambda(_) -> failwith "Anonymous functions not supported yet"


and Call = List<CallArg> * List<Id>
and Conditional = List<CallArg> *  Predicate * List<CallArg>
and Conclusion = 
| ValueOutput of List<CallArg> * List<CallArg>
| ModuleOutput of List<CallArg> * List<CallArg> * Program

and LambdaConclusion = List<CallArg*TypeDecl> * List<CallArg>

and Rule = List<Premise> * Conclusion

let symbolTableData : Map<string,SymbolDeclaration> = Map.empty
let symbolTableFunc : Map<string,SymbolDeclaration> = Map.empty
let symbolTableTypeFunc : Map<string,SymbolDeclaration> = Map.empty
let symbolTableTypeAlias : Map<string,SymbolDeclaration> = Map.empty

type SymbolContext =
  {
    DataTable             : Map<Id,SymbolDeclaration>
    FuncTable             : Map<Id,SymbolDeclaration>    
    TypeFuncTable         : Map<Id,SymbolDeclaration>
    TypeAliasTable        : Map<Id,SymbolDeclaration>
    Subtyping             : Map<TypeDecl,List<TypeDecl>>
  }
  with
    static member Empty
      with get() =
        {
          DataTable = Map.empty
          FuncTable = Map.empty
          TypeFuncTable = Map.empty
          TypeAliasTable = Map.empty
          Subtyping = Map.empty
        }

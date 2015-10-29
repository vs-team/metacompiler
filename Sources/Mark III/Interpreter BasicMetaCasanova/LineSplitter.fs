module LineSplitter

open Common

type Keyword = 
  | Func | TypeFunc | Data | DoubleArrow | HorizontalBar | SingleArrow | Instance

type BasicExpression =
  | Id of Id * Position
  | Keyword of Keyword * Position
  | Literal of Literal * Position
  | Block of List<Line>
  | Application of Bracket * List<BasicExpression>
  with 
    member this.Position =
      match this with
      | Id(_,pos) | Keyword(_,pos) | Literal(_,pos) -> pos
      | Block(ls::_) | Application(_,ls) -> 
        ls |> BasicExpression.tryGetNextPosition
      | _ -> Position.Zero
    static member tryGetNextPosition (ts:List<BasicExpression>) = 
      match ts with
      | t::_ -> t.Position
      | _ -> Position.Zero
    static member tryGetNextPosition (ts:List<List<BasicExpression>>) = 
      match ts with
      | t::_ -> BasicExpression.tryGetNextPosition t
      | _ -> Position.Zero

and Line = List<BasicExpression>

let split_lines =
  let rec split_lines (e:List<Parser.BasicExpression>) =
    let mutable lines = []
    let mutable line = []
    for x in e do
      match x with
      | Parser.Keyword(Parser.NewLine,pos) ->
        lines <- (line |> List.rev) :: lines
        line <- []
      | Parser.Keyword(Parser.Instance,pos) -> line <- Keyword(Instance,pos) :: line
      | Parser.Keyword(Parser.Func,pos) -> line <- Keyword(Func,pos) :: line
      | Parser.Keyword(Parser.TypeFunc,pos) -> line <- Keyword(TypeFunc,pos) :: line
      | Parser.Keyword(Parser.Data,pos) -> line <- Keyword(Data,pos) :: line
      | Parser.Keyword(Parser.DoubleArrow,pos) -> line <- Keyword(DoubleArrow,pos) :: line
      | Parser.Keyword(Parser.HorizontalBar,pos) -> line <- Keyword(HorizontalBar,pos) :: line
      | Parser.Keyword(Parser.SingleArrow,pos) -> line <- Keyword(SingleArrow,pos) :: line
      | Parser.Id(i,pos) ->
        line <- Id(i,pos) :: line
      | Parser.Literal(l,pos) ->
        line <- Literal(l,pos) :: line
      | Parser.Application(b,es) ->
        line <- Application(b,split_lines es) :: line
    let result = ((line |> List.rev) :: lines) |> List.rev |> List.filter (List.isEmpty >> not)
    match result with
    | [] -> []
    | [line] -> 
      line
    | _ ->
      [Block(result)]

  let cleanup_split_lines input =
    match split_lines input with
    | [Block(actual_result)] -> Some(actual_result)
    | res -> do printfn "Unexpected non-single block at root: %A" res
             None

  Caching.cached_op (fun _ -> cleanup_split_lines)

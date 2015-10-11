module LineSplitter

open Common

type Keyword = 
  | Func | Data | DoubleArrow | HorizontalBar | SingleArrow | Class | Instance

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
  let rec split_lines (e:List<Parenthesizer.BasicExpression>) =
    let mutable lines = []
    let mutable line = []
    for x in e do
      match x with
      | Parenthesizer.Keyword(Parenthesizer.NewLine,pos) ->
        lines <- (line |> List.rev) :: lines
        line <- []
      | Parenthesizer.Keyword(Parenthesizer.Class,pos) -> line <- Keyword(Class,pos) :: line
      | Parenthesizer.Keyword(Parenthesizer.Instance,pos) -> line <- Keyword(Instance,pos) :: line
      | Parenthesizer.Keyword(Parenthesizer.Func,pos) -> line <- Keyword(Func,pos) :: line
      | Parenthesizer.Keyword(Parenthesizer.Data,pos) -> line <- Keyword(Data,pos) :: line
      | Parenthesizer.Keyword(Parenthesizer.DoubleArrow,pos) -> line <- Keyword(DoubleArrow,pos) :: line
      | Parenthesizer.Keyword(Parenthesizer.HorizontalBar,pos) -> line <- Keyword(HorizontalBar,pos) :: line
      | Parenthesizer.Keyword(Parenthesizer.SingleArrow,pos) -> line <- Keyword(SingleArrow,pos) :: line
      | Parenthesizer.Id(i,pos) ->
        line <- Id(i,pos) :: line
      | Parenthesizer.Literal(l,pos) ->
        line <- Literal(l,pos) :: line
      | Parenthesizer.Application(b,es) ->
        line <- Application(b,split_lines es) :: line
    let result = ((line |> List.rev) :: lines) |> List.rev |> List.filter (List.isEmpty >> not)
    match result with
    | [] -> []
    | [line] -> 
      line
    | _ ->
      [Block(result)]

  Caching.cached_op (fun _ -> split_lines)

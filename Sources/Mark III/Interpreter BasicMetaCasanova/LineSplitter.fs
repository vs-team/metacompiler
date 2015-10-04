module LineSplitter

open Common

type Keyword = 
  | Func | Data | DoubleArrow | HorizontalBar | SingleArrow | DoubleColon

type BasicExpression =
  | Id of Id * Position
  | Keyword of Keyword * Position
  | Literal of Literal * Position
  | Block of List<Line>
  | Application of Bracket * List<BasicExpression>

and Line = List<BasicExpression>

let rec split_lines (e:List<Parenthesizer.BasicExpression>) =
  let mutable lines = []
  let mutable line = []
  for x in e do
    match x with
    | Parenthesizer.Keyword(Parenthesizer.NewLine,pos) ->
      lines <- (line |> List.rev) :: lines
      line <- []
    | Parenthesizer.Keyword(Parenthesizer.Func,pos) -> line <- Keyword(Func,pos) :: line
    | Parenthesizer.Keyword(Parenthesizer.Data,pos) -> line <- Keyword(Data,pos) :: line
    | Parenthesizer.Keyword(Parenthesizer.DoubleArrow,pos) -> line <- Keyword(DoubleArrow,pos) :: line
    | Parenthesizer.Keyword(Parenthesizer.HorizontalBar,pos) -> line <- Keyword(HorizontalBar,pos) :: line
    | Parenthesizer.Keyword(Parenthesizer.SingleArrow,pos) -> line <- Keyword(SingleArrow,pos) :: line
    | Parenthesizer.Keyword(Parenthesizer.DoubleColon,pos) -> line <- Keyword(DoubleColon,pos) :: line
    | Parenthesizer.Id(i,pos) ->
      line <- Id(i,pos) :: line
    | Parenthesizer.Literal(l,pos) ->
      line <- Literal(l,pos) :: line
    | Parenthesizer.Application(b,es) ->
      line <- Application(b,split_lines es) :: line
  let result = (line :: lines) |> List.rev |> List.filter (List.isEmpty >> not)
  match result with
  | [] -> []
  | [line] -> 
    line
  | _ ->
    [Block(result)]

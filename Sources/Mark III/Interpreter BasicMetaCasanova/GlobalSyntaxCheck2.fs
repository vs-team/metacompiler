module GlobalSyntaxCheck2

open ParserMonad
open ExtractFromToken2
open Lexer2
open Common

let skip_newline :Parser<Token,_,_> =
  prs{do! (check_keyword() NewLine) |> repeat |> ignore}

let check_decl_keyword :Parser<Token,Position,_> =
  prs{do! check_keyword() Data .|| check_keyword() Func}

let check_single_arg :Parser<Token,Position,_> =
  (extract_id() |> ignore) .|| (extract_varid() |> ignore)

let rec check_round :Parser<Token,Position,_> =
  prs{
    do! check_keyword() (Open Round)
    do! check_arg
    do! check_keyword() (Close Round)
  }

and check_lamda_signature :Parser<Token,Position,_> =
  prs{
    do! check_keyword() (Open Round)
    do! check_round .|| check_single_arg .|| check_arg
    do! check_keyword() SingleArrow
    do! check_arg
    do! check_keyword() (Close Round)
  }

and check_arg :Parser<Token,Position,_> =
  prs{
    do! check_round .|| check_lamda_signature .|| check_single_arg
    do! check_arg
  } .|| check_lamda_signature .|| prs{
    do! check_round .|| check_single_arg
    do! check_keyword() Star .|| check_keyword() Bar
    do! check_arg
  } .|| check_round .|| check_single_arg

let check_small_args :Parser<Token,Position,_> =
  prs{
    do! check_arg
    do! check_keyword() SingleArrow
  } |> repeat

let check_assosiotivity :Parser<Token,Position,_> =
  prs{
    let! st,pos = extract_id()
    if st = "L" then return ()
      elif st = "R" then return () 
      else return! fail SyntaxError
  }

let check_parse_decl :Parser<Token,Position,_> =
  prs{
    do! check_small_args |> ignore
    do! extract_string_literal() |> ignore
    do! check_keyword() SingleArrow
    do! check_small_args |> ignore
    do! check_arg
    do! check_keyword() PriorityArrow >>. 
         (extract_int_literal() .>>. check_assosiotivity) |> ignore
  }

let check_lines :Parser<Token,Position,_> =
  prs{
    do! skip_newline
    do! check_parse_decl
    do! skip_newline
    return ()
  }

let check_syntax :Parser<Token,Position,_> =
  prs{
    do! check_lines |> itterate |> ignore
    return ()
  }

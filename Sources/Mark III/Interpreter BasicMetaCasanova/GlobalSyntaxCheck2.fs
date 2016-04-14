module GlobalSyntaxCheck2
//
//open ParserMonad
//open ExtractFromToken2
//open Lexer2
//open Common
//
//let skip_newline :Parser<Token,_,_> =
//  prs{do! (check_keyword() NewLine) |> repeat |> ignore}
//
//let check_decl_keyword :Parser<Token,Position,_> =
//  prs{do! check_keyword() Data .|| check_keyword() Func 
//      .|| check_keyword() TypeFunc .|| check_keyword() TypeAlias}
//
//let check_single_arg :Parser<Token,Position,_> =
//  (extract_id() |> ignore) .|| (extract_varid() |> ignore) 
//  .|| (extract_kindid() |> ignore) 
//
//let rec check_round :Parser<Token,Position,_> =
//  prs{
//    do! check_keyword() (Open Round)
//    do! check_arg
//    do! check_keyword() (Close Round)
//  }
//
//and check_lamda_signature (arrow:Keyword) :Parser<Token,Position,_> =
//  prs{
//    do! check_keyword() (Open Round)
//    do! check_round .|| check_single_arg .|| check_arg
//    do! check_keyword() arrow
//    do! check_arg
//    do! check_keyword() (Close Round)
//  }
//
//and check_arg :Parser<Token,Position,_> =
//  prs{
//    do! check_round .|| check_lamda_signature SingleArrow .|| 
//        check_lamda_signature SingleArrow .|| check_single_arg
//    do! check_arg
//  } .|| check_lamda_signature SingleArrow .||
//        check_lamda_signature DoubleArrow .|| prs{
//    do! check_round .|| check_single_arg
//    do! extract_id() |> ignore
//    do! check_arg
//  } .|| check_round .|| check_single_arg
//
//let check_small_args :Parser<Token,Position,_> =
//  prs{
//    do! check_arg
//    do! check_keyword() SingleArrow .|| check_keyword() DoubleArrow
//  } |> repeat
//
//let check_assosiotivity :Parser<Token,Position,_> =
//  prs{
//    let! st,pos = extract_id()
//    if st = "L" then return ()
//      elif st = "R" then return () 
//      else return! fail SyntaxError
//  }
//
//let check_parse_decl :Parser<Token,Position,_> =
//  prs{
//    do! check_small_args |> ignore
//    do! extract_string_literal() |> ignore
//    do! check_keyword() SingleArrow .|| check_keyword() DoubleArrow
//    do! check_small_args |> ignore
//    do! check_arg
//    do! (check_keyword() PriorityArrow >>. 
//         (extract_int_literal() .>>. check_assosiotivity) |> ignore) .|| 
//          prs{return ()}
//  }
//
//let check_condition :Parser<Token,Position,_> =
//  prs{
//    do! check_keyword() Less    .|| check_keyword() LessEqual    .||
//        check_keyword() Greater .|| check_keyword() GreaterEqual .||
//        check_keyword() Equal
//  }
//
//let check_premiss_element :Parser<Token,Position,_> =
//  prs{
//    do! check_single_arg .|| check_keyword() (Open Round) .||
//        check_keyword() (Close Round)  
//  }
//
//let check_premiss_element_with_literal :Parser<Token,Position,_> =
//  prs{
//    do! check_premiss_element .|| (extract_int_literal() |> ignore) .||
//        (extract_string_literal() |> ignore)
//  }
//
//let check_premiss_lift (arrow) :Parser<Token,Position,_> =
//  prs{
//    do! check_premiss_element_with_literal |> repeat1 |> ignore
//    do! arrow
//    do! check_premiss_element |> repeat1 |> ignore
//    do! check_keyword() NewLine
//  }
//
//let check_premis :Parser<Token,Position,_> =
//  check_premiss_lift (check_keyword() SingleArrow) .||
//  check_premiss_lift (check_keyword() DoubleArrow) .||
//  check_premiss_lift (check_condition)             .||
//  prs{do! check_premiss_element_with_literal |> repeat1 |> ignore
//      do! check_keyword() NewLine}
//
//
//let check_rule :Parser<Token,Position,_> =
//  prs{
//    do! check_premis |> repeat |> ignore
//    do! check_keyword() HorizontalBar >>. check_keyword() NewLine
//    do! check_premiss_element |> repeat1 |> ignore
//    do! check_keyword() SingleArrow .|| check_keyword() DoubleArrow
//    do! check_premiss_element_with_literal |> repeat1 |> ignore 
//  } .|| prs{
//    do! check_premiss_element |> repeat |> ignore
//    do! check_keyword() SingleArrow .|| check_keyword() DoubleArrow
//    do! check_premiss_element_with_literal |> repeat |> ignore 
//  }
//
//let check_lines :Parser<Token,Position,_> =
//  prs{
//    do! skip_newline
//    do! (check_decl_keyword >>. check_parse_decl) .|| check_rule
//    do! skip_newline
//    return ()
//  }
//
//let check_syntax :Parser<Token,Position,_> =
//  prs{
//    do! check_lines |> itterate |> ignore
//    return ()
//  }

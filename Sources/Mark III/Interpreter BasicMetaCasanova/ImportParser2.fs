module ImportParser2

open ParserMonad
open DeclParser2
open Lexer2
open Common
(*
let check_presens :Parser<Id,List<Id*ParseScope>,Id*ParseScope> =
  prs{
    let! next = step
    let! ctxt = getContext
    if List.exists (fun (id,_) -> next = id) ctxt then
     return List.find (fun (id,_) -> next = id) ctxt
    else return! fail ImportError
  }

let parse_import :Parser<Token,List<Id*ParseScope>,Id> =
  prs{
    do! skip_newline()
    let! id,pos = UseDifferentCtxt ((check_keyword() Import) >>. extract_id) ParseScope.Zero
    return id
  }

let import_to_parsescope :Parser<Token,List<Id*ParseScope>,ParseScope> =
  prs{
    let! imp = parse_import |> repeat
    let! ctxt = getContext
    let! scp = UseDifferentSrcAndCtxt (check_presens |> itterate) imp ctxt
    return List.fold (fun state (_,x) -> ParseScope.add state x) ParseScope.Zero scp
  }

let decl_parse (curnamespace:Id) :Parser<Token,List<Id*ParseScope>,ParseScope> =
  prs{
    let! scp = import_to_parsescope
    let  scp = {scp with CurrentNamespace = curnamespace}
    return! UseDifferentCtxt parse_scope scp
  }

let start_decl_parse :Parser<Id*List<Token>,List<Id*ParseScope>,ParseScope> =
  prs{
    let! id,token = step
    return ParseScope.Zero
  }

  *)
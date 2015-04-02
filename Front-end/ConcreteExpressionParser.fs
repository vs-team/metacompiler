module ConcreteExpressionParser

#nowarn "40"

open Utilities
open ParserMonad
open BasicExpression
open ConcreteExpressionParserPrelude

let mutable debug_expr = false
let mutable debug_rules = false

let rec program() = 
  p{
    let! incs = merge_included_files
    let inc_rules = fst incs
    let inc_ctxt = snd incs
    let! imps = import_stmts
    let! ks = keywords
    let ctxt = 
          {
            PredefinedKeywords        = Keyword.Parse
            CustomKeywords            = ks
            CustomKeywordsByPrefix    = ks |> List.sortBy (fun k -> Keyword.Name k) |> List.rev
            CustomKeywordsMap         = ks |> Seq.map (fun x -> Keyword.Name x, x) |> Map.ofSeq
            InheritanceRelationships  = Map.empty
            ImportedModules           = imps
          }
    let newContext = ctxt
    do! setContext newContext
    let! inheritance = inheritanceRelationships()
    let newContext = { newContext with InheritanceRelationships = inheritance |> Utilities.transitiveClosure } ++ inc_ctxt
    do! setContext newContext
    let! rs = rules 0
    let! p = getPosition()
    return Application(Implicit, Keyword(Sequence,p, ()) :: (List.append inc_rules rs), p, ())
  }

and inheritanceRelationships() =
  let singleRelation() =
    p{
      let! relation = expr()
      match relation with
      | Application(Implicit, [Extension(concreter,_,_); Extension(is,_,_); Extension(abstracter,_,_)], _, _) ->
        let! el = empty_lines()
        return (concreter.Name, abstracter.Name)
      | _ ->
        return! fail (sprintf "Malformed inheritance relationship %A" relation)
    }
  p{
    let! r1 = singleRelation() + p { return () }
    match r1 with
    | First r ->
      let! rest = inheritanceRelationships()
      return r :: rest
    | _ -> return []
  }

and keywords =
  p{
    let! k = keyword + p{ return () }
    match k with
    | First k -> 
      let! l = empty_lines()
      let! ks = keywords
      return k :: ks
    | Second () -> 
      return []
  }

and keyword = 
  let equals = 
    p{
      let! bs = blank_space()
      let! eqk = character '='
      let! bs = blank_space()
      return ()
    }
  let label l = 
    p{
      let! bs = blank_space()
      let! l = word l
      let! bs = blank_space()
      return l
    }
  p{
    let! a = (label "Data") + (label "Func")
    match a with
    | First _ ->
        let! kw = expr()
        match kw with
        | Application(Implicit,
                      [
                        genericArgs
                        leftArgs
                        name
                        rightArgs
                        Extension({ Name = "Priority" }, _, _)
                        priority
                        Extension({ Name = "Type" }, _, _)
                        typeName
                      ], _, _) -> 
            return Keyword.Fold Data genericArgs leftArgs name rightArgs priority typeName (Extension({ Name = "Right" }, Position.Zero, ()))
        | Application(Implicit,
                      [
                        genericArgs
                        leftArgs
                        name
                        rightArgs
                        Extension({ Name = "Priority" }, _, _)
                        priority
                        Extension({ Name = "Type" }, _, _)
                        typeName
                        Extension({ Name = "Associativity"}, _, _)
                        associativity
                      ], _, _) -> 
            match associativity with
            | Extension(a, b, c) -> 
              match a.Name with
              | "Right" -> return Keyword.Fold Data genericArgs leftArgs name rightArgs priority typeName associativity
              | "Left" ->  return Keyword.Fold Data genericArgs leftArgs name rightArgs priority typeName associativity
              | _-> return failwithf "Incorrect Association"
            | _ -> return failwithf "Incorrect Association"
          //return Keyword.Fold Data genericArgs leftArgs name rightArgs priority typeName associativity
        | _ -> return failwithf "malformed expression %A" kw
    | Second _ ->
        let! kw = expr()
        match kw with
        | Application(Implicit,
                        [
                            genericArgs
                            name
                            rightArgs
                            Extension({ Name = "Priority" }, _, _)
                            priority
                            Extension({ Name = "Type" }, _, _)
                            leftHand
                        ], _, _) ->  
            let! l = label "=>"
            let! rightHand = expr()
            return Keyword.Fold Function genericArgs (Application(Square, [], Position.Zero, ())) name rightArgs priority (Application(Implicit, leftHand :: rightHand :: [], Position.Zero, ())) (Extension({ Name = "Right" }, Position.Zero, ()))
        | Application(Implicit,
                        [
                            genericArgs
                            name
                            rightArgs
                            Extension({ Name = "Priority" }, _, _)
                            priority
                            Extension({ Name = "Type" }, _, _)
                            leftHand
                            Extension({ Name = "Associativity" }, _, _)
                            associativity
                        ], _, _) ->  
            match associativity with
            | Extension(a, b, c) -> 
              match a.Name with
              | "Right" -> ()
              | "Left" ->  ()
              | _-> return failwithf "Incorrect Association"
            | _ -> return failwithf "Incorrect Association"
            let! l = label "=>"
            let! rightHand = expr()
            return Keyword.Fold Function genericArgs (Application(Square, [], Position.Zero, ())) name rightArgs priority (Application(Implicit, leftHand :: rightHand :: [], Position.Zero, ())) associativity
        | _ -> 
            return failwithf "malformed expression %A" kw
  }

  (*
        | Application(Implicit, // no generic arguments
                      [
                        leftArgs
                        name
                        rightArgs
                        Extension({ Name = "Priority" }, _, _)
                        priority
                        Extension({ Name = "Type" }, _, _)
                        typeName
                      ], _, _) -> 
            return Application(Data, Application(Square, [], Position.Zero, ()) :: leftArgs :: name :: rightArgs :: priority :: typeName :: [], Position.Zero, ())
  *)

and rules depth = 
  p{
    let! r = rule depth + (deindentation depth + end_rules())
    match r with
    | First(r) ->
      let! rs = rules depth
      return r :: rs
    | _ -> 
      return []
  }

and rule depth =
  p{
    let! pos = getPosition()
    let! cs = clauses depth
    let! nl1 = newline()
    let! m = clause depth
    let! nl2 = newline()
    do! empty_lines()
    let! depth1 = !!indentation
    if depth1 > depth then
      let! nested_rules = rules depth1
      return Application(Bracket.Implicit, Keyword(Nesting,pos, ()) :: Application(Bracket.Implicit, Keyword(FractionLine,pos, ()) :: m :: cs, pos, ()) :: nested_rules, pos, ())
    else
      return Application(Bracket.Implicit, Keyword(FractionLine,pos, ()) :: m :: cs, pos, ())
  }

and deindentation depth =
  p{
    let! depth1 = !!indentation
    if depth1 < depth then
      return ()
    else
      return! fail "Indentation error"
  }

and end_rules() = 
  p{
    let! el = empty_lines() + eof()
    return! eof()
  }

and clauses depth = 
  let rec clauses = 
    p{
      let! c = comment depth + clause depth
      do! empty_line()
      let! cs = clauses + end_clauses depth
      let cs = 
        match cs with
        | First cs -> cs
        | _ -> []
      match c with
      | First() ->
        return cs
      | Second c -> 
        return c :: cs
    }
  p{
    let! cs = clauses + end_clauses depth
    match cs with
    | First(cs) ->
      return cs
    | _ -> 
      return []
  }

and comment depth : Parser<Unit,_> = 
  p{
    do! require_indentation depth
    let! open_comment = word "//"
    let! cmt = takeWhile' ((<>) '\n')
    return ()
  }

and clause depth =
  p{
    let! pos = getPosition()
    do! require_indentation depth
    let! i = expr()
    let! bs2 = blank_space()
    let! end_clause = !!(newline()) + p{ return () }
    match i,end_clause with
    | Application(Angle, args, di, ()),(First _) ->
      return Application(Angle, Keyword(Inlined,pos, ()) :: args, di, ())
    | _ ->
      let! parseKeyword = getPredefinedKeywords()
      let! ar = parseKeyword
      let! bs3 = blank_space()
      let! o = expr()
      if debug_rules then
        do printfn "%A => %A" i o
      let! bs4 = blank_space()
      return Application(Bracket.Implicit, Keyword(ar,pos, ()) :: i :: o :: [], pos, ())
  }

and customKeyword() =
  let rec customKeyword = 
    function 
    | [] -> fail "Expected keyword"
    | k :: ks ->
      p{
        let! r = word (Keyword.Name k) + customKeyword ks
        match r with 
        | First w -> return w
        | Second w' -> return w'
      }
  p{
    let! customKeywordsByPrefix = getCustomKeywordsByPrefix()
    return! customKeyword customKeywordsByPrefix
  }

and literal() =
  let boolLiteral() =
    p{
      let! res = word "true" + word "false"
      match res with
      | First _ -> return true
      | Second _ -> return false
    }
  p{
    let! res = (floatLiteral() + intLiteral()) + (boolLiteral() + stringLiteral())
    match res with 
    | First(First f) -> return SingleLiteral f
    | First(Second i) -> return IntLiteral i
    | Second(First b) -> return BoolLiteral b
    | Second(Second s) -> return StringLiteral(new System.String(s |> Seq.toArray))
  }

and expr() = 
  let rec shrink bracket_type pos (es:List<BasicExpression<_,_,_,_,_>>) customKeywordsMap : BasicExpression<_,_,_,_,_> =
    let ariety (b:BasicExpression<_,_,_,_,_>) = 
      match b with
      | Keyword(Custom(k),pos, ()) ->
        if customKeywordsMap |> Map.containsKey k then
          let kw = customKeywordsMap.[k]
          Keyword.Ariety LeftArguments kw, Keyword.Ariety RightArguments kw
        else
          0,0
      | _ -> 0,0
    let priority (b:BasicExpression<_,_,_,_,_>,index:int) = 
      match b with
      | Keyword(Custom(k),pos, ()) ->
        if customKeywordsMap |> Map.containsKey k then
          let kw = customKeywordsMap.[k]
          match Keyword.Associativity kw with
          | Right -> Keyword.Priority kw, index
          | Left -> Keyword.Priority kw, -index
        else
          -1,-index
      | _ -> -1,-index
    let merge (n,i) l r =
      Application(Implicit, n :: (l |> List.map fst) @ (r |> List.map fst), pos, ()), i
    if debug_expr then
      do printfn "%A" es
    match bracket_type with
    | Angle ->
      if debug_expr then
        do printfn "does not need prioritization"
      Application(bracket_type, es, pos, ())
    | _ ->
      let prioritize_es = BottomUpPriorityParser.prioritize es ariety priority merge
      if debug_expr then
        do printfn "is prioritized into %A" prioritize_es
        do printfn ""
        let _ = System.Console.ReadLine()
        ()
      match prioritize_es with
      | [x] -> x
      | l -> Application(bracket_type, l, pos, ())

  and base_expr bracket_type = 
    p{
      let! pos = getPosition()
      let! e = inner_expr
      match e with
      | e::es ->
        let! customKeywordsMap = getCustomKeywordsMap()
        match shrink bracket_type pos (e :: es) customKeywordsMap with
        | Application(Implicit, args, di, ()) -> return Application(bracket_type, args, di, ())
        | e -> 
          match bracket_type with
          | Implicit ->
            return e
          | _ ->
            return Application(bracket_type, [e], e.DebugInformation, ())
      | _ -> 
        return Application(bracket_type, [], pos, ())
    }

  and inner_expr =
    p{
      let! pos = getPosition()
      let! parseKeyword = getPredefinedKeywords()
      let! end_line = !!(word "--" + parseKeyword + newline()) + p{ return () }
      match end_line with
      | First _ -> 
        return []
      | Second _ ->

        let open_bracket = (character '(' + character '[') + (character '{' + word "<<")
        let closed_bracket = (character ')' + character ']') + (character '}' + word ">>")
        let! e = (open_bracket + !!closed_bracket) + (customKeyword() + (literal() + identifier()))
        match e with
        | First(First(actual_open_bracket)) -> 
          let extracted_open_bracket = actual_open_bracket.Fold (fun x -> x.Fold id id) (fun x -> x.Fold id (fun _ -> '≪'))
          let! bs = blank_space()
          let! contextToRestore = 
            if extracted_open_bracket = '≪' then
              p{
                let! customContext = getContext()
                do! setContext ConcreteExpressionContext.CSharp
                return customContext
              }
            else
              p{
                let! customContext = getContext()
                return customContext
              }
          let! i_e = base_expr (Bracket.FromChar extracted_open_bracket)
          let! bs = blank_space()
          let! actual_closed_bracket = closed_bracket
          let extracted_closed_bracket = actual_closed_bracket.Fold (fun x -> x.Fold id id) (fun x -> x.Fold id (fun _ -> '≫'))
          if matching_brackets extracted_open_bracket extracted_closed_bracket then 
            do! setContext contextToRestore
            let! bs = blank_space()
            let! r_es = maybe_inner_expr
            return i_e :: r_es
          else
            return! fail(sprintf "Unmatched open bracket %A" extracted_open_bracket)
        | First(Second(closed_bracket)) -> 
          return []
        | Second(First(k)) -> 
          let be = Keyword(Custom(new System.String(k |> Seq.toArray)),pos, ())
          let! bs = blank_space()
          let! bes = maybe_inner_expr
          return be::bes
        | Second(Second(e)) ->
          let be = 
            match e with
            | First(l) -> Imported(l,pos, ())
            | Second(id) -> Extension({ Var.Name = new System.String(id |> Seq.toArray) },pos, ())
          let! bs = blank_space()
          let! bes = maybe_inner_expr
          return be::bes
    }
  and maybe_inner_expr = 
    p{
      let! parseKeyword = getPredefinedKeywords()
      let! es = inner_expr + (!!newline() + !!parseKeyword)
      match es with
      | First bes -> return bes
      | _ -> return []
    }

  base_expr Bracket.Implicit

and import_stmt() =
    p {
        let! bs = blank_space()
        let! w = word "import"
        let! bs = blank_space()
        let! imported_type = longIdentifier()
        return imported_type
    }

and included_file()=
    p {
        let! bs = blank_space()
        let! w = word "include"
        let! bs = blank_space()
        let! included = longIdentifier()
        let trailingIndex = (included.LastIndexOf '.')
        let path = included.Substring(0,  trailingIndex).Replace(".", "/") + included.Substring trailingIndex
        let rules = System.IO.File.ReadAllText(path)
        return (program()).Parse (rules |> Seq.toList) ConcreteExpressionContext.Empty (Position.Create path)
    }

and included_files =
    p {
        let! inc = included_file() + p { return () }
        match inc with
        | First file ->
            match file with
            | First ((rules), (_), (ctxt), (pos)) ->
                let! el = empty_lines()
                let! incs = included_files
                return (rules, ctxt) :: incs
            | _ ->
                return []
        | Second err ->
            return []
    }

and merge_included_files =
    let rec merge_rules exprs =
        [
            for exp in exprs do
            match exp with
            | x::xs ->
                match x with
                | Application (_, rules,_, _) ->
                    match rules with
                    | r::rules ->
                        yield rules
                    | _ -> ()
                | _ -> ()
            | _ -> ()
        ] |> List.concat       
    let rec merge_context ctxt =
        match ctxt with
        | x::xs ->
            let ctxts = merge_context xs
            x ++ ctxts
        | [] ->
            ConcreteExpressionContext.Empty
    p {
        let! includes = included_files
        let s = ( List.map fst includes )
        let rules = merge_rules (List.toSeq [s])
        let context = merge_context ( List.map snd includes )
        return (rules, context)
    }

and import_stmts =
    p{
        let! statement = import_stmt() + p {return ()}
        match statement with
        | First stmt ->
            let! nl = empty_lines()
            let! stmts = import_stmts
            return stmt :: stmts
        | Second _ ->
            return []
    }
and end_clauses depth =
  p{
    do! require_indentation depth
    let! fl1 = character '-'
    let! fl2 = character '-'
    let! fl = takeWhile (character '-')
    do! !!empty_line()
    return ([] : unit list)
  }

and indentation =
  p{
    let! depth = blank_space()
    return depth |> List.length
  }

and require_indentation depth =
  p{
    let! bs = blank_space()
    let depth1 = bs |> List.length
    if depth = depth1 then
      return()
    else
      return! fail "Indentation error"
  }

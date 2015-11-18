module Prioritizer
open Common
open ScopeBuilder

type Result<'scope,'typescope,'result> = 
  | Done of 'result * 'scope * 'typescope
  | Error of string * Position

type TypeParser<'scope,'typescope,'result> = 
  'scope * 'typescope -> Result<'scope,'typescope,'result>

type TypeParserBuilder() =
  member this.Return(res:'result) : TypeParser<'scope,'typescope,'result> =
    fun (scope,typescope) ->
      Done(res,scope,typescope)
  member this.ReturnFrom p = p
  member this.Bind(p:TypeParser<'scope,'typescope,'result>, k:'result->TypeParser<'scope,'typescope,'result'>) =
    fun (scope,typescope) ->
      match p (scope,typescope) with
      | Error(e,p) -> Error(e,p)
      | Done(res,scope',typescope') ->
        let out = k res (scope',typescope')
        out

let typ = TypeParserBuilder()



(* -- see TypeChecker.fs -- *)
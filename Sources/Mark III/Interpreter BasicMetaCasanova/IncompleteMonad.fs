module IncompleteMonad

type ErrorType = Error of string

type IncompleteResult<'ctxt,'unfinished> =
  | Done        of 'ctxt
  | Incomplete  of 'ctxt * 'unfinished 
  | Error       of ErrorType

//type IncompleteMonad<'ctxt,'unfinished> =
  
module Pipeline2

open Lexer2

type OptionBuilder() =
  member this.Return (res:'a) : Option<'a> = Some(res)
  member this.Bind (p:Option<'a>,k:'a->Option<'b>):Option<'b> = Option.bind k p
  member this.RetrunFrom p = p
  
let opt = OptionBuilder()

let start :Option<_> =
  opt{
    return ()
  }
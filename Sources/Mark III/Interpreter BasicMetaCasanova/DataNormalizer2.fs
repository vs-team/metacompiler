module DataNormalizer2

open Common
open ParserMonad
open DeclParser2


let normalize_data :Parser<SymbolDeclaration,Id,_> =
  prs{
    let! next = step

    return ()
  }

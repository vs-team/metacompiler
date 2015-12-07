import Prelude

TypeFunc "Tryable" => ( * => * ) => Signature

Tryable M => {
    try ('a -> M 'b) -> ('e -> M 'b) -> M 'a -> M 'b
    'e => 'a
  }

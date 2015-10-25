import Prelude

TypeFunc "EitherT" => (* => *) => * => * => *
EitherT 'M 'b 'a => 'b | ('M 'a)

ModuleFunc "either" => Monad => * => Monad

either M 'b => Module Monad (EitherT MCons^M 'b) {
    MCons => EitherT MCons^M 'b

    (Left x) >>= k => (Left x)

    ym >>=^M y
    --
    (Right ym) >>= k => k y

    return x => (Right(return^M x))

    TypeFunc "try" => ('a | 'b) => ('a -> 'c) => ('b -> 'c) => 'c
    try (Left x) p q => p x
    try (Right y) p q => q y
  }
 
ModuleFunc "match" => * => Match

match ('a | 'b) => Module (Match ('a | 'b)) {
  Head => 'a
  Tail => 'b

  match (Left x) f g => f x
  match (Right y) f g => g y
}

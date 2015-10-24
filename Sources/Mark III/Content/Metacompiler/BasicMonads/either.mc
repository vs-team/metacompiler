import Prelude

TypeFunc "EitherT" => (* => *) => * => * => *
EitherT 'M 'b 'a => 'b | ('M 'a)

ModuleFunc "either" => Monad 'M => 'b => Monad (EitherT 'M 'b)

either M 'b => Module {
    MCons => EitherT 'M 'b

    (Left x) >>= k => (Left x)

    ym >>=^M y
    --
    (Right ym) >>= k => k y

    return x => (Right(return^M x))

    TypeFunc "try" => ('a | 'b) => ('a -> 'c) => ('b -> 'c) => 'c
    try (Left x) p q => p x
    try (Right y) p q => q y
  }
 
import monad

TypeAlias "SystemIOT" => (#a => #b) => #c
SystemIOT => 'M => IO => 'M

TypeFunc "systemIO" => Monad => IO => Monad
systemIO 'M io => Monad(SystemIOT MCons^'M) {
  {x >>=^'M y
    return^'M k y} -> res
  --
  x >>= k -> res

  return x -> systemIO(return^'M(x))
}

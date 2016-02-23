import monad

TypeAlias "IoT" => (* => *) => *
IoT => 'M => 'M

TypeFunc "io" => Monad => Monad
io 'M => Monad(IoT MCons^'M) {
  pm >>= k ->

  return x -> io(return^'M(x))
}

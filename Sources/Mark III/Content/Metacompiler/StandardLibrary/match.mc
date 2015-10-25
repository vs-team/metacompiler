Signature "Match" 'a {
  TypeFunc Head => *
  TypeFunc Tail => *

  TypeFunc "match" => 'a => (Head => 'b) => (Tail => 'b) => 'b
}

Module "Match" 'a {
  TypeFunc Head => Type
  TypeFunc Tail => Type

  TypeFunc "match" => 'a => (Head => 'b) => (Tail => 'b) => 'b
}

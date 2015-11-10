module Caching

open Microsoft.FSharp.Reflection
open System.IO
open System.Reflection
open System.Runtime.Serialization
open System.Runtime.Serialization.Formatters.Binary

let serializeBinary<'a> (path:string) (x :'a) =
  let binFormatter = new BinaryFormatter()
  use stream = new StreamWriter(path)
  binFormatter.Serialize(stream.BaseStream, x)
  ()


let deserializeBinary<'a> (path:string) =
  let binFormatter = new BinaryFormatter()
  use stream = new StreamReader(path)
  binFormatter.Deserialize(stream.BaseStream) :?> 'a

let mutable invalidate_cache = false
let mutable use_cache = false

let cached_op actual_op (path:string) (cache_path_suffix) args =
  let last_write = System.IO.File.GetLastWriteTime(path)
  let cache_path = path + cache_path_suffix
  let regular_load() = 
    let source = System.IO.File.ReadAllText(path) |> Seq.toList
    let result = actual_op path args
    do if use_cache then serializeBinary cache_path result
    result
  if System.IO.File.Exists(cache_path) && not(invalidate_cache) then
    let last_write_cache = System.IO.File.GetLastWriteTime(cache_path)
    if use_cache && last_write_cache > last_write then
      deserializeBinary(cache_path)
    else
      regular_load()
  else
    regular_load()

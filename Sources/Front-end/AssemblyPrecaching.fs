module AssemblyPrecaching

open System
open System.Reflection
open System.IO

type CachedAssemblyInfo =
  {
    QualifiedName             : string
    Name                      : string
    FullName                  : string
    FilteredName              : string
    FilteredFullName          : string
    Interfaces                : System.Type[]
  }



let assemblyPrecache (netDlls : string list) (importedDlls : string list) =
  let dotnetPath = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory()

  let filterDotNetName (name : string) =
    let apostrophIndex = name.IndexOf('`')
    if (apostrophIndex > 0) then
      name.Remove(apostrophIndex)
    else
      name

  let assemblyCache = System.Collections.Generic.Dictionary<System.Type, CachedAssemblyInfo>()
  let defaultTypes =
    [for dllName in netDlls do
      let assemblyPath = Path.Combine(dotnetPath, dllName)
      yield! Assembly.LoadFile(assemblyPath).GetTypes()]

  for _type in defaultTypes do
    let cache =
      {
        QualifiedName         = _type.AssemblyQualifiedName
        Name                  = _type.Name
        FullName              = _type.FullName
        FilteredName          = _type.Name |> filterDotNetName
        FilteredFullName      = _type.FullName |> filterDotNetName
        Interfaces            = _type.GetInterfaces()
      }
    assemblyCache.Add(_type, cache)

  let customTypes =
    [for dllName in importedDlls do
      let assemblyPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), dllName)
      yield! Assembly.LoadFile(assemblyPath).GetTypes()]

  for _type in customTypes do
    let cache =
      {
        QualifiedName         = _type.AssemblyQualifiedName
        Name                  = _type.Name
        FullName              = _type.FullName
        FilteredName          = _type.Name |> filterDotNetName
        FilteredFullName      = _type.FullName |> filterDotNetName
        Interfaces            = _type.GetInterfaces()
      }
    assemblyCache.Add(_type, cache)

  assemblyCache


module TypeDefinition

type TypeConstantDescriptor = NativeValue | NativeRef | Defined

let mutable private cachedDescriptors = Map.empty

type TypeConstantDescriptor
  with
  static member FromName (*defined imports*) name =
    match name with
    | "int" -> NativeValue
    | "string" -> NativeRef
    | "bool" -> NativeValue
    | _ -> 
      match cachedDescriptors |> Map.tryFind name with
      | Some desc -> desc
      | None ->
        let res = 
          let systemType = System.Type.GetType name
          if systemType <> null then
            if systemType.IsValueType then
              NativeValue
            else
              NativeRef
          else
            Defined
        cachedDescriptors <- cachedDescriptors |> Map.add name res
        res

type TypeVariableKind =
  | GenericParameter
  | TemporaryVariable

type TypeVariableData = string * TypeVariableKind

type Type =
    | TypeVariable of TypeVariableData // 'a
    | TypeConstant of string * TypeConstantDescriptor // int
    | TypeAbstraction of Type * Type // s -> 's
    | ConstructedType of Type * Type list  // List 'a
    | Unknown
    with
        override this.ToString() =
            match this with
            | TypeAbstraction(p, v) -> sprintf "%s -> %s" (p.ToString()) (v.ToString())
            | TypeConstant(t,_) -> sprintf "%s" (t.ToString())
            | ConstructedType(t, fs) -> sprintf "%A<%A>" (t.ToString()) [ for f in fs -> f.ToString() ]
            | TypeVariable(t,_) -> sprintf "%s" t
            | Unknown -> sprintf "Unknown"
        static member compatible (==) (t1:Type) (t2:Type) =
          if t1 == t2 then true
          else
            match t1,t2 with 
            | TypeVariable _, _ 
            | _, TypeVariable _ -> true
            | TypeAbstraction(a1,b1), TypeAbstraction(a2,b2) ->
              Type.compatible (==) a1 a2 && Type.compatible (==) b1 b2
            | ConstructedType(a1,b1), ConstructedType(a2,b2) ->
              Type.compatible (==) a1 a2 && Seq.forall2 (Type.compatible (==)) b1 b2
            | _ -> false

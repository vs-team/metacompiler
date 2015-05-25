module TypeDefinition

type TypeVar = string

type TypeConstantDescriptor = NativeValue | NativeRef | Defined
  with
  static member FromName name =
    match name with
    | "int" -> NativeValue
    | "string" -> NativeRef
    | "bool" -> NativeValue
    | _ -> Defined

type Type =
    | TypeVariable of TypeVar // 'a
    | TypeConstant of string * TypeConstantDescriptor // int
    | TypeAbstraction of Type * Type // s -> 's
    | ConstructedType of Type * Type list  // List 'a
    | Unknown
    with
        override this.ToString() =
            match this with
            | TypeAbstraction(p, v) -> sprintf "%s -> %s" (p.ToString()) (v.ToString())
            | TypeConstant(t,_) -> sprintf "%s" (t.ToString())
            | ConstructedType(t, fs) -> sprintf "%A<%A>" (t) fs
            | TypeVariable(t) -> sprintf "%s" t
            | Unknown -> sprintf "Unkown"
        static member compatible (t1:Type) (t2:Type) =
          if t1 = t2 then true
          else
            match t1,t2 with 
            | TypeVariable _, _ 
            | _, TypeVariable _ -> true
            | TypeAbstraction(a1,b1), TypeAbstraction(a2,b2) ->
              Type.compatible a1 a2 && Type.compatible b1 b2
            | ConstructedType(a1,b1), ConstructedType(a2,b2) ->
              Type.compatible a1 a2 && Seq.forall2 Type.compatible b1 b2
            | _ -> false

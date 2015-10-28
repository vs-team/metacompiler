module TypeChecker

type Type = Star 
          | Signature
          | TypeId     of string
          | BigArrow   of Type*Type
          | SmallArrow of Type*Type
          | Union      of Type*Map<string,Type> // type and typeconstructors

type TypeScope = {
  Imports     : List<string>
  SymbolTable : Map<string,Type>
}

type TypeStack = Map<string,TypeScope>

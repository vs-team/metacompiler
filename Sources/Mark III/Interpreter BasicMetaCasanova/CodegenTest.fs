module CodegenTest
open CodegenInterface

let test_data:fromTypecheckerWithLove=
  let int_t:Type   = DotNetType({Namespace=["System"];Name="Int32"})
  let float_t:Type = DotNetType({Namespace=["System"];Name="Single"})
  let star_t:Type  = TypeApplication((McType({Namespace=["test";"mc"];Name="star"})),[int_t;float_t])
  let pipe_t:Type  = TypeApplication((McType({Namespace=["test";"mc"];Name="pipe"})),[int_t;float_t])
  let comma_id:Id  = {Namespace=["test";"mc"];Name="comma"}
  let left_id:Id   = {Namespace=["test";"mc"];Name="left"}
  let right_id:Id  = {Namespace=["test";"mc"];Name="right"}
  let comma_data:data = 
    { 
      args=[int_t; float_t; ];
      outputType=star_t;
    }
  let left_data:data =
    {
      args=[int_t;]; 
      outputType=pipe_t;
    }
  let right_data:data =
    {
      args=[float_t;]; 
      outputType=pipe_t;
    }
  let datas = [comma_id,comma_data; left_id,left_data; right_id,right_data]
  let main = {input=[];output=Tmp(0);premis=[];typemap=Map.empty;side_effect=true}
  {rules=Map.empty;lambdas=Map.empty;datas=datas;main=main;assemblies=[]}

let list_test:fromTypecheckerWithLove =
  let int_t:Type   = DotNetType({Namespace=["System"];Name="Int32"})
  let add_t:Type = Arrow(int_t,Arrow(int_t,int_t))
  let add_id:Id  = {Namespace=["builtin"];Name="add"}

  // data "nil" -> List
  let list_t:Type  = TypeApplication((McType({Namespace=["mc";"test"];Name="List"})),[int_t])
  let nil_id:Id    = {Namespace=["test";"mc"];Name="nil"}
  let nil_data:data =
    {
      args=[];
      outputType=list_t;
    }

  // data Int -> "::" -> List -> List
  let append_id:Id = {Namespace=["test";"mc"];Name="::"}
  let append_data:data =
    {
      args=[int_t; list_t];
      outputType=list_t;
    }
  
  // length nil -> 0
  let length_t:Type= Arrow (list_t,int_t)
  let length_id:Id = {Namespace=["test";"mc"];Name="length"}
  let length_nil:rule =
    {
      side_effect=false
      input=[Tmp(0)]
      premis=[Destructor({source=Tmp(0); destructor=nil_id; args=[]})
              Literal   ({value=I32(0);dest=Tmp(1)}) ]
      output=Tmp(1)
      typemap=Map.ofSeq [Tmp(0),list_t
                         Tmp(1),int_t ]
    }

  // length xs -> r
  // -------------------------------
  // length x::xs -> add^builtin r 1
  let length_append:rule =
    {
      side_effect=false
      input=[Tmp(0)]
      premis=[Destructor({source=Tmp(0); destructor=append_id; args=[Named("x");Named("xs")]})
              FuncClosure({func=length_id; dest=Tmp(1)})
              ApplicationCall({closure=Tmp(1); argument=Named("xs"); dest=Named("r")})
              Literal({value=I32(1); dest=Tmp(2)})
              DotNetStaticCall({func={Name="+";Namespace=["System";"Int32"]};args=[Named("r");Tmp(2)];dest=Tmp(3)})
            ]
      output=Tmp(3)
      typemap=Map.ofSeq [Tmp(0),list_t
                         Named("x"),int_t
                         Named("xs"),list_t
                         Tmp(1),Arrow(list_t,int_t)
                         Named("r"),int_t
                         Tmp(2),int_t
                         Tmp(3),int_t ]
    }

  let datas = [nil_id,nil_data; append_id,append_data]
  let Funcs = Map.ofSeq <| [length_id,[length_nil;length_append]]
  let main = 
    {
      input=[]
      output=Tmp(7)
      premis=[ConstructorClosure({func=nil_id;dest=Named("end")})
              Literal({value=I32(2);dest=Named("second")})
              ConstructorClosure({func=append_id;dest=Tmp(0)})
              Application({closure=Tmp(0); argument=Named("second"); dest=Tmp(1)})
              Application({closure=Tmp(1); argument=Named("end"); dest=Tmp(2)})
              Literal({value=I32(1);dest=Named("first")})
              Conditional({left=Named("first");predicate=Less;right=Named("second")})
              ConstructorClosure({func=append_id;dest=Tmp(3)})
              Application({closure=Tmp(3); argument=Named("first"); dest=Tmp(4)})
              Application({closure=Tmp(4); argument=Tmp(2); dest=Tmp(5)})
              FuncClosure({func=length_id; dest=Tmp(6)})
              ApplicationCall({closure=Tmp(6); argument=Tmp(5); dest=Tmp(7)})]
      typemap=Map.ofSeq [Named("end"),list_t
                         Named("second"),int_t
                         Named("first"),int_t
                         Tmp(0),Arrow(int_t,(Arrow(list_t,list_t)))
                         Tmp(1),Arrow(list_t,list_t)
                         Tmp(2),list_t
                         Tmp(3),Arrow(int_t,(Arrow(list_t,list_t)))
                         Tmp(4),Arrow(list_t,list_t)
                         Tmp(5),list_t
                         Tmp(6),Arrow(list_t,int_t)
                         Tmp(7),int_t]
      side_effect=true
    }
  {rules=Funcs;datas=datas;lambdas=Map.empty;main=main;assemblies=[]}




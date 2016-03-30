module balltest
open CodegenInterface

let ball_func = 
  let vec2_t:Type = DotNetType({Namespace=["Microsoft";"Xna";"Framework"];Name="Vector2"})
  let float_t:Type = DotNetType({Namespace=["System"];Name="Single"})
  let ball_t:Type = McType({Namespace=["BouncingBall"];Name="Ball"})
  let ball_id:Id  = {Namespace=["BouncingBall"];Name="ball"}

  let ball_data:data =
    {
      args=[vec2_t;vec2_t];
      outputType=ball_t;
    }

  let void_t:Type = DotNetType({Name="void";Namespace=[]})

  // update
  let update_id:Id = {Namespace=["BouncingBall"];Name="update"}
  let update_t:Type = float_t --> (ball_t --> ball_t)
  let update_fall_down:rule = {
    side_effect = false
    input = [Named("dt");Named("b")]
    output = Named("out")
    premis = 
      [
        // b -> ball(position velocity)
        Destructor({source=Named("b");destructor=ball_id;args=[Named("position");Named("velocity")]})
        
        // gety() -> y
        DotNetProperty({
                        property="Y"
                        instance = Named("position")
                        dest=Named("y")
                       })

        // y >= 0
        Literal({value=F32(0.0f);dest=Named("zero")})
        Literal({value=F32(500.0f);dest=Named("ground")})
        Conditional({left=Named("y");predicate=LessEqual;right=Named("ground")})


        // Vector2(0,9.81)
        Literal({value=F32(98.1f);dest=Named("g")})
        DotNetConstructor({func={Namespace=["Microsoft";"Xna";"Framework"];Name="Vector2"}
                           args=[Named("zero");Named("g")]
                           dest=Named("v2")})

        // dotproduct
        DotNetStaticCall({func={Namespace=["Microsoft";"Xna";"Framework";"Vector2"];Name="op_Multiply"}
                          args=[Named("v2");Named("dt")]
                          dest=Named("outerProduct")})

        // sum
        DotNetStaticCall({func={Namespace=["Microsoft";"Xna";"Framework";"Vector2"];Name="op_Addition"}
                          args=[Named("velocity");Named("outerProduct")]
                          dest=Named("updatedVelocity")})

       // dotproduct
        DotNetStaticCall({func={Namespace=["Microsoft";"Xna";"Framework"];Name="op_Multiply"}
                          args=[Named("updatedVelocity");Named("dt")]
                          dest=Named("outerProduct2")})

        // sum
        DotNetStaticCall({func={Namespace=["Microsoft";"Xna";"Framework"];Name="op_Addition"}
                          args=[Named("position");Named("outerProduct2")]
                          dest=Named("updatedPosition")})

        // Ball (updated_position, updated_velocity)
        ConstructorClosure({func=ball_id;dest=next_tmp()})
        Application({closure=current_tmp(); argument=Named("updatedPosition"); dest=next_tmp()})
        Application({closure=current_tmp(); argument=Named("updatedVelocity"); dest=Named("out")})
      ]
    typemap= 
      [
        Named("dt"),float_t
        Named("b"),ball_t
        Named("out"),ball_t
        Named("y"),float_t
        Named("zero"),float_t
        Named("ground"),float_t
        Named("velocity"),vec2_t
        Named("position"),vec2_t
        Named("g"),float_t
        Named("v2"),vec2_t
        Named("updatedPosition"),vec2_t
        Named("updatedVelocity"),vec2_t
        Named("outerProduct"),vec2_t
        Named("outerProduct2"),vec2_t
        reset_tmp(),(vec2_t --> (vec2_t --> ball_t))
        next_tmp(),(vec2_t --> ball_t)
      ] |> Map.ofSeq
  } 

  do reset()
  let update_bounce:rule = {
    side_effect = false
    input = [Named("dt");Named("b")]
    output = Named("out")
    premis = 
      [
        // b -> ball(position velocity)
        Destructor({source=Named("b");destructor=ball_id;args=[Named("position");Named("velocity")]})

        // gety() -> y
        DotNetProperty(
                        {
                          property="Y"
                          instance = Named("position")
                          dest=Named("y")
                        })

        // gety() -> x
        DotNetProperty({
                        property="X"
                        instance = Named("position")
                        dest=Named("x")
                       })

        // y >= 0
        Literal({value=F32(500.0f);dest=Named("ground")})
        Conditional({left=Named("y");predicate=Greater;right=Named("ground")})

        // Vector2(pos.x,zero)
        DotNetConstructor({func={Namespace=["Microsoft";"Xna";"Framework"];Name="Vector2"}
                           args=[Named("x");Named("ground")]
                           dest=Named("updatedPosition")})


        DotNetStaticCall({func={Namespace=["Microsoft";"Xna";"Framework";"Vector2"];Name="op_Subtraction"}
                          args=[Named("velocity")]
                          dest=Named("updatedVelocity")})

        // Ball (updated_position, updated_velocity)
        ConstructorClosure({func=ball_id;dest=next_tmp()})
        Application({closure=current_tmp(); argument=Named("updatedPosition"); dest=next_tmp()})
        Application({closure=current_tmp(); argument=Named("updatedVelocity"); dest=Named("out")})
      ]
    typemap= 
      [
        Named("dt"),float_t
        Named("b"),ball_t
        Named("out"),ball_t
        Named("y"),float_t
        Named("x"),float_t
        Named("ground"),float_t
        Named("velocity"),vec2_t
        Named("position"),vec2_t
        Named("updatedPosition"),vec2_t
        Named("updatedVelocity"),vec2_t
        reset_tmp(),(vec2_t --> (vec2_t --> ball_t))
        next_tmp(),(vec2_t --> ball_t)
      ] |> Map.ofSeq
  } 
  let Funcs = Map.ofSeq <| [update_id,[update_fall_down;update_bounce]]
  let main = {
    input=[]
    output=Named("ret")
    premis=[
        Literal({dest=Named("x1");value=F32(10.0f)})
        Literal({dest=Named("y1");value=F32(20.0f)})
        DotNetConstructor({dest=Named("a");func={Namespace=["Microsoft";"Xna";"Framework"];Name="Vector2"};args=[Named("x1");Named("y1")]})
        Literal({dest=Named("x2");value=F32(30.0f)})
        Literal({dest=Named("y2");value=F32(40.0f)})
        DotNetConstructor({dest=Named("b");func={Namespace=["Microsoft";"Xna";"Framework"];Name="Vector2"};args=[Named("x2");Named("y2")]})
        DotNetStaticCall({dest=Named("ret");func={Namespace=["Microsoft";"Xna";"Framework";"Vector2"];Name="op_Addition"};args=[Named("a");Named("b")]})
      ]
    typemap=Map.ofList <| [
      Named("x1"),float_t
      Named("y1"),float_t
      Named("x2"),float_t
      Named("y2"),float_t
      Named("a"),vec2_t
      Named("b"),vec2_t
      Named("ret"),vec2_t
     ]
    side_effect=true
   }
  {funcs=Funcs;datas=[ball_id,ball_data];lambdas=Map.empty;main=main;assemblies=["Microsoft.Xna.Framework.dll"]}

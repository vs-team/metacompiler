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
        // gety() -> y
        DotNetProperty({container={Namespace=["Microsoft";"Xna";"Framework"];Name="Vector2"}; 
                        property=Named("Y")
                        dest=Named("y")})

        // y >= 0
        Literal({value=F32(0.0f);dest=Named("zero")})
        Conditional({left=Named("y");predicate=GreaterEqual;right=Named("zero")})

        // b -> ball(position velocity)
        Destructor(Destructor.Create(Named("b"),ball_id,[Named("position");Named("velocity")]))

        // Vector2(0,9.81)
        Literal({value=F32(9.81f);dest=Named("g")})
        DotNetConstructor({func={Namespace=["Microsoft";"Xna";"Framework"];Name="Vector2"}
                           args=[Named("zero");Named("g")]
                           dest=Named("v2")})

        // dotproduct
        DotNetCall({func={Namespace=["Microsoft";"Xna";"Framework"];Name="Vector2.*"}
                    args=[Named("v2");Named("dt")]
                    dest=Named("outerProduct")})

        // sum
        DotNetCall({func={Namespace=["Microsoft";"Xna";"Framework"];Name="Vector2.+"}
                    args=[Named("velocity");Named("outerProduct")]
                    dest=Named("updatedVelocity")})

       // dotproduct
        DotNetCall({func={Namespace=["Microsoft";"Xna";"Framework"];Name="Vector2.*"}
                    args=[Named("updatedVelocity");Named("dt")]
                    dest=Named("outerProduct2")})

        // sum
        DotNetCall({func={Namespace=["Microsoft";"Xna";"Framework"];Name="Vector2.+"}
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
        // gety() -> y
        DotNetProperty({container={Namespace=["Microsoft";"Xna";"Framework"];Name="Vector2"}; 
                        property=Named("Y")
                        dest=Named("y")})

        // gety() -> x
        DotNetProperty({container={Namespace=["Microsoft";"Xna";"Framework"];Name="Vector2"}; 
                        property=Named("X")
                        dest=Named("x")})

        // y >= 0
        Literal({value=F32(0.0f);dest=Named("zero")})
        Conditional({left=Named("y");predicate=Less;right=Named("zero")})

        // b -> ball(position velocity)
        Destructor(Destructor.Create(Named("b"),ball_id,[Named("position");Named("velocity")]))

        // Vector2(pos.x,zero)
        DotNetConstructor({func={Namespace=["Microsoft";"Xna";"Framework"];Name="Vector2"}
                           args=[Named("x");Named("zero")]
                           dest=Named("updatedPosition")})


        DotNetCall({func={Namespace=["Microsoft";"Xna";"Framework"];Name="Vector2.-"}
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
        Named("zero"),float_t
        Named("velocity"),vec2_t
        Named("position"),vec2_t
        Named("updatedPosition"),vec2_t
        Named("updatedVelocity"),vec2_t
        reset_tmp(),(vec2_t --> (vec2_t --> ball_t))
        next_tmp(),(vec2_t --> ball_t)
      ] |> Map.ofSeq
  } 
  let Funcs = Map.ofSeq <| [update_id,[update_bounce;update_fall_down]]
  let main = {input=[];output=Tmp(0);premis=[];typemap=Map.empty.Add(Tmp(0),float_t);side_effect=true}
  {rules=Funcs;datas=[ball_id,ball_data];lambdas=Map.empty;main=main}

import Microsoft.Xna.Framework

Data "ball" -> Vector2 -> Vector2 : Ball
Func "üpdate" -> float^prelude -> Ball -> Ball : Update

b -> ball position velocity
position.Y <= 500.0f
velocity + (new Vector2(0.0f,98.1f)) * dt -> updatedVelocity
position + velocity * dt -> updatedPosition
---------------------------------------------
update dt b -> ball updatedPosition updatedVelocity

b -> ball position velocity
position.Y > 500.0f
--------------------------------------------------------
update dt b -> ball (new Vector2(position.X,500.0f)) -velocity



========================= CASANOVA =============================
entity Ball = {
  Position        : Vector2
  Velocity        : Vecotr2

  rule Positon,Velocity =
    if position.Y <= 500.0f then
      yield position + velocity * dt,velocity + (new Vector2(0.0f,98.1f)) * dt
    else
      yield (new Vector2(position.X,500.0f)), -velocity
}
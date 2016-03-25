import Framework^Xna^Microsoft
import entity

Entity "Velocity" Vector2(0.0f, 98.1f) Empty

Entity "Position" Vector2(100.0f, 0.0f) Velocity

Entity "Ball" unit Position {
  get "Position" rest => position
  get "Velocity" rest => velocity
  (if ((field^position).Y <= 500.0f) then
    (set^e "Position"
           (field^position +^Vector2 (field^velocity *^Vector2 dt))
           (set^e "Velocity"
                  (field^velocity +^Vector2 (Vector2(0.0f, 98.1f) *^Vector2 dt))
                  rest^velocity))
  else
    (set^e "Position"
           Vector2((field^position).X, 500.0f)
           -^Vector2(field^velocity))) -> res
  -----------------------------------------------------------
  update e dt -> res
}

import Framework^Xna^Microsoft
import entity

Entity "Velocity" Vector2(0.0f, 98.1f) Empty

Entity "Position" Vector2(100.0f, 0.0f) Velocity

Entity "Ball" unit Position {
  get^e "Position" Rest^e => position
  get^e "Velocity" Rest^e => velocity
  (if ((Field^position).Y <= 500.0f) then
    ((set^e "Position"
           (Field^position +^Vector2 (Field^velocity *^Vector2 dt))
           Rest^position)
     (set^e "Velocity"
            (Field^velocity +^Vector2 (Vector2(0.0f, 98.1f) *^Vector2 dt))
            Rest^velocity))
  else
    (set^e "Position"
           Vector2((Field^position).X, 500.0f)
           -^Vector2(Field^velocity))) -> res
  -----------------------------------------------------------
  update e dt -> res
}

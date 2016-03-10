import playfield
import bike
import either
import state


game
  playfield

  bikeOne

  bikeTwo

Func "start" -> 'a

playfield bikeOne bikeTwo -> gamefield
play gamefield -> res
----------
start -> res

import Keys^Forms^Windows^System

Data "speed" -> Int^builtin -> Speed
Data "colour" -> Int^builtin -> Colour
Data "controls" -> KeyCode -> KeyCode -> KeyCode -> KeyCode -> Controls
Data "position" -> Int^builtin -> Int^builtin -> Position
Data "bike" -> Speed -> Colour -> Controls -> Position -> Bike

Func "bikeSpeed" -> Int^builtin
bikeSpeed -> 2

Func "moveBike" -> Bike -> Bike
bike -> speed colour controls position
position -> x y
direction -> xspeed yspeed
(x +^builtin (speed *^builtin xspeed)) (y +^builtin (speed *^builtin yspeed)) -> newPosition
bike speed colour controls newPosition -> movedBike
---------------------------------------------------
moveBike xbike direction -> movedBike


Func "changeSpeed" -> Bike -> Int^builtin -> Bike
bike -> speed colour controls position
newSpeed colour controls posistion -> newBike
---------------------------------------------
changeSpeed bike newSpeed -> newBike

Data "fieldSize" -> Int^builtin -> Int^builtin -> Fieldsize
Data "playfield" -> Bike -> Bike -> Fieldsize -> ColouredBlocks -> Playfield

Data "coordinate" -> Int^builtin -> Int^builtin -> Coordinate
Data "nil" -> List Coordinate
Data Coordinate -> "::" -> List Coordinate -> List Coordinate

$$ variables haha they are funcs
Func "colouredBlocks" -> List Coordinate

Func "gravity" -> Float^builtin
gravity -> 9.81

Func "defaultSpeed" -> Int^builtin
defaultSpeed -> 1



Func "frameforward" -> Playfield -> Playfield

field -> b1 b2 fsize
$$ checkKeyPresses
moveBike b1 keys1 -> b1'
moveBike b2 keys2 -> b2'
$$ checkCollision
playfield b1' b2' fsize -> newField
-----------------------------------
frameforward field -> newField

Func "play" -> Playfield -> Bike
frameforward field -> newField

--
play field -> winner

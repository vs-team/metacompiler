import Framework^Xna^Microsoft
import Input^Windows^System
import entity^Casanova
import prelude


TypeFunc "PositionEntity" => String => Vector2 => EntityField => EntityField
PositionEntity label pos rest => Entity label pos rest{
  dts -> (dt,speed)
  Field^p +^Vector2 dt *^Vector2 speed -> newPos
  Entity label^p newPos Rest^p -> res
  -----------------------------------
  update p dts -> res
}

TypeFunc "TrailEntity" => String => Vector2 => EntityField => EntityField
TrailEntity label field rest => Entity label field rest{
  Entity label^t pos fields^t -> res
  ----------------------------------
  update t pos -> res
}

TypeFunc "Keys" => Key => Key => Key => Key => EntityField
Entity "Left" left Empty
Entity "Right" right Left
Entity "Up" up Right
Entity "Down" down Up => res
------------------------------
Keys left right up down => res

TypeFunc "Bike" => String => Int => Keys => Vector2 => TrialEntity => EntityField => EntityField
Entity "Colour" rgb Empty
Entity "IsAlive" True^builtin Colour
Entity "Controls" keys IsAlive
Entity "Speed" speed Controls
PositionEntity "Position" position Speed
Entity "Trail" trail Position => field
--------------------------------------
Bike label rgb keys speed position trail rest => Entity label field rest {
  $$ check if alive
  (if Field^IsAlive then
    get^b "Trial" Field^b => trial
    get^trail "Position" Rest^trail => position
    $$ update trail
    update^Field^trial Field^trial Field^position => newTrailEntity
    $$ update position
    get^position "Speed" Rest^position => speed
    update^position position (dt,Field^speed) -> newPos
    Entity label^trial newTrialEntity newPos => newTrial
    Entity Label^b newTrail Rest^b
  else
    b) -> res
  -------------------------------------
  update b dt -> res
}

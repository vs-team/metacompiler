import Framework^Xna^Microsoft
import entity
import bike


TypeFunc "BikeEntity" => String => EntityField => EntityField => EntityField
BikeEntity label bikes rest => Entity label bikes rest{
  Func "updateBikes" => EntityField => Float => EntityField
  update^b b dt
  updateBikes Rest^b dt
  ---------------------
  updateBikes b dt -> res

  updateBikes Empty dt -> Empty

  Field^bs -> bikeEntities
  updateBikes bikeEntities dt -> newBikeEntities
  set^bs label^bs newBikeEntities bs -> res
  -----------------------------------------
  update bs dt -> res
}

TypeFunc "Playfield" => String => EntityField => Vector2 => EntityField => EntityField
BikeEntity (label + "bikes") bikes Empty
-> field
-----------
Playfield label bikes size rest => Entity label field rest{

  --------
  update p dt -> res
}

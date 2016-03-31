import Framework^Xna^Microsoft
import entity
import bike


TypeFunc "Bikes" => String => EntityField => EntityField => EntityField
Bikes label bikeEntities rest => Entity label bikeEntities rest{

  Func "updateBikes" => EntityField => Float => EntityField
  update^b b dt
  updateBikes Rest^b dt
  ---------------------
  updateBikes b dt -> res

  updateBikes Empty dt -> Empty

  Field^bs -> bikes
  updateBikes bikes dt -> newBikes
  set^bs label^bs newBikes bs -> res
  ----------------------------------
  update bs dt -> res
}

TypeFunc "Playfield" => String => EntityField => Vector2 => EntityField => EntityField

-> field
-----------
Playfield label bikes size rest => Entity label field rest{

  --------
  update p dt -> res
}

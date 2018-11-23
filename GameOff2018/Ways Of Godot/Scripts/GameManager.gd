extends Spatial

#var MouseCursor = load("res://Imports/Cursor/...")


func _ready():
	#Input.set_custom_mouse_cursor(MouseCursor)
	pass 


func _process(delta):
	if Input.is_action_pressed("Quit"):
		get_tree().quit()
	pass

extends Spatial

func _ready():
	pass 


func _process(delta):
	if Input.is_action_pressed("Quit"):
		get_tree().quit()
	pass

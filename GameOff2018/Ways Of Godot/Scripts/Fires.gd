extends RigidBody


func _ready():
	pass 

func _process(delta):
	pass


func _on_Area_body_entered(body):
	print(body.get_name()) 
	body.queue_free()
	pass

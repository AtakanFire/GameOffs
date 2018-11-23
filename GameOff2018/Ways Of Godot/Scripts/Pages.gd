extends Spatial



func _ready():
	pass 

# Called every frame. 'delta' is the elapsed time since the previous frame.
#func _process(delta):
#	pass


func _on_Player_skillLearned(learnedSkills):
	get_child(learnedSkills-1).queue_free()
	pass 

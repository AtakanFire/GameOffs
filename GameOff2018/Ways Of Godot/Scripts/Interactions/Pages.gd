extends Spatial


func _on_Player_skillLearned(learnedSkills):
	get_child(0).queue_free()
	pass 

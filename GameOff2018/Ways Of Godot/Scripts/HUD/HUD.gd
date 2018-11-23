extends ViewportContainer


var blinkCooldown

func _ready():
	pass

func _process(delta):
	
	# Display on HUD
	if blinkCooldown != null: 
		$Skills/Blink.text = str((blinkCooldown.max_time - blinkCooldown.time), "/", blinkCooldown.max_time)
		
	pass


func _on_Player_blinked(cooldown):
	blinkCooldown = cooldown
	pass 


func _on_Player_skillLearned(learnedSkills):
	if learnedSkills == 1:
		$Skills/Info/Tips.text = "The new chapter was read.\nBlink learned! \n Key: \"Q\""
	
	pass 

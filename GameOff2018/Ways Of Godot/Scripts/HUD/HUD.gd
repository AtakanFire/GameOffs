extends ViewportContainer


var blinkCooldown
var fireCooldown

func _ready():
	pass

func _process(delta):
	
	# Display on HUD
	if blinkCooldown != null: 
		$Skills/Blink/Text.text = str("%.2f" % (blinkCooldown.max_time - blinkCooldown.time), "/", "%.2f" % blinkCooldown.max_time)
	if fireCooldown != null: 
		$Skills/Fire/Text.text = str("%.2f" % (fireCooldown.max_time - fireCooldown.time), "/", "%.2f" % fireCooldown.max_time)
	
	pass

func _on_Player_skillLearned(learnedSkills):
	if learnedSkills == 1:
		$Skills/Info/Tips.text = "The new chapter was read.\nBlink learned! \n Key: \"Q\""
	elif learnedSkills == 2:
		$Skills/Info/Tips.text = "The new chapter was read.\nAttack learned! \n Key: \"E\""
	
	pass 

func _on_Player_blinked(cooldown):
	blinkCooldown = cooldown
	pass 

func _on_Player_fired(cooldown):
	fireCooldown = cooldown
	pass 

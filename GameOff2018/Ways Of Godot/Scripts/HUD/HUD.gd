extends ViewportContainer

var blinkCooldown
var fireCooldown
var nearTreeTimer
var speedUpTimer

func _ready():
	pass

#warning-ignore:unused_argument
func _process(delta):
	
	# Display on HUD
	if blinkCooldown != null: 
		$Skills/Blink/Text.text = str("%.2f" % (blinkCooldown.max_time - blinkCooldown.time), "/", "%.2f" % blinkCooldown.max_time)
	if fireCooldown != null: 
		$Skills/Fire/Text.text = str("%.2f" % (fireCooldown.max_time - fireCooldown.time), "/", "%.2f" % fireCooldown.max_time)
	if nearTreeTimer != null: 
		if nearTreeTimer.is_stopped():
			$Skills/Tree/Text.text = ""
		elif nearTreeTimer.get_time_left() > 0:
			$Skills/Tree/Text.text = str("%.2f" % (nearTreeTimer.get_time_left()), "/", "%.2f" % nearTreeTimer.get_wait_time())
	if speedUpTimer != null: 
		if speedUpTimer.is_stopped():
			$"Skills/Speed Up"/Text.text = ""
		elif speedUpTimer.get_time_left() > 0:
			$"Skills/Speed Up"/Text.text = str("%.2f" % (speedUpTimer.get_time_left()), "/", "%.2f" % speedUpTimer.get_wait_time())
	
	
	pass

func _on_Player_skillLearned(learnedSkills):
	if learnedSkills == 1:
		$Skills/Info/Tips.text = "The new chapter(Act) was read.\nBlink learned! \n Key: \"Q\""
		$Skills/Blink.visible = true
	elif learnedSkills == 2:
		$Skills/Info/Tips.text = "The new chapter(Act) was read.\nAttack learned! \n Key: \"E\""
		$Skills/Fire.visible = true
		
	pass 

func _on_Player_blinked(cooldown):
	blinkCooldown = cooldown
	pass 

func _on_Player_fired(cooldown):
	fireCooldown = cooldown
	pass 

func _on_Player_finalTrigger():
	$Skills/Info/Tips.text = "Hey, Isn't that where we started?\nAlright Godot Traveler, let me give you a hint.\nTip: \"Waiting for Godot\" near the tree."
	$Skills/Tree.visible = true
	pass 
	
func _on_Player_treeTimer(timer):
	nearTreeTimer = timer
	pass
	
func _on_Player_gameover():
	$Skills/Info/Tips.text = "'ESC' for Quit"
	$Skills/Tree.visible = false
	$GameOver.visible = true
	pass 
	
	
func _on_SpeedUps_speedUpTimer(timer):
	speedUpTimer = timer
	pass
	

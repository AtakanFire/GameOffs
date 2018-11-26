extends Spatial

export (int) var speedTimerCount = 8

var speedTimer

signal speedUpTimer(timer)

func _ready():
	speedTimer = Timer.new()
	speedTimer.set_one_shot(true)
	speedTimer.set_wait_time(speedTimerCount)
	add_child(speedTimer)
	emit_signal("speedUpTimer", speedTimer)
	speedTimer.connect("timeout", self, "_on_speedTimer_timeout")
	$AnimationPlayer.play("Idle")
	pass 

func speedUp(body):
	if body.get_name() == "Player":
		$"../Player".setSpeed(50)
		speedTimer.start()
	pass

func _on_speedTimer_timeout():
	$"../Player".setSpeed(15)
	pass

func _on_SpeedUp_body_entered(body):
	speedUp(body)
	pass


func _on_SpeedUp2_body_entered(body):
	speedUp(body)
	pass 


func _on_SpeedUp3_body_entered(body):
	speedUp(body)
	pass 

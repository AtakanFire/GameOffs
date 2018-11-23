extends KinematicBody

const Cooldown = preload('res://Scripts/Library/Cooldown.gd')

export (int) var blinkCooldowner = 1 # Edit Cooldown on editor
 
onready var blinkCooldown = Cooldown.new(blinkCooldowner)

export var learnedSkills = 0 

signal blinked(cooldown)
signal skillLearned(learnedSkills)

func _ready():
	pass 

func _input(event):
	if Input.is_action_pressed("Blink") and blinkCooldown.is_ready() and learnedSkills > 0:
		blink()
	pass
	
func _process(delta):
	pass
	
func _physics_process(delta):
	blinkCooldown.tick(delta)
	#print(learnedSkills)
	pass
	
func blink():
	var Speed = Vector3(0,0,-1000)
	var Movement = get_node("CameraController").Player.transform.basis * (Speed)
	get_node("CameraController").Player.move_and_slide(Movement,Vector3(0,2,0))
	emit_signal("blinked", blinkCooldown)
	
func attack():
	# Attack To Blocks with particles
	pass
	
func explore():
	
	pass

func _on_Page_body_entered(body):
	learnedSkills+=1
	emit_signal("skillLearned", learnedSkills)
	pass 

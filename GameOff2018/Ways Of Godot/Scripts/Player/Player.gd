extends KinematicBody

const Cooldown = preload('res://Scripts/Library/Cooldown.gd')
const Fires = preload("res://Entities/Environment/Base/Interactions/Fire.tscn")

export (int) var blinkCooldowner = 1 # Edit Cooldown on editor
export (int) var fireCooldowner = 1

onready var blinkCooldown = Cooldown.new(blinkCooldowner)
onready var fireCooldown = Cooldown.new(fireCooldowner)
onready var shoot

export var learnedSkills = 0 

signal blinked(cooldown)
signal fired(cooldown)
signal skillLearned(learnedSkills)

func _ready():
	pass 

func _input(event):
	if Input.is_action_pressed("Blink") and blinkCooldown.is_ready() and learnedSkills > 0:
		blink()
	if Input.is_action_pressed("Fire") and fireCooldown.is_ready() and learnedSkills > 1:
		fire()
	pass
	
func _process(delta):
	pass
	
func _physics_process(delta):
	
	#Skills tick
	blinkCooldown.tick(delta)
	fireCooldown.tick(delta)
	
	#print(learnedSkills)
	pass
	
func blink():
	var Speed = Vector3(0,0,-1000)
	var Movement = get_node("CameraController").Player.transform.basis * (Speed)
	get_node("CameraController").Player.move_and_slide(Movement,Vector3(0,2,0))
	
	emit_signal("blinked", blinkCooldown)
	pass
	
func fire():
	shoot = Fires.instance()
	shoot.set_transform(get_node("AttackHandle").get_global_transform().orthonormalized())
	get_parent().add_child(shoot)
	
	var force = -shoot.transform.basis.z * 100
	var location = get_node("AttackHandle").get_global_transform().origin
	shoot.apply_impulse(location, force)
	
	emit_signal("fired", fireCooldown)
	pass
	
func explore():
	
	pass

func _on_Page_body_entered(body):
	learnedSkills=1
	emit_signal("skillLearned", learnedSkills)
	pass 


func _on_Page2_body_entered(body):
	learnedSkills=2
	emit_signal("skillLearned", learnedSkills)
	pass 

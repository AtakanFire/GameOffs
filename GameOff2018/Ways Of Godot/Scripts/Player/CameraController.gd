extends Spatial

# Thanks
# Base: https://github.com/Sombresonge/Third-Person-Controller (MIT License)

export(NodePath) var PlayerPath  = "" #You must specify this in the inspector!
export(float) var MovementSpeed = 15
export(float) var Acceleration = 3
export(float) var MaxJump = 19
export(float) var MouseSensitivity = 30
export(float) var RotationLimit = 45
export(float) var MaxZoom = 0.5
export(float) var MinZoom = 1.5
export(float) var ZoomSpeed = 2

var Player
var CameraPivot
var CameraPivotBaseRotation
var Direction = Vector3()
var Rotation = Vector2()
var gravity = -9.8
var Movement = Vector3()
var ZoomFactor = 1
var ActualZoom = 1
var Speed = Vector3()
var CurrentVerticalSpeed = Vector3()
var JumpAcceleration = 3
var IsAirborne = false

func _ready():
	#Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)
	Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)
	
	Player = get_node(PlayerPath)
	CameraPivot =  $CameraPivot
	CameraPivotBaseRotation = CameraPivot.rotation
	pass

func _unhandled_input(event):
	
	if event is InputEventMouseMotion:
		Rotation = event.relative
	
	if event is InputEventMouseButton:
		match event.button_index:
			BUTTON_WHEEL_UP:
				ZoomFactor -= 0.05
			BUTTON_WHEEL_DOWN:
				ZoomFactor += 0.05
		ZoomFactor = clamp(ZoomFactor, MaxZoom, MinZoom)
	
	Direction.z = 0
	Direction.x = 0
	
	if Input.is_action_pressed("Forward"):
		Direction.z -= 1
	if Input.is_action_pressed("Back"):
		Direction.z += 1
	if Input.is_action_pressed("WalkRight"): # Side Walk based Character Controlling
		Direction.x += 1
	if Input.is_action_pressed("WalkLeft"): # Side Walk based Character Controlling
		Direction.x -= 1
	if Input.is_action_just_pressed("Jump"):
		if not IsAirborne:
			CurrentVerticalSpeed = Vector3(0, MaxJump, 0)
			IsAirborne = true
		
	Direction.z = clamp(Direction.z, -1,1)
	Direction.x = clamp(Direction.x, -1,1)
	

func _physics_process(delta):
	#Rotation
	if Input.is_action_pressed("RightMouseButton"):
		Player.rotate_y(deg2rad(-Rotation.x)*delta*MouseSensitivity)
		CameraPivot.rotate_x(deg2rad(-Rotation.y)*delta*MouseSensitivity)
		CameraPivot.rotation_degrees.x = clamp(CameraPivot.rotation_degrees.x, -RotationLimit, RotationLimit)
	elif Input.is_action_pressed("LeftMouseButton"):
		CameraPivot.rotate_y(deg2rad(-Rotation.x)*delta*MouseSensitivity)
		CameraPivot.rotation_degrees.y = clamp(CameraPivot.rotation_degrees.y, -180, 180)
	elif Input.is_action_pressed("Right"): # Rotate Based Character Controlling
		Player.rotate_y(deg2rad(-5)* delta * MouseSensitivity)
	elif Input.is_action_pressed("Left"): # Rotate Based Character Controlling
		Player.rotate_y(deg2rad(5)* delta * MouseSensitivity)
	else:
			CameraPivot.rotation = CameraPivotBaseRotation
			
	Rotation = Vector2()
	
	#Movement
	var MaxSpeed = MovementSpeed * Direction.normalized()
	Speed = Speed.linear_interpolate(MaxSpeed, delta * Acceleration)
	Movement = Player.transform.basis * (Speed)
	CurrentVerticalSpeed.y += gravity * delta * JumpAcceleration
	Movement += CurrentVerticalSpeed
	Player.move_and_slide(Movement,Vector3(0,1,0))
	if Player.is_on_floor() :
		CurrentVerticalSpeed.y = 0
		IsAirborne = false
	
	#Zoom
	ActualZoom = lerp(ActualZoom, ZoomFactor, delta * ZoomSpeed)
	CameraPivot.set_scale(Vector3(ActualZoom,ActualZoom,ActualZoom))

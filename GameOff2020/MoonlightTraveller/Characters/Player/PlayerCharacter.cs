using Godot;
using System;

public class PlayerCharacter : KinematicBody
{
    [Export]
    public float speed = 10;
    [Export]
    public float acceleration = 3;
    [Export]
    public float maxJump = 18;
    [Export]
    public NodePath teleportPointPath = new NodePath("");
    
    private Vector3 direction = new Vector3();
    private Vector3 velocity = new Vector3();
    private Vector3 verticalVelocity = new Vector3();
    private Vector3 movementSpeed = new Vector3();
    private Vector2 mouseMovement = new Vector2();
    private float rotateSensitivity = 30;
    private float jumpAcceleration = 3;
    private bool isAirborne = false;
    private float gravity = -9.8f;

    public Spatial teleportPoint;


    public override void _Ready() 
    {
        Vector3 gravityVector = (Vector3)ProjectSettings.GetSetting("physics/3d/default_gravity_vector");
        gravity = (float)ProjectSettings.GetSetting("physics/3d/default_gravity") * gravityVector.y;

        teleportPoint = GetNode<Spatial>(teleportPointPath);
        if (!IsInstanceValid(teleportPoint))
        {
            GD.PrintErr("PlayerCharacter: TeleportPoint isn't valid!");
        } 
    }

    public override void _UnhandledInput(InputEvent @event) 
    { 
        if (@event is InputEventMouseMotion)
        {
            InputEventMouseMotion emm = (InputEventMouseMotion)@event;
            mouseMovement = emm.Relative;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        MoveDirection();
        RotateCharacter(delta);
        Movement(delta);
        FallLimit();
    }

    public void MoveDirection()
    {
        direction = new Vector3();

        if (Input.IsActionPressed("Forward") || (Input.IsActionPressed("Character") && Input.IsActionPressed("Camera")))
        {
            direction.z += 1;
        }
        if (Input.IsActionPressed("Backward"))
        {
            direction.z -= 1;
        }

        if (Input.IsActionPressed("Character"))
        {
            if (Input.IsActionPressed("Right"))
            {
                direction.x -= 1;
            }
            if (Input.IsActionPressed("Left"))
            {
                direction.x += 1;
            }
        }

        if (Input.IsActionJustPressed("Jump") && !isAirborne)
        {
            verticalVelocity = new Vector3(0, maxJump, 0);
            isAirborne = true;
        }

        direction.x = Mathf.Clamp(direction.x, -1, 1);
        direction.z = Mathf.Clamp(direction.z, -1, 1);
    }
    
    public void RotateCharacter(float delta) 
    {
        if (Input.IsActionPressed("Character"))
        {
            RotateY(Mathf.Deg2Rad(-mouseMovement.x) * rotateSensitivity * delta);
            mouseMovement = new Vector2();
        } 
        else if (Input.IsActionPressed("Right"))
        {
            RotateY(Mathf.Deg2Rad(-5) * rotateSensitivity * delta);
        }
        else if (Input.IsActionPressed("Left"))
        {
            RotateY(Mathf.Deg2Rad(5) * rotateSensitivity * delta);
        }
    }

    public void Movement(float delta)
    {        
        Vector3 maxSpeed = speed * direction.Normalized();
        movementSpeed = movementSpeed.LinearInterpolate(maxSpeed, acceleration * delta);
        velocity = GlobalTransform.basis.Xform(movementSpeed);
        verticalVelocity.y += gravity * jumpAcceleration * delta;
        velocity += verticalVelocity;
        MoveAndSlide(velocity, new Vector3(0, 1, 0));

        if (IsOnFloor())
        {
            verticalVelocity.y = 0f;
            isAirborne = false;
        }
        GetForwardSpeed();
    }
    
    public float GetForwardSpeed()
    {
        return movementSpeed.z;
    }

    // When falling to universe teleport to center
    private void FallLimit()
    {
        if (IsInstanceValid(teleportPoint) && Transform.origin.y < -30.0f)
        {
            Transform = teleportPoint.Transform;
        }
    }
}

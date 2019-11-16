using Godot;
using System;

public class ThirdPersonCharacter : KinematicBody
{
    [Export]
    public float speed = 10;
    [Export]
    public float acceleration = 3;
    [Export]
    public float maxJump = 18;
    
    private Vector3 direction = new Vector3();
    private Vector3 velocity = new Vector3();
    private Vector3 verticalVelocity = new Vector3();
    private Vector3 movementSpeed = new Vector3();
    private Vector2 mouseMovement = new Vector2();
    private float rotateSensitivity = 30;
    private float jumpAcceleration = 3;
    private bool isAirborne = false;
    private float gravity = -9.8f;

    public override void _Ready() { }

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
    }

    public void MoveDirection()
    {
        direction = new Vector3();

        if (Input.IsActionPressed("Forward") || (Input.IsActionPressed("RightMouseButton") && Input.IsActionPressed("LeftMouseButton")))
        {
            direction.z -= 1;
        }
        if (Input.IsActionPressed("Back"))
        {
            direction.z += 1;
        }

        if (Input.IsActionPressed("RightMouseButton"))
        {
            if (Input.IsActionPressed("Right"))
            {
                direction.x += 1;
            }
            if (Input.IsActionPressed("Left"))
            {
                direction.x -= 1;
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
        if (Input.IsActionPressed("RightMouseButton"))
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
    }
    
}

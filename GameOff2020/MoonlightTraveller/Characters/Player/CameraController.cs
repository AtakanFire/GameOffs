using Godot;
using System;

public class CameraController : Spatial
{
    [Export]
    public bool autoLookForward = false;
    [Export]
    public float rotateSensitivity = 30;
    [Export]
    public float maxZoom = 0.5f;
    [Export]
    public float minZoom = 1.5f;
    [Export]
    public float zoomSpeed = 2;

    private Spatial cameraPivot;
    private SpringArm springArm;
    private Camera camera;
    private Vector3 cameraPivotBaseRotation;
    private Vector2 mouseMovement = new Vector2();
    private bool lookForward = false;
    private float rotationLimit = 45;
    private float zoomFactor = 1;
    private float actualZoom = 1;

    public override void _Ready()
    {
        cameraPivot = GetNode<Spatial>("./");
        springArm = GetNode<SpringArm>("./SpringArm");
        Input.SetMouseMode(Input.MouseMode.Visible);
        cameraPivotBaseRotation = cameraPivot.Rotation;
    }

    public override void _UnhandledInput(InputEvent @event){
        if (@event is InputEventMouseMotion)
        {
            InputEventMouseMotion emm = (InputEventMouseMotion)@event;
            mouseMovement = emm.Relative;
        }

        if (@event is InputEventMouseButton)
        {
            InputEventMouseButton emb = (InputEventMouseButton)@event;
            //GD.Print(emb.AsText());
            if (emb.IsPressed()){          
                if (emb.ButtonIndex == (int)ButtonList.WheelUp)
                {
                    zoomFactor -= 0.05f;
                }
                else if (emb.ButtonIndex == (int)ButtonList.WheelDown)
                {
                    zoomFactor += 0.05f;
                }
                zoomFactor = Mathf.Clamp(zoomFactor, maxZoom, minZoom);
            }
        } 

        if (((Input.IsActionPressed("Character") || Input.IsActionPressed("Camera")) && mouseMovement.Length() >= 1)
            || (Input.IsActionPressed("Character") && Input.IsActionPressed("Camera")))
        {
            Input.SetMouseMode(Input.MouseMode.Captured);
            autoLookForward = false;
        }
        else if (Input.IsActionJustReleased("Character") || Input.IsActionJustReleased("Camera"))
        {
            Input.SetMouseMode(Input.MouseMode.Visible);
        }

        if (Input.IsActionJustPressed("Forward") && !(Input.IsActionJustReleased("Character") || Input.IsActionJustReleased("Camera")))
        {
            autoLookForward = true;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        CameraRotation(delta);
        ZoomControl(delta);
    }

    private void CameraRotation(float delta)
    {
        if (lookForward && Input.GetMouseMode() == Input.MouseMode.Captured)
        {
            cameraPivot.Rotation = cameraPivot.Rotation.LinearInterpolate(cameraPivotBaseRotation, (rotateSensitivity/3) * delta);
            if (cameraPivot.Rotation.DistanceTo(cameraPivotBaseRotation) < 0.01)
            {
                lookForward = false;
            }
        }

        if ((Input.IsActionJustPressed("Character") && Input.IsActionJustPressed("Camera")) ||
            (Input.IsActionJustPressed("Character") && !Input.IsActionJustPressed("Camera")))
        {
            lookForward = true;
        } 
        else if((Input.IsActionPressed("Character") && Input.IsActionPressed("Camera")) ||
            (Input.IsActionPressed("Character") && !Input.IsActionPressed("Camera"))) 
        {
            if (!lookForward  && Input.GetMouseMode() == Input.MouseMode.Captured)
            {
                lookForward = false;
                cameraPivot.RotateX(Mathf.Deg2Rad(mouseMovement.y) * rotateSensitivity * delta);
                cameraPivot.RotationDegrees = new Vector3(Mathf.Clamp(cameraPivot.RotationDegrees.x, -rotationLimit, rotationLimit), 0, 0);
            }
        } 
        else if (Input.IsActionPressed("Camera") && !Input.IsActionPressed("Character"))
        {
            lookForward = false;
            cameraPivot.Rotate(cameraPivot.Transform.basis.x.Normalized(), Mathf.Deg2Rad(mouseMovement.y) * rotateSensitivity * delta);
            cameraPivot.RotateY(Mathf.Deg2Rad(-mouseMovement.x) * rotateSensitivity * delta);
            cameraPivot.RotationDegrees = new Vector3(Mathf.Clamp( cameraPivot.RotationDegrees.x, -rotationLimit, rotationLimit), Mathf.Clamp( cameraPivot.RotationDegrees.y, -180, 180), Mathf.Clamp( cameraPivot.RotationDegrees.z, -rotationLimit, rotationLimit));
        } 
        else if (autoLookForward)
        {
            cameraPivot.Rotation = cameraPivot.Rotation.LinearInterpolate(cameraPivotBaseRotation, (rotateSensitivity/5) * delta);
        }
        mouseMovement = new Vector2();
    }

    private void ZoomControl(float delta)
    {
        actualZoom = Mathf.Lerp(actualZoom, zoomFactor, zoomSpeed * delta);
        cameraPivot.Scale = new Vector3(actualZoom, actualZoom, actualZoom);
    }
}

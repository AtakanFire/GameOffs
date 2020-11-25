using Godot;
using System;
using Godot.Collections;

public class Abilities : Node
{

    [Export]
    private float rayLength = 1000;
    [Export]
    private float targetingRange = 1000;
    [Export]
    private NodePath playerCharacterPath = new NodePath("");
    [Export]
    private NodePath cameraPath = new NodePath("");

    private Spatial playerCharacter;
    private Camera camera;

    public override void _Ready()
    {
        camera = GetNode<Camera>(cameraPath);
        if (!IsInstanceValid(camera))
        {
            GD.PrintErr("Ability: Camera isn't valid!");
        }
        playerCharacter = GetNode<Spatial>(playerCharacterPath);
        if (!IsInstanceValid(playerCharacter))
        {
            GD.PrintErr("Ability: playerCharacter isn't valid!");
        }
    }

    public override void _UnhandledInput(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("Target"))
        {
            GD.Print("Target");
            Spatial interactable = NearlestInteractable();
            if (IsInstanceValid(interactable))
            {
                GD.Print("Select to: " + interactable.Name);
            }
        }
        if (inputEvent.IsActionPressed("Ability1"))
        {
            GD.Print("Ability1");

        }
        else if (inputEvent.IsActionPressed("Ability2"))
        {
            GD.Print("Ability2");

        }
        else if (inputEvent.IsActionPressed("Ability3"))
        {
            GD.Print("Ability3");

        }
    }


    public override void _Input(InputEvent inputEvent)
    {
        if (inputEvent is InputEventMouseButton eventMouseButton)
        {
            if (inputEvent.IsActionPressed("Camera"))
            {
                Dictionary hit = RayCastFromMouse(eventMouseButton.Position);
                if (hit.Contains("collider") && hit["collider"] is KinematicBody kinematicBody)
                {
                    KinematicBody collider = (KinematicBody)hit["collider"];
                    GD.Print("Select to: " + collider.Name);
                }
            }
            else if (inputEvent.IsActionPressed("Character"))
            {
                Dictionary hit = RayCastFromMouse(eventMouseButton.Position);
                if (hit.Contains("collider") && hit["collider"] is KinematicBody kinematicBody)
                {
                    KinematicBody collider = (KinematicBody)hit["collider"];
                    GD.Print("Attack to: " + collider.Name);
                }
            }
        }
    }

    public Dictionary RayCastFromMouse(Vector2 mousePos, bool ignoreSelf = true)
    {
        var from = camera.ProjectRayOrigin(mousePos);
        var to = from + camera.ProjectRayNormal(mousePos) * rayLength;
        var spaceState = camera.GetWorld().DirectSpaceState;
        /*
        {
            collider:[KinematicBody:1814], 
            collider_id:1814, 
            normal:(0.203861, 0.630741, -0.748736), 
            position:(3.383153, 1.386803, -6.920409), 
            rid:[RID], 
            shape:0
            }
        */
        Godot.Collections.Array ignores = new Godot.Collections.Array();
        if (ignoreSelf)
        {
            ignores.Add(playerCharacter); 
        }

        return spaceState.IntersectRay(from, to, ignores);
    }

    public Spatial NearlestInteractable(bool checkRange = true)
    {
        Godot.Collections.Array interactables = GetTree().GetNodesInGroup("Interactable");
        Spatial nearlestInteractable = null;
        Vector3 playerOrigin = playerCharacter.Transform.origin;
        for (int i = 0; i < interactables.Count; i++)
        {
            Vector3 currentInteractableOrigin  = ((Spatial)(interactables[i])).Transform.origin;
            if (IsInstanceValid(nearlestInteractable))
            {
                Vector3 nearlestInteractableOrigin  = nearlestInteractable.Transform.origin;
                if (playerOrigin.DistanceTo(currentInteractableOrigin) < playerOrigin.DistanceTo(nearlestInteractableOrigin)
                    && ((playerOrigin.DistanceTo(currentInteractableOrigin) < targetingRange) || !checkRange))
                {
                    nearlestInteractable = (Spatial)interactables[i];
                }
            }
            else
            {
                if (playerOrigin.DistanceTo(currentInteractableOrigin) < targetingRange || !checkRange)
                {
                    nearlestInteractable = (Spatial)interactables[i];
                }
            }
        }
        return nearlestInteractable;
    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }




}

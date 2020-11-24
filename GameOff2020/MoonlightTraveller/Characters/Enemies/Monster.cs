using Godot;
using System;
using Godot.Collections;

public class Monster : KinematicBody
{
    [Export]
    private bool autoFollowTarget = true;    
    [Export]
    private NodePath targetPath = new NodePath("");
    [Export]
    private float acceptableRadius = 5.0f;
    [Export]
    private NodePath navigationPath = new NodePath("");

    
    [Export]
    // Monster Body Information
    private MonsterBody monsterBody = MonsterBody.Cyclops;

    public Spatial target;
    public Vector3 moveDirection = new Vector3();

    private float moveSpeed = 10f;

    private MonsterModel monsterModel;
    private Navigation navigation;
    private Vector3[] paths = new Vector3[0];
    private int pathIndex = 0;

    public override void _Ready()
    {
        monsterModel = GetNode<MonsterModel>("./Model");
        if (!IsInstanceValid(monsterModel))
        {
            GD.PrintErr("Monster: Monster Model isn't valid!");
        } 
        else
        {
            monsterModel.ChangeBody(monsterBody);
        }

        navigation = GetNode<Navigation>(navigationPath);
        if (!IsInstanceValid(navigation))
        {
            GD.PushWarning("Navigation can't found!");
        }

        target = GetNode<Spatial>(targetPath);
        if (IsInstanceValid(target))
        {
            MoveTo(target);
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        MoveDirection(delta);
        RotateCharacter(delta);
        AutoFollowDecision(delta);
    }    
    
    public override void _Process(float delta)
    {

    }

    
    public void AutoFollowDecision(float delta)
    {
        if (autoFollowTarget 
        && pathIndex >= paths.Length 
        && GlobalTransform.origin.DistanceTo(target.GlobalTransform.origin) > acceptableRadius)
        {
            MoveTo(target);
        }
    }

    public void MoveDirection(float delta)
    {
        if (pathIndex < paths.Length)
        {
            moveDirection = (paths[pathIndex] - GlobalTransform.origin);
            if (moveDirection.Length() < acceptableRadius)
            {
                pathIndex++;
            }
            else
            {
                moveDirection.y += (-9.8f); // Gravity // -(float)ProjectSettings.GetSetting("physics/3d/default_gravity")
                MoveAndSlide(moveDirection.Normalized() * moveSpeed * delta * 100, new Vector3(0, 1, 0));
            }
        }
        else
        {
            moveDirection = new Vector3();
        }
    }

    public void RotateCharacter(float delta)
    {
        if (IsInstanceValid(target))
        {
            LookAt(target.GlobalTransform.origin, Vector3.Up);
        }
        // MoveTo(target);
    }

    public void MoveTo(Spatial _target)
    {   
        paths = navigation.GetSimplePath(GlobalTransform.origin, _target.GlobalTransform.origin);
        pathIndex = 0;
    }

}

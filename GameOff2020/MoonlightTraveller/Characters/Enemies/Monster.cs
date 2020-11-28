using Godot;
using System;
using Godot.Collections;

public class Monster : KinematicBody
{
    [Export]
    private bool autoFollowTarget = true;    
    [Export]
    private float acceptableRadius = 5.0f;
    [Export]
    private NodePath targetPath = new NodePath("");
    [Export]
    private NodePath navigationPath = new NodePath("");
    
    [Export]
    // Monster Body Information
    private MonsterBody monsterBody = MonsterBody.Cyclops;

    [Signal]
    public delegate void OnReachedToTarget(bool isReached);
    [Signal]
    public delegate void OnKilled();

    public bool isDied = false;
    public Spatial target;
    public Vector3 moveDirection = new Vector3();
    public bool reachedToTarget = false;

    private float moveSpeed = 10f;

    private MonsterModel monsterModel;
    private Navigation navigation;
    private Vector3[] paths = new Vector3[0];
    private int pathIndex = 0;

    private Timer timer = new Timer();

    public override void _Ready()
    {
        monsterModel = GetNode<MonsterModel>("./Model");
        if (IsInstanceValid(monsterModel))
        {
            monsterModel.ChangeBody(monsterBody);
        } 

        if (!navigationPath.IsEmpty())
        {
            navigation = GetNode<Navigation>(navigationPath);
        }
        if (!targetPath.IsEmpty())
        {
            target = GetNode<Spatial>(targetPath);
            if (IsInstanceValid(target))
            {
                MoveTo(target);
            }
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
        if (IsInstanceValid(target))
        {
            if (autoFollowTarget && pathIndex >= paths.Length)
            {
                if (GlobalTransform.origin.DistanceTo(target.GlobalTransform.origin) > acceptableRadius)
                {
                    MoveTo(target);
                    if (reachedToTarget)
                    {
                        EmitSignal(nameof(OnReachedToTarget), false);
                    }
                    reachedToTarget = false;
                }
                else
                {
                    if (!reachedToTarget)
                    {
                        EmitSignal(nameof(OnReachedToTarget), true);
                    }
                    reachedToTarget = true;
                }
            } 
        }
        else
        {
            if (reachedToTarget)
            {
                EmitSignal(nameof(OnReachedToTarget), false);
            }
            reachedToTarget = false;
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
        if (IsInstanceValid(navigation))
        {
            paths = navigation.GetSimplePath(GlobalTransform.origin, _target.GlobalTransform.origin);
            pathIndex = 0;
        }
    }

    public void Kill()
    {
        if (!isDied)
        {
            EmitSignal(nameof(OnKilled));

            AddChild(timer);
            timer.Connect("timeout", this, "DestroySelf");
            timer.Start(4.0f);
        }
    } 

    public void DestroySelf()
    {
        QueueFree();
    }

    public void InitializeMonster(Spatial t, MonsterBody monBody, Navigation nav)
    {
        target = t;
        if (IsInstanceValid(target))
        {
            MoveTo(target);
        }
        monsterModel.ChangeBody(monBody);
        navigation = nav;
    }


}

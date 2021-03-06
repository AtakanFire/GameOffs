using Godot;
using System;
using Godot.Collections;


public enum AttackType
{
    Basic = 0,
    Ability1 = 1,
    Ability2 = 2,
    Ability3 = 3,
}

public enum CustomCursorShape
{
    Hand = 0,
    Hover = 1,
    Attack = 2,
    Check = 3,
    Point = 4,
    Cross = 5
}

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
    [Export]
    private NodePath animTreePath = new NodePath("");
    [Export]
    private bool editCursor = true;
    [Export]
    private StreamTexture[] cursors = new StreamTexture[0];
    [Export]
    private NodePath rightHandPath = new NodePath("");
    [Export]
    // Abilities[1,2,3]
    public Vector3 abilityCooldowns = new Vector3();


    private Spatial playerCharacter;
    private Camera camera;
    private AnimationTree animTree;
    private AnimationNodeStateMachinePlayback playback;

    private bool autoAttack = false;
    private Spatial selectedTarget;
    private Vector2 mousePos = new Vector2();
    public BoneAttachment rightHand;

    PackedScene basicAttackEffect = (PackedScene)ResourceLoader.Load("res://Characters/Player/Abilities/BasicProjectile.tscn");
    PackedScene ability1Effect = (PackedScene)ResourceLoader.Load("res://Characters/Player/Abilities/Ability1Projectile.tscn");
    PackedScene ability2Effect = (PackedScene)ResourceLoader.Load("res://Characters/Player/Abilities/Ability2Projectile.tscn");
    PackedScene ability3Effect = (PackedScene)ResourceLoader.Load("res://Characters/Player/Abilities/Ability3Projectile.tscn");
    PackedScene projectileHitPack = (PackedScene)ResourceLoader.Load("res://Characters/Player/Abilities/ProjectileHit.tscn");

    public float[] cooldowns = new float[3];

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
        animTree = GetNode<AnimationTree>(animTreePath);
        if (!IsInstanceValid(animTree))
        {
            GD.PrintErr("Ability: animTree isn't valid!");
        }        
        playback = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");

        rightHand = GetNode<BoneAttachment>(rightHandPath);
        if (!IsInstanceValid(rightHand))
        {
            GD.PrintErr("MonsterAI: rightHand isn't valid!");
        }
    }

    public override void _UnhandledInput(InputEvent inputEvent)
    {
        if (inputEvent.IsActionPressed("Target"))
        {
            Spatial nealestSpatial = NearlestSpatial();
            if (IsInstanceValid(nealestSpatial))
            {
                selectedTarget = nealestSpatial;
            }
        }
        if (inputEvent.IsActionPressed("Ability1"))
        {
            ActAbility(AttackType.Ability1);
        }
        else if (inputEvent.IsActionPressed("Ability2"))
        {
            ActAbility(AttackType.Ability2);
        }
        else if (inputEvent.IsActionPressed("Ability3"))
        {
            ActAbility(AttackType.Ability3);
        }
        if (inputEvent.IsActionPressed("Interact"))
        {
            Spatial nearlestSpatial = NearlestSpatial();
            if (IsInstanceValid(nearlestSpatial))
            {
                if (nearlestSpatial is Item item)
                {
                    item.Taken();
                }
            }
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
                    selectedTarget = kinematicBody;
                }
            }
            else if (inputEvent.IsActionPressed("Character"))
            {
                Dictionary hit = RayCastFromMouse(eventMouseButton.Position);
                if (hit.Contains("collider") && hit["collider"] is KinematicBody kinematicBody)
                {
                    if (kinematicBody.IsInGroup("Interactable"))
                    {
                        autoAttack = true;
                        selectedTarget = kinematicBody;
                        AttackToTarget();    
                    }
                }
                else if(hit.Contains("collider") && hit["collider"] is Item item)
                {
                    if (item.IsInGroup("Interactable"))
                    {
                        item.Taken();
                    }
                }
            }
        }
        if (inputEvent is InputEventMouseMotion)
        {
            InputEventMouseMotion emm = (InputEventMouseMotion)inputEvent;
            mousePos = emm.Position;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        SetCustomCursorShape(CustomCursorShape.Hand);
        Dictionary hit = RayCastFromMouse(mousePos);
        if (hit.Contains("collider"))
        {
            if (hit["collider"] is Monster mon) // Monster: Attack Cursor
            {
                if (!mon.isDied)
                {
                    SetCustomCursorShape(CustomCursorShape.Attack);
                }
            }
            else if (hit["collider"] is  Item spatial) 
            {
                if (spatial.IsInGroup("Interactable")) // Interactable: Hover Cursor
                {
                    SetCustomCursorShape(CustomCursorShape.Hover);
                }
            }
        }
        cooldowns[0] = Mathf.Clamp(cooldowns[0] - delta, 0.0f, abilityCooldowns.x);
        cooldowns[1] = Mathf.Clamp(cooldowns[1] - delta, 0.0f, abilityCooldowns.y);
        cooldowns[2] = Mathf.Clamp(cooldowns[2] - delta, 0.0f, abilityCooldowns.z);
    }

    public Dictionary RayCastFromMouse(Vector2 mousePos, bool ignoreSelf = true)
    {
        var from = camera.ProjectRayOrigin(mousePos);
        var to = from + camera.ProjectRayNormal(mousePos) * rayLength;
        var spaceState = camera.GetWorld().DirectSpaceState;

        Godot.Collections.Array ignores = new Godot.Collections.Array();
        if (ignoreSelf)
        {
            ignores.Add(playerCharacter); 
        }
        
        return spaceState.IntersectRay(from, to, ignores); // {collider, collider_id, normal, position, rid, shape}
    }

    // Return the nearlest spatial which group was given (default Interactable)
    public Spatial NearlestSpatial(bool checkRange = true, string groupName = "Interactable")
    {
        Godot.Collections.Array spatialGroup = GetTree().GetNodesInGroup(groupName);
        Spatial nearlestSpatial = null;
        Vector3 playerOrigin = playerCharacter.Transform.origin;
        for (int i = 0; i < spatialGroup.Count; i++)
        {
            Vector3 currentSpatialOrigin  = ((Spatial)(spatialGroup[i])).Transform.origin;
            if (IsInstanceValid(nearlestSpatial))
            {
                Vector3 nearlestSpatialOrigin  = nearlestSpatial.Transform.origin;
                if (playerOrigin.DistanceTo(currentSpatialOrigin) < playerOrigin.DistanceTo(nearlestSpatialOrigin)
                    && ((playerOrigin.DistanceTo(currentSpatialOrigin) < targetingRange) || !checkRange))
                {
                    nearlestSpatial = (Spatial)spatialGroup[i];
                }
            }
            else
            {
                if (playerOrigin.DistanceTo(currentSpatialOrigin) < targetingRange || !checkRange)
                {
                    nearlestSpatial = (Spatial)spatialGroup[i];
                }
            }
        }
        return nearlestSpatial;
    }

    private void SetCustomCursorShape(CustomCursorShape cursorShape)
    {
        if (editCursor && cursors.Length == 6)
        {
            Input.SetCustomMouseCursor(cursors[(int)cursorShape], Input.CursorShape.Arrow);
        }
    }

    private void AttackToTarget()
    {
        if (autoAttack)
        {
            if (IsInstanceValid(selectedTarget))
            {
              ActAbility(AttackType.Basic);
            }
        }
    }

    private void ActAbility(AttackType attackType)
    {
        switch (attackType)
        {
            case AttackType.Basic:
                BasicAttack();
                break;
            case AttackType.Ability1:
                Ability1Attack();
            break;
            case AttackType.Ability2:
                Ability2Attack();
            break;
            case AttackType.Ability3:
                Ability3Attack();
            break;
            default:
            break;
        }

    }

    // Spawn Follower Projectile
    private void BasicAttack() 
    {
        if (IsInstanceValid(selectedTarget))
        {
            playback.Travel("Shoot");
            if (selectedTarget is Monster mon)
            {
                Projectile basicAttack = (Projectile)basicAttackEffect.Instance();
                AddChild(basicAttack); 
                basicAttack.Transform = playerCharacter.Transform;
                basicAttack.target = selectedTarget;                        
            }
            else
            {
                GD.Print("ActAbility: Killed target isn't monster.");
                selectedTarget.QueueFree();
            }
        }
    }

    private void Ability1Attack()
    {
        if (cooldowns[0] <= 0)
        {
            playback.Travel("Punch");
            Spatial ability1 = (Spatial)ability1Effect.Instance();
            rightHand.AddChild(ability1); 
            ability1.GetChild<Particles>(0).Emitting = true;
            ability1.Transform = rightHand.Transform;
            Godot.Collections.Array nMonster = GetTree().GetNodesInGroup("Monster");
            for (int i = 0; i < nMonster.Count; i++)
            {
                Monster mon = (Monster)nMonster[i];
                if (playerCharacter.Transform.origin.DistanceTo(mon.Transform.origin) < 10)
                {
                    float angle = Mathf.Rad2Deg(playerCharacter.GlobalTransform.basis.z.AngleTo(playerCharacter.Transform.origin.DirectionTo(mon.Transform.origin).Normalized()));
                    if (angle < 30)
                    {
                        Spatial projectileHit = (Spatial)projectileHitPack.Instance();
                        mon.AddChild(projectileHit); 
                        projectileHit.GlobalTransform = mon.GlobalTransform;
                        projectileHit.GetChild<Particles>(0).Emitting = true;
                        mon.Kill();
                    }
                }
            }
            cooldowns[0] = abilityCooldowns.x;
        }
    }


    private void Ability2Attack()
    {
        if (cooldowns[1] <= 0)
        {
            playback.Travel("Slash");
            Spatial ability2 = (Spatial)ability2Effect.Instance();
            rightHand.AddChild(ability2); 
            ability2.GlobalTransform = rightHand.GlobalTransform;
            ability2.GetChild<Particles>(0).Emitting = true;
            Godot.Collections.Array nearMonster = GetTree().GetNodesInGroup("Monster");
            for (int i = 0; i < nearMonster.Count; i++)
            {
                Monster mon = (Monster)nearMonster[i];
                if (playerCharacter.Transform.origin.DistanceTo(mon.Transform.origin) < 20)
                {
                    float angle = Mathf.Rad2Deg((playerCharacter.GlobalTransform.basis.z.Rotated(playerCharacter.GlobalTransform.basis.y, -Mathf.Pi/2)).AngleTo(playerCharacter.Transform.origin.DirectionTo(mon.Transform.origin).Normalized()));
                    if (angle < 120)
                    {
                        Spatial projectileHit = (Spatial)projectileHitPack.Instance();
                        mon.AddChild(projectileHit); 
                        projectileHit.GlobalTransform = mon.GlobalTransform;
                        projectileHit.GetChild<Particles>(0).Emitting = true;
                        mon.Kill();
                    }
                }
            }
            cooldowns[1] = abilityCooldowns.y;
        }
    }


    private void Ability3Attack()
    {
        if (cooldowns[2] <= 0)
        {
            playback.Travel("Victory");
            Godot.Collections.Array nearMonsters = GetTree().GetNodesInGroup("Monster");
            Vector3 playerOrigin = playerCharacter.Transform.origin;
            Spatial ability3 = (Spatial)ability3Effect.Instance();
            AddChild(ability3); 
            ability3.GetChild<Particles>(0).Emitting = true;
            ability3.Transform = playerCharacter.Transform;
            for (int i = 0; i < nearMonsters.Count; i++)
            {
                Monster mon = (Monster)nearMonsters[i];
                Vector3 nearMonsterOrigin  = mon.Transform.origin;
                if (playerOrigin.DistanceTo(nearMonsterOrigin) < 10)
                {
                    Spatial projectileHit = (Spatial)projectileHitPack.Instance();
                    mon.AddChild(projectileHit); 
                    projectileHit.GlobalTransform = mon.GlobalTransform;
                    projectileHit.GetChild<Particles>(0).Emitting = true;
                    mon.Kill();
                }
            }
            cooldowns[2] = abilityCooldowns.z;
        }
    }

}

using Godot;
using System;

public class Projectile : Spatial
{
    [Export]
    private float moveSpeed = 4.0f;

    public Spatial target;
    PackedScene projectileHitPack = (PackedScene)ResourceLoader.Load("res://Characters/Player/Abilities/ProjectileHit.tscn");

    public override void _Ready()
    {
        
    }

    public override void _PhysicsProcess(float delta)
    {
        if (IsInstanceValid(target))
        {
            Vector3 targetTransform = (target.GlobalTransform.origin - GlobalTransform.origin).Normalized();
            GlobalTranslate(targetTransform * moveSpeed * delta);
        }
        if ((target.GlobalTransform.origin - GlobalTransform.origin).LengthSquared() < 0.5f)
        {
            HitMonster();
        }
    }

    public void HitMonster()
    {
        if (target is Monster mon)
        {
            mon.Kill();
            Spatial projectileHit = (Spatial)projectileHitPack.Instance();
            mon.AddChild(projectileHit); 
            projectileHit.GlobalTransform = mon.GlobalTransform;
            projectileHit.GetChild<Particles>(0).Emitting = true;
            QueueFree();
        }
    }
}

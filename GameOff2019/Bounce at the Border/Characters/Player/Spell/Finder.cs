using Godot;
using System;

public class Finder : Spatial
{
    [Export]
    public float moveSpeed = 1.0f;
    public Spatial target;

    private Spatial bookSpawner;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        bookSpawner = (Spatial)GetNode("../../../BookSpawner");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (target != null)
        {
            if ((target.GlobalTransform.origin - GlobalTransform.origin).LengthSquared() > 0.5f)
            {
                Vector3 targetTransform = (target.GlobalTransform.origin - GlobalTransform.origin).Normalized();
                GlobalTranslate(targetTransform * moveSpeed * delta);
            }
            else
            {
                Free();
            }
        }
    }

    public void Targeting()
    {
        if (bookSpawner.GetChildCount() > 0)
        {
            Spatial nearest = (Spatial)bookSpawner.GetChild(0);
            for (int i = 0; i < bookSpawner.GetChildCount(); i++)
            {
                Spatial element = (Spatial)bookSpawner.GetChild(i);
                if ((element.GlobalTransform.origin - GlobalTransform.origin).LengthSquared() < (nearest.GlobalTransform.origin - GlobalTransform.origin).LengthSquared())
                // if (GlobalTransform.origin.DistanceTo(element.GlobalTransform.origin) < GlobalTransform.origin.DistanceTo(nearest.GlobalTransform.origin))
                {
                    nearest = (Spatial)bookSpawner.GetChild(i);
                }
            }  
            target = nearest;
        }
    }

}

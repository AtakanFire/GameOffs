using Godot;
using System;

public class MusicNotes : Spatial
{
    [Export]
    private float hoverRange = 4.0f;
    private float beginPos = 0.0f;
    
    private float targetPos = 0.0f;
    private PlayerCharacter playerCharacter;
    private PlayerAttributes playerAttr;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        playerCharacter = GetNode<PlayerCharacter>("/root/Level/PlayerCharacter"); // Hardcoded
        if (IsInstanceValid(playerCharacter))
        {
            Godot.Collections.Array playerChilds = playerCharacter.GetChildren();
            for (int i = 0; i < playerChilds.Count; i++)
            {
                if (playerChilds[i] is PlayerAttributes pAttr)
                {
                    playerAttr = pAttr;
                }
            }
        }
    }

    public override void _Process(float delta)
    {
        HoverSpatial(delta);
    }

    private void HoverSpatial(float delta)
    {
        if (Mathf.Abs(Transform.origin.y - beginPos + hoverRange) < 0.2f)
        {
            hoverRange = -hoverRange;
        }
        GlobalTranslate(new Vector3(Transform.origin.x, Mathf.MoveToward(Transform.origin.y, beginPos + hoverRange, delta), Transform.origin.z));
    }

    private void Taken()
    {
        if (IsInstanceValid(playerAttr))
        {
            playerAttr.ChangeHealth(20);
        }
    }
}

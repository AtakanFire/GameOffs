using Godot;
using System;

public class Item : Spatial
{
    
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
        RotateObjectLocal(Vector3.Up, 0.5f * delta);
    }

    public void Taken()
    {
        if (IsInstanceValid(playerAttr))
        {
            playerAttr.ChangeHealth(20);
            QueueFree();
        }
    }
}

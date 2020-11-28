using Godot;
using System;

public class AbilityHUD : TextureButton
{
    
    public TextureProgress cooldownProgress;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        cooldownProgress = GetNode<TextureProgress>("./Cooldown");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}

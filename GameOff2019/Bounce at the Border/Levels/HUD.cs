using Godot;
using System;

public class HUD : Control
{
    [Signal]
    public delegate void EnterToPortal();
    
    private Spells spells; 
    private ProgressBar bar; 
    private AnimationPlayer animationPlayer; 

    public override void _Ready()
    {
        bar = (ProgressBar)GetNode("./Top/Middle/ProgressBar");
        bar.Value = 0.0f;
        spells = (Spells)GetNode("../Player/Spells");
        spells.Connect("BookReceived", this, nameof(_on_Spells_BookReceived));
        animationPlayer = (AnimationPlayer)GetNode("./HUDAnim");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void OpenPortal()
    {
        animationPlayer.Play("OpenPortal");
    }

    public void _on_Spells_BookReceived(float progress)
    {
        bar.Value = progress;
    }

}

using Godot;
using System;

public class HUD : Control
{
    [Signal]
    public delegate void EnterToPortal();
    
    private ProgressBar bar; 
    private AnimationPlayer animationPlayer; 

    public override void _Ready()
    {
        bar = (ProgressBar)GetNode("./Top/Middle/ProgressBar");
        bar.Value = 0.0f;
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

    public void LevelProgress(float progress)
    {
        bar.Value = progress;
    }

}

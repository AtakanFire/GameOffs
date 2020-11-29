using Godot;
using System;

public class PlayerAttributes : Node
{
    [Export]
    public float health = 100.0f;

    private MusicScorePlayer musicPlayer;

    private float targetPitch = 1.0f;
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        musicPlayer = GetNode<MusicScorePlayer>("/root/Level/MusicPlayer"); // Hardcoded
        if (!IsInstanceValid(musicPlayer))
        {
            GD.PrintErr("Attributes: musicPlayer isn't valid!");
        }   
    }

    public override void _Process(float delta)
    {
        if (IsInstanceValid(musicPlayer))
        {
            musicPlayer.PitchScale = Mathf.MoveToward(musicPlayer.PitchScale, targetPitch, delta);
        }
    }

    public void ChangeHealth(float change)
    {
        health = Mathf.Clamp(health + change, 0, 100);
        targetPitch = 0.4f + ((health / 100) * 0.6f);
    }

}

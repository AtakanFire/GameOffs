using Godot;
using System;

public class AnimationManager : AnimationTree
{

    private AnimationNodeStateMachinePlayback playback;
    private ThirdPersonCharacter character;

    public override void _Ready()
    {
        character = (ThirdPersonCharacter)GetNode("../../");
        playback = (AnimationNodeStateMachinePlayback)Get("parameters/playback");
        playback.Start("Idle-Run");
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("Forward") ||
            Input.IsActionPressed("Left") || Input.IsActionPressed("Right"))
        {
            Set("parameters/Idle-Run/blend_position", Mathf.Lerp(character.GetForwardSpeed(), 10, 0.1f));
        }  
        if (Input.IsActionPressed("Back"))
        {
            Set("parameters/Idle-Run/blend_position", Mathf.Lerp(character.GetForwardSpeed(), 0, 0.005f));
        }  
        if (Input.IsActionJustPressed("Jump") && character.IsOnFloor())
        {
            playback.Travel("Jump");
        }  
    }
}

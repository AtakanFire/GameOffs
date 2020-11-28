using Godot;
using System;

public class AnimationManager : AnimationTree
{
    [Export]
    private NodePath playerCharacterPath;
    private PlayerCharacter playerCharacter;
    private AnimationNodeStateMachinePlayback playback;

    public override void _Ready()
    {   
        if (playerCharacterPath.IsEmpty())
        {
            GD.PrintErr("Anim Manager: Player Character Path can't found!");
        }
        playerCharacter = (PlayerCharacter)GetNode(playerCharacterPath);
        playback = (AnimationNodeStateMachinePlayback)Get("parameters/playback");
        playback.Start("Idle-Run");
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionPressed("Forward") ||
        Input.IsActionPressed("Left") || Input.IsActionPressed("Right"))
        {
            Set("parameters/Idle-Run/blend_position", Mathf.Lerp(playerCharacter.GetForwardSpeed(), 10, 0.1f));
        }
        else
        {
            Set("parameters/Idle-Run/blend_position", Mathf.Lerp(playerCharacter.GetForwardSpeed(), 0, 0.005f));
        }
    }

    public override void _UnhandledInput(InputEvent inputEvent) 
    { 
        Set("parameters/conditions/Jump", inputEvent.IsActionPressed("Jump") && playerCharacter.IsOnFloor());
        // Set("parameters/conditions/Punch", inputEvent.IsActionPressed("Ability1"));
        // Set("parameters/conditions/Slash", inputEvent.IsActionPressed("Ability2"));
        // Set("parameters/conditions/Victory", inputEvent.IsActionPressed("Ability3"));
        // // Set("parameters/conditions/Defeat", inputEvent.IsActionPressed("Ability3"));
        Set("parameters/conditions/Roll", inputEvent.IsActionPressed("Roll"));
        Set("parameters/conditions/Take", inputEvent.IsActionPressed("Interact"));
    }
}

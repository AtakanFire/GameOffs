using Godot;
using System;

public class Spells : Node
{
    PackedScene step = (PackedScene)ResourceLoader.Load("res://Characters/Player/Spell/Step.tscn");
    PackedScene finder = (PackedScene)ResourceLoader.Load("res://Characters/Player/Spell/Finder.tscn");

    private ThirdPersonCharacter character;
    private BoneAttachment rightHandBone;

    public override void _Ready()
    {
        character = (ThirdPersonCharacter)GetNode("../");
        rightHandBone = (BoneAttachment)GetNode("../Witch/CharacterArmature/RightHand");
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("Step")) // LeftShift - Create Floor
        {
            Spatial instance = (Spatial)step.Instance();
            AddChild(instance);   
            instance.Transform = character.Transform;
        }  
        if (Input.IsActionJustPressed("Find"))
        {
            Finder instance = (Finder)finder.Instance();
            AddChild(instance);   
            instance.Transform = rightHandBone.GlobalTransform;
            instance.Targeting();
        }
    }
}

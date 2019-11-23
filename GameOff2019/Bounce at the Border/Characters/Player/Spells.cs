using Godot;
using System;

public class Spells : Node
{
    PackedScene step = (PackedScene)ResourceLoader.Load("res://Characters/Player/Spell/Step.tscn");

    private ThirdPersonCharacter character;

    public override void _Ready()
    {
        character = (ThirdPersonCharacter)GetNode("../");
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("Step")) // LeftShift - Create Floor
        {
            Spatial instance = (Spatial)step.Instance();
            AddChild(instance);   
            instance.Transform = character.Transform;
        }  
        if (Input.IsActionJustPressed("CastSpell"))
        {
            
        }  
    }
}

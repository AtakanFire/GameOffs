using Godot;
using System;

public class Spells : Node
{
    PackedScene step = (PackedScene)ResourceLoader.Load("res://Characters/Player/Spell/Step.tscn");
    PackedScene finder = (PackedScene)ResourceLoader.Load("res://Characters/Player/Spell/Finder.tscn");

    private Godot.Collections.Array steps = new Godot.Collections.Array();
    private ThirdPersonCharacter character;
    private BoneAttachment rightHandBone;
    private BookSpawner bookSpawner;

    [Export]
    private float playerTakeRange = 1.0f;

    public override void _Ready()
    {
        character = (ThirdPersonCharacter)GetNode("../");
        rightHandBone = (BoneAttachment)GetNode("../Witch/CharacterArmature/RightHand");
        bookSpawner = (BookSpawner)GetNode("/root/Gameplay/BookSpawner");
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("Step")) // Create Floor(Step)
        {
            Spatial instance = (Spatial)step.Instance();
            AddChild(instance);   
            instance.Transform = character.Transform;
            steps.Add(instance);
        }  
        if (Input.IsActionJustPressed("ResetSteps")) // Clear Floors(Steps)
        {
            for (int i = 0; i < steps.Count; i++)
            {
                Spatial s = (Spatial)steps[i];
                s.Free();
            }
            steps.Clear();
        }  
        if (Input.IsActionJustPressed("Find"))
        {
            Finder instance = (Finder)finder.Instance();
            AddChild(instance);   
            instance.Transform = rightHandBone.GlobalTransform;
            instance.Targeting();
        } 
        if (Input.IsActionJustPressed("Take"))
        {
            for (int i = 0; i < bookSpawner.GetChildCount(); i++)
            {
                Spatial book = (Spatial)bookSpawner.GetChild(i);
                if ((rightHandBone.GlobalTransform.origin - book.GlobalTransform.origin).LengthSquared() < playerTakeRange)
                {
                    book.Free();
                }
            }
        }
    }
}

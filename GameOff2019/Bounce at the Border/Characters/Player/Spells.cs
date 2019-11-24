using Godot;
using System;

public class Spells : Node
{
    [Signal]
    public delegate void BookReceived(float progress);

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
        bookSpawner = (BookSpawner)GetNode("../../BookSpawner");

        // Create First Step on 0,0,0
        Spatial instance = (Spatial)step.Instance();
        AddChild(instance); 
        steps.Add(instance);  
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("Step"))
        {
            SpellStep();
        }  
        if (Input.IsActionJustPressed("ResetSteps"))
        {
            SpellResetSteps();
        }  
        if (Input.IsActionJustPressed("Find"))
        {
            SpellFind();
        } 
        if (Input.IsActionJustPressed("Take"))
        {
            SkillTake();
        }
    }

    private void SpellStep() // Create floor(step)
    {
        Spatial instance = (Spatial)step.Instance();
        AddChild(instance);   
        instance.Transform = character.Transform;
        steps.Add(instance);
    }

    private void SpellResetSteps() // Remove all floors(steps)
    {
        for (int i = 0; i < steps.Count; i++)
        {
            Spatial s = (Spatial)steps[i];
            s.Free();
        }
        steps.Clear();
    }

    private void SpellFind() // Throw particle to nearest book 
    {
        Finder instance = (Finder)finder.Instance();
        AddChild(instance);   
        instance.Transform = rightHandBone.GlobalTransform;
        instance.Targeting();
    }

    private void SkillTake() // Taking Skill
    {
        Godot.Collections.Array children = bookSpawner.GetChildren();
        for (int i = 0; i < children.Count; i++)
        {
            Spatial book = (Spatial)children[i];
            if (book != null && (rightHandBone.GlobalTransform.origin - book.GlobalTransform.origin).LengthSquared() < playerTakeRange)
            {
                book.Free();
            }
        }
        float progress = ((float)bookSpawner.spawnCount - (float)bookSpawner.GetChildCount())/(float)bookSpawner.spawnCount;
        EmitSignal(nameof(BookReceived), progress);
    }
}

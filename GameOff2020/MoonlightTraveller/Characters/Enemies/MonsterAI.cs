using Godot;
using System;

public class MonsterAI : Node
{
    [Export]
    private NodePath monsterPath = new NodePath("");
    [Export]
    private NodePath monsterModelPath = new NodePath("");

    public Monster monster;
    public MonsterModel monsterModel;

    public override void _Ready()
    {
        monster = GetNode<Monster>(monsterPath);
        if (!IsInstanceValid(monster))
        {
            GD.PrintErr("MonsterAI: Monster isn't valid!");
        } 
        monsterModel = GetNode<MonsterModel>(monsterModelPath);
        if (!IsInstanceValid(monsterModel))
        {
            GD.PrintErr("MonsterAI: Monster Model isn't valid!");
        } 
    }
}

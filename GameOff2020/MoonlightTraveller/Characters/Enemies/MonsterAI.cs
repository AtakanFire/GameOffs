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

    public void _Monster_OnReachedToTarget(bool isReached)
    {
        monsterModel.animTree.Set("parameters/conditions/Bite", isReached);
    }

    public void _Monster_OnKilled()
    {
        monster.isDied = true;
        monsterModel.animTree.Set("parameters/conditions/Death", true);
        
        Godot.Collections.Array monGroup = monster.GetGroups();
        for (int i = 0; i < monGroup.Count; i++)
        {
            monster.RemoveFromGroup((string)monGroup[i]);
        }
    }

}

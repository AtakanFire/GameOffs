using Godot;
using System;

public enum MonsterBody
{
    Cyclops,
    Demon,
    Ghost,
    GreenDemon,
    Mushroom,
    Skull,
    Yeti
}

public class MonsterModel : Spatial
{
    [Export]
    private MonsterBody monsterBody = MonsterBody.Cyclops;
    [Export]
    private NodePath monsterPath = new NodePath("");


    public Monster monster;
    public Spatial body;
    public AnimationTree animTree;

    private float oldBlend = 0.0f;

    public override void _Ready()
    {
        monster = GetNode<Monster>(monsterPath);
        if (!IsInstanceValid(monster))
        {
            GD.PrintErr("MonsterAI: Monster isn't valid!");
        } 
        ChangeBody(monsterBody);
    }

    public void ChangeBody(MonsterBody monBody) 
    {
        switch (monBody)
        {
            case MonsterBody.Cyclops:
                BodyVisibility("Cyclops");
                break;
            case MonsterBody.Demon:
                BodyVisibility("Demon");
                break;
            case MonsterBody.Ghost:
                BodyVisibility("Ghost");
                break;
            case MonsterBody.GreenDemon:
                BodyVisibility("GreenDemon");
                break;
            case MonsterBody.Mushroom:
                BodyVisibility("Mushroom");
                break;
            case MonsterBody.Skull:
                BodyVisibility("Skull");
                break;
            case MonsterBody.Yeti:
                BodyVisibility("Yeti");
                break;
            default:
                GD.Print("Monster Body Name can't found in enum!");
                break;
        }
    }


    private void BodyVisibility(string Name) 
    {
        for (int i = 0; i < GetChildren().Count; i++)
        {
            GetChild<Spatial>(i).Visible = false;
        }

        body = GetNode<Spatial>("./" + Name);
        body.Visible = true;

        animTree = GetNode<AnimationTree>("./" + Name + "/AnimationTree");

        if (!IsInstanceValid(animTree))
        {
            GD.PrintErr("Anim Tree isn't valid!");
        }
    }

    public override void _Process(float delta)
    {
        if (IsInstanceValid(monster))
        {
            if (IsInstanceValid(animTree))
            {
                float movement = Mathf.Clamp(monster.moveDirection.Length(), 0, 10);
                float moveBlend = Mathf.Lerp(oldBlend, movement, 0.1f);
                oldBlend = moveBlend;
                animTree.Set("parameters/Idle-Walk/blend_position", moveBlend);
            }
        }
    }
}

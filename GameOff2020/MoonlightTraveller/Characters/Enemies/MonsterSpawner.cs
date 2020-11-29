using Godot;
using System;

public class MonsterSpawner : Spatial
{
    [Export]
    private NodePath playerCharacterPath = new NodePath();
    [Export]
    private NodePath navigationPath = new NodePath();
    [Export]
    // Monster Spawn Rate 
    private Vector2 spawnRate = new Vector2(1, 4);

    public PlayerCharacter playerCharacter;
    public Navigation navigation;
    
    PackedScene monsterPack = (PackedScene)ResourceLoader.Load("res://Characters/Enemies/Monster.tscn");

    Godot.Collections.Array<Position3D> spawnPoints = new Godot.Collections.Array<Position3D>(); 
    private RandomNumberGenerator randomNumber = new RandomNumberGenerator();
    private Timer timer = new Timer();
    private int wave = 1;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Godot.Collections.Array childs = GetChildren();
        for (int i = 0; i < childs.Count; i++)
        {
            if (childs[i] is Position3D spawnPos)
            {
                spawnPoints.Add(spawnPos);
            }
        }

        AddChild(timer);
        timer.Connect("timeout", this, "SpawnMonster");

        navigation = GetNode<Navigation>(navigationPath);
        if (!IsInstanceValid(navigation))
        {
            GD.PushWarning("Navigation can't found!");
        }
        timer.Start(4.0f);

        playerCharacter = GetNode<PlayerCharacter>(playerCharacterPath);
        if (IsInstanceValid(playerCharacter))
        {
            SpawnMonster();
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    private MonsterBody GetRandomMonBody()
    {
        return (MonsterBody)randomNumber.RandiRange(0, 6);
    }

    private void SpawnMonster()
    {
        Monster monster = (Monster)monsterPack.Instance();
        if (IsInstanceValid(monster))
        {
            AddChild(monster, true); 
            randomNumber.Randomize();
            monster.InitializeMonster(playerCharacter, GetRandomMonBody(), navigation);
            randomNumber.Randomize();
            monster.Transform = spawnPoints[randomNumber.RandiRange(0, spawnPoints.Count-1)].Transform;
        }
        timer.Start(randomNumber.RandiRange((int)spawnRate.x, (int)spawnRate.y));
    }
}

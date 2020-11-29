using Godot;
using System;

public class ItemSpawner : Spatial
{
    [Export]
    private int spawnRate = 10;

    PackedScene itemPack = (PackedScene)ResourceLoader.Load("res://Level/Interactables/MusicNotes.tscn");
    private RandomNumberGenerator randomNumber = new RandomNumberGenerator();
    Godot.Collections.Array<Position3D> spawnPoints = new Godot.Collections.Array<Position3D>(); 
    private Timer timer = new Timer();
    private PlayerAttributes playerAttributes;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        AddChild(timer);
        timer.Connect("timeout", this, "SpawnItem");
        Godot.Collections.Array childs = GetChildren();
        for (int i = 0; i < childs.Count; i++)
        {
            if (childs[i] is Position3D spawnPos)
            {
                spawnPoints.Add(spawnPos);
            }
        }
        playerAttributes = GetNode<PlayerAttributes>("/root/Level/PlayerCharacter/Attributes"); // Hardcoded
        if (IsInstanceValid(playerAttributes))
        {
            SpawnItem();
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    private void SpawnItem()
    {
        Item item = (Item)itemPack.Instance();
        if (IsInstanceValid(item))
        {
            AddChild(item, true); 
            randomNumber.Randomize();
            item.Transform = spawnPoints[randomNumber.RandiRange(0, spawnPoints.Count-1)].Transform;
        }
        timer.Start(randomNumber.RandiRange(spawnRate/2, spawnRate));
    }
}

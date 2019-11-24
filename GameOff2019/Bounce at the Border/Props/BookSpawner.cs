using Godot;
using System;

public class BookSpawner : Spatial
{
    PackedScene book = (PackedScene)ResourceLoader.Load("res://Props/Book.tscn");
    
    [Export]
    public int spawnRange = 10;

    [Export]
    public int spawnCount = 10;

    private RandomNumberGenerator rng = new RandomNumberGenerator();

    public override void _Ready()
    {
        rng.Randomize();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void SpawnBook()
    {
        Spatial instance = (Spatial)book.Instance();
        AddChild(instance);   
        Vector3 randomLocation = new Vector3(rng.RandiRange(-spawnRange, spawnRange), rng.RandiRange(-spawnRange, spawnRange), rng.RandiRange(-spawnRange, spawnRange));
        instance.Translate(randomLocation);
        instance.RotateY(rng.RandiRange(0, 360));
    }

    public void SpawnBooks()
    {
        for (int i = 0; i < spawnCount; i++)
        {
            SpawnBook();
        }
    }

    public void SpawnSettings(int sRange, int sBook)
    {
        spawnRange = sRange/2;
        spawnCount = sBook;
    }

}

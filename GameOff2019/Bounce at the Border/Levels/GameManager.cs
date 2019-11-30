using Godot;
using System;

public class GameManager : Node 
{

    [Export(PropertyHint.Range, "1,500")]
    public int gameLevel = 1;

    public bool levelCompleted = false;

    private BookSpawner bookSpawner; 
    private AnimationManager animationManager; 
    private HUD hud; 
    private Label levelLabel;
    private Button portal;


    PackedScene gameplay = (PackedScene)ResourceLoader.Load("res://Levels/Gameplay.tscn");

    public override void _Ready()
    {
        PrepareVariables();
        CreateLevel();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (levelCompleted)
        {
            animationManager.Victory();
            hud.OpenPortal();

        }
        if (Input.IsActionJustPressed("Restart"))
        {
            Restart();
        }
        if (levelCompleted && Input.IsActionJustPressed("NextLevel"))
        {
            NextLevel();
        }
        if (Input.IsActionPressed("Take"))
        {
            CalculateProgress();
        }
    }

    private void PrepareVariables()
    {
        bookSpawner = (BookSpawner)GetNode("/root/GameManager/Gameplay/BookSpawner");
        animationManager = (AnimationManager)GetNode("/root/GameManager/Gameplay/Player/Witch/AnimationTree");
        hud = (HUD)GetNode("/root/GameManager/Gameplay/HUD");
        levelLabel = (Label)GetNode("/root/GameManager/Gameplay/HUD/Bot/Middle/ProgressBar/Label");
        levelLabel.Text = gameLevel + ". level";
        portal = (Button)GetNode("/root/GameManager/Gameplay/HUD/Portal/Button");
        portal.Connect("button_down", this, nameof(_on_Button_button_down));
    }

    private void CreateLevel()
    {
        bookSpawner.SpawnSettings(gameLevel, gameLevel);
        bookSpawner.SpawnBooks();
    }

    private void Restart()
    {
        //GetTree().ReloadCurrentScene(); // Restart All Game
        gameLevel--;
        NextLevel();
    }

    private void NextLevel()
    {
        // Remove Level
        Node oldLevel = GetNode("./Gameplay");
        RemoveChild(oldLevel);
        oldLevel.CallDeferred("free");

        // Load Level
        var nextLevel = gameplay.Instance();
        AddChild(nextLevel);
        gameLevel++;
        levelCompleted = false;
        PrepareVariables();
        CreateLevel();
    }

    public void CalculateProgress()
    {
        float progress = ((float)bookSpawner.spawnCount - (float)bookSpawner.GetChildCount())/(float)bookSpawner.spawnCount;

        hud.LevelProgress(progress);

        if (progress == 1)
        {
            levelCompleted = true;
        }
    }

    public void _on_Button_button_down()
    {
        NextLevel();
    }

}

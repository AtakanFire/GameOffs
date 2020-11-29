using Godot;
using System;

public class HUD : Control
{
    [Export]
    private NodePath MusicScorePlayerPath = new NodePath();
    [Export]
    private NodePath abilitiesPath = new NodePath();
    [Export]
    private NodePath attributesPath = new NodePath();

    public MusicScorePlayer musicPlayer;
    public Abilities abilities;
    public PlayerAttributes attributes;

    private ProgressBar progress;

    private Label health;

    private AbilityHUD abilitybtn1;
    private AbilityHUD abilitybtn2;
    private AbilityHUD abilitybtn3;
    

    private bool playOver = false;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        musicPlayer = GetNode<MusicScorePlayer>(MusicScorePlayerPath);
        if (!IsInstanceValid(musicPlayer))
        {
            GD.PrintErr("HUD: musicPlayer isn't valid!");
        }
        progress = GetNode<ProgressBar>("./ProgressBar");
        if (!IsInstanceValid(progress))
        {
            GD.PrintErr("HUD: progress isn't valid!");
        }
        abilitybtn1 = GetNode<AbilityHUD>("./Abilities/Ability1");
        abilitybtn2 = GetNode<AbilityHUD>("./Abilities/Ability2");
        abilitybtn3 = GetNode<AbilityHUD>("./Abilities/Ability3");

        health = GetNode<Label>("./HealthBackground/Health");
        
        abilities = GetNode<Abilities>(abilitiesPath);
        if (!IsInstanceValid(abilities))
        {
            GD.PrintErr("HUD: abilities isn't valid!");
        }
        attributes = GetNode<PlayerAttributes>(attributesPath);
        if (!IsInstanceValid(attributes))
        {
            GD.PrintErr("HUD: attributes isn't valid!");
        }
    }

    public override void _Process(float delta)
    {
        if (IsInstanceValid(musicPlayer) && !playOver)
        {
            progress.Value = (musicPlayer.GetPlaybackPosition() / 817.0f) * 100; // 817.96s ~818s hardcoded(cant find GetMusicLenght)
            if (progress.Value == 0.999f)
            {
                playOver = true;
            }
        }
        if (IsInstanceValid(abilities))
        {
            abilitybtn1.cooldownProgress.Value = (abilities.cooldowns[0] / abilities.abilityCooldowns.x)*100;
            abilitybtn2.cooldownProgress.Value = (abilities.cooldowns[1] / abilities.abilityCooldowns.y)*100;
            abilitybtn3.cooldownProgress.Value = (abilities.cooldowns[2] / abilities.abilityCooldowns.z)*100;
        }
        if (IsInstanceValid(health))
        {
            health.Text = "%" + attributes.health;
        }
    }
}

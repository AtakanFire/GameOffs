using Godot;
using System;
// using System.Collections;
// using System.Collections.Generic;
using Godot.Collections; // Finally work with this namespace, because of Dictionary also in System

struct MusicNote
{
    public string step;
    public int octave;
    public int duration;
    public int voice;
    public MusicNote(string s, int o, int d, int v)
    {
        step = s;
        octave = o;
        duration = d;
        voice = v;
    }
    public String ToNoteString(){
        return step + " - " + octave + " - " + duration + " - " + voice;
    }
}


public class MusicScorePlayer : AudioStreamPlayer
{
    [Export]
    // Score and music sync take time so unused
    private bool AutoPlay = true;
    [Export]
    // Score and music sync take time so unused
    private bool UseScore = false;
    [Export]
    // Using only voice one
    private bool UseOnlyFirstVoice = true;

    [Export(PropertyHint.Dir, "")]
    private String scoreDir = "res://Audio/Beethoven-MoonlightSonata.json";
    [Export]
    private float timeAccurancy = 0.0063f; // This must be calculate with tempo and etc.
    private Array<Timer> timers;
 
    private Godot.Collections.Array allObject;
    private int noteIndex = 0;
    
    public override void _Ready()
    {
        if (UseScore && AutoPlay)
        {
            ReadScore();
        }
        else if(AutoPlay)
        {
            Play();
        }
    }

    public override void _Process(float delta)
    {

    }

    public override void _UnhandledInput(InputEvent inputEvent)
    {
        if (UseScore && inputEvent.IsActionPressed("Jump"))
        {
            Play();
            ActMusic();
        }
    }

    // Read json file and create timer 
    private void ReadScore() 
    {
        File file = new File();
        file.Open(scoreDir, File.ModeFlags.Read);
        String str = file.GetAsText();
        // GD.Print(str);
        JSONParseResult jsonObject = JSON.Parse(str);
        file.Close();

        if(jsonObject.Error != 0){
            GD.Print("Error:", jsonObject.ErrorLine);
        }else{
            // GD.Print(JSON.Print(jsonObject.Result));
            allObject = jsonObject.Result as Godot.Collections.Array;
            for (int i = 0; i < allObject.Count; i++)
            {
                Godot.Collections.Dictionary note = allObject[i] as Godot.Collections.Dictionary; 
                string step = (string)note["step"]; 
                int octave = ((String)(note["octave"])).ToString().ToInt();
                int duration = ((String)(note["duration"])).ToString().ToInt();              
                int voice = ((String)(note["duration"])).ToString().ToInt();              
            }
        }
        timers = new Godot.Collections.Array<Timer>();
        for (int i = 0; i < 20; i++) // Max 6 Voice in this score(I hope)
        {
            timers.Add(CreateTimer());
        }
    }

    public void ActMusic()
    {
        // if (!IsInstanceValid(timers[GetMusicsNote().voice]))
        // {
        //     timers.Insert(GetMusicsNote().voice, CreateTimer());
        // } 

        if (UseOnlyFirstVoice)
        {
            int forcedVoice = 1;
            if (GetMusicsNote().voice != forcedVoice)
            {
                noteIndex++;
                timers[forcedVoice].Start(GetMusicsNote().duration * timeAccurancy);
            }
            else
            {
                timers[forcedVoice].Stop();
                GD.Print(GetMusicsNote().ToNoteString());
                noteIndex++;
                timers[forcedVoice].Start(GetMusicsNote().duration * timeAccurancy);
            }
        }
        else
        {
            timers[GetMusicsNote().voice].Stop();
            GD.Print(GetMusicsNote().ToNoteString());
            noteIndex++;

            timers[GetMusicsNote().voice].Start(GetMusicsNote().duration * timeAccurancy);
        }

    }

    private MusicNote GetMusicsNote()
    {
        Godot.Collections.Dictionary note = allObject[noteIndex] as Godot.Collections.Dictionary; 
        string step = (string)note["step"]; 
        int octave = ((String)(note["octave"])).ToString().ToInt();
        int duration = ((String)(note["duration"])).ToString().ToInt();    
        int voice = ((String)(note["voice"])).ToString().ToInt();    
        MusicNote musicNote = new MusicNote(step, octave, duration, voice);
        return musicNote;
    }

    private Timer CreateTimer()
    {
        Timer timer = new Timer();
        timer.OneShot = false;
        timer.ProcessMode = Timer.TimerProcessMode.Physics;
        timer.Connect("timeout", this, "_on_Timer_timeout");
        AddChild(timer);
        return timer;
    }

    public void _on_Timer_timeout()
    {
        ActMusic();
    }

}

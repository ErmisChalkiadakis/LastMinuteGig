using System;
using UnityEngine;

[Serializable]
public class PercussionMusicClip
{
    public string Name;
    public AudioClip AudioClip;
    public int Tempo; /// Measured in BPM.
    public Instrument Instrument;
    public Key Key;
    public Rhythm Rhythm;
    public ButtonTiming[] ButtonTimings;

    public double ClipDuration => 60f * GetRhythmModifier() / Tempo; 
    
    private float GetRhythmModifier()
    {
        switch (Rhythm)
        {
            case Rhythm.FourFour:
                return 4;
            case Rhythm.ThreeFour:
                return 3;
            default:
                return 1;
        }
    }
}

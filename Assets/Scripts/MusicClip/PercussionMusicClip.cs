using System;
using UnityEngine;

[Serializable]
public class PercussionMusicClip
{
    public string Name;
    public AudioClip AudioClip;
    public float Tempo; /// Measured in BPM.
    public Instrument Instrument;
    public Rhythm Rhythm;
    public ButtonTiming[] ButtonTimings;

    public double ClipDuration => 60f * RhythmModifierHelper.GetRhythmModifier(Rhythm) / Tempo; 

    public PercussionMusicClip(float tempo, Rhythm rhythm)
    {
        Tempo = tempo;
        Rhythm = rhythm;
    }
}

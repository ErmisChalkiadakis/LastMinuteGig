using System;
using UnityEngine;

[Serializable]
public class PercussionMusicClip
{
    public string Name;
    public AudioClip AudioClip;
    public Tempo Tempo; /// Measured in BPM.
    public Instrument Instrument;
    public Rhythm Rhythm;
    public ButtonTiming[] ButtonTimings;

    public double ClipDuration => 60f * RhythmModifierHelper.GetRhythmModifier(Rhythm) / TempoToBPMHelper.GetBPM(Tempo); 

    public PercussionMusicClip(Tempo tempo, Rhythm rhythm)
    {
        Tempo = tempo;
        Rhythm = rhythm;
    }
}

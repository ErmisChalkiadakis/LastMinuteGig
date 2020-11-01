﻿using System;
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

    public double ClipDuration => 60f * RhythmModifierHelper.GetRhythmModifier(Rhythm) / Tempo; 
}

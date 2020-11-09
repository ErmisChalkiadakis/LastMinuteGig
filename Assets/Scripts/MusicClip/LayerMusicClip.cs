using System;
using UnityEngine;

[Serializable]
public class LayerMusicClip
{
    public string Name;
    public AudioClip AudioClip;
    public float Tempo; /// Measured in BPM.
    public Instrument Instrument;
    public Key Key;
    public Rhythm Rhythm;
    public Chord Chord;

    public double ClipDuration => 60f * RhythmModifierHelper.GetRhythmModifier(Rhythm) / Tempo;
}

using System;
using UnityEngine;

[Serializable]
public class LayerMusicClip
{
    public string Name;
    public AudioClip AudioClip;
    public int Tempo; /// Measured in BPM.
    public Instrument Instrument;
    public Key Key;
    public Rhythm Rhythm;

    public double ClipDuration => 60f * RhythmModifierHelper.GetRhythmModifier(Rhythm) / Tempo;
}

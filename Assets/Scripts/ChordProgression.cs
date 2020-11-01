using System;

[Serializable]
public class ChordProgression
{
    public string name;
    [FixEnumNames]public ChordNotation[] chords;
}

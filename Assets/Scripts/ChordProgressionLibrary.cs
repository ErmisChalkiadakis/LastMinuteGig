using UnityEngine;

using Random = System.Random;

public class ChordProgressionLibrary : ScriptableObject
{
    [SerializeField] private ChordProgression[] chordProgressions;

    public ChordProgression GetFirstChordProgression()
    {
        return chordProgressions[0];
    }

    public ChordProgression GetRandomChordProgression()
    {
        Random random = new Random();

        int index = random.Next(chordProgressions.Length);
        return chordProgressions[index];
    }
}

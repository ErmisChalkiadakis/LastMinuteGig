using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    public ChordProgression GetRandomChordProgressionOtherThan(ChordProgression chordProgression)
    {
        List<ChordProgression> otherChordProgressions = new List<ChordProgression>(chordProgressions);
        if (!otherChordProgressions.Contains(chordProgression))
        {
            Debug.LogError($"Chord progression {chordProgression} not found in the Library");
            return null;
        }
        otherChordProgressions.Remove(chordProgression);
        Random random = new Random();

        int index = random.Next(otherChordProgressions.Count);
        return otherChordProgressions[index];
    }
}

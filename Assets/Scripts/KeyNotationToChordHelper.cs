using System;
using System.Collections.Generic;
using System.Linq;

using Debug = UnityEngine.Debug;

public static class KeyNotationToChordHelper
{
    public static Chord GetChord(Key key, ChordNotation chordNotation)
    {
        string chordName = "";
        int noteDelta = RomanNumeralToInt(chordNotation);
        chordName += GetChordTonicName(key, noteDelta);
        if (!Char.IsUpper(Enum.GetName(typeof(ChordNotation), chordNotation), 0))
        {
            chordName += "m";
        }

        return (Chord)Enum.Parse(typeof(Chord), chordName);
    }

    private static string GetChordTonicName(Key key, int delta)
    {
        List<Key> keys = Enum.GetValues(typeof(Key)).Cast<Key>().ToList();

        int indexOfKey = keys.IndexOf(key);
        int indexToGet = indexOfKey + delta;
        if (indexToGet >= keys.Count)
        {
            indexToGet -= keys.Count;
        }

        return Enum.GetName(typeof(Key), keys[indexToGet]);
    }

    private static int RomanNumeralToInt(ChordNotation chordNotation)
    {
        switch (chordNotation)
        {
            case ChordNotation.I:
                return 0;
            case ChordNotation.ii:
                return 2;
            case ChordNotation.iii:
                return 4;
            case ChordNotation.IV:
                return 5;
            case ChordNotation.V:
                return 7;
            case ChordNotation.vi:
                return 9;
            case ChordNotation.vii:
                return 11;
            default:
                Debug.Log($"Something went wrong when converting numeral to integer");
                return 0;
        }
    }
}

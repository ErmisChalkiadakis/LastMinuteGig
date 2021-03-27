using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class TutorialMusicClipManager : MonoBehaviour
{
    [Serializable]
    private class TutorialClip
    {
        public Rhythm Rhythm => rhythm;
        public Tempo Tempo => tempo;

        [SerializeField] private Rhythm rhythm;
        [SerializeField] private Tempo tempo;
    }

    [SerializeField] private MusicMixer musicMixer;
    [SerializeField] private MusicClipInputManager inputManager;
    [SerializeField] private PercussionMusicClipLibrary percussionClipLibrary;
    [SerializeField] private InputMusicClipLibrary inputClipLibrary;
    [SerializeField] private ChordProgressionLibrary chordProgressionLibrary;
    [SerializeField] private int loopCount;

    private Random random;

    protected void Awake()
    {
        random = new Random();
    }

    public void StartTutorialClips()
    {
        GenerateTutorialClips();
    }

    private void GenerateTutorialClips()
    {
        ChordProgression chordProgression = chordProgressionLibrary.GetFirstChordProgression();
        Rhythm rhythm = GetRandomEnum<Rhythm>();
        Tempo tempo = GetRandomEnum<Tempo>();
        Key key = GetRandomEnum<Key>();

        for (int i = 0; i < loopCount; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                MusicalChange change = GetRandomMusicalChange();
                if (change == MusicalChange.Tempo)
                {
                    tempo = GetRandomEnumOtherThan(tempo);
                }
                else
                {
                    rhythm = GetRandomEnumOtherThan(rhythm);
                }

                int chordIndex = 0;
                for (int k = 0; k < 4; k++)
                {
                    PercussionMusicClip percussionClip = percussionClipLibrary.GetRandomClipWithRhythmAndTempo(rhythm, tempo);
                    InputMusicClip inputClip = inputClipLibrary.GetClipWithInstrumentAndChord(
                        Instrument.ElectricGuitar, 
                        KeyNotationToChordHelper.GetChord(key, chordProgression.chords[chordIndex]));
                    MusicClip clip = new MusicClip(percussionClip, inputClip, null);
                    musicMixer.QueueClip(clip);
                    chordIndex++;
                }
            }
        }
    }

    private MusicalChange GetRandomMusicalChange()
    {
        return (random.Next(2) == 0) ? MusicalChange.Tempo : MusicalChange.Rhythm;
    }

    private T GetRandomEnum<T>() where T : Enum
    {
        Array values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(random.Next(values.Length));
    }
    
    private T GetRandomEnumOtherThan<T>(T en) where T : Enum
    {
        T[] allValues = (T[])Enum.GetValues(typeof(T));
        Enum test = Enum.Parse(typeof(T), en.ToString()) as Enum;
        int index = Convert.ToInt32(test);
        T[] newValues = new T[allValues.Length - 1];
        int counter = 0;
        for (int i = 0; i < allValues.Length; i++)
        {
            if (i != index)
            {
                newValues[counter] = allValues[i];
                counter++;
            }
        }

        T returnValue = newValues[random.Next(newValues.Length)];
        return returnValue;
    }
}

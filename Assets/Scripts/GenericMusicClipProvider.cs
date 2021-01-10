using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GenericMusicClipProvider : IMusicClipSetProvider
{
    public const int CALIBRATION_COUNT = 15;

    private ChordProgressionLibrary chordProgressionLibrary;
    private PercussionMusicClipLibrary percussionLibrary;
    private InputMusicClipLibrary inputLibrary;
    private LayerMusicClipLibrary layerLibrary;

    private List<MusicClipSet> clipSets;
    private MusicClipResults[] clipResults;
    private List<MusicalChange> musicalChanges;
    private List<MusicalChange> calibrationMusicalChanges;

    public GenericMusicClipProvider()
    {
        chordProgressionLibrary = Resources.Load<ChordProgressionLibrary>("Libraries/ChordProgressionLibrary");
        percussionLibrary = Resources.Load<PercussionMusicClipLibrary>("Libraries/PercussionMusicClipLibrary");
        inputLibrary = Resources.Load<InputMusicClipLibrary>("Libraries/InputMusicClipLibrary");
        layerLibrary = Resources.Load<LayerMusicClipLibrary>("Libraries/LayerMusicClipLibrary");

        clipSets = new List<MusicClipSet>();
        musicalChanges = new List<MusicalChange>();
        PopulateCalibrationMusicalChanges();
    }

    public MusicClipSet GetFirstClipSet()
    {
        ChordProgression chordProgression = chordProgressionLibrary.GetRandomChordProgression();
        Key key = GetRandomEnum<Key>();
        Rhythm rhythm = GetRandomEnum<Rhythm>();
        Tempo tempo = GetRandomEnum<Tempo>();
        Debug.Log($"First clip created with Tempo: {tempo}, Rhythm: {rhythm}, Key: {key} and Chord Progression: {chordProgression.name}");

        MusicClipSet clipSet = GenerateClipSetWithParameters(key, tempo, rhythm, chordProgression);
        clipSets.Add(clipSet);
        return clipSet;
    }

    public MusicClipSet GetNextClipSet(MusicClipResults[] musicClipResults)
    {
        clipResults = musicClipResults;
        UpdateWeights();

        MusicalChange change;
        change = (calibrationMusicalChanges.Count > 0) ? GetRandomCalibrationMusicalChange() : FindNextMusicalChange();

        //TODO: Add chord progression and Key changes periodically
        MusicClipSet musicClipSet = GenerateNextClipSetWithChange(change);
        clipSets.Add(musicClipSet);
        return musicClipSet;
    }

    private T GetRandomEnum<T>()
    {
        Array values = Enum.GetValues(typeof(T));
        Random random = new Random();
        return (T)values.GetValue(random.Next(values.Length));
    }

    private MusicalChange GetMusicalChangeBetweenSets(MusicClipSet first, MusicClipSet second)
    {
        if (first.IsEmpty)
        {
            return MusicalChange.RestToPlay;
        }
        if (second.IsEmpty)
        {
            return MusicalChange.PlayToRest;
        }
        if (first.Tempo != second.Tempo)
        {
            return MusicalChange.Tempo; 
        }
        if (first.Rhythm != second.Rhythm)
        {
            return MusicalChange.Rhythm;
        }
        else
        {
            return MusicalChange.Key;
        }
    }

    private void PopulateCalibrationMusicalChanges()
    {
        calibrationMusicalChanges = new List<MusicalChange>();
        int mod = CALIBRATION_COUNT % 3;
        int div = CALIBRATION_COUNT / 3;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < div; j++)
            {
                calibrationMusicalChanges.Add(GetChangeFromIndex(i));
            }
        }
        for (int i = 0; i < mod; i++)
        {
            calibrationMusicalChanges.Add(GetChangeFromIndex(i));
        }
    }

    private MusicalChange GetChangeFromIndex(int index)
    {
        if (index == 0)
        {
            return MusicalChange.PlayToRest;
        }
        if (index == 1)
        {
            return MusicalChange.Tempo;
        }
        if (index == 2)
        {
            return MusicalChange.Rhythm;
        }
        return default;
    }

    private MusicalChange GetRandomCalibrationMusicalChange()
    {
        Random random = new Random();
        int index = random.Next(calibrationMusicalChanges.Count);
        MusicalChange change = calibrationMusicalChanges[index];
        calibrationMusicalChanges.RemoveAt(index);
        return change;
    }

    private MusicalChange FindNextMusicalChange()
    {
        MusicalChange change = MusicalChange.Tempo;

        if (musicalChanges.Count == 0)
        {
            // TODO: Implement
        }

        return change;
    }

    private MusicClipSet GenerateEmptyClipSet(Key key, Tempo tempo, Rhythm rhythm, ChordProgression chordProgression)
    {
        MusicClip emptyClip = new MusicClip(0, tempo, rhythm);
        MusicClip[] clipArray = new MusicClip[1];
        clipArray[0] = emptyClip;
        MusicClipSet emptyClipSet = new MusicClipSet(clipArray, key, rhythm, tempo, chordProgression, true);
        return emptyClipSet;
    }

    private MusicClipSet GenerateClipSetWithParameters(Key key, Tempo tempo, Rhythm rhythm, ChordProgression chordProgression)
    {
        int clipCount = chordProgression.chords.Length;
        MusicClip[] musicClips = new MusicClip[clipCount];
        for (int i = 0; i < clipCount; i++)
        {
            PercussionMusicClip percussionClip = percussionLibrary.GetRandomClipWithRhythmAndTempo(rhythm, tempo);
            InputMusicClip inputClip = inputLibrary.GetClipWithInstrumentAndChord(Instrument.ElectricGuitar, KeyNotationToChordHelper.GetChord(key, chordProgression.chords[i]));
            LayerMusicClip[] layerMusicClips = new LayerMusicClip[0];
            musicClips[i] = new MusicClip(percussionClip, inputClip, layerMusicClips);
        }

        return new MusicClipSet(musicClips, key, rhythm, tempo, chordProgression, false);
    }

    private MusicClipSet GenerateNextClipSetWithChange(MusicalChange change)
    {
        MusicClipSet nextClipSet = null;
        MusicClipSet previousClipSet = clipSets[clipSets.Count - 1];
        Key key = previousClipSet.Key;
        Rhythm rhythm = previousClipSet.Rhythm;
        Tempo tempo = previousClipSet.Tempo;
        ChordProgression chordProgression = previousClipSet.ChordProgression;

        if (change == MusicalChange.Tempo)
        {
            Debug.Log($"Creating a tempo change");
            tempo = GetRandomEnumOtherThan(tempo);
        }
        else if (change == MusicalChange.Rhythm)
        {
            Debug.Log($"Creating a rhythm change");
            rhythm = GetRandomEnumOtherThan(rhythm);
        }
        else if (change == MusicalChange.PlayToRest)
        {
            Debug.Log($"Creating a play-rest change");
            return GenerateEmptyClipSet(key, tempo, rhythm, chordProgression);
        }

        nextClipSet = GenerateClipSetWithParameters(key, tempo, rhythm, chordProgression);

        return nextClipSet;
    }

    private void UpdateWeights()
    {
        // TODO: Implement
    }

    private T GetRandomEnumOtherThan<T>(T value)
    {
        T randomValue = default(T);
        Array values = Enum.GetValues(typeof(T));
        if (values.Length < 2)
        {
            Debug.LogError($"Enum of type {typeof(T)} doesn't contain more than one value.");
            return value;
        }

        Random random = new Random();
        bool randomValueFound = false;
        while (!randomValueFound)
        {
            randomValue = (T)values.GetValue(random.Next(values.Length));
            if (!randomValue.Equals(value))
            {
                randomValueFound = true;
            }
        }
        return randomValue;
    }
}

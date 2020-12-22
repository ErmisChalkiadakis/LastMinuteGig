using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class GenericMusicClipProvider : IMusicClipSetProvider
{
    private ChordProgressionLibrary chordProgressionLibrary;
    private PercussionMusicClipLibrary percussionLibrary;
    private InputMusicClipLibrary inputLibrary;
    private LayerMusicClipLibrary layerLibrary;

    private List<MusicClipSet> clipSets;
    private MusicClipResults[] clipResults;
    private List<MusicalChange> musicalChanges;

    public GenericMusicClipProvider()
    {
        chordProgressionLibrary = Resources.Load<ChordProgressionLibrary>("Libraries/ChordProgressionLibrary");
        percussionLibrary = Resources.Load<PercussionMusicClipLibrary>("Libraries/PercussionMusicClipLibrary");
        inputLibrary = Resources.Load<InputMusicClipLibrary>("Libraries/InputMusicClipLibrary");
        layerLibrary = Resources.Load<LayerMusicClipLibrary>("Libraries/LayerMusicClipLibrary");

        clipSets = new List<MusicClipSet>();
    }

    public MusicClipSet GetFirstClipSet()
    {
        ChordProgression chordProgression = chordProgressionLibrary.GetRandomChordProgression();
        Key key = GetRandomEnum<Key>();
        Rhythm rhythm = GetRandomEnum<Rhythm>();
        Tempo tempo = GetRandomEnum<Tempo>();

        int clipCount = chordProgression.chords.Length;
        MusicClip[] musicClips = new MusicClip[clipCount];
        for (int i = 0; i < clipCount; i++)
        {
            PercussionMusicClip percussionClip = percussionLibrary.GetRandomClipWithRhythmAndTempo(rhythm, tempo);
            InputMusicClip inputClip = inputLibrary.GetClipWithInstrumentAndChord(Instrument.ElectricGuitar, KeyNotationToChordHelper.GetChord(key, chordProgression.chords[i]));
            LayerMusicClip[] layerMusicClips = new LayerMusicClip[0];
            musicClips[i] = new MusicClip(percussionClip, inputClip, layerMusicClips);
        }

        MusicClipSet clipSet = new MusicClipSet(musicClips, key, rhythm, tempo, false);
        clipSets.Add(clipSet);
        return clipSet;
    }

    public MusicClipSet GetNextClipSet(MusicClipResults[] musicClipResults)
    {
        clipResults = musicClipResults;
        MusicalChange nextChange = FindNextBestChange(musicClipResults, clipSets, musicalChanges);

        // Add new musical change to the list
        return null;
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

    private MusicalChange FindNextMusicalChange()
    {
        MusicalChange change = MusicalChange.Tempo;

        if (musicalChanges.Count == 0)
        {

        }

        return change;
    }
}

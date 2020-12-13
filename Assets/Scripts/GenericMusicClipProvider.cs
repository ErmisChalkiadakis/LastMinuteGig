using System;
using UnityEngine;
using Random = System.Random;

public class GenericMusicClipProvider : IMusicClipSetProvider
{
    private ChordProgressionLibrary chordProgressionLibrary;
    private PercussionMusicClipLibrary percussionLibrary;
    private InputMusicClipLibrary inputLibrary;
    private LayerMusicClipLibrary layerLibrary;

    public GenericMusicClipProvider()
    {
        chordProgressionLibrary = Resources.Load<ChordProgressionLibrary>("Libraries/ChordProgressionLibrary");
        percussionLibrary = Resources.Load<PercussionMusicClipLibrary>("Libraries/PercussionMusicClipLibrary");
        inputLibrary = Resources.Load<InputMusicClipLibrary>("Libraries/InputMusicClipLibrary");
        layerLibrary = Resources.Load<LayerMusicClipLibrary>("Libraries/LayerMusicClipLibrary");
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

        return new MusicClipSet(musicClips, key, rhythm, tempo);
    }

    public MusicClipSet GetNextClipSet(MusicClipResults[] musicClipResults)
    {
        return null;
    }

    private T GetRandomEnum<T>()
    {
        Array values = Enum.GetValues(typeof(T));
        Random random = new Random();
        return (T)values.GetValue(random.Next(values.Length));
    }
}

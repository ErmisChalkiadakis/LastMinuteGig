using System;
using System.Collections.Generic;
using UnityEngine;

public class MusicClipManager : MonoBehaviour
{
    private const float FLIP_INTERVAL = 0.5f;

    [SerializeField] private MusicMixer musicMixer;
    [SerializeField] private MusicClipInputManager inputManager;
    [SerializeField] private PercussionMusicClipLibrary musicClipLibrary;
    [SerializeField] private ChordProgressionLibrary chordProgressionLibrary;

    private ChordProgression currentChordProgression;
    private int chordProgressionIndex = 0;

    private List<MusicClipResults> clipResults;

    private Key currentKey;

    private double nextEventTime = double.MaxValue;

    protected void Awake()
    {
        inputManager.ClipInputFinalizedEvent += OnClipInputFinalizedEvent;

        clipResults = new List<MusicClipResults>();
    }

    protected void Start()
    {
        currentKey = Key.C;
        currentChordProgression = chordProgressionLibrary.GetRandomChordProgression();
        QueueNextClip();
    }

    protected void OnDestroy()
    {
        inputManager.ClipInputFinalizedEvent -= OnClipInputFinalizedEvent;
    }

    protected void Update()
    {
        if (AudioSettings.dspTime > nextEventTime - FLIP_INTERVAL)
        {
            QueueNextClip();
        }
    }

    private void OnClipInputFinalizedEvent(MusicClipResults clipResults)
    {
        this.clipResults.Add(clipResults);
    }

    private void QueueNextClip()
    {
        ChordNotation notation = currentChordProgression.chords[chordProgressionIndex];
        Chord chordToGet = KeyNotationToChordHelper.GetChord(currentKey, notation);

        /*
        if (clip != null)
        {
            musicMixer.QueueClip(clip);
        }

        if (++chordProgressionIndex >= currentChordProgression.chords.Length)
        {
            chordProgressionIndex -= currentChordProgression.chords.Length;
        }

        nextEventTime = AudioSettings.dspTime + clip.ClipDuration;
        */
    }
}

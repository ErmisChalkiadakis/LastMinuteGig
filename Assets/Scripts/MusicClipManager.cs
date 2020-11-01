using System;
using System.Collections.Generic;
using UnityEngine;

public class MusicClipManager : MonoBehaviour
{
    private const float FLIP_INTERVAL = 0.5f;

    [SerializeField] private MusicMixer musicMixer;
    [SerializeField] private MusicClipInputManager inputManager;
    [SerializeField] private PercussionMusicClipLibrary percussionClipLibrary;
    [SerializeField] private InputMusicClipLibrary inputClipLibrary;
    [SerializeField] private LayerMusicClipLibrary layerClipLibrary;
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
        PercussionMusicClip percussionClip = percussionClipLibrary.GetRandomClip();
        InputMusicClip inputClip = inputClipLibrary.GetRandomClip();

        // TODO fix this
        //LayerMusicClip layerClip = layerClipLibrary.GetRandomClip();

        MusicClip clip = new MusicClip(percussionClip, inputClip, new LayerMusicClip[0]);
        musicMixer.QueueClip(clip);

        nextEventTime = AudioSettings.dspTime + clip.Duration;

        /*
        ChordNotation notation = currentChordProgression.chords[chordProgressionIndex];
        Chord chordToGet = KeyNotationToChordHelper.GetChord(currentKey, notation);

        if (clip != null)
        {
            musicMixer.QueueClip(clip);
        }

        if (++chordProgressionIndex >= currentChordProgression.chords.Length)
        {
            chordProgressionIndex -= currentChordProgression.chords.Length;
        }
        */
    }
}

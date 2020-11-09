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
    [SerializeField] private ClipSetSequence clipSetSequence;

    //private ChordProgression currentChordProgression;
    //private int chordProgressionIndex = 0;
    //private Key currentKey;
    private int clipSetIndex;
    private MusicClip previousClip;

    private PercussionMusicClip currentPercussionClip;
    private InputMusicClip currentInputClip;
    private List<LayerMusicClip> currentLayerClips;

    private List<MusicClipResults> clipResults;

    private double nextEventTime = double.MaxValue;

    protected void Awake()
    {
        inputManager.ClipInputFinalizedEvent += OnClipInputFinalizedEvent;

        clipResults = new List<MusicClipResults>();
        currentLayerClips = new List<LayerMusicClip>();
    }

    protected void Start()
    {
        //currentKey = Key.C;
        //currentChordProgression = chordProgressionLibrary.GetRandomChordProgression();
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
        //ChordNotation notation = currentChordProgression.chords[chordProgressionIndex];
        //Chord chordToGet = KeyNotationToChordHelper.GetChord(currentKey, notation);
        ClipSet clipSet = clipSetSequence.getClipSetAtIndex(clipSetIndex);

        currentPercussionClip = percussionClipLibrary.GetClipWithName(clipSet.PercussionClipName);
        currentInputClip = inputClipLibrary.GetClipWithName(clipSet.InputClipName);
        currentLayerClips.Clear();
        foreach (var name in clipSet.LayerClipNames)
        {
            currentLayerClips.Add(layerClipLibrary.GetClipWithName(name));
        }

        MusicClip clip;
        if (currentPercussionClip == null && currentInputClip == null)
        {
            clip = new MusicClip(previousClip.PercussionClip.Tempo, previousClip.PercussionClip.Rhythm);
        }
        else
        {
            clip = new MusicClip(currentPercussionClip, currentInputClip, currentLayerClips.ToArray());
        }
        musicMixer.QueueClip(clip);

        nextEventTime = AudioSettings.dspTime + clip.Duration;
        clipSetIndex++;
        previousClip = clip;

        //if (++chordProgressionIndex >= currentChordProgression.chords.Length)
        //{
        //    chordProgressionIndex -= currentChordProgression.chords.Length;
        //}
    }
}

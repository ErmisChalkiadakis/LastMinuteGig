using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicClipManager : MonoBehaviour
{
    public delegate void SequenceEndedHandler();
    public event SequenceEndedHandler SequenceEndedEvent;

    private const float FLIP_INTERVAL = 0.5f;
    private const float DELAY_UNTIL_FIRST = 1f;

    [SerializeField] private MusicMixer musicMixer;
    [SerializeField] private MusicClipInputManager inputManager;
    [SerializeField] private PercussionMusicClipLibrary percussionClipLibrary;
    [SerializeField] private InputMusicClipLibrary inputClipLibrary;
    [SerializeField] private LayerMusicClipLibrary layerClipLibrary;
    [SerializeField] private ChordProgressionLibrary chordProgressionLibrary;
    [SerializeField] private ClipSetSequence clipSetSequence;

    private MusicClipSet activeClipSet;

    private int clipSetIndex;
    private MusicClip previousClip;
    private IMusicClipSetProvider clipSetProvider;

    private PercussionMusicClip currentPercussionClip;
    private InputMusicClip currentInputClip;
    private List<LayerMusicClip> currentLayerClips;
    private int clipId = 0;

    private List<MusicClipResults> clipResults;

    private double nextEventTime = double.MaxValue;

    protected void Awake()
    {
        inputManager.ClipInputFinalizedEvent += OnClipInputFinalizedEvent;

        clipResults = new List<MusicClipResults>();
        currentLayerClips = new List<LayerMusicClip>();
        clipSetProvider = new GenericMusicClipProvider();
        activeClipSet = clipSetProvider.GetFirstClipSet();
        clipSetIndex = 0;
    }

    protected void Start()
    {
        nextEventTime = AudioSettings.dspTime + DELAY_UNTIL_FIRST;
    }

    protected void OnDestroy()
    {
        inputManager.ClipInputFinalizedEvent -= OnClipInputFinalizedEvent;
    }

    protected void Update()
    {
        if (AudioSettings.dspTime + FLIP_INTERVAL > nextEventTime)
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
        bool activeClipSetEnded = clipSetIndex >= activeClipSet.MusicClips.Length;
        if (activeClipSetEnded)
        {
            activeClipSet = clipSetProvider.GetNextClipSet(clipResults.ToArray());
            clipSetIndex = 0;
        }

        /*
        if (activeClipSet.IsEmpty)
        {
            Debug.Log($"Protocol Ending");
            nextEventTime = double.MaxValue;
            SequenceEndedEvent?.Invoke();
            return;
        }*/

        MusicClip clip = activeClipSet.MusicClips[clipSetIndex];
        musicMixer.QueueClip(clip);
        nextEventTime += clip.Duration;
        clipSetIndex++;
    }

    private void LogClipResults(MusicClipResults clipResults)
    {
        string results = "";
        foreach (var inputWindow in clipResults.InputWindowResults)
        {
            results += $"Any button: {inputWindow.AnyButtonSelected}\n" +
                $"Correct button: {inputWindow.CorrectButtonSelected}\n" +
                $"First button: {inputWindow.FirstButtonSelected}\n";
        }
        Debug.Log(results);
    }
}

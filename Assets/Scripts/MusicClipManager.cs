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
        IMusicClipSetProvider provider = new GenericMusicClipProvider();
        activeClipSet = provider.GetFirstClipSet();
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
        Debug.Log($"Manager knowing clip is finalized");
        this.clipResults.Add(clipResults);

        if (clipSetSequence.SequenceLength <= clipSetIndex)
        {
            nextEventTime = double.MaxValue;
            SequenceEndedEvent?.Invoke();
        }
    }

    private void QueueNextClip()
    {
        Debug.Log($"Manager queueing Clip");
        ClipSet clipSet = clipSetSequence.getClipSetAtIndex(clipSetIndex);
        clipSetIndex++;

        currentPercussionClip = percussionClipLibrary.GetClipWithName(clipSet.PercussionClipName);
        currentInputClip = inputClipLibrary.GetClipWithName(clipSet.InputClipName);
        currentLayerClips.Clear();
        foreach (var name in clipSet?.LayerClipNames)
        {
            currentLayerClips.Add(layerClipLibrary.GetClipWithName(name));
        }

        MusicClip clip;
        if (currentPercussionClip == null && currentInputClip == null)
        {
            clip = new MusicClip(clipId, previousClip.PercussionClip.Tempo, previousClip.PercussionClip.Rhythm);
        }
        else
        {
            clip = new MusicClip(clipId, currentPercussionClip, currentInputClip, currentLayerClips.ToArray());
        }
        clipId++;
        musicMixer.QueueClip(clip);

        nextEventTime += clip.Duration;
        previousClip = clip;
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

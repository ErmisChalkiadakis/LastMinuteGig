using System;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private int clipSetsPerSong = 20;

    private MusicClipSet activeClipSet;

    private IMusicClipSetProvider clipSetProvider;

    private List<MusicClipResults> clipResults;

    protected void Awake()
    {
        inputManager.ClipInputFinalizedEvent += OnClipInputFinalizedEvent;

        clipResults = new List<MusicClipResults>();
        clipSetProvider = new GenericMusicClipProvider();
        QueueClips();
    }

    protected void OnDestroy()
    {
        inputManager.ClipInputFinalizedEvent -= OnClipInputFinalizedEvent;
    }

    private void OnClipInputFinalizedEvent(MusicClipResults clipResults)
    {
        this.clipResults.Add(clipResults);
    }

    private void QueueClips()
    {
        activeClipSet = clipSetProvider.GetFirstClipSet();
        for (int i = 0; i < activeClipSet.ClipCount; i++)
        {
            musicMixer.QueueClip(activeClipSet.MusicClips[i]);
        }

        for (int j = 0; j < clipSetsPerSong; j++)
        {
            activeClipSet = clipSetProvider.GetNextClipSet(clipResults.ToArray());
            for (int i = 0; i < activeClipSet.ClipCount; i++)
            {
                musicMixer.QueueClip(activeClipSet.MusicClips[i]);
            }
        }
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

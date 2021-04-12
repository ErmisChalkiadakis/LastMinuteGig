using System;
using System.Collections.Generic;
using UnityEngine;

public class MusicClipManager : MonoBehaviour
{
    public delegate void SequenceEndedHandler();
    public event SequenceEndedHandler SequenceEndedEvent;

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
    }

    protected void OnDestroy()
    {
        inputManager.ClipInputFinalizedEvent -= OnClipInputFinalizedEvent;
    }

    public void StartSong()
    {
        QueueClips();
    }

    private void OnClipInputFinalizedEvent(MusicClipResults clipResults)
    {
        if (clipResults.ID == activeClipSet.MusicClips[activeClipSet.MusicClips.Length - 1].ID)
        {
            Debug.Log($"Final Clip Played");
            SequenceEndedEvent?.Invoke();
        }

        this.clipResults.Add(clipResults);
        Debug.Log(clipResults);
    }

    private void QueueClips()
    {
        activeClipSet = clipSetProvider.GetFirstClipSet();
        for (int i = 0; i < activeClipSet.ClipCount; i++)
        {
            musicMixer.QueueClip(activeClipSet.MusicClips[i]);
        }

        for (int j = 0; j < clipSetsPerSong - 1; j++)
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

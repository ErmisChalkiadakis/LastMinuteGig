using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickFigure : MonoBehaviour
{
    private static int ROCK_HASH = Animator.StringToHash("Rock");
    private static int PLAY_HASH = Animator.StringToHash("Play");
    private const string CHORD = "Chord";

    [SerializeField] private Animator animator;
    [SerializeField] private MusicMixer musicMixer;
    [SerializeField] private InstrumentButton instrumentButton;

    private bool isRocking;

    protected void Awake()
    {
        musicMixer.ClipScheduledEvent += OnClipScheduledEvent;
        instrumentButton.ButtonSelectedEvent += OnButtonSelectedEvent;
    }

    protected void OnDestroy()
    {
        musicMixer.ClipScheduledEvent -= OnClipScheduledEvent;
    }

    private void OnClipScheduledEvent(PercussionMusicClip scheduledClip, double startingTime)
    {
        if (!isRocking)
        {
            StartCoroutine(Rock(startingTime));
        }

        StartCoroutine(ChangeChord(scheduledClip, startingTime));
        // TODO: Handle animator's speed based on clip tempo.
    }

    private void OnButtonSelectedEvent(ButtonID buttonId)
    {
        animator.SetTrigger(PLAY_HASH);
    }

    private IEnumerator Rock(double startingTime)
    {
        while (AudioSettings.dspTime < startingTime)
        {
            yield return null;
        }

        isRocking = true;
        animator.SetTrigger(ROCK_HASH);
    }

    private IEnumerator ChangeChord(PercussionMusicClip scheduledClip, double startingTime)
    {
        while (AudioSettings.dspTime < startingTime)
        {
            yield return null;
        }

        // TODO: fix this.
        //string newChordName = CHORD + scheduledClip.MiddleButtonClipData.chord.ToString();
        //animator.SetTrigger(Animator.StringToHash(newChordName));
    }
}
